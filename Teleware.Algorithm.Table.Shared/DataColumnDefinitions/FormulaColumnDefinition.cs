using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;
using Teleware.Algorithm.TableBuilder.Shared.Cells;

namespace Teleware.Algorithm.TableBuilder.Shared.DataColumnDefinitions
{
    public class FormulaColumnDefinition : DataColumnDefinition
    {
        private string _columnText;

        public FormulaColumnDefinition(
            string columnText,
            IFormula formula) : this(columnText, formula, null)
        {
        }

        public FormulaColumnDefinition(
            string columnText,
            IFormula formula,
            Func<Cell, DataRowBuildContext, Cell> cellDecorator) : base(cellDecorator)
        {
            _columnText = columnText;
            Formula = formula;
        }

        public IFormula Formula { get; set; }

        public override string ColumnText
        {
            get
            {
                return _columnText;
            }
        }

        protected override Cell BuildCell(DataRowBuildContext context)
        {
            return new FormulaCell(this.Formula);
        }
    }
}