using Afiniti.Framework.LoggingTracing;
using Common;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ListServiceProxy.ServiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Border = DocumentFormat.OpenXml.Spreadsheet.Border;
using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;
using Row = DocumentFormat.OpenXml.Spreadsheet.Row;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;

namespace ListServiceProxy.Helpers
{
    public class OpenXMLExcelHelper
    {
        #region Commercial
        public byte[] CreateExcelEPPlusList(FlatModelExport ResponseData)
        {
            try
            {
                return ExportDataSet(ResponseData);
            }
            catch (Exception e)
            {
                //StringCollection eMessages = new StringCollection
                //{
                //    DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ffffff") + " CommercialAPI:CreateExcelForCommercial-- Exec Entered"
                //};
                //eMessages.Add(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ffffff") + " CommercialAPI:CreateExcelForCommercial--" + e);
                //LoggingUtility.LogTrace("CommercialAPI", eMessages);
                LoggingUtility.LogException(e);

                return null;
            }
        }

        private byte[] ExportDataSet(FlatModelExport flat)
        {
            var templateStream = new MemoryStream();
            using (var workbook = SpreadsheetDocument.Create(templateStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                GenerateStylesheet(workbook);
                workbook.WorkbookPart.Workbook = new Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new Sheets();
                uint sheetId = 1;

                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                sheetPart.Worksheet = new Worksheet(sheetData);
                Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }
                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = "Commercial Gate" };
                sheets.Append(sheet);

                #region Commercial
                Row headerRow = new Row();

                if (flat?.CommercialQueues?.Count > 0)
                {
                    var data = flat.CommercialQueues;
                    SetChild(headerRow, "Account Key");
                    SetChild(headerRow, "Account Name");
                    SetChild(headerRow, "Account Status");
                    SetChild(headerRow, data[0].NDA_YesNo.AttributeName ?? "NDA Signed");
                    SetChild(headerRow, data[0].NDA_Date.AttributeName ?? "NDA Signed Date");
                    SetChild(headerRow, data[0].RelationshipPerson.AttributeName ?? "Strongest Relationship Person");
                    SetChild(headerRow, data[0].RelationshipStrength.AttributeName ?? "Strongest Relationship Level");
                    SetChild(headerRow, data[0].BoardMember.AttributeName ?? "Affiliate / Advisor");
                    SetChild(headerRow, data[0].CurrencyType.AttributeName ?? "Currency");
                    SetChild(headerRow, data[0].YearsUsed.AttributeName ?? "Year for Data Points Used");
                    SetChild(headerRow, data[0].CustomersRGUs.AttributeName ?? "Total Customers / RGUs / Relationships");
                    SetChild(headerRow, data[0].AnnualRevenues.AttributeName ?? "Annual Revenues");
                    SetChild(headerRow, data[0].GrossMargins.AttributeName ?? "Gross Margin (%)");
                    SetChild(headerRow, data[0].AnnualGrossChurn.AttributeName ?? "Percentage of Churn / Renewals through Contact Centre (%)");
                    SetChild(headerRow, data[0].AnnualGrossAdditions.AttributeName ?? "Percentage of Additions / Sales through Contact Centre (%)");
                    SetChild(headerRow, data[0].MonthlyCustomerChurn.AttributeName ?? "Annual or Monthly Customer Churn Rate (%)");
                    SetChild(headerRow, data[0].AnnualNetAdditions.AttributeName ?? "Annual Net Additional Customers / RGUs / Relationships");
                    SetChild(headerRow, "Last Comments");
                    SetChild(headerRow, "Queue Name");
                    SetChild(headerRow, "Queue Type");
                    SetChild(headerRow, "Queue Status");
                    foreach (var Item in data)
                    {
                        if (Item.AccountQueues != null && Item.AccountQueues.Count > 0)
                        {
                            int kIndex = 0;
                            SetChild(headerRow, Item.AccountQueues[kIndex].QueueACD.AttributeName ?? "ACD");
                            SetChild(headerRow, Item.AccountQueues[kIndex].GSDLead.AttributeName ?? "GSD Lead (Confirmed Feasibility)");
                            SetChild(headerRow, Item.AccountQueues[kIndex].AILead.AttributeName ?? "AI Lead (Confirmed Gain Estimate)");
                            SetChild(headerRow, Item.AccountQueues[kIndex].BALead.AttributeName ?? "BA Lead (Confirmed Optimisation Metric)");
                            SetChild(headerRow, Item.AccountQueues[kIndex].CallVolume.AttributeName ?? "Calls");
                            SetChild(headerRow, Item.AccountQueues[kIndex].PercentOn.AttributeName ?? "ON %");
                            SetChild(headerRow, Item.AccountQueues[kIndex].OptimizationMetricFeeType.AttributeName ?? "Optimisation Metric");
                            SetChild(headerRow, Item.AccountQueues[kIndex].OptimizationMetricFeeTypeValue.AttributeName ?? "Baseline value");
                            SetChild(headerRow, Item.AccountQueues[kIndex].PricingScheme.AttributeName ?? "Pricing Structure");
                            SetChild(headerRow, Item.AccountQueues[kIndex].UpliftNumber.AttributeName ?? "Uplift %");
                            SetChild(headerRow, Item.AccountQueues[kIndex].UpliftValueEstimate.AttributeName ?? "Uplift Value Estimate");
                            SetChild(headerRow, Item.AccountQueues[kIndex].PilotStrategicRationale.AttributeName ?? "Pilot Strategic Rationale");
                            SetChild(headerRow, Item.AccountQueues[kIndex].PilotPercentOn.AttributeName ?? "ON% for Pilot");
                            SetChild(headerRow, Item.AccountQueues[kIndex].PilotPricing.AttributeName ?? "Pilot Pricing");
                            SetChild(headerRow, Item.AccountQueues[kIndex].DeploymentYesNo.AttributeName ?? "New ACD Deployment");
                            SetChild(headerRow, Item.AccountQueues[kIndex].DeploymentLocation.AttributeName ?? "Location");
                            SetChild(headerRow, Item.AccountQueues[kIndex].DeploymentComplexity.AttributeName ?? "Deployment Complexity");
                            SetChild(headerRow, Item.AccountQueues[kIndex].AccountTeamLocation.AttributeName ?? "Account Team Location");
                            SetChild(headerRow, Item.AccountQueues[kIndex].AccountTeamFTE.AttributeName ?? "Average FTEs Assigned to the Account");
                            int jj = 40;
                            foreach (var forcast in Item.AccountQueues[kIndex].QueueForecast)
                            {

                                SetChild(headerRow, forcast.ForecastNumber.AttributeName);
                                jj++;
                            }
                            foreach (var forcast in Item.AccountQueues[kIndex].QueueForecast)
                            {
                                SetChild(headerRow, forcast.ForecastPhase.AttributeName);
                                jj++;
                            }
                            foreach (var forcast in Item.AccountQueues[kIndex].QueueForecast)
                            {
                                SetChild(headerRow, forcast.ForecastClientCost.AttributeName);
                                jj++;
                            }
                            foreach (var forcast in Item.AccountQueues[kIndex].QueueForecast)
                            {
                                SetChild(headerRow, forcast.ForecastThirdParty.AttributeName);
                                jj++;
                            }
                            foreach (var forcast in Item.AccountQueues[kIndex].QueueForecast)
                            {
                                SetChild(headerRow, forcast.ForecastOther.AttributeName);
                                jj++;
                            }
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    sheetData.AppendChild(headerRow);
                    foreach (var gIn in data)//data
                    {
                        Row newRow = new Row();
                        SetChild(newRow, gIn.AccountKey.ToString());
                        SetChild(newRow, gIn.AccountName);
                        SetChild(newRow, gIn.AccountStatus.ToString());
                        SetChildWithFormat(newRow, gIn.NDA_YesNo.DisplayString, gIn.NDA_YesNo.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.NDA_Date.DisplayString, gIn.NDA_Date.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.RelationshipPerson.DisplayString, gIn.RelationshipPerson.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.RelationshipStrength.DisplayString, gIn.RelationshipStrength.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.BoardMember.DisplayString, gIn.BoardMember.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.CurrencyType.DisplayString, gIn.CurrencyType.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.YearsUsed.DisplayString, gIn.YearsUsed.AttributeDataType, true);
                        SetChildWithFormat(newRow, gIn.CustomersRGUs.DisplayString, gIn.CustomersRGUs.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.AnnualRevenues.DisplayString, gIn.AnnualRevenues.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.GrossMargins.DisplayString, gIn.GrossMargins.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.AnnualGrossChurn.DisplayString, gIn.AnnualGrossChurn.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.AnnualGrossAdditions.DisplayString, gIn.AnnualGrossAdditions.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.MonthlyCustomerChurn.DisplayString, gIn.MonthlyCustomerChurn.AttributeDataType, false);
                        SetChildWithFormat(newRow, gIn.AnnualNetAdditions.DisplayString, gIn.AnnualNetAdditions.AttributeDataType, false);
                        SetChild(newRow, gIn.LastComment?.TextPlain);

                        if (gIn.AccountQueues != null && gIn.AccountQueues.Count > 0)
                        {
                            for (int k = 0; k < gIn?.AccountQueues.Count; k++)
                            {
                                SetChild(newRow, gIn.AccountQueues[k].QueueName);
                                SetChild(newRow, gIn.AccountQueues[k].QueueTypeName);
                                SetChild(newRow, gIn.AccountQueues[k].QueueStatus.ToString());

                                SetChildWithFormat(newRow, gIn.AccountQueues[k].QueueACD.DisplayString, gIn.AccountQueues[k].QueueACD.AttributeDataType, false);

                                SetChildWithFormat(newRow, gIn.AccountQueues[k].GSDLead.DisplayString, gIn.AccountQueues[k].GSDLead.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].AILead.DisplayString, gIn.AccountQueues[k].AILead.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].BALead.DisplayString, gIn.AccountQueues[k].BALead.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].CallVolume.DisplayString, gIn.AccountQueues[k].CallVolume.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].PercentOn.DisplayString, gIn.AccountQueues[k].PercentOn.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].OptimizationMetricFeeType.DisplayString, gIn.AccountQueues[k].OptimizationMetricFeeType.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].OptimizationMetricFeeTypeValue.DisplayString, gIn.AccountQueues[k].OptimizationMetricFeeTypeValue.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].PricingScheme.DisplayString, gIn.AccountQueues[k].PricingScheme.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].UpliftNumber.DisplayString, gIn.AccountQueues[k].UpliftNumber.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].UpliftValueEstimate.DisplayString, gIn.AccountQueues[k].UpliftValueEstimate.AttributeDataType, false);

                                SetChildWithFormat(newRow, gIn.AccountQueues[k].PilotStrategicRationale.DisplayString, gIn.AccountQueues[k].PilotStrategicRationale.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].PilotPercentOn.DisplayString, gIn.AccountQueues[k].PilotPercentOn.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].PilotPricing.DisplayString, gIn.AccountQueues[k].PilotPricing.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].DeploymentYesNo.DisplayString, gIn.AccountQueues[k].DeploymentYesNo.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].DeploymentLocation.DisplayString, gIn.AccountQueues[k].DeploymentLocation.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].DeploymentComplexity.DisplayString, gIn.AccountQueues[k].DeploymentComplexity.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].AccountTeamLocation.DisplayString, gIn.AccountQueues[k].AccountTeamLocation.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AccountQueues[k].AccountTeamFTE.DisplayString, gIn.AccountQueues[k].AccountTeamFTE.AttributeDataType, false);

                                int jj = 40;
                                foreach (var forcast in gIn.AccountQueues[k].QueueForecast)
                                {
                                    SetChildWithFormat(newRow, forcast.ForecastNumber.DisplayString, forcast.ForecastNumber.AttributeDataType, false);
                                    jj++;
                                }
                                foreach (var forcast in gIn.AccountQueues[k].QueueForecast)
                                {
                                    SetChildWithFormat(newRow, forcast.ForecastPhase.DisplayString, forcast.ForecastPhase.AttributeDataType, false);
                                    jj++;
                                }
                                foreach (var forcast in gIn.AccountQueues[k].QueueForecast)
                                {
                                    SetChildWithFormat(newRow, forcast.ForecastClientCost.DisplayString, forcast.ForecastClientCost.AttributeDataType, false);
                                    jj++;
                                }
                                foreach (var forcast in gIn.AccountQueues[k].QueueForecast)
                                {
                                    SetChildWithFormat(newRow, forcast.ForecastThirdParty.DisplayString, forcast.ForecastThirdParty.AttributeDataType, false);
                                    jj++;
                                }
                                foreach (var forcast in gIn.AccountQueues[k].QueueForecast)
                                {
                                    SetChildWithFormat(newRow, forcast.ForecastOther.DisplayString, forcast.ForecastOther.AttributeDataType, false);
                                    jj++;
                                }
                                sheetData.AppendChild(newRow);
                                newRow = new Row();
                                SetChild(newRow, gIn.AccountKey.ToString());
                                SetChild(newRow, gIn.AccountName);
                                SetChild(newRow, gIn.AccountStatus.ToString());
                                SetChildWithFormat(newRow, gIn.NDA_YesNo.DisplayString, gIn.NDA_YesNo.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.NDA_Date.DisplayString, gIn.NDA_Date.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.RelationshipPerson.DisplayString, gIn.RelationshipPerson.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.RelationshipStrength.DisplayString, gIn.RelationshipStrength.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.BoardMember.DisplayString, gIn.BoardMember.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.CurrencyType.DisplayString, gIn.CurrencyType.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.YearsUsed.DisplayString, gIn.YearsUsed.AttributeDataType, true);
                                SetChildWithFormat(newRow, gIn.CustomersRGUs.DisplayString, gIn.CustomersRGUs.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AnnualRevenues.DisplayString, gIn.AnnualRevenues.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.GrossMargins.DisplayString, gIn.GrossMargins.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AnnualGrossChurn.DisplayString, gIn.AnnualGrossChurn.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AnnualGrossAdditions.DisplayString, gIn.AnnualGrossAdditions.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.MonthlyCustomerChurn.DisplayString, gIn.MonthlyCustomerChurn.AttributeDataType, false);
                                SetChildWithFormat(newRow, gIn.AnnualNetAdditions.DisplayString, gIn.AnnualNetAdditions.AttributeDataType, false);
                                SetChild(newRow, gIn.LastComment?.TextPlain);
                            }
                        }
                        else
                        {
                            sheetData.AppendChild(newRow);
                        }
                    }

                }
                else
                {
                    SetChild(headerRow, "No data found, try changing filters");
                    sheetData.AppendChild(headerRow);
                }

                #endregion Commercial

                #region GenieFX
                sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                sheetData = new SheetData();
                sheetPart.Worksheet = new Worksheet(sheetData);
                sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }
                sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = "FX Rates" };
                sheets.Append(sheet);

                if (flat?.Currencies?.Count > 0)
                {
                    List<CommercialAttributes> data2 = flat.Currencies;
                    headerRow = new Row();
                    SetChild(headerRow, "FX Currency");
                    SetChild(headerRow, "FX Rate");
                    sheetData.AppendChild(headerRow);
                    foreach (var gIn in data2)
                    {
                        headerRow = new Row();
                        SetChild(headerRow, gIn.DisplayString);
                        SetChildWithFormat(headerRow, gIn.ValueString, "Currency", true);
                        sheetData.AppendChild(headerRow);
                    }
                }
                else
                {
                    headerRow = new Row();
                    SetChild(headerRow, "No data found, try changing filters");
                    sheetData.AppendChild(headerRow);
                }
                #endregion GenieFX
                workbook.Save();
            }
            templateStream.Dispose();
            return templateStream.ToArray();
        }

        #endregion Commercial

        #region General

        public static string ReplaceCommasAndDollarSign(string query)
        {
            var newQuery = new StringBuilder();
            var level = 0;

            foreach (var c in query)
            {
                if (c == ',' || c == '$')
                {
                    if (level == 0)
                    {
                        newQuery.Append("");
                    }
                    else
                    {
                        newQuery.Append(c);
                    }
                }
            }
            return newQuery.ToString();
        }

        private void SetChild(Row headerRow, string Childname)
        {
            Cell cell = new Cell
            {
                DataType = CellValues.String,
                CellValue = new CellValue(Childname)
            };
            headerRow.AppendChild(cell);
        }

        private void SetChildWithFormatExtended(Row row, string Rowvalue, string attrType, bool? Flag, string CircleValue)
        {
            Cell cell = new Cell();
            if (attrType == "DropDown" && Flag == null)  //CEO/CTO/COO mapping
            {
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue("⬤"/*Rowvalue*/);// fixed unicode character added just for safer side though Rowvalue will have it in it already
                switch (Rowvalue)
                {
                    case "0":
                        cell.StyleIndex = 7U;
                        row.AppendChild(cell);
                        return;
                    case "1":
                        cell.StyleIndex = 9U;
                        row.AppendChild(cell);
                        return;
                    case "2":
                        cell.StyleIndex = 10U;
                        row.AppendChild(cell);
                        return;
                    case "3":
                        cell.StyleIndex = 8U;
                        row.AppendChild(cell);
                        return;
                    default:
                        cell.StyleIndex = 7U;
                        row.AppendChild(cell);
                        return;
                }
            }
            //if (CircleValue == "0")//grey
            //{
            //    cell.StyleIndex = 7U;
            //}
            //else if (CircleValue == "1")//red
            //{
            //    cell.StyleIndex = 9U;
            //}
            //else if (CircleValue == "2")//yellow
            //{
            //    cell.StyleIndex = 10U;
            //}
            //else if (CircleValue == "3")//green
            //{
            //    cell.StyleIndex = 8U;
            //}
            //row.AppendChild(cell);
        }

        private void SetChildWithFormat(Row row, string Rowvalue, string attrType, bool? Flag)
        {
            if ((attrType == "Number" || attrType == "Currency" || attrType == "Percent") && !string.IsNullOrEmpty(Rowvalue))
            {
                if (Rowvalue.Contains(','))
                {
                    Rowvalue = Rowvalue.Replace(",", "");
                }
                if (Rowvalue.Contains('$'))
                {
                    Rowvalue = Rowvalue.Replace("$", "");
                }
                // Rowvalue = ReplaceCommasAndDollarSign(Rowvalue);
            }

            Cell cell = new Cell();
            if (attrType == "Number")
            {
                cell.DataType = CellValues.Number;
                cell.StyleIndex = 2U;
                cell.CellValue = new CellValue(Rowvalue);
            }
            else if (attrType == "Percent")
            {
                cell.DataType = CellValues.Number;
                cell.StyleIndex = 3U;// 5U;
                cell.CellValue = new CellValue(Rowvalue);
            }
            else if (attrType == "Currency")
            {
                if (Flag == true)//"FX Rate"
                {
                    cell.DataType = CellValues.Number;
                    cell.CellValue = new CellValue(Rowvalue);
                }
                else
                {
                    cell.DataType = CellValues.Number;
                    cell.StyleIndex = 5U;
                    cell.CellValue = new CellValue(Rowvalue);
                }
            }
            else if (attrType == "DropDown" && Flag == true)//years used
            {
                cell.DataType = CellValues.Number;
                cell.StyleIndex = 6U;
                cell.CellValue = new CellValue(Rowvalue);
            }
            else
            {
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(Rowvalue);
            }
            row.AppendChild(cell);
        }

        private Stylesheet GenerateStylesheet(SpreadsheetDocument spreadsheet)
        {
            WorkbookStylesPart stylesheet = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac x16r2" } };
            stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
            stylesheet1.AddNamespaceDeclaration("x16r2", "http://schemas.microsoft.com/office/spreadsheetml/2015/02/main");

            NumberingFormats numberingFormats1 = new NumberingFormats() { Count = (UInt32Value)2U };
            NumberingFormat numberingFormat1 = new NumberingFormat() { NumberFormatId = (UInt32Value)164U, FormatCode = "#,##0_);[Red](#,##0)" };
            NumberingFormat numberingFormat2 = new NumberingFormat() { NumberFormatId = (UInt32Value)39U, FormatCode = "#,##0.00_);[Red](#,##0.00)" };
            numberingFormats1.Append(numberingFormat1);
            numberingFormats1.Append(numberingFormat2);

            Fonts fonts1 = new Fonts() { Count = (UInt32Value)5U, KnownFonts = true };
            Font font1 = new Font();
            FontSize fontSize1 = new FontSize() { Val = 11D };
            Color color1 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName1 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };
            font1.Append(fontSize1);
            font1.Append(color1);
            font1.Append(fontName1);
            font1.Append(fontFamilyNumbering1);
            font1.Append(fontScheme1);
            fonts1.Append(font1);

            Font font2 = new Font();//green
            Color color2 = new Color() { Rgb = "29A825" };
            font2.Append(color2);
            fonts1.Append(font2);

            Font font3 = new Font();//yellow
            Color color3 = new Color() { Rgb = "FFC000" };
            font3.Append(color3);
            fonts1.Append(font3);

            Font font4 = new Font();//red
            Color color4 = new Color() { Rgb = "E9474D" };
            font4.Append(color4);
            fonts1.Append(font4);

            Font font5 = new Font();//grey
            Color color5 = new Color() { Rgb = "C8C8C8" };
            font5.Append(color5);
            fonts1.Append(font5);

            Fills fills1 = new Fills() { Count = 2U };
            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };
            fill1.Append(patternFill1);
            Fill fill2 = new Fill();
            PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };
            fill2.Append(patternFill2);
            fills1.Append(fill1);
            fills1.Append(fill2);

            Borders borders1 = new Borders() { Count = (UInt32Value)1U };
            Border border1 = new Border();
            LeftBorder leftBorder1 = new LeftBorder();
            RightBorder rightBorder1 = new RightBorder();
            TopBorder topBorder1 = new TopBorder();
            BottomBorder bottomBorder1 = new BottomBorder();
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();
            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);
            borders1.Append(border1);

            CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };
            cellStyleFormats1.Append(cellFormat1);

            CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)11U };
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)6U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyNumberFormat = true };
            CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)164U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyNumberFormat = true };
            CellFormat cellFormat3_ = new CellFormat() { NumberFormatId = (UInt32Value)10U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyNumberFormat = true };
            CellFormat cellFormat4_ = new CellFormat() { NumberFormatId = (UInt32Value)3U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyNumberFormat = true };
            CellFormat cellFormat_ = new CellFormat() { NumberFormatId = (UInt32Value)39U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyNumberFormat = true };
            CellFormat cellFormat_Simple = new CellFormat() { NumberFormatId = (UInt32Value)1U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyNumberFormat = true };

            CellFormat cellFormat_GreenCircleFiller = new CellFormat() { FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, ApplyAlignment = true };
            CellFormat cellFormat_YellowCircleFiller = new CellFormat() { FontId = (UInt32Value)2U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, ApplyAlignment = true };
            CellFormat cellFormat_RedCircleFiller = new CellFormat() { FontId = (UInt32Value)3U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, ApplyAlignment = true };
            CellFormat cellFormat_GreyCircleFiller = new CellFormat() { FontId = (UInt32Value)4U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, ApplyAlignment = true };

            Alignment HorizontalAlign_YCircle = new Alignment() { Horizontal = HorizontalAlignmentValues.Center };
            Alignment HorizontalAlign_GCircle = new Alignment() { Horizontal = HorizontalAlignmentValues.Center };
            Alignment HorizontalAlign_GreyCircle = new Alignment() { Horizontal = HorizontalAlignmentValues.Center };
            Alignment HorizontalAlign_RCircle = new Alignment() { Horizontal = HorizontalAlignmentValues.Center };

            cellFormat_GreenCircleFiller.Append(HorizontalAlign_GCircle);
            cellFormat_YellowCircleFiller.Append(HorizontalAlign_YCircle);
            cellFormat_RedCircleFiller.Append(HorizontalAlign_RCircle);
            cellFormat_GreyCircleFiller.Append(HorizontalAlign_GreyCircle);

            cellFormats1.Append(cellFormat2);
            cellFormats1.Append(cellFormat3);
            cellFormats1.Append(cellFormat4);
            cellFormats1.Append(cellFormat3_);
            cellFormats1.Append(cellFormat4_);
            cellFormats1.Append(cellFormat_);
            cellFormats1.Append(cellFormat_Simple);

            cellFormats1.Append(cellFormat_GreyCircleFiller);
            cellFormats1.Append(cellFormat_GreenCircleFiller);
            cellFormats1.Append(cellFormat_RedCircleFiller);
            cellFormats1.Append(cellFormat_YellowCircleFiller);

            CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };
            cellStyles1.Append(cellStyle1);

            DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
            TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

            StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();
            StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
            stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
            X14.SlicerStyles slicerStyles1 = new X14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };
            stylesheetExtension1.Append(slicerStyles1);

            StylesheetExtension stylesheetExtension2 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
            stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");
            OpenXmlUnknownElement openXmlUnknownElement1 = OpenXmlUnknownElement.CreateOpenXmlUnknownElement("<x15:timelineStyles defaultTimelineStyle=\"TimeSlicerStyleLight1\" xmlns:x15=\"http://schemas.microsoft.com/office/spreadsheetml/2010/11/main\" />");
            stylesheetExtension2.Append(openXmlUnknownElement1);

            stylesheetExtensionList1.Append(stylesheetExtension1);
            stylesheetExtensionList1.Append(stylesheetExtension2);

            stylesheet1.Append(numberingFormats1);
            stylesheet1.Append(fonts1);
            stylesheet1.Append(fills1);
            stylesheet1.Append(borders1);
            stylesheet1.Append(cellStyleFormats1);
            stylesheet1.Append(cellFormats1);
            stylesheet1.Append(cellStyles1);
            stylesheet1.Append(differentialFormats1);
            stylesheet1.Append(tableStyles1);
            stylesheet1.Append(stylesheetExtensionList1);
            stylesheet.Stylesheet = stylesheet1;
            stylesheet.Stylesheet.Save();
            return stylesheet1;
        }

        public byte[] CreateAndExportEmptyExcel()
        {
            var templateStream = new MemoryStream();
            using (var workbook = SpreadsheetDocument.Create(templateStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new Sheets();
                uint sheetId = 1;
                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                sheetPart.Worksheet = new Worksheet(sheetData);
                Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }
                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = "Sheet 1" };
                sheets.Append(sheet);
                Row headerRow = new Row();
                List<String> columns = new List<string>();
                columns.Add("No data found");
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue("No data found, try changing filters");
                headerRow.AppendChild(cell);
                sheetData.AppendChild(headerRow);
                workbook.Save();
            }
            templateStream.Dispose();
            return templateStream.ToArray();
        }

        #endregion General

        #region SummaryPipeline


        public byte[] CreateExcelForPipelineSummary(ViewerAndPagingOfPipelineSummaryFlatModelmGFQGkGm ResponseData)
        {
            try
            {
                return ExportDataSetForSummaryP(ResponseData);
            }
            catch (Exception e)
            {
                //StringCollection eMessages = new StringCollection
                //{
                //    DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ffffff") + " PiplineSummaryAPI:CreateExcelForPipelineSummary-- Exec Entered"
                //};
                //eMessages.Add(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ffffff") + " PiplineSummaryAPI:CreateExcelForPipelineSummary--" + e);
                //LoggingUtility.LogTrace("PiplineSummaryAPI", eMessages);           
                LoggingUtility.LogException(e);
                return null;
            }
        }

        private byte[] ExportDataSetForSummaryP(ViewerAndPagingOfPipelineSummaryFlatModelmGFQGkGm flat)
        {
            var templateStream = new MemoryStream();
            using (var workbook = SpreadsheetDocument.Create(templateStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                GenerateStylesheet(workbook);
                workbook.WorkbookPart.Workbook = new Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new Sheets();
                uint sheetId = 1;

                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                sheetPart.Worksheet = new Worksheet(sheetData);
                Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();


                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }
                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = "Pipeline Summary" };
                sheets.Append(sheet);

                #region Summary
                Row newRow = new Row();
                var data = flat.ReviewResults;
                SetChild(newRow, data[0].Country == null || string.IsNullOrEmpty(data[0].Country.AttributeName) ? "Country" : data[0].Country.AttributeName);
                SetChild(newRow, data[0].AccountLead == null || string.IsNullOrEmpty(data[0].AccountLead.AttributeName) ? "Account Lead" : data[0].AccountLead.AttributeName);
                SetChild(newRow, data[0].Account == null || string.IsNullOrEmpty(data[0].Account.AttributeName) ? "Account" : data[0].Account.AttributeName);
                SetChild(newRow, data[0].AccountStatus == null || string.IsNullOrEmpty(data[0].AccountStatus.AttributeName) ? "Account Status" : data[0].AccountStatus.AttributeName);
                SetChild(newRow, data[0].Queue == null || string.IsNullOrEmpty(data[0].Queue.AttributeName) ? "Queue Name" : data[0].Queue.AttributeName);
                SetChild(newRow, data[0].QueueStatus == null || string.IsNullOrEmpty(data[0].QueueStatus.AttributeName) ? "Queue Status" : data[0].QueueStatus.AttributeName);
                SetChild(newRow, data[0].ScopeGate == null || string.IsNullOrEmpty(data[0].ScopeGate.AttributeName) ? "Scope Gate" : data[0].ScopeGate.AttributeName);
                SetChild(newRow, data[0].DesignGate == null || string.IsNullOrEmpty(data[0].DesignGate.AttributeName) ? "Design Gate" : data[0].DesignGate.AttributeName);
                SetChild(newRow, data[0].TargetGoLive == null || string.IsNullOrEmpty(data[0].TargetGoLive.AttributeName) ? "Target Go-Live" : data[0].TargetGoLive.AttributeName);
                SetChild(newRow, data[0].ActualGoLive == null || string.IsNullOrEmpty(data[0].ActualGoLive.AttributeName) ? "Actual Go-Live" : data[0].ActualGoLive.AttributeName);
                SetChild(newRow, data[0].LegalStatus == null || string.IsNullOrEmpty(data[0].LegalStatus.AttributeName) ? "Legal Status" : data[0].LegalStatus.AttributeName);
                SetChild(newRow, data[0].AccountContract == null || string.IsNullOrEmpty(data[0].AccountContract.AttributeName) ? "Account Contract" : data[0].AccountContract.AttributeName);
                SetChild(newRow, data[0].QueueContract == null || string.IsNullOrEmpty(data[0].QueueContract.AttributeName) ? "Queue Contract" : data[0].QueueContract.AttributeName);
                SetChild(newRow, data[0].LongtermCommercials == null || string.IsNullOrEmpty(data[0].LongtermCommercials.AttributeName) ? "Longterm Commercials" : data[0].LongtermCommercials.AttributeName);
                SetChild(newRow, data[0].AgreementExpiration == null || string.IsNullOrEmpty(data[0].AgreementExpiration.AttributeName) ? "Agreement Expiration" : data[0].AgreementExpiration.AttributeName);
                SetChild(newRow, data[0].ContractOnPercent == null || string.IsNullOrEmpty(data[0].ContractOnPercent.AttributeName) ? "Contract ON %" : data[0].ContractOnPercent.AttributeName);
                SetChild(newRow, data[0].ClientBusinessCase == null || string.IsNullOrEmpty(data[0].ClientBusinessCase.AttributeName) ? "Client business" : data[0].ClientBusinessCase.AttributeName);
                SetChild(newRow, data[0].AfinitiBusinessCase == null || string.IsNullOrEmpty(data[0].AfinitiBusinessCase.AttributeName) ? "Afiniti business" : data[0].AfinitiBusinessCase.AttributeName);
                SetChild(newRow, data[0].CC1Status == null || string.IsNullOrEmpty(data[0].CC1Status.AttributeName) ? "CC1 Status" : data[0].CC1Status.AttributeName);
                SetChild(newRow, data[0].CC2Status == null || string.IsNullOrEmpty(data[0].CC2Status.AttributeName) ? "CC2 Status" : data[0].CC2Status.AttributeName);
                sheetData.AppendChild(newRow);

                foreach (var gIn in data)//data
                {
                    newRow = new Row();
                    SetChildWithFormat(newRow, gIn.Country == null ? "" : gIn.Country.DisplayString, gIn.Country == null ? "" : gIn.Country.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AccountLead == null ? "" : gIn.AccountLead.DisplayString, gIn.AccountLead == null ? "" : gIn.AccountLead.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.Account == null ? "" : gIn.Account.DisplayString, gIn.Account == null ? "" : gIn.Account.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AccountStatus == null ? "" : gIn.AccountStatus.DisplayString, gIn.AccountStatus == null ? "" : gIn.AccountStatus.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.Queue == null ? "" : gIn.Queue.DisplayString, gIn.Queue == null ? "" : gIn.Queue.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.QueueStatus == null ? "" : gIn.QueueStatus.DisplayString, gIn.QueueStatus == null ? "" : gIn.QueueStatus.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.ScopeGate == null ? "" : gIn.ScopeGate.DisplayString, gIn.ScopeGate == null ? "" : gIn.ScopeGate.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.DesignGate == null ? "" : gIn.DesignGate.DisplayString, gIn.DesignGate == null ? "" : gIn.DesignGate.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.TargetGoLive == null ? "" : gIn.TargetGoLive.DisplayString, gIn.TargetGoLive == null ? "" : gIn.TargetGoLive.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.ActualGoLive == null ? "" : gIn.ActualGoLive.DisplayString, gIn.ActualGoLive == null ? "" : gIn.ActualGoLive.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.LegalStatus == null ? "" : gIn.LegalStatus.DisplayString, gIn.LegalStatus == null ? "" : gIn.LegalStatus.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AccountContract == null ? "" : gIn.AccountContract.DisplayString, gIn.AccountContract == null ? "" : gIn.AccountContract.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.QueueContract == null ? "" : gIn.QueueContract.DisplayString, gIn.QueueContract == null ? "" : gIn.QueueContract.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.LongtermCommercials == null ? "" : gIn.LongtermCommercials.DisplayString, gIn.LongtermCommercials == null ? "" : gIn.LongtermCommercials.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AgreementExpiration == null ? "" : gIn.AgreementExpiration.DisplayString, gIn.AgreementExpiration == null ? "" : gIn.AgreementExpiration.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.ContractOnPercent == null ? "" : gIn.ContractOnPercent.DisplayString, gIn.ContractOnPercent == null ? "" : gIn.ContractOnPercent.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.ClientBusinessCase == null ? "" : gIn.ClientBusinessCase.DisplayString, gIn.ClientBusinessCase == null ? "" : gIn.ClientBusinessCase.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AfinitiBusinessCase == null ? "" : gIn.AfinitiBusinessCase.DisplayString, gIn.AfinitiBusinessCase == null ? "" : gIn.AfinitiBusinessCase.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.CC1Status == null ? "" : gIn.CC1Status.DisplayString, gIn.CC1Status == null ? "" : gIn.CC1Status.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.CC2Status == null ? "" : gIn.CC2Status.DisplayString, gIn.CC2Status == null ? "" : gIn.CC2Status.AttributeDataType, false);
                    sheetData.AppendChild(newRow);
                }
                #endregion Summary
                workbook.Save();
            }
            templateStream.Dispose();
            return templateStream.ToArray();
        }

        #endregion SummaryPipeline

        #region AiRo

        public byte[] CreateExcelForAiRo(ViewerAndPagingOfAvayaAccountsFlatModelmGFQGkGm pImportData)
        {
            try
            {
                return ExportDataSetForAiRo(pImportData);
            }
            catch (Exception e)
            {
                LoggingUtility.LogException(e);
                return null;
            }
        }

        public byte[] ExportDataSetForAiRo(ViewerAndPagingOfAvayaAccountsFlatModelmGFQGkGm pFlatModel)
        {
            ApplicationTrace.Log("AiRoSalesExport:ExportDataSetForAiRo", Status.Started);
            var templateStream = new MemoryStream();
            using (var workbook = SpreadsheetDocument.Create(templateStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                GenerateStylesheet(workbook);
                workbook.WorkbookPart.Workbook = new Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new Sheets();
                uint sheetId = 1;

                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                sheetPart.Worksheet = new Worksheet(sheetData);
                Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();

                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }
                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = "AiRoSales" };
                sheets.Append(sheet);

                #region AiRO
                Row newRow = new Row();
                var data = pFlatModel.ReviewResults;
                SetChild(newRow, data[0].Account == null || string.IsNullOrEmpty(data[0].Account.AttributeName) ? "Account" : data[0].Account.AttributeName);
                SetChild(newRow, data[0].Priority == null || string.IsNullOrEmpty(data[0].Priority.AttributeName) ? "Priority" : data[0].Priority.AttributeName);
                SetChild(newRow, data[0].LeadWith == null || string.IsNullOrEmpty(data[0].LeadWith.AttributeName) ? "Lead With" : data[0].LeadWith.AttributeName);
                SetChild(newRow, data[0].AccountStatus == null || string.IsNullOrEmpty(data[0].AccountStatus.AttributeName) ? "Account Status" : data[0].AccountStatus.AttributeName);
                SetChild(newRow, data[0].AccountPhase == null || string.IsNullOrEmpty(data[0].AccountPhase.AttributeName) ? "Account Phase" : data[0].AccountPhase.AttributeName);
                SetChild(newRow, data[0].FirstQueue == null || string.IsNullOrEmpty(data[0].FirstQueue.AttributeName) ? "Target Queue" : data[0].FirstQueue.AttributeName);
                SetChild(newRow, data[0].FirstQueueStatus == null || string.IsNullOrEmpty(data[0].FirstQueueStatus.AttributeName) ? "Target Queue Status" : data[0].FirstQueueStatus.AttributeName);
                SetChild(newRow, data[0].CEOMeeting == null || string.IsNullOrEmpty(data[0].CEOMeeting.AttributeName) ? "CEO (Sponsor)" : data[0].CEOMeeting.AttributeName);
                SetChild(newRow, data[0].CTOMeeting == null || string.IsNullOrEmpty(data[0].CTOMeeting.AttributeName) ? "CTO (How)" : data[0].CTOMeeting.AttributeName);
                SetChild(newRow, data[0].COOMeeting == null || string.IsNullOrEmpty(data[0].COOMeeting.AttributeName) ? "COO (KPIs)" : data[0].COOMeeting.AttributeName);
                SetChild(newRow, data[0].AccountLead == null || string.IsNullOrEmpty(data[0].AccountLead.AttributeName) ? "Account Lead" : data[0].AccountLead.AttributeName);
                SetChild(newRow, data[0].Vertical == null || string.IsNullOrEmpty(data[0].Vertical.AttributeName) ? "Vertical" : data[0].Vertical.AttributeName);
                SetChild(newRow, data[0].Segment == null || string.IsNullOrEmpty(data[0].Segment.AttributeName) ? "Segment" : data[0].Segment.AttributeName);
                SetChild(newRow, data[0].Seats == null || string.IsNullOrEmpty(data[0].Seats.AttributeName) ? "Seats" : data[0].Seats.AttributeName);
                SetChild(newRow, data[0].ACD == null || string.IsNullOrEmpty(data[0].ACD.AttributeName) ? "ACD" : data[0].ACD.AttributeName);
                SetChild(newRow, data[0].AvayaRelease == null || string.IsNullOrEmpty(data[0].AvayaRelease.AttributeName) ? "Avaya Release" : data[0].AvayaRelease.AttributeName);
                SetChild(newRow, data[0].AvayaAreaLeaders == null || string.IsNullOrEmpty(data[0].AvayaAreaLeaders.AttributeName) ? "Avaya Area Leaders" : data[0].AvayaAreaLeaders.AttributeName);
                SetChild(newRow, data[0].AvayaRegionalSalesLeaders == null || string.IsNullOrEmpty(data[0].AvayaRegionalSalesLeaders.AttributeName) ? "Avaya Regional Sales Leaders" : data[0].AvayaRegionalSalesLeaders.AttributeName);
                SetChild(newRow, data[0].AvayaAE == null || string.IsNullOrEmpty(data[0].AvayaAE.AttributeName) ? "Avaya AE" : data[0].AvayaAE.AttributeName);
                SetChild(newRow, data[0].AvayaSE == null || string.IsNullOrEmpty(data[0].AvayaSE.AttributeName) ? "Avaya SE" : data[0].AvayaSE.AttributeName);
                SetChild(newRow, data[0].AvayaCSD == null || string.IsNullOrEmpty(data[0].AvayaCSD.AttributeName) ? "Avaya CSD" : data[0].AvayaCSD.AttributeName);
                SetChild(newRow, data[0].JAPS == null || string.IsNullOrEmpty(data[0].JAPS.AttributeName) ? "JAPS#" : data[0].JAPS.AttributeName);
                SetChild(newRow, data[0].AvayaPartner == null || string.IsNullOrEmpty(data[0].AvayaPartner.AttributeName) ? "Avaya Partner" : data[0].AvayaPartner.AttributeName);
                SetChild(newRow, data[0].Region == null || string.IsNullOrEmpty(data[0].Region.AttributeName) ? "Region" : data[0].Region.AttributeName);
                SetChild(newRow, data[0].Airport == null || string.IsNullOrEmpty(data[0].Airport.AttributeName) ? "Airport" : data[0].AvayaNotes.AttributeName);
                SetChild(newRow, data[0].AvayaNotes == null || string.IsNullOrEmpty(data[0].AvayaNotes.AttributeName) ? "Avaya Notes" : data[0].AvayaNotes.AttributeName);
                SetChild(newRow, data[0].SalesNotes == null || string.IsNullOrEmpty(data[0].SalesNotes.AttributeName) ? "Sales Notes" : data[0].SalesNotes.AttributeName);
                sheetData.AppendChild(newRow);
                foreach (var gIn in data)//data
                {
                    newRow = new Row();
                    SetChildWithFormat(newRow, gIn.Account == null ? "" : gIn.Account.DisplayString, gIn.Account == null ? "" : gIn.Account.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.Priority == null ? "" : gIn.Priority.DisplayString, gIn.Priority == null ? "" : gIn.Priority.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.LeadWith == null ? "" : gIn.LeadWith.DisplayString, gIn.LeadWith == null ? "" : gIn.LeadWith.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AccountStatus == null ? "" : gIn.AccountStatus.DisplayString, gIn.AccountStatus == null ? "" : gIn.AccountStatus.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AccountPhase == null ? "" : gIn.AccountPhase.DisplayString, gIn.AccountPhase == null ? "" : gIn.AccountPhase.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.FirstQueue == null ? "" : gIn.FirstQueue.DisplayString, gIn.FirstQueue == null ? "" : gIn.FirstQueue.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.FirstQueueStatus == null ? "" : gIn.FirstQueueStatus.DisplayString, gIn.FirstQueueStatus == null ? "" : gIn.FirstQueueStatus.AttributeDataType, false);
                    SetChildWithFormatExtended(newRow, gIn.CEOMeeting == null ? "" : gIn.CEOMeeting.DisplayString, gIn.CEOMeeting == null ? "" : gIn.CEOMeeting.AttributeDataType, null, gIn.CEOMeeting?.ValueString);
                    SetChildWithFormatExtended(newRow, gIn.CTOMeeting == null ? "" : gIn.CTOMeeting.DisplayString, gIn.CTOMeeting == null ? "" : gIn.CTOMeeting.AttributeDataType, null, gIn.CTOMeeting?.ValueString);
                    SetChildWithFormatExtended(newRow, gIn.COOMeeting == null ? "" : gIn.COOMeeting.DisplayString, gIn.COOMeeting == null ? "" : gIn.COOMeeting.AttributeDataType, null, gIn.COOMeeting?.ValueString);
                    SetChildWithFormat(newRow, gIn.AccountLead == null ? "" : gIn.AccountLead.DisplayString, gIn.AccountLead == null ? "" : gIn.AccountLead.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.Vertical == null ? "" : gIn.Vertical.DisplayString, gIn.Vertical == null ? "" : gIn.Vertical.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.Segment == null ? "" : gIn.Segment.DisplayString, gIn.Segment == null ? "" : gIn.Segment.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.Seats == null ? "" : gIn.Seats.DisplayString, gIn.Seats == null ? "" : gIn.Seats.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.ACD == null ? "" : gIn.ACD.DisplayString, gIn.ACD == null ? "" : gIn.ACD.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AvayaRelease == null ? "" : gIn.AvayaRelease.DisplayString, gIn.AvayaRelease == null ? "" : gIn.AvayaRelease.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AvayaAreaLeaders == null ? "" : gIn.AvayaAreaLeaders.DisplayString, gIn.AvayaAreaLeaders == null ? "" : gIn.AvayaAreaLeaders.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AvayaRegionalSalesLeaders == null ? "" : gIn.AvayaRegionalSalesLeaders.DisplayString, gIn.AvayaRegionalSalesLeaders == null ? "" : gIn.AvayaRegionalSalesLeaders.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AvayaAE == null ? "" : gIn.AvayaAE.DisplayString, gIn.AvayaAE == null ? "" : gIn.AvayaAE.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AvayaSE == null ? "" : gIn.AvayaSE.DisplayString, gIn.AvayaSE == null ? "" : gIn.AvayaSE.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AvayaCSD == null ? "" : gIn.AvayaCSD.DisplayString, gIn.AvayaCSD == null ? "" : gIn.AvayaCSD.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.JAPS == null ? "" : gIn.JAPS.DisplayString, gIn.JAPS == null ? "" : gIn.JAPS.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AvayaPartner == null ? "" : gIn.AvayaPartner.DisplayString, gIn.AvayaPartner == null ? "" : gIn.AvayaPartner.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.Region == null ? "" : gIn.Region.DisplayString, gIn.Region == null ? "" : gIn.Region.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.Airport == null ? "" : gIn.Airport.DisplayString, gIn.Airport == null ? "" : gIn.Airport.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.AvayaNotes == null ? "" : gIn.AvayaNotes.DisplayString, gIn.AvayaNotes == null ? "" : gIn.AvayaNotes.AttributeDataType, false);
                    SetChildWithFormat(newRow, gIn.SalesNotes == null ? "" : gIn.SalesNotes.DisplayString, gIn.SalesNotes == null ? "" : gIn.SalesNotes.AttributeDataType, false);

                    sheetData.AppendChild(newRow);
                }
                #endregion AiRO
                workbook.Save();
            }
            templateStream.Dispose();
            ApplicationTrace.Log("AiRoSalesExport:ExportDataSetForAiRo", Status.Completed);
            return templateStream.ToArray();
        }

        #endregion AiRo




    }
}
