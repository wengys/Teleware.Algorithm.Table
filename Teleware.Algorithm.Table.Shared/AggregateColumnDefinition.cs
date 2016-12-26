using System;
using System.Dynamic;
using Teleware.Algorithm.TableBuilder.BuildContext;

namespace Teleware.Algorithm.TableBuilder
{
    /// <summary>
    /// 聚合列定义基类
    /// </summary>
    public abstract class AggregateColumnDefinition
    {
        private static Func<Cell, AggregateRowBuildContext, Cell> _id = (c, ctx) => c;

        /// <summary>
        /// 初始化聚合列
        /// </summary>
        /// <param name="colNum">列所在坐标</param>
        /// <param name="cellDecorator">单元格装饰器，用于修饰此列生成的单元格</param>
        public AggregateColumnDefinition(int colNum, Func<Cell, AggregateRowBuildContext, Cell> cellDecorator)
        {
            Metadata = new ExpandoObject();
            ColNum = colNum;
            CellDecorator = cellDecorator ?? _id;
        }

        /// <summary>
        /// 列坐标（从0开始）
        /// </summary>
        public int ColNum { get; }

        /// <summary>
        /// 扩展元数据
        /// </summary>
        public dynamic Metadata { get; }

        /// <summary>
        /// 单元格装饰器
        /// </summary>
        public Func<Cell, AggregateRowBuildContext, Cell> CellDecorator { get; }

        /// <summary>
        /// 创建单元格
        /// </summary>
        /// <param name="context">数据行创建上下文</param>
        /// <returns>单元格</returns>
        /// <remarks>只创建单元格实例</remarks>
        protected abstract Cell BuildCell(AggregateRowBuildContext context);

        /// <summary>
        /// 创建单元格
        /// </summary>
        /// <param name="context">数据行创建上下文</param>
        /// <returns>单元格</returns>
        public Cell CreateCell(AggregateRowBuildContext context)
        {
            var cell = BuildCell(context);
            context.RegisterCell(cell, this);
            return cell;
        }
    }
}