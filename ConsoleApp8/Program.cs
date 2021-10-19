using HtmlAgilityPack;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleApp8
{
    class Program 
    {
        private static string token { get; set; } = "2059463909:AAHaA4iNqO4rS8YWYp_RJdImWRhjZJHjluM";
        private static TelegramBotClient client;
        public static string mode = "none";
        public static string url;
        public static string save_path;
        public static string name;

        static void Main(string[] args)
        {
            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            client.StopReceiving();            
        }

        private async static void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg != null)
            {

                switch (msg.Text)
                {
                    case "Сегодня":
                        try
                        {
                            string answer = "";
                            string table = "";
                            HtmlWeb ws = new HtmlWeb();
                            ws.OverrideEncoding = Encoding.UTF8;
                            HtmlDocument docum = ws.Load("https://cross-apk.ru/index.php?option=com_content&view=article&id=3004&Itemid=1802");
                            ArrayList list = new ArrayList();
                            foreach (HtmlNode node in docum.DocumentNode.SelectNodes("//div[contains(@class, 'item-page')]//a[@href]"))
                            {
                                if (node.InnerText == "Расписание занятий на " + GetFirstDayOfWeek(DateTime.Now).ToString("dd.MM") + "-" + GetLastDayOfWeek(DateTime.Now).ToString("dd.MM.yyyy"))
                                {
                                    url = "https://cross-apk.ru/" + node.GetAttributeValue("href", null);
                                }
                            }
                            save_path = @"";
                            WebClient wc = new WebClient();
                            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
                            save_path = @"";
                            name = DateTime.Now.ToString("yyyy_MM_dd") + ".pdf";
                            wc.DownloadFile(url, save_path + name);
                            try
                            {
                                string text_lessons;
                                PDDocument doc = null;
                                try
                                {
                                    doc = PDDocument.load(save_path + name);
                                    PDFTextStripper stripper = new PDFTextStripper();
                                    text_lessons = stripper.getText(doc).Replace("\r", "");
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


                                switch (Convert.ToInt32(DateTime.Now.DayOfWeek))
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
                                    Regex regex_5 = new Regex(@"" + day_one + "(.+\n)+" + day_two + "");
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
                                    Regex regex = new Regex(@"" + day_one + "(.+\n)+");
                                    MatchCollection matches_1 = regex.Matches(text_lessons);

                                    if (matches_1.Count > 0)
                                    {
                                        foreach (Match match in matches_1)
                                        {
                                            answer += match.Value;
                                        }

                                    }
                                }
                                Regex regex_1 = new Regex(@"1н(\n)а(\n)(.+\n)+1н(\n)б");
                                MatchCollection matches_2 = regex_1.Matches(answer);
                                if (matches_2.Count > 0)
                                {
                                    table += DateTime.Now.ToString("dd MMMM yyyy ") + "\n";
                                    foreach (Match match in matches_2)
                                    {
                                        table += match.Value.Replace("1н\nа", "").Replace("1н\nб", "");
                                    }

                                }
                                System.IO.File.Delete(name);
                            }
                            catch
                            {

                            }
                            await client.SendTextMessageAsync(chatId: msg.Chat.Id, text: table, replyMarkup: GetButtons());
                        }
                        catch
                        {
                            await client.SendTextMessageAsync(chatId: msg.Chat.Id, text: DateTime.Now.ToString("dd.MM.yyyy") + "\n" + "\n" + "На этот день нет расписания", replyMarkup: GetButtons());
                        }
                        break;
                    case "Завтра":
                        DateTime data = DateTime.Today.AddDays(+1);
                        try
                        {
                            
                            string answer = "";
                            string table = "";
                            HtmlWeb ws = new HtmlWeb();
                            ws.OverrideEncoding = Encoding.UTF8;
                            HtmlDocument docum = ws.Load("https://cross-apk.ru/index.php?option=com_content&view=article&id=3004&Itemid=1802");
                            ArrayList list = new ArrayList();
                            foreach (HtmlNode node in docum.DocumentNode.SelectNodes("//div[contains(@class, 'item-page')]//a[@href]"))
                            {
                                if (node.InnerText == "Расписание занятий на " + GetFirstDayOfWeek(data).ToString("dd.MM") + "-" + GetLastDayOfWeek(data).ToString("dd.MM.yyyy"))
                                {
                                    url = "https://cross-apk.ru/" + node.GetAttributeValue("href", null);
                                }
                            }
                            save_path = @"";
                            WebClient wc = new WebClient();
                            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
                            save_path = @"";
                            name = data.ToString("yyyy_MM_dd") + ".pdf";
                            wc.DownloadFile(url, save_path + name);
                            try
                            {
                                string text_lessons;
                                PDDocument doc = null;
                                try
                                {
                                    doc = PDDocument.load(save_path + name);
                                    PDFTextStripper stripper = new PDFTextStripper();
                                    text_lessons = stripper.getText(doc).Replace("\r", "");
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
                                    Regex regex_5 = new Regex(@"" + day_one + "(.+\n)+" + day_two + "");
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
                                    Regex regex = new Regex(@"" + day_one + "(.+\n)+");
                                    MatchCollection matches_1 = regex.Matches(text_lessons);

                                    if (matches_1.Count > 0)
                                    {
                                        foreach (Match match in matches_1)
                                        {
                                            answer += match.Value;
                                        }

                                    }
                                }
                                Regex regex_1 = new Regex(@"1н(\n)а(\n)(.+\n)+1н(\n)б");
                                MatchCollection matches_2 = regex_1.Matches(answer);
                                if (matches_2.Count > 0)
                                {
                                    table += data.ToString("dd MMMM yyyy ") + "\n";
                                    foreach (Match match in matches_2)
                                    {
                                        table += match.Value.Replace("1н\nа", "").Replace("1н\nб", "");
                                    }

                                }
                                System.IO.File.Delete(name);
                            }
                            catch
                            {

                            }
                            await client.SendTextMessageAsync(chatId: msg.Chat.Id, text: table, replyMarkup: GetButtons());
                        }
                        catch
                        {
                            await client.SendTextMessageAsync(chatId: msg.Chat.Id, text: data.ToString("dd.MM.yyyy") + "\n" + "\n" + "На этот день нет расписания", replyMarkup: GetButtons());
                        }

                        break;
                    case "Послезавтра":
                        DateTime data_3 = DateTime.Today.AddDays(+2);
                        try
                        {

                            string answer = "";
                            string table = "";
                            HtmlWeb ws = new HtmlWeb();
                            ws.OverrideEncoding = Encoding.UTF8;
                            HtmlDocument docum = ws.Load("https://cross-apk.ru/index.php?option=com_content&view=article&id=3004&Itemid=1802");
                            ArrayList list = new ArrayList();
                            foreach (HtmlNode node in docum.DocumentNode.SelectNodes("//div[contains(@class, 'item-page')]//a[@href]"))
                            {
                                if (node.InnerText == "Расписание занятий на " + GetFirstDayOfWeek(data_3).ToString("dd.MM") + "-" + GetLastDayOfWeek(data_3).ToString("dd.MM.yyyy"))
                                {
                                    url = "https://cross-apk.ru/" + node.GetAttributeValue("href", null);
                                }
                            }
                            save_path = @"";
                            WebClient wc = new WebClient();
                            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
                            save_path = @"";
                            name = data_3.ToString("yyyy_MM_dd") + ".pdf";
                            wc.DownloadFile(url, save_path + name);
                            try
                            {
                                string text_lessons;
                                PDDocument doc = null;
                                try
                                {
                                    doc = PDDocument.load(save_path + name);
                                    PDFTextStripper stripper = new PDFTextStripper();
                                    text_lessons = stripper.getText(doc).Replace("\r", "");
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


                                switch (Convert.ToInt32(data_3.DayOfWeek))
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
                                    Regex regex_5 = new Regex(@"" + day_one + "(.+\n)+" + day_two + "");
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
                                    Regex regex = new Regex(@"" + day_one + "(.+\n)+");
                                    MatchCollection matches_1 = regex.Matches(text_lessons);

                                    if (matches_1.Count > 0)
                                    {
                                        foreach (Match match in matches_1)
                                        {
                                            answer += match.Value;
                                        }

                                    }
                                }
                                Regex regex_1 = new Regex(@"1н(\n)а(\n)(.+\n)+1н(\n)б");
                                MatchCollection matches_2 = regex_1.Matches(answer);
                                if (matches_2.Count > 0)
                                {
                                    table += data_3.ToString("dd MMMM yyyy ") + "\n";
                                    foreach (Match match in matches_2)
                                    {
                                        table += match.Value.Replace("1н\nа", "").Replace("1н\nб", "");
                                    }

                                }
                                System.IO.File.Delete(name);
                            }
                            catch
                            {

                            }
                            await client.SendTextMessageAsync(chatId: msg.Chat.Id, text: table, replyMarkup: GetButtons());
                        }
                        catch
                        {
                            await client.SendTextMessageAsync(chatId: msg.Chat.Id, text: data_3.ToString("dd.MM.yyyy") + "\n" + "\n" + "На этот день нет расписания", replyMarkup: GetButtons());
                        }
                        break;
                    default:
                        try
                        {
                            DateTime data_1 = Convert.ToDateTime(msg.Text);
                            string answer = "";
                            string table = "";
                            try
                            {
                                HtmlWeb ws = new HtmlWeb();
                                ws.OverrideEncoding = Encoding.UTF8;
                                HtmlDocument docum = ws.Load("https://cross-apk.ru/index.php?option=com_content&view=article&id=3004&Itemid=1802");
                                ArrayList list = new ArrayList();
                                foreach (HtmlNode node in docum.DocumentNode.SelectNodes("//div[contains(@class, 'item-page')]//a[@href]"))
                                {
                                    if (node.InnerText == "Расписание занятий на " + GetFirstDayOfWeek(data_1).ToString("dd.MM") + "-" + GetLastDayOfWeek(data_1).ToString("dd.MM.yyyy"))
                                    {
                                        url = "https://cross-apk.ru/" + node.GetAttributeValue("href", null);
                                    }
                                }
                                save_path = @"";
                                WebClient wc = new WebClient();
                                Calendar calendar = CultureInfo.InvariantCulture.Calendar;
                                save_path = @"";
                                name = data_1.ToString("yyyy_MM_dd") + ".pdf";
                                wc.DownloadFile(url, save_path + name);
                                try
                                {
                                    string text_lessons;
                                    PDDocument doc = null;
                                    try
                                    {
                                        doc = PDDocument.load(save_path + name);
                                        PDFTextStripper stripper = new PDFTextStripper();
                                        text_lessons = stripper.getText(doc).Replace("\r", "");
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


                                    switch (Convert.ToInt32(data_1.DayOfWeek))
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
                                        Regex regex_5 = new Regex(@"" + day_one + "(.+\n)+" + day_two + "");
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
                                        Regex regex = new Regex(@"" + day_one + "(.+\n)+");
                                        MatchCollection matches_1 = regex.Matches(text_lessons);

                                        if (matches_1.Count > 0)
                                        {
                                            foreach (Match match in matches_1)
                                            {
                                                answer += match.Value;
                                            }

                                        }
                                    }
                                    Regex regex_1 = new Regex(@"1н(\n)а(\n)(.+\n)+1н(\n)б");
                                    MatchCollection matches_2 = regex_1.Matches(answer);
                                    if (matches_2.Count > 0)
                                    {
                                        table += data_1.ToString("dd MMMM yyyy ") + "\n";
                                        foreach (Match match in matches_2)
                                        {
                                            table += match.Value.Replace("1н\nа", "").Replace("1н\nб", "");
                                        }

                                    }
                                    System.IO.File.Delete(name);
                                }
                                catch
                                {

                                }
                                await client.SendTextMessageAsync(chatId: msg.Chat.Id, text: table, replyMarkup: GetButtons());
                            }
                            catch
                            {
                                await client.SendTextMessageAsync(chatId: msg.Chat.Id, text: data_1.ToString("dd.MM.yyyy") + "\n" + "\n" + "На этот день нет расписания", replyMarkup: GetButtons());
                            }
                        }
                        catch
                        {
                            await client.SendTextMessageAsync(
                                                chatId: msg.Chat.Id,
                                                text: "Введите дату в формате \"01.01.2000\"",
                                                replyMarkup: GetButtons()
                                                );
                        }
                        break;
                }


            }
        }

        static DateTime GetFirstDayOfWeek(DateTime date)
        {
            if (Convert.ToInt32(date.DayOfWeek) != 1)
            {
                var firstDayOfWeek = date.AddDays(-((date.DayOfWeek - DayOfWeek.Monday + 7) % 7));
                if (firstDayOfWeek.Year != date.Year)
                    firstDayOfWeek = new DateTime(date.Year, 1, 1);
                return firstDayOfWeek;
            }
            else
            {
                return date;
            }
        }

        static DateTime GetLastDayOfWeek(DateTime date)
        {
            var lastDayOfWeek = date.AddDays((DayOfWeek.Saturday - date.DayOfWeek + 7) % 7);
            if (lastDayOfWeek.Year != date.Year)
                lastDayOfWeek = new DateTime(date.Year, 12, 31);
            return lastDayOfWeek;
        }

        private static IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton> { new KeyboardButton { Text = "Сегодня" }, new KeyboardButton { Text = "Завтра" }, new KeyboardButton { Text = "Послезавтра" } }
                }
            };
        }

    }
}
