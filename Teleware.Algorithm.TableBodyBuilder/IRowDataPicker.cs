using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder;

namespace Teleware.Algorithm.TableBodyBuilder
{
    /// <summary>
    /// 行数据分拣器
    /// </summary>
    /// <remarks>
    /// 表格中的数据将通过此分拣器分为不同行中不同键表示的数据。
    /// 分拣结果可以看成一个数据库表，每行均为不同列组成的元祖。
    /// </remarks>
    public interface IRowDataPicker
    {
        /// <summary>
        /// 分拣数据
        /// </summary>
        /// <param name="rawDatas">用于生成表格的原始数据</param>
        /// <returns>按行列组织的数据</returns>
        IEnumerable<IDictionary<string, dynamic>> PickRowDatas(dynamic rawDatas);
    }
}