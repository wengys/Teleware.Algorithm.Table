using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.BuildContext;
using Teleware.Algorithm.TableBuilder.ColumnFormulas;
using Teleware.Algorithm.TableBuilder.Rows;
using Xunit;

namespace Teleware.Algorithm.TableBuilder.Shared.Tests.ColumnFormulas
{
    public class SumRefKeysFormulaTests
    {
        [Fact]
        public void SumTest()
        {
            var data1 = new Dictionary<string, dynamic>()
            {
                ["a"] = 1,
                ["b"] = 3,
                ["c"] = 5
            };

            var data2 = new Dictionary<string, dynamic>()
            {
                ["a"] = 2,
                ["b"] = 4,
                ["c"] = 6
            };

            var formula = new SumRefKeysFormula<int>(v => v, (a, b) => a + b, data1.Keys.ToArray());
            var dataRow1 = new TestDataRow(null, new DataRowBuildContext(null, data1, 0));
            var dataRow2 = new TestDataRow(null, new DataRowBuildContext(null, data2, 0));
            var expected = Enumerable.Sum(data1.Values.Concat(data2.Values).Cast<int>());

            var actual = formula.Execute(new DataRow[] { dataRow1, dataRow2 });

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void SumNoneExistsKeyTest()
        {
            var data1 = new Dictionary<string, dynamic>()
            {
                ["a"] = 1,
            };
            var data2 = new Dictionary<string, dynamic>()
            {
            };
            var formula = new SumRefKeysFormula<int>(v => v, (a, b) => a + b, "a", "unknown");
            var dataRow1 = new TestDataRow(null, new DataRowBuildContext(null, data1, 0));
            var dataRow2 = new TestDataRow(null, new DataRowBuildContext(null, data2, 0));
            var expected = 1;

            var actual = formula.Execute(new DataRow[] { dataRow1, dataRow2 });

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void DecimalSumTest()
        {
            var data1 = new Dictionary<string, dynamic>()
            {
                ["a"] = 1M,
                ["b"] = 3M,
                ["c"] = 5M
            };

            var data2 = new Dictionary<string, dynamic>()
            {
                ["a"] = 2M,
                ["b"] = 4M,
                ["c"] = 6M
            };

            var formula = new SumRefKeysDecimalFormula(v => v, data1.Keys.ToArray());
            var dataRow1 = new TestDataRow(null, new DataRowBuildContext(null, data1, 0));
            var dataRow2 = new TestDataRow(null, new DataRowBuildContext(null, data2, 0));
            var expected = Enumerable.Sum(data1.Values.Concat(data2.Values).Cast<decimal>());

            var actual = formula.Execute(new DataRow[] { dataRow1, dataRow2 });

            Assert.Equal(actual, expected);
        }

        [Fact]
        public void DecimalSumNoneExistsKeyTest()
        {
            var data1 = new Dictionary<string, dynamic>()
            {
                ["a"] = 1M,
            };
            var data2 = new Dictionary<string, dynamic>()
            {
            };
            var formula = new SumRefKeysDecimalFormula(v => v, "a", "unknown");
            var dataRow1 = new TestDataRow(null, new DataRowBuildContext(null, data1, 0));
            var dataRow2 = new TestDataRow(null, new DataRowBuildContext(null, data2, 0));
            var expected = 1;

            var actual = formula.Execute(new DataRow[] { dataRow1, dataRow2 });

            Assert.Equal(actual, expected);
        }

        private class TestDataRow : DataRow
        {
            public TestDataRow(IList<Cell> cells, DataRowBuildContext rowBuildContext) : base(cells, rowBuildContext)
            {
            }
        }
    }
}