using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;
using Teleware.Algorithm.TableBuilder.Shared.Cells;
using Teleware.Algorithm.TableBuilder.Shared.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Shared.Rows;

namespace Teleware.Algorithm.TableBodyBuilder.Impl
{
    public class DefaultTableBodyBuilder : ITableBodyBuilder
    {
        private IRowDataPicker _picker;
        private IMergeCellsCollector _mergeCellsCollector;
        private IEnumerable<AggregateRowDefinition> _aggregateRows;
        private DataRowDefinition _dataRow;

        public DefaultTableBodyBuilder(
            IRowDataPicker picker,
            DataRowDefinition dataRow,
            IEnumerable<AggregateRowDefinition> aggregateRows,
            IMergeCellsCollector mergeCellsCollector)
        {
            _picker = picker;
            _dataRow = dataRow;
            _mergeCellsCollector = mergeCellsCollector;
            _aggregateRows = aggregateRows ?? new AggregateRowDefinition[0];
        }

        private static TResult MapRowByType<TResult>(Row row, Func<DataRow, TResult> dataRowMapper, Func<AggregateRow, TResult> aggregateRowMapper)
        {
            if (row is DataRow)
            {
                return dataRowMapper((DataRow)row);
            }
            else if (row is AggregateRow)
            {
                return aggregateRowMapper((AggregateRow)row);
            }
            else
            {
                throw new System.NotSupportedException($"无法识别的行类型: {row.GetType()}");
            }
        }

        private static void VisitRowByType(Row row, Action<DataRow> dataRowVisitor, Action<AggregateRow> aggregateRowVisitor)
        {
            if (row is DataRow)
            {
                dataRowVisitor((DataRow)row);
            }
            else if (row is AggregateRow)
            {
                aggregateRowVisitor((AggregateRow)row);
            }
            else
            {
                throw new System.NotSupportedException($"无法识别的行类型: {row.GetType()}");
            }
        }

        public TableBody Build(dynamic rawDatas)
        {
            IDictionary<string, dynamic>[] rowDatas = GetRowDatas(rawDatas);
            var body = CreateBody(rowDatas);
            var aggregateRows = CreateAggregateRows(body);
            MergeAggregateRows(body, aggregateRows);
            for (int i = 0; i < body.Count; i++)
            {
                var row = body[i];
                EvalFormulaCells(row);
                body[i] = DecorateRowAndCells(row);
            }
            var mergeCellGroups = CollectMergeCellGroups(body);

            return new TableBody(body, mergeCellGroups);
        }

        private Row DecorateRowAndCells(Row r)
        {
            return MapRowByType<Row>(r, dr =>
            {
                var ctx = dr.RowBuildContext;
                for (int i = 0; i < dr.Cells.Count; i++)
                {
                    dr.Cells[i] = ctx.DecorateCell(dr.Cells[i]);
                }
                return ctx.DecorateRow(dr);
            }, ar =>
            {
                var ctx = ar.RowBuildContext;
                for (int i = 0; i < ar.Cells.Count; i++)
                {
                    ar.Cells[i] = ctx.DecorateCell(ar.Cells[i]);
                }
                return ctx.DecorateRow(ar);
            });
        }

        private static void MergeAggregateRows(IList<Row> body, List<Tuple<int, AggregateRow>> aggregateRows)
        {
            foreach (var aggregateRow in aggregateRows.Reverse<Tuple<int, AggregateRow>>())
            {
                body.Insert(aggregateRow.Item1, aggregateRow.Item2);
            }
        }

        private List<Tuple<int, AggregateRow>> CreateAggregateRows(IList<Row> body)
        {
            var aggregateRows = new List<Tuple<int, AggregateRow>>();
            foreach (var aggregateRowDef in _aggregateRows)
            {
                var firstRow = true;
                string lastKey = null;
                var rowsToAggregate = new List<Tuple<int, DataRow>>();
                for (var i = 0; i < body.Count; i++)
                {
                    var row = (DataRow)body[i];
                    var key = aggregateRowDef.AggregateKeySelector(row);
                    if (firstRow)
                    {
                        firstRow = false;
                        lastKey = key;
                        rowsToAggregate.Add(Tuple.Create(i, row));
                    }
                    else if (lastKey == key)
                    {
                        rowsToAggregate.Add(Tuple.Create(i, row));
                    }
                    else if (lastKey != key)
                    {
                        var aggregateRowTuple = CreateAggregateRow(lastKey, aggregateRowDef, rowsToAggregate);
                        aggregateRows.Add(aggregateRowTuple);
                        rowsToAggregate.Clear();
                        lastKey = key;
                        rowsToAggregate.Add(Tuple.Create(i, row));
                    }
                }
                if (rowsToAggregate.Count > 0)
                {
                    var aggregateRowTuple = CreateAggregateRow(lastKey, aggregateRowDef, rowsToAggregate);
                    aggregateRows.Add(aggregateRowTuple);
                }
            }

            return aggregateRows.OrderBy(t => t.Item1).ToList();
        }

