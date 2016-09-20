using CommandLine;
using CommandLine.Text;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSVtoSQL
{
    class Program
    {

        class Options
        {
            [Option('i', "input", Required = false, DefaultValue = "input.csv",
              HelpText = "CSV file to process ")]
            public string InputFile { get; set; }

            [Option('o', "output", Required = false, DefaultValue = "output.sql",
            HelpText = "FILE with Output  ")]
            public string OutputFile { get; set; }

            [Option('t', "template", Required = true, DefaultValue = "template.txt",
            HelpText = "Template file where data will be replaced form input file . Sample Template text  insert into mytable (column1,column2) values ({0},{1})  ")]
            public string TemplateFile { get; set; }

            [Option('s', "csvseparator", Required = false, DefaultValue = ",",
            HelpText = "CSV Separator any char or \\t - tabulator")]
            public string FieldSeparator { get; set; }

            [Option('l', "line", Required = false, DefaultValue = "2",
            HelpText = "Start form Line")]
            public string startFromLine { get; set; }

            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }


        static void Main(string[] args)
        {
            // for test only
            // args = new string[] { "--help" };
            // args = new string[] { "-s", "\\t", "-t", "template.txt", "-i", "test.xls", "-l", "5" };
             //args = new string[] { "-t", "template.txt", "-i", "Book1.xlsx" };

            try
            {

                var options = new Options();
                CommandLine.Parser.Default.ParseArguments(args, options);

                if (options.LastParserState != null && options.LastParserState.Errors.Count > 0 || options.FieldSeparator==null)
                {
                    return;
                }

                char[] fieldSeparator;
                if (options.FieldSeparator == "\\t")
                    fieldSeparator = "\t".ToCharArray();                    //tab
                else
                    fieldSeparator = options.FieldSeparator.ToCharArray();  //default comma 

                List<string> lines = new List<string>();
                Console.WriteLine("CSVtoSQL, CSV Parser to Script File using Template, https://github.com/marcinKotynia/CSVtoSQL");
                Console.WriteLine(" for help type CSVtoSQL.exe --help");

                Console.WriteLine();
                Console.WriteLine("Options:"); 
                Console.WriteLine("-t {0,-20} {1}", "template", options.TemplateFile);
                Console.WriteLine("-i {0,-20} {1}" , "csv file or xlsx file",options.InputFile);
                Console.WriteLine("-o {0,-20} {1}", "oputput file", options.OutputFile);
                Console.WriteLine("-s {0,-20} {1}", "separator", options.FieldSeparator);
                Console.WriteLine("-l {0,-20} {1}", "start from Line", options.startFromLine);
                Console.WriteLine("loading template...");
                string template = System.IO.File.ReadAllText(options.TemplateFile); // example Insert into table (column1, column2) values({0}, {1})"
                Console.WriteLine(template);


                int i = 0;
                StringBuilder sb = new StringBuilder();

                if ( options.InputFile.EndsWith("xlsx")) //EXCEL
                {
                    var package = new ExcelPackage(new FileInfo(options.InputFile));

                    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                    var start = workSheet.Dimension.Start;
                    var end = workSheet.Dimension.End;
                    
                    for (int row = start.Row; row <= end.Row; row++)
                    { // Row by row...

                        i++;
                        if (i < int.Parse(options.startFromLine))
                            continue;

                        List<string> rowx = new List<string>();
                        for (int col = start.Column; col <= end.Column; col++)
                        {
                            rowx.Add(workSheet.Cells[row, col].Text); // This got me the actual value I needed.

                          
                        }
                        Console.Write("\r{0} of {1}", i, end.Row);
                        sb.AppendLine(string.Format(template, rowx.ToArray()));

                        
                    }

                }
                else //CSV processing
                {
                    using (StreamReader r = new StreamReader(options.InputFile))
                    {
                        string line;
                        while ((line = r.ReadLine()) != null)
                        {
                            lines.Add(line);
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine("Processing template...");

                    foreach (string s in lines)
                    {
                        i++;
                        if (i < int.Parse(options.startFromLine))
                            continue;

                        Console.Write("\r{0} of {1}", i, lines.Count);
                        sb.AppendLine(string.Format(template, s.Split(fieldSeparator)));

                    }
                }


                Console.WriteLine("");
                Console.WriteLine("Saving to File...");
                //generate output only if no errors
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(options.OutputFile, true))
                {
                    file.Write(sb);

                }
                
                Console.WriteLine("DONE");

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR " + ex.Message.ToString());

            }

        }




    }
}
