# CSVtoSQL

This little dirty application will help you to convert CSV file into set of SQL commands.
To do that you can

- provide input file in csv format
- define template file which store template for each row
- CSVtoSQL till generate ouput file where set of commands will be saved > later you can use for example osql to execute this statements


Options:

  -i, --input           (Default: input.csv) CSV file to be processedv

  -o, --output          (Default: output.sql) FILE with Output

  -t, --template        Required. (Default: template.txt) FILE with template .
                        Sample Template text  insert into test (xxx,yyy) values
                        ( {1},{2})

  -s, --csvseparator    (Default: ,) CSV Separator

  --help                Display this help screen.
  
  
  
SAMPLE Command line

fileToMSSQL.exe -s \t  -i test.xls -t template.txt
