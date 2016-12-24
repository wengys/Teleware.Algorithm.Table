using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared;

namespace Teleware.Algorithm.TableBodyBuilder
{
    /// <summary>
    /// 定义一个表身生成器
    /// </summary>
    /// <remarks>
    /// 表格生成分为以下步骤
    /// <list type="number">
    /// <item>将数据分拣为行数据</item>
    /// <item>创建数据行（数据行可包括公式列，不过此时不进行计算）</item>
    /// <item>根据数据行创建公式行</item>
    /// <item>合并数据行与公式行</item>
    /// <item>从左往右，从上到下计算单元格公式（公式单元格只依赖数据行，不考虑两个公式单元格相互依赖）</item>
    /// <item>从左往右，从上到下执行单元格/行修饰器</item>
    /// <item>收集合并单元格坐标</item>
    /// </list>
    /// </remarks>
    public interface ITableBodyBuilder
    {
        /// <summary>
        /// 创建表身
        /// </summary>
        /// <param name="rawDatas">用于创建表身的数据</param>
        /// <returns>表身</returns>
        TableBody Build(dynamic rawDatas);
    }
}