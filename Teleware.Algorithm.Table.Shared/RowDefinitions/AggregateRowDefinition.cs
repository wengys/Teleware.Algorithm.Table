using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;
using Teleware.Algorithm.TableBuilder.Shared.Rows;

namespace Teleware.Algorithm.TableBuilder.Shared.RowDefinitions
{
    public class AggregateRowDefinition
    {
        private static Func<AggregateRow, AggregateRowBuildContext, AggregateRow> _id = (r, ctx) => r;

        public AggregateRowDefinition(Func<DataRow, string> aggregateKeySelector, IEnumerable<AggregateColumnDefinition> columns, Func<AggregateRow, AggregateRowBuildContext, AggregateRow> rowDecorator)
        {
            AggregateKeySelector = aggregateKeySelector;
            Columns = columns;
            RowDecorator = rowDecorator ?? _id;
        }

        public AggregateRowDefinition(Func<DataRow, string> aggregateKeySelector, IEnumerable<AggregateColumnDefinition> columns)
            : this(aggregateKeySelector, columns, null)
        {
        }

        public Func<DataRow, string> AggregateKeySelector { get; }
        public IEnumerable<AggregateColumnDefinition> Columns { get; }
        public Func<AggregateRow, AggregateRowBuildContext, AggregateRow> RowDecorator { get; }

        //public AggregateRow DecoreteRow(AggregateRow row)
        //{
        //    return RowDecorator(row);
        //}
    }
}