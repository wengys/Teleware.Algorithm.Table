using System;
using Teleware.Algorithm.TableBuilder.BuildContext;
using Teleware.Algorithm.TableBuilder.Cells;

namespace Teleware.Algorithm.TableBuilder.AggregateColumnDefinitions
{
    /// <summary>
    /// 文本聚合列定义
    /// </summary>
    /// <remarks>
    /// 文本聚合列指聚合列中的文本列，如总计行中显示“总计：”的单元格
    /// </remarks>
    public class TextAggregateColumnDefinition : AggregateColumnDefinition
    {
        private readonly Func<AggregateRowBuildContext, string> _textGetter;

        /// <summary>
        /// 初始化新文本聚合列
        /// </summary>
        /// <param name="colNum">列坐标（从0开始）</param>
        /// <param name="textGetter">列文本生成方法</param>
        public TextAggregateColumnDefinition(int colNum, Func<AggregateRowBuildContext, string> textGetter) : this(colNum, textGetter, null)
        {
        }

        /// <summary>
        /// 初始化新文本聚合列
        /// </summary>
        /// <param name="colNum">列坐标（从0开始）</param>
        /// <param name="text">列静态文本</param>
        public TextAggregateColumnDefinition(int colNum, string text) : this(colNum, text, null)
        {
        }

        /// <summary>
        /// 初始化新文本聚合列
        /// </summary>
        /// <param name="colNum">列坐标（从0开始）</param>
        /// <param name="textGetter">列文本生成方法</param>
        /// <param name="cellDecorator">单元格装饰器</param>
        public TextAggregateColumnDefinition(int colNum, Func<AggregateRowBuildContext, string> textGetter, Func<Cell, AggregateRowBuildContext, Cell> cellDecorator) : base(colNum, cellDecorator)
        {
            this._textGetter = textGetter;
        }

        /// <summary>
        /// 初始化新文本聚合列
        /// </summary>
        /// <param name="colNum">列坐标（从0开始）</param>
        /// <param name="text">列静态文本</param>
        /// <param name="cellDecorator">单元格装饰器</param>
        public TextAggregateColumnDefinition(int colNum, string text, Func<Cell, AggregateRowBuildContext, Cell> cellDecorator) : base(colNum, cellDecorator)
        {
            this._textGetter = (ctx) => text;
        }

        /// <see cref="AggregateColumnDefinition.BuildCell(AggregateRowBuildContext)"/>
        protected override Cell BuildCell(AggregateRowBuildContext context)
        {
            return new StaticTextCell(_textGetter(context));
        }
    }
}