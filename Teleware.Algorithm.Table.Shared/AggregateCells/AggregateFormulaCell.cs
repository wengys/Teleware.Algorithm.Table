﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;

namespace Teleware.Algorithm.TableBuilder.Shared.AggregateCells
{
    public class AggregateFormulaCell : Cell
    {
        public AggregateFormulaCell(IFormula formula) : base()
        {
            this.Formula = formula;
        }

        public IFormula Formula { get; }

        public void ExecuteFormula(IEnumerable<Rows.DataRow> dataRows)
        {
            Value = Formula.Execute(dataRows);
        }
    }
}