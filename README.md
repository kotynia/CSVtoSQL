# CSVtoSQL  (from 0.4 EXCEL XLSX support)

## Description

This little dirty application will help you to convert CSV file into set of SQL commands.
To do that you can

- provide input file in csv format (with any separator) or EXCEL file (XLSX format)
- define template file
- CSVtoSQL will process each line and generate ouput file where script will be saved
- later you can use for example osql to execute this statements

## Options

~~~
Options:
  -i, --input           (Default: input.csv) CSV or EXCEL file to be processed 

  -o, --output          (Default: output.sql) FILE with Output

  -t, --template        Required. (Default: template.txt) Template file where
                        data will be replaced form input file . Sample Template
                        text  insert into mytable (column1,column2) values
                        ({0},{1})

  -s, --csvseparator    (Default: ,) CSV Separator any char or \t for tabulator

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

## Known Issues and nice features
1. Note about parsing CSV
This is dirty solution so do not expect complex parsing of CSV file
for example if your column separator is "," this sample below will not work:

~~~
header,header
column1row1,column2row1
column1row2,"column2, row2"
column1row3,column2row3
~~~

2. EXCEL Procesor get VALUES not CELL TEXT
This is feature it means that if you have function for example a1+a2 = 4 CSVtoSQL will take VALUE = 4 not formula

3. Creative usage
CSVtoSQL could be used to generate any content for example simple web pages 

Template like ths will generate table body on output 
TEMPLATE
~~~
<tr><td>{0}</td><td>{1}</td></tr>
~~~

