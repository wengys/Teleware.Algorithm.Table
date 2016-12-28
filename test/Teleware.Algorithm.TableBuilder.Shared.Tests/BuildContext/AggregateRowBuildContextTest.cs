using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.BuildContext;
using Teleware.Algorithm.TableBuilder.Cells;
using Teleware.Algorithm.TableBuilder.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Rows;
using Xunit;

namespace Teleware.Algorithm.TableBuilder.Shared.Tests.BuildContext
{
    public class AggregateRowBuildContextTest
    {
        [Fact]
        public void CellRegisterAndFetchTest()
        {
            var rowDefinition = A.Fake<AggregateRowDefinition>();
            var context = new AggregateRowBuildContext(rowDefinition, "", Enumerable.Empty<DataRow>(), 0);
            var cell = EmptyCell.Singleton;
            var expected = A.Fake<AggregateColumnDefinition>();

            context.RegisterCell(cell, expected);
            var actual = context.GetCellDefinition(cell);//同时测试了两个方法，只为了省事

            Assert.Same(expected, actual);
        }

        [Fact]
        public void UnknownCellDefinitionFetchTest()
        {
            var rowDefinition = A.Fake<AggregateRowDefinition>();
            var context = new AggregateRowBuildContext(rowDefinition, "", Enumerable.Empty<DataRow>(), 0);
            var cell = EmptyCell.Singleton;

            var actual = context.GetCellDefinition(cell);

            Assert.Equal(null, actual);
        }

        [Fact]
        public void DecorateCellTest()
        {
            var rowDefinition = A.Fake<AggregateRowDefinition>();
            var context = new AggregateRowBuildContext(rowDefinition, "", Enumerable.Empty<DataRow>(), 0);
            var cell = EmptyCell.Singleton;
            var expectedDecoratedCell = new TestCell();
            var definition = new TestAggregationColumnDefnition(0, (c, ctx) => expectedDecoratedCell);
            context.RegisterCell(cell, definition);

            var decoratedCell = context.DecorateCell(cell);

            Assert.Same(expectedDecoratedCell, decoratedCell);
        }

        [Fact]
        public void DecorateRowTest()
        {
            var expectedDecoratedRow = new AggregateRow(null, null);
            var rowDefinition = new TestAggregationRowDefinition(null, null, (ar, ctx) => expectedDecoratedRow);
            var context = new AggregateRowBuildContext(rowDefinition, "", Enumerable.Empty<DataRow>(), 0);
            var row = new AggregateRow(null, context);

            var decoratedRow = context.DecorateRow(row);

            Assert.Same(expectedDecoratedRow, decoratedRow);
        }

        private class TestCell : Cell
        {
        }

        private class TestAggregationColumnDefnition : AggregateColumnDefinition
        {
            public TestAggregationColumnDefnition(int colNum, Func<Cell, AggregateRowBuildContext, Cell> cellDecorator) : base(colNum, cellDecorator)
            {
            }

            protected override Cell BuildCell(AggregateRowBuildContext context)
            {
                throw new NotImplementedException();
            }
        }

        private class TestAggregationRowDefinition : AggregateRowDefinition
        {
            public TestAggregationRowDefinition(Func<DataRow, string> aggregateKeySelector, IEnumerable<AggregateColumnDefinition> columns, Func<AggregateRow, AggregateRowBuildContext, AggregateRow> rowDecorator) : base(aggregateKeySelector, columns, rowDecorator)
            {
            }

            public TestAggregationRowDefinition(Func<DataRow, string> aggregateKeySelector, IEnumerable<AggregateColumnDefinition> columns) : base(aggregateKeySelector, columns)
            {
            }
        }
    }
}