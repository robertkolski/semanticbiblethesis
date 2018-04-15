using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLModerateName
{
    public static class IDataReaderExtensions
    {
        public static string GetString(this IDataReader reader, string name)
        {
            object value = reader[name];
            if (value == DBNull.Value)
            {
                return null;
            }
            if (value is string)
            {
                return (string)value;
            }
            return value.ToString();
        }
    }
}
