using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder;
using Teleware.Algorithm.TableBuilder.AggregateColumnDefinitions;
using Teleware.Algorithm.TableBuilder.ColumnFormulas;
using Teleware.Algorithm.TableBuilder.DataColumnDefinitions;
using Teleware.Algorithm.TableBuilder.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Rows;
using Teleware.Algorithm.TableBuilder.TableBodyBuilder;

namespace Sample
{
    public class Program2
    {
        private static void Main()
        {
            //TODO: 如何处理占比？如何更有效处理聚合标题？
            var builder = TableBodyBuilderConfiguration.Instance
                .SetRowDataPicker(new SimpleObjectPropertyPicker<Data>())
                .SetDataRowDefinition(new DataRowDefinition(new DataColumnDefinition[] {
                    new ReferenceColumnDefinition("大类","大类"),
                    new ReferenceColumnDefinition("小类1","小类1"),
                    new ReferenceColumnDefinition("小类","小类2"),
                    new ReferenceColumnDefinition("值","值"),
                    new ReferenceColumnDefinition("占比","大类占比")
                }))
                .SetAggregateRowsDefinition(
                    new AggregateRowDefinition(dr => dr.RowBuildContext.GetColumnDataByRefKey("大类"), new AggregateColumnDefinition[] {
                        new TextAggregateColumnDefinition(0,aggregateContext=>aggregateContext.AggregateKey),
                        new TextAggregateColumnDefinition(1,aggregateContext=>"sum-"+aggregateContext.AggregateKey+":"),
                        new FormulaAggregateColumnDefinition(3,new SumRefKeysDecimalFormula(value=>(decimal)value,"值")),
                        //new TextAggregateColumnDefinition("占比","大类占比")
                    })
                )
                .SetMergeCellsCollectors(new SameValueMergeCellsCollector(0, 1), new SameValueMergeCellsCollector(1, 3, RowTypes.AggregateRow, true))
                .CreateBuilder();
            var tableBody = builder.Build(GetDatas());
        }

        private static IEnumerable<Data> GetDatas()
        {
            yield return new Data
            {
                大类 = "A",
                小类1 = "AA1",
                小类2 = "AA2",
                值 = 3,
                大类占比 = 0.5M
            };
            yield return new Data
            {
                大类 = "A",
                小类1 = "AB1",
                小类2 = "AB2",
                值 = 4,
                大类占比 = 0.5M
            };
            yield return new Data
            {
                大类 = "A",
                小类1 = "AC1",
                小类2 = "AC2",
                值 = 3,
                大类占比 = 0.5M
            };
            yield return new Data
            {
                大类 = "B",
                小类1 = "BA1",
                小类2 = "BA2",
                值 = 2,
                大类占比 = 0.5M
            };
            yield return new Data
            {
                大类 = "B",
                小类1 = "BB1",
                小类2 = "BB2",
                值 = 1,
                大类占比 = 0.5M
            };
            yield return new Data
            {
                大类 = "B",
                小类1 = "BC1",
                小类2 = "BC2",
                值 = 7,
                大类占比 = 0.5M
            };
        }
    }

    public class SameValueMergeCellsCollector : IMergeCellsCollector
    {
        private int _maxColNum;
        private int _minColNum;
        private RowTypes _rowTypes;
        private bool _sameAggregateKey;
        private bool _treatNullEqualsAny;

        /// <summary>
        /// [<paramref name="minColNum"/>, <paramref name="maxColNum"/>)
        /// </summary>
        /// <param name="minColNum"></param>
        /// <param name="maxColNum"></param>
        public SameValueMergeCellsCollector(int minColNum = 0, int maxColNum = int.MaxValue, RowTypes rowTypes = RowTypes.Any, bool treatNullEqualsAny = false, bool sameAggregateKey = true)
        {
            _maxColNum = maxColNum;
            _minColNum = minColNum;
            _rowTypes = rowTypes;
            _treatNullEqualsAny = treatNullEqualsAny;
            _sameAggregateKey = sameAggregateKey;
        }

