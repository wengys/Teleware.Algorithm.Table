using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.TableBodyBuilder;

namespace Teleware.Algorithm.TableBuilder.PowerTools.RowDataPickers
{
    public class SimpleObjectPropertyPicker<TType> : IRowDataPicker
    {
        private static readonly IEnumerable<System.Reflection.PropertyInfo> _properties;
        private readonly Action<RowDataPickedEventArgs> _onRowDataPicked;

        static SimpleObjectPropertyPicker()
        {
            _properties = typeof(TType).GetProperties();
        }

        public SimpleObjectPropertyPicker()
        {
        }

        public SimpleObjectPropertyPicker(Action<RowDataPickedEventArgs> onRowDataPicked)
        {
            _onRowDataPicked = onRowDataPicked;
        }

        public IEnumerable<IDictionary<string, dynamic>> PickRowDatas(dynamic rawDatas)
        {
            var objs = (IEnumerable<TType>)rawDatas;
            var ctx = new Dictionary<string, dynamic>();
            foreach (var obj in objs)
            {
                var rowData = new Dictionary<string, dynamic>();
                foreach (var property in _properties)
                {
                    var value = property.GetGetMethod().Invoke(obj, null);
                    rowData.Add(property.Name, value);
                }
                _onRowDataPicked?.Invoke(new RowDataPickedEventArgs
                {
                    RawObjects = objs,
                    CurrentRawObject = obj,
                    RowData = rowData,
                    RowDataPickContext = ctx
                });
                yield return rowData;
            }
        }

        public class RowDataPickedEventArgs : EventArgs
        {
            public IEnumerable<TType> RawObjects { get; set; }
            public TType CurrentRawObject { get; set; }
            public Dictionary<string, dynamic> RowData { get; set; }
            public Dictionary<string, dynamic> RowDataPickContext { get; set; }
        }
    }
}