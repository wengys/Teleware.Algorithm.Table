using System.Collections.Generic;
using Teleware.Algorithm.TableBuilder.Shared.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Shared.Rows;

namespace Teleware.Algorithm.TableBuilder.Shared.BuildContext
{
    /// <summary>
    /// 聚合行创建上下文
    /// </summary>
    public class AggregateRowBuildContext
    {
        private Dictionary<Cell, AggregateColumnDefinition> _cellDefMapping = new Dictionary<Cell, AggregateColumnDefinition>();

        /// <summary>
        /// 将要用于聚合计算的数据行
        /// </summary>
        public IEnumerable<DataRow> RowsToAggregate { get; }

        /// <summary>
        /// 聚合键
        /// </summary>
        public string AggregateKey { get; }

        /// <summary>
        /// 行定义
        /// </summary>
        public AggregateRowDefinition Definition { get; }

        /// <summary>
        /// 初始化上下文
        /// </summary>
        /// <param name="aggregateRowDefinition">行定义</param>
        /// <param name="aggregateKey">聚合键</param>
        /// <param name="rowsToAggregate">将要用于聚合计算的数据行</param>
        public AggregateRowBuildContext(AggregateRowDefinition aggregateRowDefinition, string aggregateKey, IEnumerable<DataRow> rowsToAggregate)
        {
            Definition = aggregateRowDefinition;
            RowsToAggregate = rowsToAggregate;
            AggregateKey = aggregateKey;
        }

        /// <summary>
        /// 注册单元格
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="column">单元格对应的列定义</param>
        public void RegisterCell(Cell cell, AggregateColumnDefinition column)
        {
            _cellDefMapping.Add(cell, column);
        }

        /// <summary>
        /// 返回单元格对应列定义
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public AggregateColumnDefinition GetCellDefinition(Cell cell)
        {
            return _cellDefMapping[cell];
        }

        /// <summary>
        /// 使用单元格装饰器修饰单元格
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>修饰后的单元格</returns>
        public Cell DecorateCell(Cell cell)
        {
            AggregateColumnDefinition colDef;
            if (_cellDefMapping.TryGetValue(cell, out colDef))
            {
                cell = colDef.CellDecorator(cell, this);
            }
            return cell;
        }

        /// <summary>
        /// 使用行装饰器修饰行
        /// </summary>
        /// <param name="row">行</param>
        /// <returns>修饰后的行</returns>
        public AggregateRow DecorateRow(AggregateRow row)
        {
            return Definition.RowDecorator(row, this);
        }
    }
}