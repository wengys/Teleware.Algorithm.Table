using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder;
using Teleware.Algorithm.TableBuilder.RowDefinitions;
using Teleware.Algorithm.TableBuilder.TableBodyBuilder.Impl;

namespace Teleware.Algorithm.TableBuilder.TableBodyBuilder
{
    /// <summary>
    /// 表身生成器配置
    /// </summary>
    public class TableBodyBuilderConfiguration
    {
        private IEnumerable<AggregateRowDefinition> _aggregateRows;
        private IRowDataPicker _collector;
        private DataRowDefinition _dataRow;
        private IMergeCellsCollector _mergeCellsCollector;

        /// <summary>
        /// 获取新配置实例
        /// </summary>
        public static TableBodyBuilderConfiguration Instance
        {
            get
            {
                return new TableBodyBuilderConfiguration();
            }
        }

        /// <summary>
        /// 创建生成器
        /// </summary>
        /// <returns></returns>
        public ITableBodyBuilder CreateBuilder()
        {
            return new DefaultTableBodyBuilder(
                this._collector,
                this._dataRow,
                this._aggregateRows,
                this._mergeCellsCollector
            );
        }

        /// <summary>
        /// 设置聚合行定义
        /// </summary>
        /// <param name="aggregateRows">聚合行定义</param>
        /// <returns></returns>
        public TableBodyBuilderConfiguration SetAggregateRowsDefinition(params AggregateRowDefinition[] aggregateRows)
        {
            this._aggregateRows = aggregateRows;
            return this;
        }

        /// <summary>
        /// 设置聚合行定义
        /// </summary>
        /// <param name="aggregateRows">聚合行定义</param>
        /// <returns></returns>
        public TableBodyBuilderConfiguration SetAggregateRowsDefinition(IEnumerable<AggregateRowDefinition> aggregateRows)
        {
            this._aggregateRows = aggregateRows;
            return this;
        }

        /// <summary>
        /// 设置数据行定义
        /// </summary>
        /// <param name="dataRow">数据行定义</param>
        /// <returns></returns>
        public TableBodyBuilderConfiguration SetDataRowDefinition(DataRowDefinition dataRow)
        {
            this._dataRow = dataRow;
            return this;
        }

        /// <summary>
        /// 设置合并列采集器
        /// </summary>
        /// <param name="mergeCellsCollector">合并列采集器实例</param>
        /// <returns></returns>
        public TableBodyBuilderConfiguration SetMergeCellsCollector(IMergeCellsCollector mergeCellsCollector)
        {
            this._mergeCellsCollector = mergeCellsCollector;
            return this;
        }

        /// <summary>
        /// 设置行数据分拣器
        /// </summary>
        /// <param name="picker">分拣器实例</param>
        /// <returns></returns>
        public TableBodyBuilderConfiguration SetRowDataPicker(IRowDataPicker picker)
        {
            this._collector = picker;
            return this;
        }
    }
}