using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared.RowDefinitions;
using Teleware.Algorithm.TableBuilder.Shared.Rows;

namespace Teleware.Algorithm.TableBuilder.Shared.BuildContext
{
    public class DataRowBuildContext
    {
        private Dictionary<Cell, DataColumnDefinition> _cellDefMapping = new Dictionary<Cell, DataColumnDefinition>();

        public DataRowBuildContext(DataRowDefinition definition, IDictionary<string, dynamic> rowDatas)
        {
            Datas = rowDatas;
            Definition = definition;
        }

        public DataRowDefinition Definition { get; }

        public IDictionary<string, dynamic> Datas { get; }

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

        public void RegisterCell(Cell cell, DataColumnDefinition column)
        {
            _cellDefMapping.Add(cell, column);
        }

        public Cell DecorateCell(Cell cell)
        {
            return _cellDefMapping[cell].CellDecorator(cell, this);
        }

        public DataRow DecorateRow(DataRow row)
        {
            return Definition.RowDecorator(row, this);
        }
    }
}