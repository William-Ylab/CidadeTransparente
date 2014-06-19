using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Lib.Entities;

namespace Lib.Utils
{

    /// <summary>
    /// Summary description for ExcelUtils
    /// </summary>
    public class ExcelUtils
    {

        private const string FORMAT_BOLD_TYPE = "B";
        private const string FORMAT_ITALIC_TYPE = "I";
        private const string FORMAT_UNDERLINE_TYPE = "U";
        private const string OPEN_TAG_BOLD = "&lt;b&gt;";
        private const string CLOSE_TAG_BOLD = "&lt;/b&gt;";
        private const string OPEN_TAG_ITALIC = "&lt;i&gt;";
        private const string CLOSE_TAG_ITALIC = "&lt;/i&gt;";
        private const string OPEN_TAG_UNDERLINE = "&lt;u&gt;";
        private const string CLOSE_TAG_UNDERLINE = "&lt;/u&gt;";


        private const string PROTECT_SHEET_PASSWORD = "#fdt2013#";

        public ExcelUtils()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private String[] actions = new String[] { "", "0", "0.25", "0.5", "0.75", "1", "N/A" };

        #region [Public Methods]

        public bool createExcel(BaseForm baseForm, string filePath, bool protectSheets = false, long responseFormId = 0)
        {
            if (baseForm != null)
            {
                XSSFWorkbook wb = translateFormToWorkBook(baseForm, protectSheets, responseFormId);

                FileStream file = new FileStream(filePath, FileMode.Create);
                wb.Write(file);
                file.Close();

                return true;
            }

            return false;
        }

        public System.Data.DataSet importExcelFile(string filePath)
        {
            XSSFWorkbook wb = null;
            DataSet result = new DataSet();
            DataTable dt = new DataTable();

            //Abre o arquivo de excel.
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                wb = new XSSFWorkbook(file);
                file.Close();
            }

            //Cria o datable
            ISheet sheet = wb.GetSheetAt(0);
            dt = new DataTable();
            dt.TableName = sheet.SheetName;

