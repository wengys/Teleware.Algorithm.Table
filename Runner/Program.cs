using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBodyBuilder;
using Teleware.Algorithm.TableBuilder.Shared;
using Teleware.Algorithm.TableBuilder.Shared.AggregateColumnDefinitions;
using Teleware.Algorithm.TableBuilder.Shared.ColumnFormulas;
using Teleware.Algorithm.TableBuilder.Shared.DataColumnDefinitions;
using Teleware.Algorithm.TableBuilder.Shared.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Shared.Rows;

namespace Runner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //TODO: 行装饰器
            Func<DataRow, string> aggregateKeySelector = dataRow => "total";
            var builder = TableBodyBuilderConfiguration.Instance
               .SetRowDataPicker(new TestPicker())
               .SetDataRow(new DataRowDefinition(new DataColumnDefinition[] {
                   new ReferenceColumnDefinition("a", "a",  (c, ctx) =>
                   {
                       var v = ctx.GetColumnDataByRefKey("a");
                       c.Metadata.ResourceUrl = "http://www.qq.com/" + v;
                       return c;
                   }),
                   new ReferenceColumnDefinition("b", "b"),
                   new ReferenceColumnDefinition("c", "c"),
                   new FormulaColumnDefinition("total", new SumRefKeysDecimalFormula("a", "b", "c"))
               }, (r, ctx) =>
               {
                   r.Metadata.Strong = r.Cells[3].Value > 8;
                   return r;
               }))
               .SetAggregateRows(new AggregateRowDefinition(aggregateKeySelector, new AggregateColumnDefinition[] {
                   new TextAggregateColumnDefinition(0,"sum:"),
                   new FormulaAggregateColumnDefinition(3,new SumRefKeysDecimalFormula("a","b","c"),(c,ctx)=> {
                       c.Metadata.ResourceUrl="http://www.baidu.com?view="+ctx.AggregateKey;
                       return c;
                   })
               }, (r, ctx) =>
               {
                   r.Metadata.Strong = true;
                   return r;
               }))
               .SetMergeCellsCollector(new TestMergeCellsCollector())
               .CreateBuilder();
            var before = GC.GetTotalMemory(false);
            var body = builder.Build(null);
            GC.Collect();
            GC.WaitForFullGCComplete();
            var after = GC.GetTotalMemory(false);

            var delta = after - before;
        }

        private class TestMergeCellsCollector : IMergeCellsCollector
        {
            public IEnumerable<Tuple<CellReference, CellReference>> Collect(IList<Row> rows)
            {
                yield return Tuple.Create(new CellReference(3, 1), new CellReference(3, 2));
                yield return Tuple.Create(new CellReference(3, 0), new CellReference(3, 1));
            }
        }

        private class TestPicker : IRowDataPicker
        {
            public IEnumerable<IDictionary<string, dynamic>> PickRowDatas(dynamic rawDatas)
            {
                yield return new Dictionary<string, dynamic> {
                    { "a",1UL },
                    { "b",3 },
                    {"c",5 },
                };
                yield return new Dictionary<string, dynamic> {
                    { "a",2 },
                    { "b",4 },
                };
            }
        }
    }
}