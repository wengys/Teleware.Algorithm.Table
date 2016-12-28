using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Rows;
using Teleware.Algorithm.TableBuilder.TableBodyBuilder;

namespace Teleware.Algorithm.TableBuilder.PowerTools.MergeCellsCollectors
{
    public enum AcceptRowTypes
    {
        AggregateRow = 1,
        DataRow = 2,
        Any = AggregateRow | DataRow
    }

    public class SameValueMergeCellsCollector : IMergeCellsCollector
    {
        private readonly int _maxColNum;
        private readonly int _minColNum;
        private readonly bool _sameAggregateKey;
        private readonly bool _treatNullEqualsAny;
        private readonly Func<Row, bool> _isRowAccept;

        /// <summary>
        /// [<paramref name="minColNum"/>, <paramref name="maxColNum"/>)
        /// </summary>
        /// <param name="minColNum"></param>
        /// <param name="maxColNum"></param>
        public SameValueMergeCellsCollector(int minColNum = 0, int maxColNum = int.MaxValue, AcceptRowTypes rowTypes = AcceptRowTypes.Any, bool treatNullEqualsAny = false, bool sameAggregateKey = true)
        {
            _maxColNum = maxColNum;
            _minColNum = minColNum;
            _treatNullEqualsAny = treatNullEqualsAny;
            _sameAggregateKey = sameAggregateKey;
            switch (rowTypes)
            {
                case AcceptRowTypes.DataRow:
                    _isRowAccept = (r) => r is DataRow;
                    break;

                case AcceptRowTypes.AggregateRow:
                    _isRowAccept = (r) => r is AggregateRow;
                    break;

                case AcceptRowTypes.Any:
                    _isRowAccept = (r) => true;
                    break;
            }
        }

        public IEnumerable<Tuple<CellReference, CellReference>> Collect(IList<Row> rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                if (!_isRowAccept(row))
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
                        if (!_isRowAccept(belowRow))
                        {
                            continue;
                        }
                        var belowAggregateRow = belowRow as AggregateRow;
                        var curAggregateRow = row as AggregateRow;
                        if (belowAggregateRow != null && curAggregateRow != null)
                        {
                            if (_sameAggregateKey)
                            {
                                if (belowAggregateRow.RowBuildContext.AggregateKey != curAggregateRow.RowBuildContext.AggregateKey)
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
}