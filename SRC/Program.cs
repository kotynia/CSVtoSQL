﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace CSVtoSQL
{
    class Program
    {

        class Options
        {
            [Option('i', "input", Required = false, DefaultValue = "input.csv",
              HelpText = "CSV file to be processedv ")]
            public string InputFile { get; set; }

            [Option('o', "output", Required = false, DefaultValue = "output.sql",
            HelpText = "FILE with Output  ")]
            public string OutputFile { get; set; }

            [Option('t', "template", Required = true, DefaultValue = "template.txt",
            HelpText = "FILE with template . Sample Template text  insert into test (xxx,yyy) values ( {1},{2})  ")]
            public string TemplateFile { get; set; }

            [Option('s', "csvseparator", Required = false, DefaultValue = ",",
            HelpText = "CSV Separator")]
            public string FieldSeparator { get; set; }

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
            try
            {

                var options = new Options();
                CommandLine.Parser.Default.ParseArguments(args, options);

                if (options.LastParserState != null && options.LastParserState.Errors.Count > 0)
                {
                    return;
                }



                 char fieldSeparator = options.FieldSeparator[0]; //default comma 

                List<string> lines = new List<string>();

                Console.WriteLine();
                Console.WriteLine("Template = " +  options.TemplateFile);

                Console.WriteLine();
                Console.WriteLine("Loading template...");
                string template = System.IO.File.ReadAllText(options.TemplateFile); // example Insert into table (column1, column2) values({0}, {1})"
                Console.WriteLine(template);

                Console.WriteLine();
                Console.WriteLine("Data file = " + options.InputFile);

                Console.WriteLine();
                Console.WriteLine("Oputput file = " + options.OutputFile);

                Console.WriteLine();
                Console.WriteLine("Field Separator = " + options.FieldSeparator);

                using (StreamReader r = new StreamReader(options.InputFile))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Processing commands...");
                int i = 1;
                StringBuilder sb = new StringBuilder();
                foreach (string s in lines)
                {
                    Console.Write(i++ + " ");
                    sb.AppendLine(string.Format(template, s.Split(fieldSeparator)));
                }

                //generate output only if no errors
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(options.OutputFile, true))
                {
                    file.Write(sb);
                    
                }

                Console.WriteLine();
                Console.WriteLine("DONE");

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR " + ex.Message.ToString());

            }

        }




    }
}
