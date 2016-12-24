using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teleware.Algorithm.TableBuilder.Shared
{
    public abstract class Cell
    {
        public Cell()
        {
            Metadata = new ExpandoObject();
        }

        public dynamic Metadata { get; }
        public dynamic Value { get; protected set; }

        public override string ToString()
        {
            return Value?.ToString() ?? "";
        }
    }
}