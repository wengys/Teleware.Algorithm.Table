using System.Collections.Generic;
using Teleware.Algorithm.TableBuilder.AggregateColumnDefinitions;
using Teleware.Algorithm.TableBuilder.DataColumnDefinitions;

namespace Teleware.Algorithm.TableBuilder.Cells
{
    /// <summary>
    /// 公式单元格
    /// </summary>
    /// <seealso cref="FormulaColumnDefinition"/>
    /// <seealso cref="FormulaAggregateColumnDefinition"/>
    public class FormulaCell : Cell
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="formula">公式</param>
        public FormulaCell(IFormula formula) : base()
        {
            this.Formula = formula;
        }

        /// <summary>
        /// 公式
        /// </summary>
        public IFormula Formula { get; }

        /// <summary>
        /// 执行公式并将结果赋值给<see cref="Cell.Value"/>
        /// </summary>
        /// <param name="dataRows">此公式相关数据行</param>
        public void ExecuteFormula(IEnumerable<Rows.DataRow> dataRows)
        {
            Value = Formula.Execute(dataRows);
        }
    }
}