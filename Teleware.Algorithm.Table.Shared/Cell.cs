using System.Dynamic;

namespace Teleware.Algorithm.TableBuilder.Shared
{
    /// <summary>
    /// 单元格基类
    /// </summary>
    public abstract class Cell
    {
        /// <summary>
        /// 初始化单元格
        /// </summary>
        public Cell()
        {
            Metadata = new ExpandoObject();
        }

        /// <summary>
        /// 扩展元数据
        /// </summary>
        public dynamic Metadata { get; }

        /// <summary>
        /// 单元格值
        /// </summary>
        public dynamic Value { get; protected set; }

        /// <summary>
        /// 返回单元格的文本表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value?.ToString() ?? "";
        }
    }
}