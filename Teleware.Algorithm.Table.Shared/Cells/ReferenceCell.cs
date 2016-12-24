using Teleware.Algorithm.TableBuilder.Shared.DataColumnDefinitions;

namespace Teleware.Algorithm.TableBuilder.Shared.Cells
{
    /// <summary>
    /// 引用单元格
    /// </summary>
    /// <seealso cref="ReferenceColumnDefinition"/>
    public class ReferenceCell : Cell
    {
        /// <summary>
        /// 初始化引用列
        /// </summary>
        /// <param name="value">单元格值</param>
        /// <param name="data">单元格原始数据</param>
        public ReferenceCell(
            dynamic value,
            dynamic data
            )
        {
            Value = value;
            RawData = data;
        }

        /// <summary>
        /// 单元格原始数据
        /// </summary>
        public dynamic RawData { get; }
    }
}