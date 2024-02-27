using System;
using System.Collections.Generic;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using Microsoft.Office.Interop.Word;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CongratulationsApp
{
    class Names
    {
        private string name;
        private string gender;

        public Names(string name, string gender)
        {
            this.name = name;
            this.gender = gender;
        }

        public string Name
        {
            set { this.name = value; }
            get { return name; }
        }

        public string Gender
        {
            set { this.gender = value; }
            get { return gender; }
        }
    }

    class Phrases
    {
        private string phrase;
        public Phrases(string phrase)
        {
            this.phrase = phrase;
        }
        public string Phrase
        {
            set { this.phrase = value; }
            get { return phrase; }
        }
    }

    class Triads
    {
        private Phrases firstPhrase;
        private Phrases secondPhrase;
        private Phrases thirdPhrase;

        public Triads(Phrases firstPhrase, Phrases secondPhrase, Phrases thirdPhrase)
        {
            this.firstPhrase = firstPhrase;
            this.secondPhrase = secondPhrase;
            this.thirdPhrase = thirdPhrase;
        }

        public Phrases FirstPhrase 
        {
            set { this.firstPhrase = value; }
            get { return firstPhrase; }
        }
        public Phrases SecondPhrase
        { 
            set { this.secondPhrase = value; }
            get { return secondPhrase; } }
        public Phrases ThirdPhrase
        {
            set { this.thirdPhrase = value; }
            get { return thirdPhrase; } }
    }

    class Program
    {
        public static List<Names> ListOfNames;
        public static Phrases[,] ArrayOfPhrases;
        public static List<Triads> ListOfTriads = new List<Triads>();
        public static List<List<KeyValuePair<int, int>>> GlobalListOfUsedTriads = new List<List<KeyValuePair<int, int>>>(); // { { [1,1], [3,4], [2,3] },{ [2,1], [3,6], [2,3] }  }

        public static string FileName, FontName;

        public static void Main(string[] args)
        {
            Console.WriteLine("Получение данных из входного файла Excel...");
            Console.WriteLine();
            LoadFromExcel();
            Console.WriteLine("Данные из Excel получены успешно!");

            Console.WriteLine();

            Console.WriteLine("Оценим возможность генерации уникальных триад...");
            Console.WriteLine();
            if (IsPossibleToGenerateUniqueTriads()) Console.WriteLine("Количества фраз достаточно для создания уникальных поздравлений!");
            else
            {
                Console.WriteLine("Ошибка!\nКоличества фраз недостаточно для создания уникальных поздравлений!");
                return;
            }

            Console.WriteLine();

            Console.WriteLine("Генерация триад поздравлений...");
            Console.WriteLine();
            MakeRandomTriads();
            Console.WriteLine("Триады успешно сгенерированы!");

            Console.WriteLine();

            Console.WriteLine("Создание поздравлений в Word...");
            Console.WriteLine();
            ExportToWord();
            Console.WriteLine("Открытки готовы!");
        }
        public static void LoadFromExcel()
        {
            Excel.Application excel = null;
            Excel.Workbook wbk = null;
            int CountOfColumns = 0; int CountOfRows = 0; int CountOfRowsInColumn = 0;
            try
            {
                excel = new Excel.Application(); // создается объект эксель

                wbk = excel.Workbooks.Open(AppContext.BaseDirectory + @"\congrats.xlsx"); // к нему привязывается книга ( AppContext.BaseDirectory - путь до папки с exe)
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            if (excel != null && wbk != null) // считаем кол-во строк и столбцов в таблице
            {
                Excel.Worksheet ws = (Excel.Worksheet)wbk.Worksheets[1]; // первая страница

                for (int j = 1; j < ws.Columns.Count; j++)
                {
                    CountOfRowsInColumn = 0;

                    if (((Excel.Range)ws.Cells[1, j]).Value == null) break; // проход по ширине таблицы

                    for (int i = 0; i < ws.Rows.Count; i++)
                    {

                        if (((Excel.Range)ws.Cells[i + 1, j]).Value == null) break;

                        CountOfRowsInColumn += 1; // кол-во строк в очередном столбце


                    }
                    if (CountOfRowsInColumn > 0)
                        CountOfColumns += 1;
                    if (CountOfRowsInColumn > CountOfRows)
                        CountOfRows = CountOfRowsInColumn;
                }

                ArrayOfPhrases = new Phrases[CountOfRows + 1, CountOfColumns + 1]; // вся таблица с группами поздравлений, включая заголовки

                for (int i = 0; i < CountOfColumns; i++)
                {
                    for (int j = 1; j <= CountOfRows; j++)
                    {
                        if (((Excel.Range)ws.Cells[j, i + 1]).Value == null) break;

                        ArrayOfPhrases[j, i + 1] = new Phrases(((Excel.Range)ws.Cells[j, i + 1]).Value.ToString());
                    }
                }

                ListOfNames = new List<Names>();

                Excel.Worksheet names = (Excel.Worksheet)wbk.Worksheets[2]; // вторая страница
                for (int j = 1; j < names.Rows.Count; j++)
                {
                    if (((Excel.Range)names.Cells[j + 1, 1]).Value == null) break;

                    ListOfNames.Add(new Names(((Excel.Range)names.Cells[j + 1, 1]).Value.ToString(), ((Excel.Range)names.Cells[j + 1, 2]).Value.ToString()));
                }

                Excel.Worksheet config = (Excel.Worksheet)wbk.Worksheets[3]; // третья страница
                FileName = @"\" + ((Excel.Range)config.Cells[1, 1]).Value.ToString();
                FontName = ((Excel.Range)config.Cells[2, 1]).Value.ToString();

                if (config != null) Marshal.FinalReleaseComObject(config);
                if (names != null) Marshal.FinalReleaseComObject(names);
                if (ws != null) Marshal.FinalReleaseComObject(ws);
            }

            if (wbk != null)
            {
                wbk.Close();
                Marshal.FinalReleaseComObject(wbk);
            }

            if (excel != null)
            {
                excel.Application.Quit();
                excel.Quit();
                Marshal.ReleaseComObject(excel);
            }
        }

        public static bool IsPossibleToGenerateUniqueTriads()
        {
            if (ArrayOfPhrases.GetLength(1) < 3)
            {
                return false;
            }
            long CountOfUniqueTriads = 0;
            for (int i = 0; i < ArrayOfPhrases.GetLength(1); i++)
            {
                for (int j = i + 1; j < ArrayOfPhrases.GetLength(1); j++)
                {
                    for (int k = j + 1; k < ArrayOfPhrases.GetLength(1); k++)
                    {
                        CountOfUniqueTriads += ArrayOfPhrases.GetLength(1) ^ 3;    // * 10 
                    }
                }
            }
            if (CountOfUniqueTriads < ListOfNames.Count) return false;
            return true;
        }

        public static void MakeRandomTriads() // построение триад - генерируем сразу для всех имен из списка
        {
            for (int name = 0; name < ListOfNames.Count; name++)
            {
                List<Phrases> Triad = new List<Phrases>();
                Triads NewTriad;

                int c, r;
                Random rnd = new Random();
                List<int> ListOfColumns = new List<int>();
                List<int> ListOfRows = new List<int>();

                for (int i = 2; i <= ArrayOfPhrases.GetLength(0); i++)
                {
                    ListOfRows.Add(i);
                }

                for (int i = 1; i <= ArrayOfPhrases.GetLength(1); i++)
                {
                    ListOfColumns.Add(i);
                }

                List<KeyValuePair<int, int>> ListOfUsedInTriad = new List<KeyValuePair<int, int>>();
                int CountOfIdenticalPhrases = 3;

                while (CountOfIdenticalPhrases == 3) // если среди всех триад нашлась хотя бы одна, совпадающая с триадой из списка триад, то нам такая не подходит
                                                     // поэтому генерируем триаду до тех пор, пока она не станет уникальной
                {
                    ListOfUsedInTriad = new List<KeyValuePair<int, int>>();  // список фраз, находящихся в триаде
                                                                                                          

                    int FirstNumberOfUsedColumn = 0;
                    int SecondNumberOfUsedColumn = 0;

                    r = ListOfRows[rnd.Next(0, ListOfRows.Count - 1)]; // берем рандомную ячейку
                    c = ListOfColumns[rnd.Next(0, ListOfColumns.Count - 1)];

                    Triad.Add(ArrayOfPhrases[r, c]); // добавляем пожелание в триаду
                    KeyValuePair<int, int> kvp = new KeyValuePair<int, int>(r, c); // добавляем ячейку в список использованных в триаде 
                    ListOfUsedInTriad.Add(kvp);
                    FirstNumberOfUsedColumn = c;
                    
                    for (int i = 0; i < 2; i++)
                    {

                        for (int j = 0; j < ListOfUsedInTriad.Count; j++) 
                        {
                            while (ListOfUsedInTriad[j].Key == r && ListOfUsedInTriad[j].Value == c || FirstNumberOfUsedColumn == c || SecondNumberOfUsedColumn == c) // пока не окажется так, что ячейки не было в триаде ранее
                                                                                                                                                                      // и пока не найдется ячйка из неиспользованных в триаде столбцов
                                                                                                                                                                      // будем генерировать ячейку
                            {
                                r = ListOfRows[rnd.Next(0, ListOfRows.Count - 1)];
                                c = ListOfColumns[rnd.Next(0, ListOfColumns.Count - 1)];
                            }
                        }
                        Triad.Add(ArrayOfPhrases[r, c]); // добавляем пожелание в триаду
                        kvp = new KeyValuePair<int, int>(r, c); // добавляем ячейку в список использованных в триаде 
                        ListOfUsedInTriad.Add(kvp);
                        SecondNumberOfUsedColumn = c;
                    }                                                                 // здесь имеем полностью сгенерированную триаду с оригинальными фразами

                    if (GlobalListOfUsedTriads.Count == 0) // в списке триад нет триад
                    {
                        GlobalListOfUsedTriads.Add(ListOfUsedInTriad);

                    }
                    else
                    {
                        for (int triad = 0; triad < GlobalListOfUsedTriads.Count; triad++) // проверяем каждую триаду из имеющихся
                        {
                            CountOfIdenticalPhrases = 0;
                            foreach (KeyValuePair<int, int> WishInGlobal in GlobalListOfUsedTriads[triad]) // рассм пару из списка триад // wish - это { { [wish], [,], [,] }, { }, ....}
                            {
                                foreach (KeyValuePair<int, int> WishInTemp in ListOfUsedInTriad)
                                {
                                    if (WishInGlobal.Key == WishInTemp.Key && WishInGlobal.Value == WishInTemp.Value) CountOfIdenticalPhrases += 1;
                                }
                            }
                            if (CountOfIdenticalPhrases == 3) break; // выходим из последнего for и попадаем снова в while
                        }

                    }
                }
                GlobalListOfUsedTriads.Add(ListOfUsedInTriad);
                NewTriad = new Triads(Triad[0], Triad[1], Triad[2]);
                ListOfTriads.Add(NewTriad);
            }
            foreach (List<KeyValuePair<int, int>> triad in GlobalListOfUsedTriads ) // вывод в консоль всех использованных триад
            {
                string res = "";
                foreach (KeyValuePair<int, int> wish in triad)
                {
                    string s = " ( " + wish.Key.ToString() + " " + wish.Value.ToString() + " ) ";
                    res += s;
                }
                Console.WriteLine(res);
                Console.WriteLine();
            }
            
        }

        public static void ExportToWord()
        {
            Word.Document doc = null;
            Word.Application word = null;
            try
            {
                word = new Word.Application(); // создание объекта ворд
                //word.Visible = true;

                doc = word.Documents.Add(AppContext.BaseDirectory + FileName);
                doc.Activate();
                Word.Bookmarks wBookmarks = doc.Bookmarks; // сведения о заметках
                Word.Range wRange;

                string Dear = "";
                if (ListOfNames[0].Gender == "м") Dear = "Дорогой";
                else Dear = "Дорогая";
                              
                string[] DataForFirst = new string[5] { Dear, ListOfNames[0].Name, ListOfTriads[0].FirstPhrase.Phrase + ",", ListOfTriads[0].SecondPhrase.Phrase + ",", ListOfTriads[0].ThirdPhrase.Phrase + "!" };

                int i = 0;
                foreach (Word.Bookmark mark in wBookmarks)
                {
                    wRange = mark.Range;
                    wRange.Text = DataForFirst[i];
                    i++;
                }

                for (int j = 1; j < ListOfNames.Count(); j++)
                {
                    if (ListOfNames[j].Gender == "м") Dear = "Дорогой";
                    else Dear = "Дорогая";

                    string[] DataForOthers = new string[5] { Dear, ListOfNames[j].Name, ListOfTriads[j].FirstPhrase.Phrase + ",", ListOfTriads[j].SecondPhrase.Phrase + ",", ListOfTriads[j].ThirdPhrase.Phrase + "!" };

                    word.Selection.InsertFile(AppContext.BaseDirectory + FileName, "", false, false, false); // вставка шаблона
                    word.Selection.InsertBreak(); // вставка разрыва страницы

                    int k = 0;
                    foreach (Word.Bookmark mark in wBookmarks)
                    {
                        wRange = mark.Range;
                        wRange.Text = DataForOthers[k];
                        k++;
                    }
                }
                word.Selection.SetRange(doc.Content.Start, doc.Content.End);
                word.Selection.Font.Name = FontName; // меняем шрифт во всем документе сразу

                if (!System.IO.Directory.Exists(AppContext.BaseDirectory + @"\PostCardsFolder")) // если не существует такой папки
                {
                    System.IO.Directory.CreateDirectory(AppContext.BaseDirectory + @"\PostCardsFolder"); // создаем ее
                    word.ActiveDocument.SaveAs2(AppContext.BaseDirectory + @"\PostCardsFolder\PostCards"); // сохраняем документ с открытками в папку
                }
                else
                {
                    System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(AppContext.BaseDirectory + @"\PostCardsFolder"); 
                    int FileNumber = DI.GetFiles().Length; // возвращает кол-во файлов в папке
                    string NewName = @"\PostCardsFolder\PostCards(" + FileNumber.ToString() + ")";

                    while (System.IO.File.Exists(AppContext.BaseDirectory + NewName + ".docx"))
                    {
                        ++FileNumber;
                        NewName = @"\PostCardsFolder\PostCards(" + FileNumber.ToString() + ")";
                    }
                    word.ActiveDocument.SaveAs2(AppContext.BaseDirectory + NewName) ;
                }
                doc.Close();
                word.Quit();
                doc = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }

        
}
