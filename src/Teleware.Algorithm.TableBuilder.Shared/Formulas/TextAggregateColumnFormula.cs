using System;
using System.Collections.Generic;
using Teleware.Algorithm.TableBuilder.BuildContext;
using Teleware.Algorithm.TableBuilder.Rows;

namespace Teleware.Algorithm.TableBuilder.Formulas
{
    /// <summary>
    /// 文本聚合列公式
    /// </summary>
    /// <remarks>
    /// 由于行序号只有到公式计算阶段才能确定，故而聚合列文本采用公式计算
    /// </remarks>
    internal class TextAggregateColumnFormula : IFormula
    {
        private readonly AggregateRowBuildContext _context;
        private readonly Func<AggregateRowBuildContext, string> _textGetter;

        public TextAggregateColumnFormula(Func<AggregateRowBuildContext, string> textGetter, AggregateRowBuildContext context)
        {
            _textGetter = textGetter;
            _context = context;
        }

        public dynamic Execute(IEnumerable<DataRow> rows)
        {
            return _textGetter.Invoke(_context);
        }
    }
}