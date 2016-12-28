using System.Collections.Generic;
using System.Dynamic;

namespace Teleware.Algorithm.TableBuilder
{
    /// <summary>
    /// 行
    /// </summary>
    public abstract class Row
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="cells">此行相关的单元格</param>
        protected Row(IList<Cell> cells)
        {
            Cells = cells;
            Metadata = new ExpandoObject();
        }

        /// <summary>
        /// 此行相关的单元格
        /// </summary>
        public IList<Cell> Cells { get; set; }

        /// <summary>
        /// 扩展元数据
        /// </summary>
        public dynamic Metadata { get; }
    }
}