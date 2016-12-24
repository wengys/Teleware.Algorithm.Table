using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teleware.Algorithm.TableBuilder.Shared
{
    public struct CellReference
    {
        public CellReference(int rowNum, int colNum)
        {
            this.ColNum = colNum;
            this.RowNum = rowNum;
        }

        public int ColNum { get; set; }
        public int RowNum { get; set; }

        public override string ToString()
        {
            return $"({RowNum}, {ColNum})";
        }
    }
}