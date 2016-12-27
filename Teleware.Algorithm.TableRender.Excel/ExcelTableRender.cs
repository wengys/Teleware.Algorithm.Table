using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder;
using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;
using Tuple = System.Tuple;

namespace Teleware.Algorithm.TableRender.Excel
{
    public class ExcelTableRender
    {
        public Stream Render(Teleware.Algorithm.TableBuilder.Table table)
        {
            var sheetElems = CreateSpreadsheetInMemory();
            var stream = sheetElems.Item1;
            var spreadsheetDocument = sheetElems.Item2;
            var workbook = sheetElems.Item3;
            var worksheet = sheetElems.Item4;
            var sheetData = sheetElems.Item5;
            var sharedStringTable = sheetElems.Item6;

            RenderRows(table.TableHead, sheetData, sharedStringTable);
            RenderRows(table.TableBody, sheetData, sharedStringTable);

            RenderMergeCells(table.TableHead?.MergeCellGroups, 0, sheetData, worksheet);
            RenderMergeCells(table.TableBody.MergeCellGroups, table.TableHead?.Rows.Count ?? 0, sheetData, worksheet);

            SaveSpreadsheetInMemory(stream, spreadsheetDocument, workbook);
            return stream;
        }

        private void RenderMergeCells(IEnumerable<IEnumerable<CellReference>> mergeCellGroups, int rowOffset, SheetData sheetData, Worksheet worksheet)
        {
            // TODO: 完成坐标转换计算
            //if (mergeCellGroups == null)
            //{
            //    return;
            //}
            //foreach (var mergeCellGroup in mergeCellGroups)
            //{
            //    MergeCells mergeCells;
            //    mergeCells = GetMergeCells(worksheet);
            //    MergeCell mergeCell = new MergeCell() { Reference = new StringValue("A1" + ":" + "B1") };
            //    mergeCells.Append(mergeCell);

            //    worksheet.Save();
            //}
        }

