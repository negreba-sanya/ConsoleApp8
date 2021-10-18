using iTextSharp.text.pdf.parser;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    class Program
    {   //(.+\n)+
        //https://cross-apk.ru/docs/raspisanie_2021/11.10-16.10.pdf
        public static string url;
        public static string save_path;
        public static string name;

        static void Main(string[] args)
        {
            try
            {
                DateTime data = Convert.ToDateTime(Console.ReadLine());
                string answer = "";
                string table = "";
                try
                {
                    WebClient wc = new WebClient();
                    Calendar calendar = CultureInfo.InvariantCulture.Calendar;

                    url = "https://cross-apk.ru/docs/raspisanie_" + data.ToString("yyyy") + "/" + GetFirstDayOfWeek(data).ToString("dd.MM.") + "-" + GetLastDayOfWeek(data).ToString("dd.MM") + ".pdf";
                    save_path = @"";
                    name = data.ToString("yyyy_MM_dd") + ".pdf";
                    wc.DownloadFile(url, save_path + name);
                    answer = GetFirstDayOfWeek(data).ToString("MM.dd") + "-" + GetLastDayOfWeek(data).ToString("MM.dd") + "\n" + "\n";
                    try
                    {
                        string text_lessons;
                        PDDocument doc = null;
                        try
                        {
                            doc = PDDocument.load(save_path + name);
                            PDFTextStripper stripper = new PDFTextStripper();
                            text_lessons = stripper.getText(doc).Replace("\r", "").Replace("\n", "");
                        }
                        finally
                        {
                            if (doc != null)
                            {
                                doc.close();
                            }
                        }
                        string day_one = "";
                        string day_two = "";
                        switch (Convert.ToInt32(data.DayOfWeek))
                        {
                            case 1:
                                day_one = "Понедельник";
                                day_two = "Вторник";
                                break;
                            case 2:
                                day_one = "Вторник";
                                day_two = "Среда";
                                break;
                            case 3:
                                day_one = "Среда";
                                day_two = "Четверг";
                                break;
                            case 4:
                                day_one = "Четверг";
                                day_two = "Пятница";
                                break;
                            case 5:
                                day_one = "Пятница";
                                day_two = "Суббота";
                                break;
                            case 6:
                                day_one = "Суббота";
                                break;
                        }
                        if (day_two != "")
                        {
                            Regex regex_5 = new Regex(@"" + day_one + "(.+)" + day_two + "");
                            MatchCollection matches_3 = regex_5.Matches(text_lessons);

                            if (matches_3.Count > 0)
                            {
                                foreach (Match match in matches_3)
                                {
                                    answer += match.Value;
                                }

                            }
                            else
                            {
                                answer = "Совпадений не найдено";
                            }
                        }
                        else
                        {
                            Regex regex = new Regex(@"" + day_one + "(.+)");
                            MatchCollection matches_1 = regex.Matches(text_lessons);

                            if (matches_1.Count > 0)
                            {
                                foreach (Match match in matches_1)
                                {
                                    answer += match.Value;
                                }

                            }
                            else
                            {
                                answer = "Совпадений не найдено";
                            }
                        }
                        Regex regex_1 = new Regex(@"1на(.+)1нб");
                        MatchCollection matches_2 = regex_1.Matches(answer);
                        if (matches_2.Count > 0)
                        {
                            foreach (Match match in matches_2)
                            {
                                table += match.Value.Replace("1на", "").Replace("1нб", "");
                            }

                        }
                        else
                        {
                            table = "Совпадений не найдено";
                        }

                        Regex regex_2 = new Regex(@"[0-9]{1}\s\D+(.[0-9]{2}){1,2}\s\D+\s[0-9]{1}-[0-9]{2}\s\D+");
                        MatchCollection matches_4 = regex_2.Matches(table);
                        table = "";
                        if (matches_4.Count > 0 && matches_4.Count >= 5)
                        {
                            for (int i = 0; i < matches_4.Count + 1; i++)
                            {
                                try
                                {
                                    Regex reg_ch = new Regex(@"[0-9]{1}");
                                    MatchCollection matc = reg_ch.Matches(matches_4[i].Value);
                                    Regex reg_c = new Regex(@"[0-9]{1}");
                                    MatchCollection mat = reg_ch.Matches(matches_4[i + 1].Value);

                                    if (Convert.ToInt32(mat[0].Value) == 1 + Convert.ToInt32(matc[0].Value))
                                    {
                                        table += matches_4[i].Value + "\n";
                                    }
                                    else
                                    {
                                        table += matches_4[i].Value + "\n";
                                        table += Convert.ToString(Convert.ToInt32(matc[0].Value) + 1) + " Обеденный перерыв" + "\n";
                                    }
                                }
                                catch
                                {

                                }
                                
                            }
                        }
                        else
                        {
                            table = "Совпадений не найдено";
                        }
                        System.IO.File.Delete(name);
                    }
                    catch
                    {
                    }
                }
                catch
                {

                }
                Console.WriteLine(table);
                Console.ReadKey();
            }
            catch
            {
                Console.ReadKey();
            }
        }
        static DateTime GetFirstDayOfWeek(DateTime date)
        {
            var firstDayOfWeek = date.AddDays(-((date.DayOfWeek - DayOfWeek.Monday + 7) % 7));
            if (firstDayOfWeek.Year != date.Year)
                firstDayOfWeek = new DateTime(date.Year, 1, 1);
            return firstDayOfWeek;
        }

        static DateTime GetLastDayOfWeek(DateTime date)
        {
            var lastDayOfWeek = date.AddDays((DayOfWeek.Saturday - date.DayOfWeek + 7) % 7);
            if (lastDayOfWeek.Year != date.Year)
                lastDayOfWeek = new DateTime(date.Year, 12, 31);
            return lastDayOfWeek;
        }

    }
}
