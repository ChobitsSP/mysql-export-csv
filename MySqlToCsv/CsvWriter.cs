using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlToCsv
{
    public static class CsvWriter
    {
        #region Constants
        private const char DefaultDelimiter = ',';
        private const char DefaultQuote = '"';
        private const char DefaultEscape = '"';
        #endregion

        #region Write

        public static TextWriter WriteRows(IEnumerable<string[]> rows, params string[] cols)
        {
            TextWriter sw = new StringWriter();
            Write(sw, cols);
            Write(sw, rows);
            return sw;
        }

        public static void Write(TextWriter writer, IEnumerable<string[]> rows)
        {
            foreach (var row in rows)
            {
                Write(writer, row, DefaultDelimiter);
            }
        }

        public static void Write(TextWriter writer, IEnumerable<string> row)
        {
            Write(writer, row, DefaultDelimiter);
        }

        public static void Write(TextWriter writer, IEnumerable<string> row, char delimiter)
        {
            bool start = false;
            foreach (string str in row)
            {
                if (start == true)
                    writer.Write(delimiter);
                else
                    start = true;
                writer.Write(CsvString(str, delimiter));
            }
            writer.WriteLine();
        }

        public static string CsvString(string str, char delimiter)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            if (str.Contains(delimiter.ToString()) || str.Contains(DefaultQuote.ToString()) || str.Contains(@"\r") || str.Contains(@"\n"))
            {
                var sb = new StringBuilder();
                sb.Append(DefaultEscape);
                foreach (char c in str)
                {
                    if (c == DefaultQuote) sb.Append(DefaultQuote);
                    sb.Append(c);
                }
                sb.Append(DefaultEscape);
                return sb.ToString();
            }
            return str;
        }

        #endregion
    }
}
