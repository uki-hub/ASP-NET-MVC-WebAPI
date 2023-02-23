using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7_BOT
{
    public class GenerateHemTableClasses
    {
        private SQL _SQL { get; set; }
        private string _nameSpace { get; set; }

        public GenerateHemTableClasses(string nameSpace, bool isHem)
        {
            string cs = isHem ? ConfigurationManager.ConnectionStrings["HemCS"].ConnectionString : ConfigurationManager.ConnectionStrings["HerCS"].ConnectionString;
                       
            _SQL = new SQL(cs);
            _nameSpace = nameSpace;
        }

        public void generateHem()
        {

            Console.WriteLine("Getting Tables");

            var tables = getHemTables();

            Console.WriteLine($"Found {tables.Count} tables");

            Console.ReadKey();

            var generatedTableClasses = new List<string>();
            for (int i = 0; i < tables.Count; i++)
            {
                Console.Clear();

                Console.WriteLine($"Generate {tables[i]} table, {i + 1} of {tables.Count}");

                ConsoleLoadingBar.LoadBar(i, tables.Count);

                generatedTableClasses.Add(generateTableClass(tables[i], _nameSpace));                
            }

            System.IO.Directory.CreateDirectory("out");

            for (int i = 0; i < tables.Count; i++)
            {
                Console.Clear();

                Console.WriteLine($"Publishing class files, {i + 1} of {tables.Count}");

                ConsoleLoadingBar.LoadBar(i, tables.Count);

                using (var sw = new StreamWriter($@"out\{tables[i]}.cs"))
                {
                    sw.Write(generatedTableClasses[i]);
                }
            }           

            Console.ReadKey();
        }

        public void generateHer()
        {

            Console.WriteLine("Getting Tables");

            var tables = getHerTables();

            Console.WriteLine($"Found {tables.Count} tables");

            Console.ReadKey();

            var generatedTableClasses = new List<string>();
            for (int i = 0; i < tables.Count; i++)
            {
                Console.Clear();

                Console.WriteLine($"Generate {tables[i]} table, {i + 1} of {tables.Count}");

                ConsoleLoadingBar.LoadBar(i, tables.Count);

                generatedTableClasses.Add(generateTableClass(tables[i], _nameSpace));
            }

            System.IO.Directory.CreateDirectory("out");

            for (int i = 0; i < tables.Count; i++)
            {
                Console.Clear();

                Console.WriteLine($"Publishing class files, {i + 1} of {tables.Count}");

                ConsoleLoadingBar.LoadBar(i, tables.Count);

                using (var sw = new StreamWriter($@"out\{tables[i]}.cs"))
                {
                    sw.Write(generatedTableClasses[i]);
                }
            }

            Console.ReadKey();
        }


        List<string> getHemTables()
        {
            var result = new List<string>();

            var ds = _SQL.Query(@"
                                SELECT 
	                                TABLE_NAME
	                                FROM INFORMATION_SCHEMA.TABLES
	                                WHERE 
			                                TABLE_TYPE = 'BASE TABLE' 
		                                AND TABLE_CATALOG='HEMaster'
		                                AND TABLE_NAME like 'HEM%'
	                                Order by TABLE_NAME");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                result.Add(ds.Tables[0].Rows[i]["TABLE_NAME"].ToString().Trim().ToUpper());
            }

            return result;
        }

        List<string> getHerTables()
        {
            var result = new List<string>();

            var ds = _SQL.Query(@"
                                SELECT 
	                                TABLE_NAME
	                                FROM INFORMATION_SCHEMA.TABLES
	                                WHERE 
			                                TABLE_TYPE = 'BASE TABLE' 
		                                AND TABLE_CATALOG='HEReserv'
		                                AND TABLE_NAME like 'HER%'
	                                Order by TABLE_NAME");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                result.Add(ds.Tables[0].Rows[i]["TABLE_NAME"].ToString().Trim().ToUpper());
            }

            return result;
        }

        string generateTableClass(string tableName, string nameSpace)
        {
            string result = "using System; \n\n";

            var ds = _SQL.Query($@"
                                declare @TableName sysname = '{tableName}'
                                declare @Result varchar(max) = 'namespace {nameSpace} 
                                {{'

                                select @Result = @Result + 'public class ' + @TableName + '
                                {{'

                                select @Result = @Result + '
                                    public ' + ColumnType + NullableSign + ' ' + ColumnName + ' {{ get; set; }}
                                '
                                from
                                (
                                    select 
                                        replace(col.name, ' ', '_') ColumnName,
                                        column_id ColumnId,
                                        case typ.name 
                                            when 'bigint' then 'long'
                                            when 'binary' then 'byte[]'
                                            when 'bit' then 'bool'
                                            when 'char' then 'string'
                                            when 'date' then 'DateTime'
                                            when 'datetime' then 'DateTime'
                                            when 'datetime2' then 'DateTime'
                                            when 'datetimeoffset' then 'DateTimeOffset'
                                            when 'decimal' then 'decimal'
                                            when 'float' then 'double'
                                            when 'image' then 'byte[]'
                                            when 'int' then 'int'
                                            when 'money' then 'decimal'
                                            when 'nchar' then 'string'
                                            when 'ntext' then 'string'
                                            when 'numeric' then 'decimal'
                                            when 'nvarchar' then 'string'
                                            when 'real' then 'float'
                                            when 'smalldatetime' then 'DateTime'
                                            when 'smallint' then 'short'
                                            when 'smallmoney' then 'decimal'
                                            when 'text' then 'string'
                                            when 'time' then 'TimeSpan'
                                            when 'timestamp' then 'long'
                                            when 'tinyint' then 'byte'
                                            when 'uniqueidentifier' then 'Guid'
                                            when 'varbinary' then 'byte[]'
                                            when 'varchar' then 'string'
                                            else 'UNKNOWN_' + typ.name
                                        end ColumnType,
                                        case 
                                            when col.is_nullable = 1 and typ.name in ('bigint', 'bit', 'date', 'datetime', 'datetime2', 'datetimeoffset', 'decimal', 'float', 'int', 'money', 'numeric', 'real', 'smalldatetime', 'smallint', 'smallmoney', 'time', 'tinyint', 'uniqueidentifier') 
                                            then '?' 
                                            else '' 
                                        end NullableSign
                                    from sys.columns col
                                        join sys.types typ on
                                            col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id
                                    where object_id = object_id(@TableName)
                                ) t
                                order by ColumnId

                                set @Result = @Result  + '
                                }} }}'

                                select @Result as result");

            result = result +  ds.Tables[0].Rows[0]["result"].ToString();
            result = result.Replace("params", "_params");
            result = result.Replace("#", "");

            return result;
        }
    }
}
