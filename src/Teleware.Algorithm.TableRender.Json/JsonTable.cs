using System.Collections.Generic;

namespace Teleware.Algorithm.TableRender.Json
{
    /// <summary>
    /// Json格式的表格
    /// </summary>
    public class JsonTable
    {
        /// <summary>
        /// 头
        /// </summary>
        public IEnumerable<JsonRow> Head { get; set; }

        /// <summary>
        /// 身
        /// </summary>
        public IEnumerable<JsonRow> Body { get; set; }
    }

    /// <summary>
    /// Json行
    /// </summary>
    public class JsonRow
    {
        /// <summary>
        /// Json单元格
        /// </summary>
        public IEnumerable<JsonCell> Cells { get; set; }

        /// <summary>
        /// 元数据
        /// </summary>
        public dynamic Metadata { get; set; }
    }

    /// <summary>
    /// Json单元格
    /// </summary>
    public class JsonCell
    {
        /// <summary>
        /// RowSpan
        /// </summary>
        public int RowSpan { get; set; }

        /// <summary>
        /// ColSpan
        /// </summary>
        public int ColSpan { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public dynamic Value { get; set; }

        /// <summary>
        /// 元数据
        /// </summary>
        public dynamic Metadata { get; set; }
    }
}