using System;
using Teleware.Algorithm.TableBuilder.BuildContext;
using Teleware.Algorithm.TableBuilder.Cells;

namespace Teleware.Algorithm.TableBuilder.DataColumnDefinitions
{
    /// <summary>
    /// 公式列定义
    /// </summary>
    /// <remarks>
    /// 公式列指由行中由其他列计算所得的列, 类似 SELETE A, B, A+B AS C FROM FOO 中的 C
    /// </remarks>
    public class FormulaColumnDefinition : DataColumnDefinition
    {
        /// <summary>
        /// 初始化公式列定义
        /// </summary>
        /// <param name="columnText">列头</param>
        /// <param name="formula">公式</param>
        public FormulaColumnDefinition(
            string columnText,
            IFormula formula) : this(columnText, formula, null)
        {
        }

        /// <summary>
        /// 初始化公式列定义
        /// </summary>
        /// <param name="columnText">列头</param>
        /// <param name="formula">公式</param>
        /// <param name="cellDecorator">列装饰器</param>
        public FormulaColumnDefinition(
            string columnText,
            IFormula formula,
            Func<Cell, DataRowBuildContext, Cell> cellDecorator) : base(cellDecorator)
        {
            ColumnText = columnText;
            Formula = formula;
        }

        /// <summary>
        /// 单元格值生成公式
        /// </summary>
        public IFormula Formula { get; set; }

        /// <summary>
        /// 列头
        /// </summary>
        public override string ColumnText { get; }

        /// <see cref="DataColumnDefinition.BuildCell(DataRowBuildContext)"/>
        protected override Cell BuildCell(DataRowBuildContext context)
        {
            return new FormulaCell(Formula);
        }
    }
}