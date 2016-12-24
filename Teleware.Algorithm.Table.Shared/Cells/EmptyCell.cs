namespace Teleware.Algorithm.TableBuilder.Shared.Cells
{
    /// <summary>
    /// 空单元格
    /// </summary>
    public class EmptyCell : Cell
    {
        private EmptyCell() : base()
        {
        }

        /// <summary>
        /// 返回空单元格唯一实例
        /// </summary>
        public static EmptyCell Singleton { get; } = new EmptyCell();
    }
}