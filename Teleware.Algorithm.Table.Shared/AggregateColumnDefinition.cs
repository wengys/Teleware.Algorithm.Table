using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.BuildContext;

namespace Teleware.Algorithm.TableBuilder.Shared
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

        public int ColNum { get; }

        public dynamic Metadata { get; }

        public Func<Cell, AggregateRowBuildContext, Cell> CellDecorator { get; }

        protected abstract Cell BuildCell(AggregateRowBuildContext context);

        public Cell CreateCell(AggregateRowBuildContext context)
        {
            var cell = BuildCell(context);
            context.RegisterCell(cell, this);
            return cell;
        }
    }
}