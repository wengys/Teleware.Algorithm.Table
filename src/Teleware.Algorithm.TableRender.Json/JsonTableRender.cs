using System.Collections.Generic;
using System.Linq;
using Teleware.Algorithm.TableBuilder;

namespace Teleware.Algorithm.TableRender.Json
{
    /// <summary>
    /// Json表格渲染器
    /// </summary>
    /// <remarks>
    /// 本渲染器用于将<see cref="Table"/>渲染为适合前端（主要是HTML页面）处理的<see cref="JsonTable"/>
    /// </remarks>
    public class JsonTableRender
    {
        /// <summary>
        /// 渲染表格
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public JsonTable Render(Table table)
        {
            var jsonTable = new JsonTable();
            jsonTable.Head = MapToJsonRow(table.TableHead);
            jsonTable.Body = MapToJsonRow(table.TableBody);

            MergeCells(jsonTable.Head, table.TableHead?.MergeCellGroups);
            MergeCells(jsonTable.Body, table.TableBody?.MergeCellGroups);

            return jsonTable;
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="contentRows"></param>
        /// <param name="mergeCellGroups"></param>
        private void MergeCells(IEnumerable<JsonRow> contentRows, IEnumerable<IEnumerable<CellReference>> mergeCellGroups)
        {
            if (mergeCellGroups == null)
            {
                return;
            }
            List<CellReference> cellsToRemove = new List<CellReference>();
            foreach (var mergeCellGroup in mergeCellGroups)
            {
                var masterCellRef = mergeCellGroup.First(); // 合并组的第一个单元格是左上角那个要保留单单元格
                var rowspan = mergeCellGroup.Max(t => t.RowNum) - masterCellRef.RowNum + 1;
                var colspan = mergeCellGroup.Max(t => t.ColNum) - masterCellRef.ColNum + 1;

                var materCell = ((List<JsonCell>)((List<JsonRow>)contentRows)[masterCellRef.RowNum].Cells)[masterCellRef.ColNum];
                materCell.ColSpan = colspan;
                materCell.RowSpan = rowspan;

                cellsToRemove.AddRange(mergeCellGroup.Skip(1));
            }

            cellsToRemove.Sort();

            for (int i = cellsToRemove.Count - 1; i >= 0; i--)
            {
                var cellReference = cellsToRemove[i];
                ((List<JsonCell>)((List<JsonRow>)contentRows)[cellReference.RowNum].Cells).RemoveAt(cellReference.ColNum);
            }
        }

        /// <summary>
        /// 表格映射
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        private static List<JsonRow> MapToJsonRow(ITableContentSection section)
        {
            return section?.Rows.Select(tr => new JsonRow
            {
                Metadata = tr.Metadata,
                Cells = tr.Cells.Select(td => new JsonCell
                {
                    ColSpan = 1,
                    RowSpan = 1,
                    Value = td.Value,
                    Metadata = td.Metadata
                }).ToList()
            }).ToList() ?? new List<JsonRow>();
        }
    }
}