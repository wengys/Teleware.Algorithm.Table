using Teleware.Algorithm.TableBuilder.DataColumnDefinitions;

namespace Teleware.Algorithm.TableBuilder.Cells
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
        /// <param name="rawData">单元格原始数据</param>
        public ReferenceCell(
            dynamic value,
            dynamic rawData
            )
        {
            Value = value;
            RawData = rawData;
        }

        /// <summary>
        /// 单元格原始数据
        /// </summary>
        public dynamic RawData { get; }
    }
}