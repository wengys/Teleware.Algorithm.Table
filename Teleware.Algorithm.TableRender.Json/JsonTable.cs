using System.Collections.Generic;

namespace Teleware.Algorithm.TableRender.Json
{
    public class JsonTable
    {
        public IEnumerable<JsonRow> Head { get; set; }
        public IEnumerable<JsonRow> Body { get; set; }
    }

    public class JsonRow
    {
        public IEnumerable<JsonCell> Cells { get; set; }
        public dynamic Metadata { get; set; }
    }

    public class JsonCell
    {
        public int RowSpan { get; set; }
        public int ColSpan { get; set; }
        public dynamic Value { get; set; }
        public dynamic Metadata { get; set; }
    }
}