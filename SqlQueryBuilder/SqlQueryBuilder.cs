using System.Collections.ObjectModel;
using System;
using System.Text;
using System.Collections.Generic;

namespace SqlQueryBuilder
{
    [Flags]
    public enum ColumnAttributes
    {
        None = 0,
        PrimaryKey = 1,
        AutoIncrement = 2,
        Unique = 4,
        NotNull = 8,
        Ascending = 16,
        Descending = 32
    }
    static class SqlHelper
    {
        public static Dictionary<Type, string> TypeMapping = new Dictionary<Type, string>()
        {
            { typeof(bool), "INTEGER" },
            { typeof(byte), "INTEGER" },
            { typeof(sbyte), "INTEGER" },
            { typeof(char), "INTEGER" },
            { typeof(decimal), "REAL" },
            { typeof(double), "REAL" },
            { typeof(float), "REAL" },
            { typeof(int), "INTEGER" },
            { typeof(uint), "INTEGER" },
            { typeof(long), "INTEGER" },
            { typeof(ulong), "INTEGER" },
            { typeof(short), "INTEGER" },
            { typeof(ushort), "INTEGER" },
            { typeof(string), "TEXT" }
        };

        public static string ValueToSqliteValue(object value)
        {
            if (value is string)
            {
                return $"'{value}'";
            }
            else if (value is DateTime)
            {
                var dateTime = (DateTime)value;

                return $"'{dateTime.ToString("yyyy-MM-dd hh:mm:ss.fff")}'";
            }
            else if (value is bool)
            {
                return (bool)value ? 1.ToString() : 0.ToString();
            }

            return value.ToString();
        }

        public static string[] ValuesToSqliteValues(Dictionary<string, object>.ValueCollection values)
        {
            string[] sqlValues = new string[values.Count];

            int i = 0;

            foreach (var value in values)
            {
                sqlValues[i] = ValueToSqliteValue(value);
                i++;
            }

            return sqlValues;
        }
    } 

    public interface ISqlFragment
    {
        string ToString();
    }

    public interface IConstraint
    {

    }

    public abstract class AbstractConstraint : ISqlFragment
    {
        public abstract override string ToString();
    }

    public class LogicalConnective : AbstractConstraint
    {
        protected string _operator;
        public AbstractConstraint Left { get; set; }
        public AbstractConstraint Right { get; set; }

        public override string ToString()
        {
            return $"( {Left} {_operator} {Right} )";
        }
    }

    public class And : LogicalConnective
    {
        public And()
        {
            _operator = "AND";
        }
    }

    public class Or : LogicalConnective
    {
        public Or()
        {
            _operator = "OR";
        }
    }

    public class Condition : AbstractConstraint
    {
        protected string _operator;
        public object Left { get; set; }
        public object Right { get; set; }

        public override string ToString()
        {
            return $"( {Left} {_operator} {Right} )";
        }
    }

    public class Equals : Condition
    {
        public Equals()
        {
            _operator = "=";
        }
    }

    public class NotEquals : Condition
    {
        public NotEquals()
        {
            _operator = "!=";
        }
    }

    public class SelectQuery : ISqlFragment
    {
        public List<string> Columns { get; set; } = new List<string>();

        public string From { get; set; }

        public AbstractConstraint Where { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT ");
            
            if (Columns.Count < 1)
            {
                stringBuilder.Append("* ");
            }
            else
            {
                stringBuilder.Append(String.Join(", ", Columns));
                stringBuilder.Append(" ");
            }

            stringBuilder.Append("FROM ");
            stringBuilder.Append(From);

            if (Where != null)
            {
                stringBuilder.Append(" WHERE ");
                stringBuilder.Append(Where.ToString());
            }

            stringBuilder.Append(";");

            return stringBuilder.ToString();
        }
    }

