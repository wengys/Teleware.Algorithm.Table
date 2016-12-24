using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teleware.Algorithm.TableBuilder.Shared.Cells
{
    public class EmptyCell : Cell
    {
        private EmptyCell() : base()
        {
        }

        public static EmptyCell Singleton { get; } = new EmptyCell();
    }
}