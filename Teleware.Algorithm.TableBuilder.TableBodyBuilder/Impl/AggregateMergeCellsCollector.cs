using System;
using System.Collections.Generic;
using System.Linq;

namespace Teleware.Algorithm.TableBuilder.TableBodyBuilder.Impl
{
    internal class AggregateMergeCellsCollector : IMergeCellsCollector
    {
        private IMergeCellsCollector[] _mergeCellsCollector;

        public AggregateMergeCellsCollector(IMergeCellsCollector[] mergeCellsCollector)
        {
            this._mergeCellsCollector = mergeCellsCollector;
        }

        public IEnumerable<Tuple<CellReference, CellReference>> Collect(IList<Row> rows)
        {
            var mergeCellPairs = _mergeCellsCollector.SelectMany(collector => collector.Collect(rows)).Distinct();
            return mergeCellPairs;
        }
    }
}