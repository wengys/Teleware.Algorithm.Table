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
    public class DataRowBuildContextTest
    {
        [Fact]
        public void CellRegisterAndFetchTest()
        {
            var rowDefinition = A.Fake<DataRowDefinition>();
            var context = new DataRowBuildContext(rowDefinition, null, 0);
            var cell = EmptyCell.Singleton;
            var expected = A.Fake<DataColumnDefinition>();

            context.RegisterCell(cell, expected);
            var actual = context.GetCellDefinition(cell);//同时测试了两个方法，只为了省事

            Assert.Same(expected, actual);
        }

        [Fact]
        public void UnknownCellDefinitionFetchTest()
        {
            var rowDefinition = A.Fake<DataRowDefinition>();
            var context = new DataRowBuildContext(rowDefinition, null, 0);
            var cell = EmptyCell.Singleton;

            var actual = context.GetCellDefinition(cell);

            Assert.Equal(null, actual);
        }

        [Fact]
        public void DecorateCellTest()
        {
            var rowDefinition = A.Fake<DataRowDefinition>();
            var context = new DataRowBuildContext(rowDefinition, null, 0);
            var cell = EmptyCell.Singleton;
            var expectedDecoratedCell = new TestCell();
            var definition = new TestDataColumnDefnition((c, ctx) => expectedDecoratedCell);
            context.RegisterCell(cell, definition);

            var decoratedCell = context.DecorateCell(cell);

            Assert.Same(expectedDecoratedCell, decoratedCell);
        }

        [Fact]
        public void DecorateRowTest()
        {
            var expectedDecoratedRow = new DataRow(null, null);
            var rowDefinition = new TestDataRowDefinition(null, (ar, ctx) => expectedDecoratedRow);
            var context = new DataRowBuildContext(rowDefinition, null, 0);
            var row = new DataRow(null, context);

            var decoratedRow = context.DecorateRow(row);

            Assert.Same(expectedDecoratedRow, decoratedRow);
        }

        private class TestCell : Cell
        {
        }

        private class TestDataColumnDefnition : DataColumnDefinition
        {
            public TestDataColumnDefnition(Func<Cell, DataRowBuildContext, Cell> cellDecorator) : base(cellDecorator)
            {
            }

            protected override Cell BuildCell(DataRowBuildContext context)
            {
                throw new NotImplementedException();
            }

            public override string ColumnText { get; }
        }

        private class TestDataRowDefinition : DataRowDefinition
        {
            public TestDataRowDefinition(IEnumerable<DataColumnDefinition> columns, Func<DataRow, DataRowBuildContext, DataRow> rowDecorator) : base(columns, rowDecorator)
            {
            }

            public TestDataRowDefinition(IEnumerable<DataColumnDefinition> columns) : base(columns)
            {
            }
        }
    }
}