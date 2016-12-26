namespace Teleware.Algorithm.TableBuilder.Cells
{
    /// <summary>
    /// 静态文本单元格
    /// </summary>
    public class StaticTextCell : Cell
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="text">单元格文本</param>
        public StaticTextCell(string text) : base()
        {
            Value = text;
        }
    }
}