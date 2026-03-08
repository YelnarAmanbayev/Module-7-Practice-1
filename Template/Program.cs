using System;

namespace Template
{
    class Program
    {
        static void Main(string[] args)
        {
            ReportGenerator pdf = new PdfReport();
            ReportGenerator excel = new ExcelReport();
            ReportGenerator html = new HtmlReport();
            ReportGenerator csv = new CsvReport();

            Console.WriteLine("PDF отчет");
            pdf.GenerateReport();

            Console.WriteLine("\nExcel отчет");
            excel.GenerateReport();

            Console.WriteLine("\nHTML отчет");
            html.GenerateReport();

            Console.WriteLine("\nCSV отчет");
            csv.GenerateReport();
        }
    }
}