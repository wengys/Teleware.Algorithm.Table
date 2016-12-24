namespace Teleware.Algorithm.TableBuilder.Shared
{
    /// <summary>
    /// 表示对表格中某个单元格的引用
    /// </summary>
    public struct CellReference
    {
        /// <summary>
        /// 初始化新单元格
        /// </summary>
        /// <param name="rowNum">行坐标（从0开始）</param>
        /// <param name="colNum">列坐标（从0开始）</param>
        public CellReference(int rowNum, int colNum)
        {
            this.ColNum = colNum;
            this.RowNum = rowNum;
        }

        /// <summary>
        /// 列坐标（从0开始）
        /// </summary>
        public int ColNum { get; set; }

        /// <summary>
        /// 行坐标（从0开始）
        /// </summary>
        public int RowNum { get; set; }

        /// <summary>
        /// 返回单元格引用的文字表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({RowNum}, {ColNum})";
        }

        /// <summary>
        /// 创建新单元格引用
        /// </summary>
        /// <param name="rowNum">行坐标</param>
        /// <param name="colNum">列坐标</param>
        /// <returns></returns>
        public static CellReference Create(int rowNum, int colNum)
        {
            return new CellReference(rowNum, colNum);
        }
    }
}