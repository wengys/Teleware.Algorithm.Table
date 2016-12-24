using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared;

namespace Teleware.Algorithm.TableBodyBuilder
{
    public interface IMergeCellsCollector
    {
        IEnumerable<Tuple<CellReference, CellReference>> Collect(IList<Row> rows);
    }
}