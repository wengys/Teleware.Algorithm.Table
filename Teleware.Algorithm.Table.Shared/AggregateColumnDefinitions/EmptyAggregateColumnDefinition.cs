//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Teleware.Algorithm.TableBuilder.Shared.BuildContext;
//using Teleware.Algorithm.TableBuilder.Shared.Cells;

//namespace Teleware.Algorithm.TableBuilder.Shared.AggregateColumnDefinitions
//{
//    public class EmptyAggregateColumnDefinition : AggregateColumnDefinition
//    {
//        public EmptyAggregateColumnDefinition(int colNum) : this(colNum, null)
//        {
//        }

//        public EmptyAggregateColumnDefinition(int colNum, Func<Cell, AggregateRowBuildContext, Cell> cellDecorator) : base(colNum, cellDecorator)
//        {
//        }

//        protected override Cell BuildCell(AggregateRowBuildContext context)
//        {
//            return new EmptyCell();
//        }
//    }
//}