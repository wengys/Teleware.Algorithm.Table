using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.AggregateColumnDefinitions;
using Teleware.Algorithm.TableBuilder.BuildContext;
using Teleware.Algorithm.TableBuilder.Cells;
using Teleware.Algorithm.TableBuilder.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Rows;
using Xunit;

namespace Teleware.Algorithm.TableBuilder.Shared.Tests.AggregateColumnDefinitions
{
    public class TextAggregateColumnDefinitionTest
    {
        [Fact]
        public void StringTemplateTest()
        {
            var aggregateRowIndex = 3;
            var rowIndex = 10;
            var aggregateKey = "key";
            var stringTemplate = "{0} {1} {2}";
            var definition = new TextAggregateColumnDefinition(0, stringTemplate);
            var rowDefinition = A.Fake<AggregateRowDefinition>();
            var context = new AggregateRowBuildContext(rowDefinition, aggregateKey, Enumerable.Empty<DataRow>(), aggregateRowIndex);
            context.RowIndex = rowIndex;

            var expected = string.Format(stringTemplate, aggregateKey, rowIndex, aggregateRowIndex);

            var cell = (FormulaCell)definition.CreateCell(context);
            cell.ExecuteFormula(Enumerable.Empty<DataRow>());

            Assert.Equal(expected, cell.Value);
        }
    }
}