        private Tuple<int, AggregateRow> CreateAggregateRow(string lastKey, AggregateRowDefinition aggregateRowDef, List<Tuple<int, DataRow>> rowsToAggregate)
        {
            var rowNum = rowsToAggregate[rowsToAggregate.Count - 1].Item1 + 1;
            var cellCount = rowsToAggregate.First().Item2.Cells.Count;

            var cells = Enumerable.Repeat<Cell>(EmptyCell.Singleton, cellCount).ToList();
            var aggregateBuildContext = new AggregateRowBuildContext(aggregateRowDef, lastKey, rowsToAggregate.Select(ra => ra.Item2).ToArray());

            foreach (var column in aggregateRowDef.Columns)
            {
                var cell = column.CreateCell(aggregateBuildContext);
                cells[column.ColNum] = cell;
            }

            return Tuple.Create(rowNum, new AggregateRow(cells, aggregateBuildContext));
        }

        private void EvalFormulaCells(Row row)
        {
            VisitRowByType(row, (dr) =>
            {
                foreach (var cell in dr.Cells)
                {
                    if (cell is FormulaCell)
                    {
                        (cell as FormulaCell).ExecuteFormula(new[] { (DataRow)row });
                    }
                }
            }, (ar) =>
            {
                foreach (var cell in ar.Cells)
                {
                    if (cell is FormulaCell)
                    {
                        (cell as FormulaCell).ExecuteFormula(((AggregateRow)row).RowBuildContext.RowsToAggregate);
                    }
                }
            });
        }

        private IList<Row> CreateBody(IDictionary<string, dynamic>[] rowDatas)
        {
            var body = rowDatas.Select(rd =>
            {
                var rowBuildContext = new DataRowBuildContext(_dataRow, rd);
                var rowColumns = _dataRow.Columns.Select(colDef =>
                {
                    return colDef.CreateCell(rowBuildContext);
                }).ToList();
                return new DataRow(rowColumns, rowBuildContext);
            }).ToList<Row>();
            return body;
        }

        private IDictionary<string, dynamic>[] GetRowDatas(dynamic rawDatas)
        {
            IEnumerable<IDictionary<string, dynamic>> rowDatas = _picker.PickRowDatas(rawDatas);
            return rowDatas.ToArray();
        }

        private IEnumerable<IEnumerable<CellReference>> CollectMergeCellGroups(IList<Row> data)
        {
            if (_mergeCellsCollector == null)
            {
                return Enumerable.Empty<IEnumerable<CellReference>>();
            }
            var mergeCellPairs = _mergeCellsCollector.Collect(data)
                .OrderBy(t => t.Item1.RowNum)
                .ThenBy(t => t.Item1.ColNum).ToArray(); // 确保至少起始单元格从左上到右下

            var visitedCells = new List<CellReference>();
            var mergeCellGroups = new List<List<CellReference>>();
            foreach (var mergeCellPair in mergeCellPairs)
            {
                if (visitedCells.Any(t => t.Equals(mergeCellPair.Item1))) // 如果起始单元格已处理，则跳过
                {
                    continue;
                }
                List<CellReference> mergeCellList = CollectMergeGroup(mergeCellPairs, mergeCellPair);
                visitedCells.AddRange(mergeCellList.OrderBy(c => c.RowNum).ThenBy(c => c.ColNum)); // 确保返回结果有序
                mergeCellGroups.Add(mergeCellList);
            }

            return mergeCellGroups;
        }

        private static List<CellReference> CollectMergeGroup(Tuple<CellReference, CellReference>[] mergeCells, Tuple<CellReference, CellReference> mergeCellPair)
        {
            var mergeCellList = new List<CellReference>();
            mergeCellList.Add(mergeCellPair.Item1);
            mergeCellList.Add(mergeCellPair.Item2);

            for (int i = 1; i < mergeCellList.Count; i++)
            {
                var nextMergeSource = mergeCellList[i];
                var nextMergeCellPairs = mergeCells.Where(t => t.Item1.Equals(nextMergeSource));
                foreach (var tuple in nextMergeCellPairs)
                {
                    mergeCellList.Add(tuple.Item2);
                }
            }

            return mergeCellList;
        }
    }
}