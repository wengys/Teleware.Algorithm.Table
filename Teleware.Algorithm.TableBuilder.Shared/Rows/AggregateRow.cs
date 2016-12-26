using System.Collections.Generic;
using Teleware.Algorithm.TableBuilder.BuildContext;

namespace Teleware.Algorithm.TableBuilder.Rows
{
    /// <summary>
    /// 聚合行
    /// </summary>
    /// <remarks>
    /// 聚合行指根据特定数据行聚合计算所产生的行
    /// </remarks>
    public class AggregateRow : Row
    {
        /// <summary>
        /// 初始化聚合行
        /// </summary>
        /// <param name="cells">此行包括的所有单元格</param>
        /// <param name="rowBuildContext">行创建上下文</param>
        public AggregateRow(IList<Cell> cells, AggregateRowBuildContext rowBuildContext) : base(cells)
        {
            RowBuildContext = rowBuildContext;
        }

        /// <summary>
        /// 行创建上下文
        /// </summary>
        public AggregateRowBuildContext RowBuildContext { get; set; }
    }
}