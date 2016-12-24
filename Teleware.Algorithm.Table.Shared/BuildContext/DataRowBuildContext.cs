﻿using System.Collections.Generic;
using Teleware.Algorithm.TableBuilder.Shared.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Shared.Rows;

namespace Teleware.Algorithm.TableBuilder.Shared.BuildContext
{
    /// <summary>
    /// 数据行创建上下文
    /// </summary>
    public class DataRowBuildContext
    {
        private Dictionary<Cell, DataColumnDefinition> _cellDefMapping = new Dictionary<Cell, DataColumnDefinition>();

        /// <summary>
        /// 初始化上下文
        /// </summary>
        /// <param name="definition">行定义</param>
        /// <param name="rowDatas">行数据</param>
        public DataRowBuildContext(DataRowDefinition definition, IDictionary<string, dynamic> rowDatas)
        {
            Datas = rowDatas;
            Definition = definition;
        }

        /// <summary>
        /// 行定义
        /// </summary>
        public DataRowDefinition Definition { get; }

        /// <summary>
        /// 行数据
        /// </summary>
        public IDictionary<string, dynamic> Datas { get; }

        /// <summary>
        /// 根据键获取列数据
        /// </summary>
        /// <param name="refKey"></param>
        /// <returns></returns>
        public dynamic GetColumnDataByRefKey(string refKey)
        {
            dynamic data;
            if (Datas.TryGetValue(refKey, out data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 注册单元格
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="column">单元格对应的列定义</param>
        public void RegisterCell(Cell cell, DataColumnDefinition column)
        {
            _cellDefMapping.Add(cell, column);
        }

        /// <summary>
        /// 返回单元格对应列定义
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public DataColumnDefinition GetCellDefinition(Cell cell)
        {
            return _cellDefMapping[cell];
        }

        /// <summary>
        /// 使用单元格装饰器修饰单元格
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>修饰后的单元格</returns>
        public Cell DecorateCell(Cell cell)
        {
            return _cellDefMapping[cell].CellDecorator(cell, this);
        }

        /// <summary>
        /// 使用行装饰器修饰行
        /// </summary>
        /// <param name="row">行</param>
        /// <returns>修饰后的行</returns>
        public DataRow DecorateRow(DataRow row)
        {
            return Definition.RowDecorator(row, this);
        }
    }
}