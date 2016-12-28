using System.Collections.Generic;
using Teleware.Algorithm.TableBuilder.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Rows;

namespace Teleware.Algorithm.TableBuilder.BuildContext
{
    /// <summary>
    /// 聚合行创建上下文
    /// </summary>
    public class AggregateRowBuildContext
    {
        private readonly Dictionary<Cell, AggregateColumnDefinition> _cellDefMapping = new Dictionary<Cell, AggregateColumnDefinition>();

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
        /// <param name="aggregateRowIndex">聚合行号</param>
        public AggregateRowBuildContext(AggregateRowDefinition aggregateRowDefinition, string aggregateKey, IEnumerable<DataRow> rowsToAggregate, int aggregateRowIndex)
        {
            Definition = aggregateRowDefinition;
            RowsToAggregate = rowsToAggregate;
            AggregateKey = aggregateKey;
            AggregateRowIndex = aggregateRowIndex;
        }

        /// <summary>
        /// 聚合行号
        /// </summary>
        public int AggregateRowIndex { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public int RowIndex { get; set; }

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
        /// <returns>如果没有单元格注册记录，则返回null</returns>
        public AggregateColumnDefinition GetCellDefinition(Cell cell)
        {
            AggregateColumnDefinition colDef;
            if (_cellDefMapping.TryGetValue(cell, out colDef))
            {
                return colDef;
            }
            return null;
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