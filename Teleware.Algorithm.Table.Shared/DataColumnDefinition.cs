using System;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;

namespace Teleware.Algorithm.TableBuilder.Shared
{
    /// <summary>
    /// 数据列定义基类
    /// </summary>
    public abstract class DataColumnDefinition
    {
        private static Func<Cell, DataRowBuildContext, Cell> _id = (c, ctx) => c;

        /// <summary>
        /// 初始化数据列
        /// </summary>
        /// <param name="cellDecorator">单元格装饰器，用于修饰此列生成的单元格</param>
        public DataColumnDefinition(Func<Cell, DataRowBuildContext, Cell> cellDecorator)
        {
            CellDecorator = cellDecorator ?? _id;
        }

        /// <summary>
        /// 创建单元格
        /// </summary>
        /// <param name="context">数据行创建上下文</param>
        /// <returns>单元格</returns>
        /// <remarks>只创建单元格实例</remarks>
        protected abstract Cell BuildCell(DataRowBuildContext context);

        /// <summary>
        /// 创建单元格
        /// </summary>
        /// <param name="context">数据行创建上下文</param>
        /// <returns>单元格</returns>
        public Cell CreateCell(DataRowBuildContext context)
        {
            var cell = BuildCell(context);
            context.RegisterCell(cell, this);
            return cell;
        }

        /// <summary>
        /// 单元格装饰器
        /// </summary>
        public Func<Cell, DataRowBuildContext, Cell> CellDecorator { get; }

        /// <summary>
        /// 列头
        /// </summary>
        public abstract string ColumnText { get; }
    }
}