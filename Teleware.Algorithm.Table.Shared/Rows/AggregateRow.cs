using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;

namespace Teleware.Algorithm.TableBuilder.Shared.Rows
{
    public class AggregateRow : Row
    {
        public AggregateRow(IList<Cell> cells, AggregateRowBuildContext rowBuildContext) : base(cells)
        {
            RowBuildContext = rowBuildContext;
        }

        public AggregateRowBuildContext RowBuildContext { get; set; }
    }
}