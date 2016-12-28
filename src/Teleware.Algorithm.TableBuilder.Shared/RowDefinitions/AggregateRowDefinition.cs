﻿using System;
using System.Collections.Generic;
using Teleware.Algorithm.TableBuilder.BuildContext;
using Teleware.Algorithm.TableBuilder.Cells;
using Teleware.Algorithm.TableBuilder.Rows;

namespace Teleware.Algorithm.TableBuilder.RowDefinitions
{
    /// <summary>
    /// 聚合行定义
    /// </summary>
    /// <remarks>
    /// 聚合行指根据特定数据行聚合计算所产生的行，比如：合计行、总计行
    /// </remarks>
    /// <see cref="AggregateRow"/>
    public class AggregateRowDefinition
    {
        private static readonly Func<AggregateRow, AggregateRowBuildContext, AggregateRow> _id = (r, ctx) => r;

        /// <summary>
        /// 初始化新聚合行定义
        /// </summary>
        /// <param name="aggregateRowDefinitionName">聚合列定义名，聚合列列表内唯一</param>
        /// <param name="aggregateKeySelector">聚合键生成器, 参见<see cref="AggregateKeySelector"/></param>
        /// <param name="columns">聚合行中的聚合列定义</param>
        /// <param name="rowDecorator">行装饰器</param>
        public AggregateRowDefinition(string aggregateRowDefinitionName, Func<DataRow, string> aggregateKeySelector, IEnumerable<AggregateColumnDefinition> columns, Func<AggregateRow, AggregateRowBuildContext, AggregateRow> rowDecorator)
        {
            Name = aggregateRowDefinitionName;
            AggregateKeySelector = aggregateKeySelector;
            Columns = columns;
            RowDecorator = rowDecorator ?? _id;
        }

        /// <summary>
        /// 初始化新聚合行定义
        /// </summary>
        /// <param name="aggregateRowDefinitionName">聚合列定义名，聚合列列表内唯一</param>
        /// <param name="aggregateKeySelector">聚合键生成器, 参见<see cref="AggregateKeySelector"/></param>
        /// <param name="columns">聚合行中的聚合列定义</param>
        public AggregateRowDefinition(string aggregateRowDefinitionName, Func<DataRow, string> aggregateKeySelector, IEnumerable<AggregateColumnDefinition> columns)
            : this(aggregateRowDefinitionName, aggregateKeySelector, columns, null)
        {
        }

        /// <summary>
        /// 聚合列定义名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 聚合键生成器
        /// </summary>
        /// <remarks>
        /// 对于每个数据行，将生成一个聚合键。聚合键相同的连续数据行将被视为一组，每组将产生对应的聚合行。
        /// </remarks>
        public Func<DataRow, string> AggregateKeySelector { get; }

        /// <summary>
        /// 聚合列定义
        /// </summary>
        /// <remarks>
        /// 对于没有定义的列，将自动填充<see cref="EmptyCell"/>
        /// </remarks>
        public IEnumerable<AggregateColumnDefinition> Columns { get; }

        /// <summary>
        /// 行装饰器
        /// </summary>
        public Func<AggregateRow, AggregateRowBuildContext, AggregateRow> RowDecorator { get; }
    }
}