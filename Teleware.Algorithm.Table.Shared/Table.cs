using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teleware.Algorithm.TableBuilder.Shared
{
    /// <summary>
    /// 表格
    /// </summary>
    public class Table
    {
        /// <summary>
        /// 初始化表格
        /// </summary>
        /// <param name="tableHead">表头</param>
        /// <param name="tableBody">表身</param>
        public Table(TableHead tableHead, TableBody tableBody)
        {
            TableBody = tableBody;
            TableHead = tableHead;
        }

        /// <summary>
        /// 表身
        /// </summary>
        public TableBody TableBody { get; set; }

        /// <summary>
        /// 表头
        /// </summary>
        public TableHead TableHead { get; set; }
    }

    /// <summary>
    /// 表身
    /// </summary>
    public class TableBody
    {
        /// <summary>
        /// 初始化表身
        /// </summary>
        /// <param name="body">表身行</param>
        /// <param name="mergeCellGroups">待合并单元格组</param>
        public TableBody(IList<Row> body, IEnumerable<IEnumerable<CellReference>> mergeCellGroups)
        {
            Rows = body;
            MergeCellGroups = mergeCellGroups;
        }

        /// <summary>
        /// 表身行
        /// </summary>
        public IList<Row> Rows { get; set; }

        /// <summary>
        /// 待合并单元格组
        /// </summary>
        public IEnumerable<IEnumerable<CellReference>> MergeCellGroups { get; set; }
    }

    /// <summary>
    /// 表头
    /// </summary>
    public class TableHead
    {
        /// <summary>
        /// 初始化表头
        /// </summary>
        /// <param name="body">表头行</param>
        /// <param name="mergeCellGroups">待合并单元格组</param>
        public TableHead(IList<Row> body, IEnumerable<IEnumerable<CellReference>> mergeCellGroups)
        {
            Rows = body;
            MergeCellGroups = mergeCellGroups;
        }

        /// <summary>
        /// 表头行
        /// </summary>
        public IList<Row> Rows { get; set; }

        /// <summary>
        /// 待合并单元格组
        /// </summary>
        public IEnumerable<IEnumerable<CellReference>> MergeCellGroups { get; set; }
    }
}