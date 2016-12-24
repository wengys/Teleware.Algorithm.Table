using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teleware.Algorithm.TableBuilder.Shared.Cells
{
    public class StaticTextCell : Cell
    {
        public StaticTextCell(string text) : base()
        {
            Value = text;
        }
    }
}