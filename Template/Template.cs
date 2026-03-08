using System;

namespace Template
{
    abstract class ReportGenerator
    {
        public void GenerateReport()
        {
            LogStep("Начало генерации отчета");

            GetData();
            FormatData();
            CreateHeader();
            CreateBody();
            CreateFooter();

            if (CustomerWantsSave())
            {
                SaveReport();
            }

            if (CustomerWantsSendByEmail())
            {
                SendByEmail();
            }

            LogStep("Генерация отчета завершена");
        }

        protected void GetData()
        {
            LogStep("Получение данных");
            Console.WriteLine("Данные получены");
        }

        protected abstract void FormatData();
        protected abstract void CreateHeader();
        protected abstract void CreateBody();
        protected abstract void CreateFooter();

        protected virtual void SaveReport()
        {
            LogStep("Сохранение отчета");
            Console.WriteLine("Отчет сохранен");
        }

        protected virtual void SendByEmail()
        {
            LogStep("Отправка отчета по почте");
            Console.WriteLine("Отчет отправлен по электронной почте");
        }

        protected virtual bool CustomerWantsSave()
        {
            return GetUserAnswer("Сохранить отчет? (да/нет): ");
        }

        protected virtual bool CustomerWantsSendByEmail()
        {
            return GetUserAnswer("Отправить отчет по электронной почте? (да/нет): ");
        }

        protected void LogStep(string message)
        {
            Console.WriteLine("[LOG] " + message);
        }

        protected bool GetUserAnswer(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine()?.Trim().ToLower();

            if (input == "да" || input == "д" || input == "yes" || input == "y")
                return true;

            if (input == "нет" || input == "н" || input == "no" || input == "n")
                return false;

            Console.WriteLine("Некорректный ввод. По умолчанию выбран ответ нет");
            return false;
        }
    }

    class PdfReport : ReportGenerator
    {
        protected override void FormatData()
        {
            Console.WriteLine("Форматирование данных для PDF");
        }

        protected override void CreateHeader()
        {
            Console.WriteLine("Создание заголовка PDF");
        }

        protected override void CreateBody()
        {
            Console.WriteLine("Создание содержимого PDF");
        }

        protected override void CreateFooter()
        {
            Console.WriteLine("Создание нижней части PDF");
        }

        protected override void SaveReport()
        {
            Console.WriteLine("PDF отчет сохранен (.pdf)");
        }
    }

    class ExcelReport : ReportGenerator
    {
        protected override void FormatData()
        {
            Console.WriteLine("Форматирование данных для Excel");
        }

        protected override void CreateHeader()
        {
            Console.WriteLine("Создание заголовка Excel");
        }

        protected override void CreateBody()
        {
            Console.WriteLine("Создание таблицы Excel");
        }

        protected override void CreateFooter()
        {
            Console.WriteLine("Создание нижней части Excel");
        }

        protected override void SaveReport()
        {
            Console.WriteLine("Excel отчет сохранен (.xlsx)");
        }
    }

    class HtmlReport : ReportGenerator
    {
        protected override void FormatData()
        {
            Console.WriteLine("Форматирование данных для HTML");
        }

        protected override void CreateHeader()
        {
            Console.WriteLine("Создание HTML заголовка");
        }

        protected override void CreateBody()
        {
            Console.WriteLine("Создание HTML содержимого");
        }

        protected override void CreateFooter()
        {
            Console.WriteLine("Создание HTML подвала");
        }

        protected override void SaveReport()
        {
            Console.WriteLine("HTML отчет сохранен (.html)");
        }
    }

    class CsvReport : ReportGenerator
    {
        protected override void FormatData()
        {
            Console.WriteLine("Форматирование данных для CSV");
        }

        protected override void CreateHeader()
        {
            Console.WriteLine("Создание строки заголовков CSV");
        }

        protected override void CreateBody()
        {
            Console.WriteLine("Создание строк данных CSV");
        }

        protected override void CreateFooter()
        {
            Console.WriteLine("CSV не требует нижней части");
        }

        protected override void SaveReport()
        {
            Console.WriteLine("CSV отчет сохранен (.csv)");
        }
    }
}