        private static MergeCells GetMergeCells(Worksheet worksheet)
        {
            MergeCells mergeCells;
            if (worksheet.Elements<MergeCells>().Any())
            {
                mergeCells = worksheet.Elements<MergeCells>().First();
            }
            else
            {
                mergeCells = new MergeCells();

                // Insert a MergeCells object into the specified position.
                if (worksheet.Elements<CustomSheetView>().Any())
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<CustomSheetView>().First());
                }
                else if (worksheet.Elements<DataConsolidate>().Any())
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<DataConsolidate>().First());
                }
                else if (worksheet.Elements<SortState>().Any())
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SortState>().First());
                }
                else if (worksheet.Elements<AutoFilter>().Any())
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<AutoFilter>().First());
                }
                else if (worksheet.Elements<Scenarios>().Any())
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<Scenarios>().First());
                }
                else if (worksheet.Elements<ProtectedRanges>().Any())
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<ProtectedRanges>().First());
                }
                else if (worksheet.Elements<SheetProtection>().Any())
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetProtection>().First());
                }
                else if (worksheet.Elements<SheetCalculationProperties>().Any())
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetCalculationProperties>().First());
                }
                else
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetData>().First());
                }
            }

            return mergeCells;
        }

        private static void RenderRows(ITableContentSection section, SheetData sheetData, SharedStringTable sharedStringTable)
        {
            if (section == null)
            {
                return;
            }
            foreach (var row in section.Rows)
            {
                var excelRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                foreach (var cell in row.Cells)
                {
                    CellValues cellDataType = GetCellType(cell.Value);
                    DocumentFormat.OpenXml.Spreadsheet.Cell excelCell;
                    if (cellDataType == CellValues.Number)
                    {
                        excelCell = MapToNumberCell(cell);
                    }
                    else if (cellDataType == CellValues.String)
                    {
                        excelCell = MapToStringCell(cell, sharedStringTable);
                    }
                    else
                    {
                        throw new NotSupportedException($"不支持的单元格数据类型: {cellDataType}");
                    }
                    excelRow.AppendChild(excelCell);
                }

                sheetData.AppendChild(excelRow);
            }
        }

        private static Cell MapToStringCell(TableBuilder.Cell cell, SharedStringTable sharedStringTable)
        {
            Cell excelCell;
            var text = cell.Value?.ToString() ?? "";

            int index = GetSharedStringItemIndex(sharedStringTable, text);
            excelCell = new Cell();
            excelCell.CellValue = new CellValue(index.ToString());
            excelCell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            return excelCell;
        }

        private static int GetSharedStringItemIndex(SharedStringTable sharedStringTable, dynamic text)
        {
            int index = 0;
            int i = 0;
            bool found = false;
            foreach (SharedStringItem item in sharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    index = i;
                    found = true;
                    break;
                }

                i++;
            }
            if (!found)
            {
                sharedStringTable.AppendChild(
                    new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
                sharedStringTable.Save();
                index = i;
            }

            return index;
        }

        private static Cell MapToNumberCell(TableBuilder.Cell cell)
        {
            return new Cell()
            {
                DataType = CellValues.Number,
                CellValue = new CellValue(cell.Value.ToString())
            };
        }

        private static CellValues GetCellType(object cellValue)
        {
            CellValues cellDataType;
            var type = cellValue?.GetType() ?? typeof(string);
            switch (type.Name) //TODO: 完善类型判断
            {
                case "Int32":
                case "UInt32":
                case "Int64":
                case "UInt64":
                case "Decimal":
                case "Double":
                case "Float":
                    return CellValues.Number;

                case "String":
                    return CellValues.String;

                ////            case "DateTime":
                ////                return CellValues.Date;
                default:
                    cellDataType = CellValues.String;
                    break;
            }

            return cellDataType;
        }

        private void SaveSpreadsheetInMemory(MemoryStream memoryStream, SpreadsheetDocument spreadsheetDocument, Workbook workbook)
        {
            workbook.Save();
            spreadsheetDocument.Close();
            memoryStream.Seek(0, SeekOrigin.Begin);
        }

        private Tuple<MemoryStream, SpreadsheetDocument, Workbook, Worksheet, SheetData, SharedStringTable> CreateSpreadsheetInMemory()
        {
            var memoryStream = new MemoryStream();
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.
                Create(memoryStream, SpreadsheetDocumentType.Workbook);
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            var workbook = new Workbook();
            workbookpart.Workbook = workbook;
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            var worksheet = new Worksheet(sheetData);
            worksheetPart.Worksheet = worksheet;
            SharedStringTablePart sharedStringTablePart = spreadsheetDocument.WorkbookPart.AddNewPart<SharedStringTablePart>();
            var sharedStringTable = new SharedStringTable();
            sharedStringTablePart.SharedStringTable = sharedStringTable;
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                AppendChild<Sheets>(new Sheets());
            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet1"
            };
            sheets.Append(sheet);
            return Tuple.Create(memoryStream, spreadsheetDocument, workbook, worksheet, sheetData, sharedStringTable);
        }

        public string ToExcelColNum(int colNum)
        {
            string rtn = string.Empty;
            List<int> iList = new List<int>();

            //To single Int
            while (colNum / 26 != 0 || colNum % 26 != 0)
            {
                iList.Add(colNum % 26);
                colNum /= 26;
            }

            //Change 0 To 26
            for (int j = 0; j < iList.Count - 1; j++)
            {
                if (iList[j] == 0)
                {
                    iList[j + 1] -= 1;
                    iList[j] = 26;
                }
            }
            //Remove 0 at last
            if (iList[iList.Count - 1] == 0)
            {
                iList.Remove(iList[iList.Count - 1]);
            }

            //To String
            for (int j = iList.Count - 1; j >= 0; j--)
            {
                char c = (char)(iList[j] + 64);
                rtn += c.ToString();
            }

            return rtn;
        }
    }
}