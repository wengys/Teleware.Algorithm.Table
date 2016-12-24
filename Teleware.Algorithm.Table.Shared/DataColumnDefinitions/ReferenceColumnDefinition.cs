using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;
using Teleware.Algorithm.TableBuilder.Shared.Cells;

namespace Teleware.Algorithm.TableBuilder.Shared.DataColumnDefinitions
{
    public class ReferenceColumnDefinition : DataColumnDefinition
    {
        protected static Func<dynamic, dynamic> _id = a => a;

        private string _columnText;
        private Func<dynamic, dynamic> _valueGetter;

        public ReferenceColumnDefinition(
            string columnText,
            string refKey
            ) : this(columnText, refKey, null, null)
        {
        }

        public ReferenceColumnDefinition(
            string columnText,
            string refKey,
            Func<dynamic, dynamic> valueGetter
            ) : this(columnText, refKey, valueGetter, null)
        {
        }

        public ReferenceColumnDefinition(
            string columnText,
            string refKey,
            Func<Cell, DataRowBuildContext, Cell> cellDecorator
            ) : this(columnText, refKey, null, cellDecorator)
        {
        }

        public ReferenceColumnDefinition(
            string columnText,
            string refKey,
            Func<dynamic, dynamic> valueGetter,
            Func<Cell, DataRowBuildContext, Cell> cellDecorator
            ) : base(cellDecorator)
        {
            _columnText = columnText;
            RefKey = refKey;
            _valueGetter = valueGetter ?? _id;
        }

        public string RefKey { get; }

        public override string ColumnText
        {
            get
            {
                return _columnText;
            }
        }

        protected override Cell BuildCell(DataRowBuildContext context)
        {
            var data = context.GetColumnDataByRefKey(RefKey);
            if (data == null)
            {
                return EmptyCell.Singleton;
            }
            var column = new ReferenceCell(_valueGetter(data), data);
            return column;
        }
    }
}