        public IEnumerable<Tuple<CellReference, CellReference>> Collect(IList<Row> rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                if (!_rowTypes.HasFlag(row.RowType))
                {
                    continue;
                }
                var startColNum = Math.Max(0, _minColNum);
                var endColNum = Math.Min(row.Cells.Count, _maxColNum);
                for (int j = startColNum; j < endColNum; j++)
                {
                    var cell = row.Cells[j];
                    var rightCellColNum = j + 1;
                    var belowCellRowNum = i + 1;
                    if (rightCellColNum < endColNum)
                    {
                        var rightCell = row.Cells[rightCellColNum];
                        if (cell.Value == null || rightCell.Value == null) //TODO: test
                        {
                            if (_treatNullEqualsAny)
                            {
                                yield return Tuple.Create(CellReference.Create(i, j), CellReference.Create(i, rightCellColNum));
                                continue;
                            }
                            else
                            {
                                if (cell.Value == null && rightCell.Value == null)
                                {
                                    yield return Tuple.Create(CellReference.Create(i, j), CellReference.Create(i, rightCellColNum));
                                }
                                continue;
                            }
                        }
                        if (cell.Value.Equals(rightCell.Value))
                        {
                            yield return Tuple.Create(CellReference.Create(i, j), CellReference.Create(i, rightCellColNum));
                        }
                    }
                    if (belowCellRowNum < rows.Count)
                    {
                        var belowRow = rows[belowCellRowNum];
                        if (!_rowTypes.HasFlag(belowRow.RowType))
                        {
                            continue;
                        }
                        if (belowRow.RowType == RowTypes.AggregateRow && row.RowType == RowTypes.AggregateRow)
                        {
                            if (_sameAggregateKey)
                            {
                                if (((AggregateRow)belowRow).RowBuildContext.AggregateKey != ((AggregateRow)row).RowBuildContext.AggregateKey)
                                {
                                    continue;
                                }
                            }
                        }
                        var belowCell = belowRow.Cells[j];
                        if (cell.Value == null || belowCell.Value == null)
                        {
                            if (_treatNullEqualsAny)
                            {
                                yield return Tuple.Create(CellReference.Create(i, j), CellReference.Create(belowCellRowNum, j));
                                continue;
                            }
                            else
                            {
                                if (cell.Value == null && belowCell.Value == null)
                                {
                                    yield return Tuple.Create(CellReference.Create(i, j), CellReference.Create(belowCellRowNum, j));
                                }
                                continue;
                            }
                        }
                        if (cell.Value.Equals(belowCell.Value))
                        {
                            yield return Tuple.Create(CellReference.Create(i, j), CellReference.Create(belowCellRowNum, j));
                        }
                    }
                }
            }
        }
    }

    public class SimpleObjectPropertyPicker<TType> : IRowDataPicker
    {
        private static IEnumerable<System.Reflection.PropertyInfo> _properties;
        private Action<RowDataPickedEventArgs> _onRowDataPicked;

        static SimpleObjectPropertyPicker()
        {
            _properties = typeof(TType).GetProperties();
        }

        public SimpleObjectPropertyPicker()
        {
        }

        public SimpleObjectPropertyPicker(Action<RowDataPickedEventArgs> onRowDataPicked)
        {
            _onRowDataPicked = onRowDataPicked;
        }

        public IEnumerable<IDictionary<string, dynamic>> PickRowDatas(dynamic rawDatas)
        {
            var objs = (IEnumerable<TType>)rawDatas;
            var ctx = new Dictionary<string, dynamic>();
            foreach (var obj in objs)
            {
                var rowData = new Dictionary<string, dynamic>();
                foreach (var property in _properties)
                {
                    var value = property.GetGetMethod().Invoke(obj, null);
                    rowData.Add(property.Name, value);
                }
                _onRowDataPicked?.Invoke(new RowDataPickedEventArgs
                {
                    RawObjects = objs,
                    CurrentRawObject = obj,
                    RowData = rowData,
                    RowDataPickContext = ctx
                });
                yield return rowData;
            }
        }

        public class RowDataPickedEventArgs : EventArgs
        {
            public IEnumerable<TType> RawObjects { get; set; }
            public TType CurrentRawObject { get; set; }
            public Dictionary<string, dynamic> RowData { get; set; }
            public Dictionary<string, dynamic> RowDataPickContext { get; set; }
        }
    }

    public class Data
    {
        public string 大类 { get; set; }
        public string 小类1 { get; set; }
        public string 小类2 { get; set; }
        public int 值 { get; set; }
        public decimal 大类占比 { get; set; }
    }
}