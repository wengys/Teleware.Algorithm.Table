using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Shared.Rows;

namespace Teleware.Algorithm.TableBuilder.Shared.BuildContext
{
    public class AggregateRowBuildContext
    {
        private Dictionary<Cell, AggregateColumnDefinition> _cellDefMapping = new Dictionary<Cell, AggregateColumnDefinition>();

        public IEnumerable<DataRow> RowsToAggregate { get; }
        public string AggregateKey { get; }
        public AggregateRowDefinition Definition { get; }

        public AggregateRowBuildContext(AggregateRowDefinition aggregateRowDefinition, string aggregateKey, IEnumerable<DataRow> rowsToAggregate)
        {
            Definition = aggregateRowDefinition;
            RowsToAggregate = rowsToAggregate;
            AggregateKey = aggregateKey;
        }

        public void RegisterCell(Cell cell, AggregateColumnDefinition column)
        {
            _cellDefMapping.Add(cell, column);
        }

        public Cell DecorateCell(Cell cell)
        {
            AggregateColumnDefinition colDef;
            if (_cellDefMapping.TryGetValue(cell, out colDef))
            {
                cell = colDef.CellDecorator(cell, this);
            }
            return cell;
        }

        public AggregateRow DecorateRow(AggregateRow row)
        {
            return Definition.RowDecorator(row, this);
        }
    }
}