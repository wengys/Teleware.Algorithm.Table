using System;
using Teleware.Algorithm.TableBuilder.BuildContext;
using Teleware.Algorithm.TableBuilder.Cells;

namespace Teleware.Algorithm.TableBuilder.DataColumnDefinitions
{
    /// <summary>
    /// 引用列定义
    /// </summary>
    /// <remarks>
    /// 引用列指由行中特定<see cref="RefKey"/>的数据直接产生的列, 类似 SELETE NAME FROM PERSONS 中的 NAME
    /// </remarks>
    public class ReferenceColumnDefinition : DataColumnDefinition
    {
        private static Func<dynamic, dynamic> _id = a => a;

        private string _columnText;
        private Func<dynamic, dynamic> _valueMapper;

        /// <summary>
        /// 初始化新引用列定义
        /// </summary>
        /// <param name="columnText">列头</param>
        /// <param name="refKey">列相关数据的键</param>
        public ReferenceColumnDefinition(
            string columnText,
            string refKey
            ) : this(columnText, refKey, null, null)
        {
        }

        /// <summary>
        /// 初始化新引用列定义
        /// </summary>
        /// <param name="columnText">列头</param>
        /// <param name="refKey">列相关数据的键</param>
        /// <param name="valueMapper">值映射器</param>
        public ReferenceColumnDefinition(
            string columnText,
            string refKey,
            Func<dynamic, dynamic> valueMapper
            ) : this(columnText, refKey, valueMapper, null)
        {
        }

        /// <summary>
        /// 初始化新引用列定义
        /// </summary>
        /// <param name="columnText">列头</param>
        /// <param name="refKey">列相关数据的键</param>
        /// <param name="cellDecorator">单元格装饰器</param>
        public ReferenceColumnDefinition(
            string columnText,
            string refKey,
            Func<Cell, DataRowBuildContext, Cell> cellDecorator
            ) : this(columnText, refKey, null, cellDecorator)
        {
        }

        /// <summary>
        /// 初始化新引用列定义
        /// </summary>
        /// <param name="columnText">列头</param>
        /// <param name="refKey">列相关数据的键</param>
        /// <param name="valueMapper">值映射器</param>
        /// <param name="cellDecorator">单元格装饰器</param>
        public ReferenceColumnDefinition(
            string columnText,
            string refKey,
            Func<dynamic, dynamic> valueMapper,
            Func<Cell, DataRowBuildContext, Cell> cellDecorator
            ) : base(cellDecorator)
        {
            _columnText = columnText;
            RefKey = refKey;
            _valueMapper = valueMapper ?? _id;
        }

        /// <summary>
        /// 列相关数据的键
        /// </summary>
        /// <remarks>
        /// 用于指定列所关联的行数据
        /// </remarks>
        public string RefKey { get; }

        /// <summary>
        /// 列头
        /// </summary>
        public override string ColumnText
        {
            get
            {
                return _columnText;
            }
        }

        /// <see cref="DataColumnDefinition.BuildCell(DataRowBuildContext)"/>
        protected override Cell BuildCell(DataRowBuildContext context)
        {
            var data = context.GetColumnDataByRefKey(RefKey);
            if (data == null)
            {
                return EmptyCell.Singleton;
            }
            var column = new ReferenceCell(_valueMapper(data), data);
            return column;
        }
    }
}