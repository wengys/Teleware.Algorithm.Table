using System.Collections.Generic;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;

namespace Teleware.Algorithm.TableBuilder.Shared.Rows
{
    /// <summary>
    /// 数据行
    /// </summary>
    /// <remarks>
    /// 数据行指由表格中的数据直接组成的行
    /// </remarks>
    public class DataRow : Row
    {
        /// <summary>
        /// 初始化数据行
        /// </summary>
        /// <param name="cells">此行包括的所有单元格</param>
        /// <param name="rowBuildContext">行创建上下文</param>
        public DataRow(IList<Cell> cells, DataRowBuildContext rowBuildContext) : base(cells)
        {
            RowBuildContext = rowBuildContext;
        }

        /// <summary>
        /// 行创建上下文
        /// </summary>
        public DataRowBuildContext RowBuildContext { get; }
    }
}