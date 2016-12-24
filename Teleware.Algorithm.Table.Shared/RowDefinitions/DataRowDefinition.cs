using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;
using Teleware.Algorithm.TableBuilder.Shared.Rows;

namespace Teleware.Algorithm.TableBuilder.Shared.RowDefinitions
{
    public class DataRowDefinition
    {
        private static Func<DataRow, DataRowBuildContext, DataRow> _id = (r, ctx) => r;

        public DataRowDefinition(IEnumerable<DataColumnDefinition> columns, Func<DataRow, DataRowBuildContext, DataRow> rowDecorator)
        {
            Columns = columns;
            RowDecorator = rowDecorator ?? _id;
        }

        public DataRowDefinition(IEnumerable<DataColumnDefinition> columns) : this(columns, null)
        {
            Columns = columns;
        }

        public IEnumerable<DataColumnDefinition> Columns { get; }
        public Func<DataRow, DataRowBuildContext, DataRow> RowDecorator { get; }

        //public DataRow DecoreteRow(DataRow row)
        //{
        //    return RowDecorator(row);
        //}
    }
}