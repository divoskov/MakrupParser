using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MarkupParser
{
    internal class JsonParser
    {
        private readonly DataTable json;

        public JsonParser(string filename) {
            // Parsing .json to Dictionary
            string plain;
            string pattern_start = @"\w+: \[(\w+|\d+)";
            string pattern_middle = @"\w+|\d+";
            string pattern_end = @"(\w+|\d+)\]";

            Regex regex_start = new(pattern_start);
            Regex regex_middle = new(pattern_middle);
            Regex regex_end = new(pattern_end);

            json = new();

            using (StreamReader sr = new(filename))
            {
                plain = sr.ReadToEnd().Trim();
            }
            string[] parts = plain.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("{", "").Replace("}", "").Replace(@"""", "").Split(",");

            List<string> column = new();
            Dictionary<string, string[]> dict = new();
            foreach (string part in parts)
            {
                if (regex_start.IsMatch(part)) {
                    column = new();
                    string[] sub = part.Split(":");
                    column.Add(sub[0]);
                    column.Add(sub[1].Replace("[", "").Trim());
                }
                else if (regex_end.IsMatch(part))
                {
                    string key = part.Replace("]", "").Trim();
                    column.Add(key);
                    key = column[0];
                    column.RemoveAt(0);
                    dict.Add(key, column.ToArray());
                }
                else if (regex_middle.IsMatch(part))
                {
                    column.Add(part.Trim());
                }
            }

            // Creating rows and columns
            int maxRowLength = this.MaxRowLength(dict);

            DataRow[] rows = new DataRow[maxRowLength];
            for (int j = 0; j < maxRowLength; j++)
                rows[j] = json.NewRow();

            foreach (string key in dict.Keys)
            {
                json.Columns.Add(key, typeof(string));
            }

            // Filling the rows
            int i = 0;
            foreach (var key in dict.Keys)
            {
                for (int j = 0; j < dict[key].Length; j++)
                {
                    rows[j][i] = dict[key][j];
                }
                i++;
            }

            // Adding rows to json DataTable
            foreach (DataRow row in rows)
            {
                json.Rows.Add(row);
            }
        }

        public void BindGrid(DataGridView dataGrid)
        {
            dataGrid.DataSource = this.json;
        }

        private int MaxRowLength(Dictionary<string, string[]> dict)
        {
            int max = 0;
            foreach (string key in dict.Keys) { 
                if (dict[key].Length > max)
                {
                    max = dict[key].Length;
                }
            }

            return max;
        }

        public void Save(DataGridView dataGrid, string filename)
        {
            ;
        }
    }
}
