using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder;

namespace Teleware.Algorithm.TableBuilder.TableBodyBuilder
{
    /// <summary>
    /// 合并列采集器
    /// </summary>
    public interface IMergeCellsCollector
    {
        /// <summary>
        /// 采集合并列
        /// </summary>
        /// <param name="rows">行</param>
        /// <returns>两两一组需要合并的列</returns>
        IEnumerable<Tuple<CellReference, CellReference>> Collect(IList<Row> rows);
    }
}