using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;
using Teleware.Algorithm.TableBuilder.Shared.Cells;

namespace Teleware.Algorithm.TableBuilder.Shared.AggregateColumnDefinitions
{
    public class TextAggregateColumnDefinition : AggregateColumnDefinition
    {
        private readonly Func<AggregateRowBuildContext, string> _textGetter;

        public TextAggregateColumnDefinition(int colNum, Func<AggregateRowBuildContext, string> textGetter) : this(colNum, textGetter, null)
        {
        }

        public TextAggregateColumnDefinition(int colNum, string text) : this(colNum, text, null)
        {
        }

        public TextAggregateColumnDefinition(int colNum, Func<AggregateRowBuildContext, string> textGetter, Func<Cell, AggregateRowBuildContext, Cell> cellDecorator) : base(colNum, cellDecorator)
        {
            this._textGetter = textGetter;
        }

        public TextAggregateColumnDefinition(int colNum, string text, Func<Cell, AggregateRowBuildContext, Cell> cellDecorator) : base(colNum, cellDecorator)
        {
            this._textGetter = (ctx) => text;
        }

        protected override Cell BuildCell(AggregateRowBuildContext context)
        {
            return new StaticTextCell(_textGetter(context));
        }
    }
}