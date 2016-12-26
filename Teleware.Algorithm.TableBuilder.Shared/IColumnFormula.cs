using System.Collections.Generic;
using Teleware.Algorithm.TableBuilder.Rows;

namespace Teleware.Algorithm.TableBuilder
{
    /// <summary>
    /// 表示一个公式
    /// </summary>
    public interface IFormula
    {
        /// <summary>
        /// 计算公式
        /// </summary>
        /// <param name="rows">此公式相关数据行</param>
        /// <returns>计算结果</returns>
        dynamic Execute(IEnumerable<DataRow> rows);
    }
}