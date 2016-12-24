using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;
using Teleware.Algorithm.TableBuilder.Shared.Cells;

namespace Teleware.Algorithm.TableBuilder.Shared.AggregateColumnDefinitions
{
    public class FormulaAggregateColumnDefinition : AggregateColumnDefinition
    {
        public FormulaAggregateColumnDefinition(int colNum, IFormula formula) : this(colNum, formula, null)
        {
        }

        public FormulaAggregateColumnDefinition(int colNum, IFormula formula, Func<Cell, AggregateRowBuildContext, Cell> cellDecorator) : base(colNum, cellDecorator)
        {
            Formula = formula;
        }

        public IFormula Formula { get; set; }

        protected override Cell BuildCell(AggregateRowBuildContext context)
        {
            return new FormulaCell(this.Formula);
        }
    }
}