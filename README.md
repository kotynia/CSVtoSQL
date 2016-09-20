# CSVtoSQL

## Description

This little dirty application will help you to convert CSV file into set of SQL commands.
To do that you can

- provide input file in csv format
- define template file which store template for each row
- CSVtoSQL will generate ouput file where set of commands will be saved 
- later you can use for example osql to execute this statements

## Options

~~~
Options:
  -i, --input           (Default: input.csv) CSV file to be processedv

  -o, --output          (Default: output.sql) FILE with Output

  -t, --template        Required. (Default: template.txt) Template file where
                        data will be replaced form input file . Sample Template
                        text  insert into mytable (column1,column2) values
                        ({0},{1})

  -s, --csvseparator    (Default: ,) CSV Separator any char or   - tabulator

  -l, --line            (Default: 2) Start form Line

  --help                Display this help screen.
~~~  
  

## SAMPLE Command line

Use tab character as column separator
~~~
CSVtoSQL.exe -t template.txt -i input.csv -s \t
~~~

All parameters provided
~~~
CSVtoSQL.exe -t mytemplate.txt -i data.csv -s \t -o myoutputfile.sql -l 4
~~~

## SAMPLE FLOW
Each row and column in csv is processed using temaplte file 

### Sample data file data.csv
~~~
header,header
column1row1,column2row1
column1row2,column2row2
column1row3,column2row3
~~~

### sample template mytemplate.txt 
~~~
insert into mytable (column1,column2) values ('{0}','{1}');
~~~

###Command
~~~
CSVtoSQL.exe -t mytemplate.txt -i data.csv 
~~~

###RESULT  output.sql
~~~
insert into mytable (column1,column2) values ('column1row1','column2row1');
insert into mytable (column1,column2) values ('column1row2','column2row2');
insert into mytable (column1,column2) values ('column1row3','column2row3');
~~~


## Requirements
.NET Framework 4

