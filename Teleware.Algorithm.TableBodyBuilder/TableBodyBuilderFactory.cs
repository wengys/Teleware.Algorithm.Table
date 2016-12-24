using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBodyBuilder.Impl;
using Teleware.Algorithm.TableBuilder.Shared;
using Teleware.Algorithm.TableBuilder.Shared.RowDefinitions;

namespace Teleware.Algorithm.TableBodyBuilder
{
    public class TableBodyBuilderConfiguration
    {
        private IEnumerable<AggregateRowDefinition> _aggregateRows;
        private IRowDataPicker _collector;
        private DataRowDefinition _dataRow;
        private IMergeCellsCollector _mergeCellsCollector;

        public static TableBodyBuilderConfiguration Instance
        {
            get
            {
                return new TableBodyBuilderConfiguration();
            }
        }

        public ITableBodyBuilder CreateBuilder()
        {
            return new DefaultTableBodyBuilder(
                this._collector,
                this._dataRow,
                this._aggregateRows,
                this._mergeCellsCollector
            );
        }

        public TableBodyBuilderConfiguration SetAggregateRows(params AggregateRowDefinition[] aggregateRows)
        {
            this._aggregateRows = aggregateRows;
            return this;
        }

        public TableBodyBuilderConfiguration SetAggregateRows(IEnumerable<AggregateRowDefinition> aggregateRows)
        {
            this._aggregateRows = aggregateRows;
            return this;
        }

        public TableBodyBuilderConfiguration SetDataRow(DataRowDefinition dataRow)
        {
            this._dataRow = dataRow;
            return this;
        }

        public TableBodyBuilderConfiguration SetMergeCellsCollector(IMergeCellsCollector mergeCellsCollector)
        {
            this._mergeCellsCollector = mergeCellsCollector;
            return this;
        }

        public TableBodyBuilderConfiguration SetRowDataPicker(IRowDataPicker picker)
        {
            this._collector = picker;
            return this;
        }
    }
}