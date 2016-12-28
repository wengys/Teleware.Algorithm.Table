using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder;
using Teleware.Algorithm.TableBuilder.AggregateColumnDefinitions;
using Teleware.Algorithm.TableBuilder.ColumnFormulas;
using Teleware.Algorithm.TableBuilder.DataColumnDefinitions;
using Teleware.Algorithm.TableBuilder.PowerTools.MergeCellsCollectors;
using Teleware.Algorithm.TableBuilder.PowerTools.RowDataPickers;
using Teleware.Algorithm.TableBuilder.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Rows;
using Teleware.Algorithm.TableBuilder.TableBodyBuilder;

namespace Sample
{
    public class Program2
    {
        private static void Main()
        {
            //TODO: 如何处理占比？如何更有效处理聚合标题？
            var builder = TableBodyBuilderConfiguration.Instance
                .SetRowDataPicker(new SimpleObjectPropertyPicker<Data>())
                .SetDataRowDefinition(new DataRowDefinition(new DataColumnDefinition[] {
                    new ReferenceColumnDefinition("大类","大类"),
                    new ReferenceColumnDefinition("小类1","小类1"),
                    new ReferenceColumnDefinition("小类","小类2"),
                    new ReferenceColumnDefinition("值","值"),
                    new ReferenceColumnDefinition("占比","大类占比")
                }))
                .SetAggregateRowsDefinition(
                    new AggregateRowDefinition("sum", dr => dr.RowBuildContext.GetColumnDataByRefKey("大类"), new AggregateColumnDefinition[] {
                        new TextAggregateColumnDefinition(0,aggregateContext=>aggregateContext.AggregateKey),
                        new TextAggregateColumnDefinition(1,"sum-{0}:"),
                        new FormulaAggregateColumnDefinition(3,new SumRefKeysDecimalFormula(value=>(decimal)value,"值")),
                        //new TextAggregateColumnDefinition("占比","大类占比")
                    })
                )
                .SetMergeCellsCollectors(new SameValueMergeCellsCollector(0, 1), new SameValueMergeCellsCollector(1, 3, AcceptRowTypes.AggregateRow, true))
                .CreateBuilder();
            var tableBody = builder.Build(GetDatas());
        }

        private static IEnumerable<Data> GetDatas()
        {
            yield return new Data
            {
                大类 = "A",
                小类1 = "AA1",
                小类2 = "AA2",
                值 = 3,
                大类占比 = 0.5M
            };
            yield return new Data
            {
                大类 = "A",
                小类1 = "AB1",
                小类2 = "AB2",
                值 = 4,
                大类占比 = 0.5M
            };
            yield return new Data
            {
                大类 = "A",
                小类1 = "AC1",
                小类2 = "AC2",
                值 = 3,
                大类占比 = 0.5M
            };
            yield return new Data
            {
                大类 = "B",
                小类1 = "BA1",
                小类2 = "BA2",
                值 = 2,
                大类占比 = 0.5M
            };
            yield return new Data
            {
                大类 = "B",
                小类1 = "BB1",
                小类2 = "BB2",
                值 = 1,
                大类占比 = 0.5M
            };
            yield return new Data
            {
                大类 = "B",
                小类1 = "BC1",
                小类2 = "BC2",
                值 = 7,
                大类占比 = 0.5M
            };
        }
    }

    public class Data
    {
        public string 大类 { get; set; }
        public string 小类1 { get; set; }
        public string 小类2 { get; set; }
        public int 值 { get; set; }
        public decimal 大类占比 { get; set; }
    }
}