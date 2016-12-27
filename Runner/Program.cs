using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder;
using Teleware.Algorithm.TableBuilder.AggregateColumnDefinitions;
using Teleware.Algorithm.TableBuilder.ColumnFormulas;
using Teleware.Algorithm.TableBuilder.DataColumnDefinitions;
using Teleware.Algorithm.TableBuilder.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Rows;
using Teleware.Algorithm.TableBuilder.TableBodyBuilder;
using Teleware.Algorithm.TableRender.Excel;
using Teleware.Algorithm.TableRender.Json;
using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;
using Table = Teleware.Algorithm.TableBuilder.Table;

namespace Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //// TODO: UNIT TEST
            //var zero = CellReference.Create(0, 0);
            //var zero2 = CellReference.Create(0, 0);
            //var zeroOne = CellReference.Create(0, 1);
            //var zeroTwo = CellReference.Create(0, 2);
            //var oneZero = CellReference.Create(1, 0);

            //var ax = zero.CompareTo(zero2);
            //var bx = zero.CompareTo(zeroOne);
            //var cx = zeroOne.CompareTo(oneZero);
            //var dx = oneZero.CompareTo(zeroOne);
            //var ex = oneZero.CompareTo(zeroTwo);

            Func<DataRow, string> aggregateKeySelector = dataRow => "total";
            var builder = TableBodyBuilderConfiguration.Instance
               .SetRowDataPicker(new TestPicker())
               .SetDataRowDefinition(new DataRowDefinition(new DataColumnDefinition[] {
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
               .SetAggregateRowsDefinition(new AggregateRowDefinition(aggregateKeySelector, new AggregateColumnDefinition[] {
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

            var body = builder.Build(null);
            var table = new Table(null, body);

            var render = new ExcelTableRender();
            var excelTable = render.Render(table);
            File.WriteAllBytes("text.xlsx", ((MemoryStream)excelTable).ToArray());
            //var render = new JsonTableRender();
            //var jTable = render.Render(table);
        }

        private class TestMergeCellsCollector : IMergeCellsCollector
        {
            public IEnumerable<System.Tuple<CellReference, CellReference>> Collect(IList<Teleware.Algorithm.TableBuilder.Row> rows)
            {
                yield return System.Tuple.Create(new CellReference(2, 1), new CellReference(2, 2));
                yield return System.Tuple.Create(new CellReference(2, 0), new CellReference(2, 1));
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