            //Cria as 6 colunas no datable
            //1- Indice da pergunta
            //2- Pergunta
            //3- Adiciona o campo com dropdown e libera para edição
            //4- Observações
            //5- Tipo da linha
            //6- Id da pergunta
            //7- Id do formulario
            for (int i = 0; i < 8; i++)
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + i).ToString());
            }

            //Percorre as linhas do excel
            System.Collections.IEnumerator enumerator = sheet.GetRowEnumerator();

            while (enumerator.MoveNext())
            {
                XSSFRow currentRow = (XSSFRow)enumerator.Current;
                DataRow dr = dt.NewRow();

                string typeRow = currentRow.GetCell(4).ToString();

                //Se for pergunta
                if (typeRow.ToUpper() == "P")
                {
                    for (int i = 0; i < currentRow.LastCellNum; i++)
                    {
                        ICell cell = currentRow.GetCell(i);

                        if (cell == null)
                            dr[i] = null;
                        else
                            dr[i] = cell.ToString();
                    }

                    dt.Rows.Add(dr);
                }
            }

            result.Tables.Add(dt);

            return result;
        }

        #endregion

        #region [Private Methods]

        private XSSFWorkbook createWorkBook()
        {
            XSSFWorkbook xssfWorkbook = new XSSFWorkbook();

            //create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Amarribo";

            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Questionário Transparência";

            return xssfWorkbook;
        }

        //private BaseForm getCurrentForm()
        //{
        //    BaseForm baseForm = null;

        //    using (Repositories.BaseFormRepository repository = new Repositories.BaseFormRepository(null))
        //    {
        //        baseForm = repository.getInstanceByPeriodDate(DateTime.Now);
        //    }

        //    return baseForm;
        //}

        /// <summary>
        /// Traduz de Questionário da Amarribo para arquivo excel (Workbook)
        /// </summary>
        /// <param name="baseForm">Questionário de transparência</param>
        /// <param name="protectSheets">Flag de proteção do questionário</param>
        /// <returns></returns>
        private XSSFWorkbook translateFormToWorkBook(BaseForm baseForm, bool protectSheets, long rfId = 0)
        {
            try
            {
                XSSFWorkbook wb = null;

                if (baseForm != null)
                {
                    wb = createWorkBook();

                    IFont titleFont = wb.CreateFont();
                    IFont subTitleFont = wb.CreateFont();
                    IFont titleQuestion = wb.CreateFont();
                    titleFont.FontHeight = 20;

                    subTitleFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                    subTitleFont.FontHeight = 15;

                    titleQuestion.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

                    ICellStyle cellTitleStyle = wb.CreateCellStyle();
                    //cellTitleStyle.Alignment = HorizontalAlignment.Center;
                    cellTitleStyle.SetFont(titleFont);

                    ICellStyle cellSubTitleStyle = wb.CreateCellStyle();
                    cellSubTitleStyle.SetFont(subTitleFont);
                    //cellSubTitleStyle.Alignment = HorizontalAlignment.Center;

                    ICellStyle cellTitleQuestionStyle = wb.CreateCellStyle();
                    cellTitleQuestionStyle.SetFont(titleQuestion);

                    ICellStyle cellQuestion = wb.CreateCellStyle();
                    cellQuestion.WrapText = true;

                    ICellStyle cellAnswer = wb.CreateCellStyle();
                    cellAnswer.IsLocked = false;

                    ICellStyle cellHidden = wb.CreateCellStyle();
                    cellHidden.IsHidden = true;

                    if (baseForm.BaseBlocks != null)
                    {
                        //Será criada uma sheet única para todas as perguntas
                        XSSFSheet sheet = null;

                        //Total rows no atual bloco
                        int row = 0;
                        //Titulo da aba tem limite de 30 caracteres.
                        var sheetName = baseForm.Name.Length > 30 ? baseForm.Name.Substring(0, 30) : baseForm.Name;
                        sheet = (XSSFSheet)wb.CreateSheet(Commons.StringUtils.removeSpecialCaracters(sheetName));

                        if (protectSheets)
                            sheet.ProtectSheet(PROTECT_SHEET_PASSWORD);

                        if (baseForm.BaseBlocks != null)
                        {
                            if (baseForm.BaseBlocks.Count > 0)
                            {
                                int count = 1;
                                foreach (var bb in baseForm.BaseBlocks)
                                {
                                    if (bb.BaseSubBlocks != null && bb.BaseSubBlocks.Count > 0)
                                    {
                                        sheet.CreateRow(row).CreateCell(0).SetCellValue(bb.Name);
                                        sheet.GetRow(row).GetCell(0).CellStyle = cellTitleStyle;

                                        sheet.GetRow(row).CreateCell(4).SetCellValue("B");

                                        //Merge celula do titulo
                                        sheet.AddMergedRegion(new CellRangeAddress(row, row, 0, 3));

                                        row++;
                                        //Para cada subbloco
                                        foreach (var bsb in bb.BaseSubBlocks)
                                        {
                                            sheet.CreateRow(row).CreateCell(0).SetCellValue(bsb.Name);
                                            sheet.GetRow(row).GetCell(0).CellStyle = cellSubTitleStyle;

                                            sheet.GetRow(row).CreateCell(4).SetCellValue("S");

                                            //Merge celula do titulo
                                            sheet.AddMergedRegion(new CellRangeAddress(row, row, 0, 3));

                                            row++;

                                            //Adiciona os subtitulos
                                            sheet.CreateRow(row).CreateCell(0).SetCellValue("Índice");
                                            sheet.GetRow(row).GetCell(0).CellStyle = cellTitleQuestionStyle;

                                            sheet.GetRow(row).CreateCell(1).SetCellValue("Pergunta");
                                            sheet.GetRow(row).GetCell(1).CellStyle = cellTitleQuestionStyle;

                                            sheet.GetRow(row).CreateCell(2).SetCellValue("Resposta");
                                            sheet.GetRow(row).GetCell(2).CellStyle = cellTitleQuestionStyle;

                                            sheet.GetRow(row).CreateCell(3).SetCellValue("Observações");
                                            sheet.GetRow(row).GetCell(3).CellStyle = cellTitleQuestionStyle;

                                            sheet.GetRow(row).CreateCell(4).SetCellValue("TP");


                                            row++;
                                            if (bsb.BaseQuestions != null && bsb.BaseQuestions.Count > 0)
                                            {
                                                //Utilizado para gerar com respostas
                                                //var rand = new Random(DateTime.Now.Millisecond);
                                                int initRow = row;
                                                foreach (var bq in bsb.BaseQuestions)
                                                {
                                                    Entities.Answer answer = null;

                                                    if (rfId > 0)
                                                    {
                                                        answer = bq.Answers.Where(f => f.ResponseFormId == rfId).FirstOrDefault();
                                                    }

                                                    //Indice da pergunta
                                                    sheet.CreateRow(row).CreateCell(0).SetCellValue(count);

                                                    //Pergunta
                                                    //sheet.GetRow(row).CreateCell(1).SetCellValue(bq.Question);
                                                    ICell cellQ = sheet.GetRow(row).CreateCell(1);
                                                    cellQ.CellStyle = cellQuestion;

                                                    formatStyleQuestion(bq.Question, ref cellQ, ref wb);

                                                    //Adiciona o campo com dropdown e libera para edição
                                                    //int r = rand.Next(1, 4);
                                                    if (answer != null && rfId > 0)
                                                    {
                                                        if (answer.Score.HasValue)
                                                        {
                                                            sheet.GetRow(row).CreateCell(2).SetCellValue(answer.Score.Value.ToString());//("");//(actions[r])
                                                        }
                                                        else
                                                        {
                                                            sheet.GetRow(row).CreateCell(2).SetCellValue("N/A");//("");//(actions[r])
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sheet.GetRow(row).CreateCell(2).SetCellValue("");//("");//(actions[r])
                                                    }

                                                    
                                                    sheet.GetRow(row).GetCell(2).CellStyle = cellAnswer;

                                                    //observações.
                                                    sheet.GetRow(row).CreateCell(3).SetCellValue("");
                                                    sheet.GetRow(row).GetCell(3).CellStyle = cellAnswer;

                                                    //Tipo da linha
                                                    sheet.GetRow(row).CreateCell(4).SetCellValue("P");
                                                    sheet.GetRow(row).GetCell(4).CellStyle = cellHidden;

                                                    //Adiciona o id da pergunta
                                                    sheet.GetRow(row).CreateCell(5).SetCellValue(bq.Id);
                                                    sheet.GetRow(row).GetCell(5).CellStyle = cellHidden;

                                                    //Id do formulario
                                                    sheet.GetRow(row).CreateCell(6).SetCellValue(bq.BaseSubBlock.BaseBlock.BaseFormId);
                                                    sheet.GetRow(row).GetCell(6).CellStyle = cellHidden;

                                                    //Id do responseForm
                                                    sheet.GetRow(row).CreateCell(7).SetCellValue(rfId);
                                                    sheet.GetRow(row).GetCell(7).CellStyle = cellHidden;

                                                    row++;
                                                    count++;
                                                }


                                                XSSFDataValidationHelper dvHelper = new XSSFDataValidationHelper(sheet);
                                                XSSFDataValidationConstraint constraint = (XSSFDataValidationConstraint)dvHelper.CreateExplicitListConstraint(actions);
                                                CellRangeAddressList addressList = new CellRangeAddressList(initRow, row - 1, 2, 2);
                                                XSSFDataValidation validation = (XSSFDataValidation)dvHelper.CreateValidation(constraint, addressList);
                                                validation.ShowErrorBox = true;
                                                validation.CreateErrorBox("Valor inválido", "Valor inserido não é permitido");
                                                sheet.AddValidationData(validation);
                                            }
                                        }
                                    }
                                }
                            }
                        }


                        sheet.SetColumnWidth(1, 25500);
                        sheet.SetColumnWidth(3, 25500);
                        sheet.SetColumnWidth(4, 0); //0 de width para esconder a coluna do id, NPOI não tem solução para hide de coluna
                        sheet.SetColumnWidth(5, 0); //0 de width para esconder a coluna do id, NPOI não tem solução para hide de coluna
                        sheet.SetColumnWidth(6, 0); //0 de width para esconder a coluna do id, NPOI não tem solução para hide de coluna
                        sheet.SetColumnWidth(7, 0); //0 de width para esconder a coluna do id, NPOI não tem solução para hide de coluna

                    }
                }

                return wb;
            }
            catch (Exception ex)
            {
                Lib.Log.ErrorLog.saveError("Lib.Utils.ExcelUtils.translateFormToWorkBook", ex);
                throw ex;
            }
        }

        private void formatStyleQuestion(string p, ref ICell cell, ref XSSFWorkbook wb)
        {
            List<FormatExcelTextAux> list = new List<FormatExcelTextAux>();

            string question = p.Replace("&lt;br&gt;", System.Environment.NewLine).Replace("&lt;br/&gt;", System.Environment.NewLine); ;

            getStyleListFromHTMLTags(ref question, ref list);

            //Atribui o texto sem HTML para celula
            cell.SetCellValue(question);

            //Aplica a fonte com os estilos
            foreach (var i in list)
            {
                IFont font = wb.CreateFont();

                if (i.FormatType.Contains(FORMAT_BOLD_TYPE))
                    font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

                if (i.FormatType.Contains(FORMAT_ITALIC_TYPE))
                    font.IsItalic = true;

                if (i.FormatType.Contains(FORMAT_UNDERLINE_TYPE))
                    font.Underline = FontUnderlineType.Single;


                cell.RichStringCellValue.ApplyFont(i.OpenTagIndex, i.OpenTagIndex + i.Length, font);
            }
        }

        private void getStyleListFromHTMLTags(ref string question, ref List<FormatExcelTextAux> list)
        {
            int openTagIndex = -1;
            int closeTagIndex = -1;
            List<string> formatTypes = null;
            string currentOpenTag = null;
            string innerCurrentOpenTag = null;

            //Busca o range para aplicar os estilos
            while ((currentOpenTag = containsInnerTags(question)) != null)
            {
                formatTypes = new List<string>();

                getIndexesByTag(question, out openTagIndex, out closeTagIndex, currentOpenTag);

                string currentCloseTag = getCloseTagByOpenTag(currentOpenTag);

                //Recupera o texto que será sobrescrito do inicio da tag até o fim da tag de fechar
                var block = question.Substring(openTagIndex, (closeTagIndex + currentCloseTag.Length) - openTagIndex);
                var newBlock = block.Replace(currentOpenTag, "").Replace(currentCloseTag, "");

                while ((innerCurrentOpenTag = containsInnerTags(newBlock)) != null)
                {
                    int openInnerTagIndex = -1;
                    int closeInnerTagIndex = -1;
                    string innerCurrentCloseTag = getCloseTagByOpenTag(innerCurrentOpenTag);
                    string currentFormatType = getFormatTypeByTag(innerCurrentOpenTag);

                    getIndexesByTag(newBlock, out openInnerTagIndex, out closeInnerTagIndex, innerCurrentOpenTag);

                    //Verifica se existe uma das tags tratadas
                    if (newBlock.Contains(innerCurrentOpenTag) && newBlock.Contains(innerCurrentCloseTag))
                    {
                        //Enquanto existir tags internas
                        openInnerTagIndex = newBlock.IndexOf(innerCurrentOpenTag);
                        closeInnerTagIndex = newBlock.IndexOf(innerCurrentCloseTag, openInnerTagIndex);

                        //Recupera o texto que será sobrescrito do inicio da tag até o fim da tag de fechar
                        var innerBlock = newBlock.Substring(openInnerTagIndex, (closeInnerTagIndex + innerCurrentCloseTag.Length) - openInnerTagIndex);
                        newBlock = innerBlock.Replace(innerCurrentOpenTag, "").Replace(innerCurrentCloseTag, "");

                        question = question.Replace(innerBlock, newBlock);

                        formatTypes.Add(currentFormatType);
                    }
                    else
                    {
                        //Tag quebrada remove ela
                        newBlock = newBlock.Replace(innerCurrentOpenTag, "").Replace(innerCurrentCloseTag, "");
                    }
                }

                getIndexesByTag(question, out openTagIndex, out closeTagIndex, currentOpenTag);

                //Recupera o texto que será sobrescrito do inicio da tag até o fim da tag de fechar
                block = question.Substring(openTagIndex, (closeTagIndex + currentCloseTag.Length) - openTagIndex);
                newBlock = block.Replace(currentOpenTag, "").Replace(currentCloseTag, "");
                formatTypes.Add(getFormatTypeByTag(currentOpenTag));

                list.Add(new FormatExcelTextAux()
                {
                    FormatType = formatTypes.ToArray(),
                    OpenTagIndex = openTagIndex,
                    CloseTagIndex = closeTagIndex,
                    Length = closeTagIndex - (openTagIndex + currentOpenTag.Length)
                });

                //Retiro do bloco tratado as tags HTML
                question = question.Replace(block, newBlock);

                adjustListFromIndex(openTagIndex, (block.Length - newBlock.Length), ref list);
            }
        }

        private void getIndexesByTag(string text, out int openTagIndex, out int closeTagIndex, string openTag)
        {
            openTagIndex = text.IndexOf(openTag);

            string closeTag = "";
            switch (openTag)
            {
                case OPEN_TAG_BOLD:
                    closeTag = CLOSE_TAG_BOLD;
                    break;
                case OPEN_TAG_ITALIC:
                    closeTag = CLOSE_TAG_ITALIC;
                    break;
                case OPEN_TAG_UNDERLINE:
                    closeTag = CLOSE_TAG_UNDERLINE;
                    break;
                default:
                    throw new ApplicationException("Tag desconhecida");
                    break;
            }

            //Procura a tag de fechar
            closeTagIndex = text.IndexOf(closeTag, openTagIndex);
        }

        private string containsInnerTags(string block)
        {
            List<int> indexes = new List<int>();
            int indexBold = -1, indexUnderline = -1, indexItalic = -1;

            indexBold = block.IndexOf(OPEN_TAG_BOLD);
            indexItalic = block.IndexOf(OPEN_TAG_ITALIC);
            indexUnderline = block.IndexOf(OPEN_TAG_UNDERLINE);

            if (indexBold != -1)
                indexes.Add(indexBold);

            if (indexUnderline != -1)
                indexes.Add(indexUnderline);

            if (indexItalic != -1)
                indexes.Add(indexItalic);


            int closestIndex = indexes.Count > 0 ? indexes.Min() : -2;

            if ((block.Contains(OPEN_TAG_BOLD) && block.Contains(CLOSE_TAG_BOLD)) && closestIndex == indexBold)
                return OPEN_TAG_BOLD;

            if ((block.Contains(OPEN_TAG_ITALIC) && block.Contains(CLOSE_TAG_ITALIC)) && closestIndex == indexItalic)
                return OPEN_TAG_ITALIC;

            if ((block.Contains(OPEN_TAG_UNDERLINE) && block.Contains(CLOSE_TAG_UNDERLINE)) && closestIndex == indexUnderline)
                return OPEN_TAG_UNDERLINE;

            return null;
        }

        private void adjustListFromIndex(int openTagIndex, int adjustLength, ref List<FormatExcelTextAux> list)
        {
            list.ForEach(i =>
            {
                if (i.OpenTagIndex > openTagIndex)
                {
                    i.OpenTagIndex = i.OpenTagIndex - adjustLength;
                    i.CloseTagIndex = i.CloseTagIndex - adjustLength;
                }
            });
        }

        private string getCloseTagByOpenTag(string openTag)
        {
            switch (openTag)
            {
                case OPEN_TAG_BOLD:
                    return CLOSE_TAG_BOLD;
                case OPEN_TAG_ITALIC:
                    return CLOSE_TAG_ITALIC;
                case OPEN_TAG_UNDERLINE:
                    return CLOSE_TAG_UNDERLINE;
                default:
                    throw new ApplicationException("Tag desconhecida");
            }
        }

        private string getOpenTagByCloseTag(string closeTag)
        {
            switch (closeTag)
            {
                case CLOSE_TAG_BOLD:
                    return OPEN_TAG_BOLD;
                case CLOSE_TAG_ITALIC:
                    return OPEN_TAG_ITALIC;
                case CLOSE_TAG_UNDERLINE:
                    return OPEN_TAG_UNDERLINE;
                default:
                    throw new ApplicationException("Tag desconhecida");
            }
        }

        private string getFormatTypeByTag(string openOrCloseTag)
        {
            switch (openOrCloseTag)
            {
                case OPEN_TAG_BOLD:
                case CLOSE_TAG_BOLD:
                    return FORMAT_BOLD_TYPE;
                case OPEN_TAG_ITALIC:
                case CLOSE_TAG_ITALIC:
                    return FORMAT_ITALIC_TYPE;
                case OPEN_TAG_UNDERLINE:
                case CLOSE_TAG_UNDERLINE:
                    return FORMAT_UNDERLINE_TYPE;
                default:
                    throw new ApplicationException("Tag desconhecida");
            }
        }

        #endregion

    }

    public class FormatExcelTextAux
    {
        public string[] FormatType { get; set; }
        public int OpenTagIndex { get; set; }
        public int CloseTagIndex { get; set; }
        public int Length { get; set; }
    }
}