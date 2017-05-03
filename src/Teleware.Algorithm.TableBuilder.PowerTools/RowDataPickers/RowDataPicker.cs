using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.TableBodyBuilder;
using System.Reflection;

namespace Teleware.Algorithm.TableBuilder.PowerTools.RowDataPickers
{
    /// <summary>
    /// 简单对象数据分拣器
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public class SimpleObjectPropertyPicker<TType> : IRowDataPicker
    {
        private static readonly IEnumerable<System.Reflection.PropertyInfo> _properties;
        private readonly Action<RowDataPickedEventArgs> _onRowDataPicked;

        static SimpleObjectPropertyPicker()
        {
            _properties = typeof(TType).GetProperties();
        }

        /// <summary>
        ///
        /// </summary>
        public SimpleObjectPropertyPicker()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="onRowDataPicked">行数据分拣完成事件</param>
        public SimpleObjectPropertyPicker(Action<RowDataPickedEventArgs> onRowDataPicked)
        {
            _onRowDataPicked = onRowDataPicked;
        }

        /// <summary>
        /// 分拣数据
        /// </summary>
        /// <param name="rawDatas">用于生成表格的原始数据</param>
        /// <returns>按行列组织的数据</returns>
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
                    Metadata = ctx
                });
                yield return rowData;
            }
        }

        /// <summary>
        /// 行数据分拣完成事件
        /// </summary>
        public class RowDataPickedEventArgs : EventArgs
        {
            /// <summary>
            /// 所有行原始对象
            /// </summary>
            public IEnumerable<TType> RawObjects { get; set; }

            /// <summary>
            /// 当前行对象
            /// </summary>
            public TType CurrentRawObject { get; set; }

            /// <summary>
            /// 行数据
            /// </summary>
            public Dictionary<string, dynamic> RowData { get; set; }

            /// <summary>
            /// 元数据（扩展用）
            /// </summary>
            public Dictionary<string, dynamic> Metadata { get; set; }
        }
    }
}