    public class InsertQuery : ISqlFragment
    {
        public string Table { get; set; }
        public Dictionary<string, object> Values = new Dictionary<string, object>();

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("INSERT INTO ");
            stringBuilder.Append(Table);
            stringBuilder.Append(" ( ");
            stringBuilder.Append(String.Join(", ", Values.Keys));
            stringBuilder.Append(" ) VALUES ( ");
            stringBuilder.Append(String.Join(", ", SqlHelper.ValuesToSqliteValues(Values.Values)));
            stringBuilder.Append(" );");

            return stringBuilder.ToString();
        }
    }

    public class UpdateQuery : ISqlFragment
    {
        public string Table { get; set; }

        public Dictionary<string, object> Values = new Dictionary<string, object>();

        public AbstractConstraint Where { get; set; } 

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("UPDATE ");
            stringBuilder.Append(Table);
            stringBuilder.Append(" SET ");

            string[] stringColumns = new string[Values.Count];
            int i = 0;
            
            foreach(KeyValuePair<string, object> column in Values)
            {
                stringColumns[i] = $"{column.Key} = {SqlHelper.ValueToSqliteValue(column.Value)}";
                i++;
            }

            stringBuilder.Append(String.Join(", ", stringColumns));

            if (Where != null)
            {
                stringBuilder.Append(" WHERE ");
                stringBuilder.Append(Where.ToString());
            }

            stringBuilder.Append(";");

            return stringBuilder.ToString();
        }
    }

    public class DeleteQuery
    {
        public string Table { get; set; }

        public AbstractConstraint Where { get; set; } 

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("DELETE FROM ");
            stringBuilder.Append(Table);

            if (Where != null)
            {
                stringBuilder.Append(" WHERE ");
                stringBuilder.Append(Where.ToString());
            }

            stringBuilder.Append(";");

            return stringBuilder.ToString();
        }
    }

    public class Column
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public ColumnAttributes Attributes { get; set; } = ColumnAttributes.None;
    }
    public class CreateQuery
    {
        public string Table { get; set; }

        public bool IfNotExists { get; set; } = false;

        public List<Column> Columns { get; set; } = new List<Column>();
    
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CREATE TABLE ");

            if (IfNotExists)
            {
                stringBuilder.Append("IF NOT EXISTS ");
            }

            stringBuilder.Append(Table);
            stringBuilder.Append(" ( ");

            string[] stringColumns = new string[Columns.Count];

            for (int i = 0; i < Columns.Count; i++)
            {
                Column column = Columns[i];

                if (column.Attributes != ColumnAttributes.None)
                {
                    ColumnAttributes columnAttributes = column.Attributes;

                    string stringAttribute = "";

                    if ((columnAttributes & ColumnAttributes.PrimaryKey) > 0)
                    {
                        stringAttribute = "PRIMARY KEY";

                        if ((columnAttributes & ColumnAttributes.AutoIncrement) > 0)
                        {
                            stringAttribute += " AUTOINCREMENT";
                        }
                    }
                    else if ((columnAttributes & ColumnAttributes.Unique) > 0)
                    {
                        stringAttribute = "UNIQUE";
                    }
                    else if ((columnAttributes & ColumnAttributes.NotNull) > 0)
                    {
                        stringAttribute = "NOT NULL";
                    }

                    stringColumns[i] = $"{column.Name} {SqlHelper.TypeMapping[column.Type]} {stringAttribute}";
                }
                else
                {
                    stringColumns[i] = $"{column.Name} {SqlHelper.TypeMapping[column.Type]}";
                }
            }
            
            stringBuilder.Append(String.Join(", ", stringColumns));
            stringBuilder.Append(" );");

            return stringBuilder.ToString();
        }
    }

    public class DropQuery
    {
        public string Table { get; set; }

        public bool IfExists { get; set; } = false;

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("DROP TABLE ");

            if (IfExists)
            {
                stringBuilder.Append("IF EXISTS ");
            }

            stringBuilder.Append(Table);
            stringBuilder.Append(";");

            return stringBuilder.ToString();
        }
    }
}