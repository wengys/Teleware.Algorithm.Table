using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teleware.Algorithm.TableBuilder.Shared.Cells
{
    public class ReferenceCell : Cell
    {
        public ReferenceCell(
            dynamic value,
            dynamic data
            )
        {
            Value = value;
            Data = data;
        }

        public dynamic Data { get; }
    }
}