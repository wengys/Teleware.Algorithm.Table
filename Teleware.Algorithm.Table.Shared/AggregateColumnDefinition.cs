using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;

namespace Teleware.Algorithm.TableBuilder.Shared
{
    public abstract class AggregateColumnDefinition
    {
        private static Func<Cell, AggregateRowBuildContext, Cell> _id = (c, ctx) => c;

        public AggregateColumnDefinition(int colNum, Func<Cell, AggregateRowBuildContext, Cell> cellDecorator)
        {
            Metadata = new ExpandoObject();
            ColNum = colNum;
            CellDecorator = cellDecorator ?? _id;
        }

        public int ColNum { get; }

        public dynamic Metadata { get; }

        public Func<Cell, AggregateRowBuildContext, Cell> CellDecorator { get; }

        protected abstract Cell BuildCell(AggregateRowBuildContext context);

        public Cell CreateCell(AggregateRowBuildContext context)
        {
            var cell = BuildCell(context);
            context.RegisterCell(cell, this);
            return cell;
        }
    }
}