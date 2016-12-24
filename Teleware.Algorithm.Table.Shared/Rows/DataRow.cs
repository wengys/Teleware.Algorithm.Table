using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;
using Teleware.Algorithm.TableBuilder.Shared.RowDefinitions;

namespace Teleware.Algorithm.TableBuilder.Shared.Rows
{
    public class DataRow : Row
    {
        public DataRow(IList<Cell> cells, DataRowBuildContext rowBuildContext) : base(cells)
        {
            RowBuildContext = rowBuildContext;
        }

        public DataRowBuildContext RowBuildContext { get; }
    }
}