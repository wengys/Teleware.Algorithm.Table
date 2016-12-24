using System;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;
using Teleware.Algorithm.TableBuilder.Shared.Cells;

namespace Teleware.Algorithm.TableBuilder.Shared.AggregateColumnDefinitions
{
    /// <summary>
    /// 公式聚合列定义
    /// </summary>
    /// <remarks>
    /// 公式聚合列指由多个列聚合计算而产生的列，如总计行中合计其他列总和的列
    /// </remarks>
    public class FormulaAggregateColumnDefinition : AggregateColumnDefinition
    {
        /// <summary>
        /// 初始化新合计列
        /// </summary>
        /// <param name="colNum">列坐标（从0开始）</param>
        /// <param name="formula">公式</param>
        public FormulaAggregateColumnDefinition(int colNum, IFormula formula) : this(colNum, formula, null)
        {
        }

        /// <summary>
        /// 初始化新合计列
        /// </summary>
        /// <param name="colNum">列坐标（从0开始）</param>
        /// <param name="formula">公式</param>
        /// <param name="cellDecorator">单元格装饰器</param>
        public FormulaAggregateColumnDefinition(int colNum, IFormula formula, Func<Cell, AggregateRowBuildContext, Cell> cellDecorator) : base(colNum, cellDecorator)
        {
            Formula = formula;
        }

        /// <summary>
        /// 公式
        /// </summary>
        public IFormula Formula { get; set; }

        /// <see cref="AggregateColumnDefinition.BuildCell(AggregateRowBuildContext)"/>
        protected override Cell BuildCell(AggregateRowBuildContext context)
        {
            return new FormulaCell(this.Formula);
        }
    }
}