﻿using System;
using System.Collections.Generic;
using Teleware.Algorithm.TableBuilder.BuildContext;
using Teleware.Algorithm.TableBuilder.Rows;

namespace Teleware.Algorithm.TableBuilder.RowDefinitions
{
    /// <summary>
    /// 数据行定义
    /// </summary>
    /// <remarks>
    /// 数据行指由表格中的数据直接产生的行，类似SQL中的SELECT语句所产生的表格
    /// </remarks>
    /// <seealso cref="DataRow"/>
    public class DataRowDefinition
    {
        private static readonly Func<DataRow, DataRowBuildContext, DataRow> _id = (r, ctx) => r;

        /// <summary>
        /// 初始化新数据行定义
        /// </summary>
        /// <param name="columns">行所包括的列定义</param>
        /// <param name="rowDecorator">行装饰器</param>
        public DataRowDefinition(IEnumerable<DataColumnDefinition> columns, Func<DataRow, DataRowBuildContext, DataRow> rowDecorator)
        {
            Columns = columns;
            RowDecorator = rowDecorator ?? _id;
        }

        /// <summary>
        /// 初始化新数据行定义
        /// </summary>
        /// <param name="columns">行所包括的列定义</param>
        public DataRowDefinition(IEnumerable<DataColumnDefinition> columns) : this(columns, null)
        {
        }

        /// <summary>
        /// 行所包括的列定义
        /// </summary>
        public IEnumerable<DataColumnDefinition> Columns { get; }

        /// <summary>
        /// 行装饰器
        /// </summary>
        public Func<DataRow, DataRowBuildContext, DataRow> RowDecorator { get; }
    }
}