using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Threading.Tasks;
using System.Data.Common;
using System.Xml.Linq;

namespace MarkupParser
{
    internal class XmlParser
    {
        private readonly DataTable xml;
        public XmlParser(string filename) 
        {
            /*string plain;
            xml = new();

            using (StreamReader sr = new StreamReader(filename))
            {
                plain = sr.ReadToEnd().Replace("<Table>", "").Replace("</Table>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
            }
            string[] columns = plain.Split("</Column>");
            List<string[]> rows = new();
            foreach (string column in columns.SkipLast(1))
            {
                string[] items = column.Replace("</Item>", "").Split("<Item>");
                string col_name = items[0].Substring(items[0].IndexOf("=") + 1).Replace("\"", "").Replace(">", "").Trim();
                xml.Columns.Add(col_name);
                rows.Add(items.Skip(1).ToArray());
            }

            List<DataRow> dataRows = new();
            for (int i = 0; i < rows[0].Length; i++)
                dataRows.Add(xml.NewRow());

            for (int i = 0; i < rows.Count; i++)
                for (int j = 0; j < rows[0].Length; j++)
                    dataRows[j][i] = rows[i][j];

            foreach(DataRow row in dataRows)
                xml.Rows.Add(row);*/

            var xmlDoc = new XmlDocument();
            var dict = new Dictionary<string, List<string>>();
            xmlDoc.Load(filename);
            xml = new();

            var root = xmlDoc.DocumentElement;
            var columns = root.ChildNodes;

            foreach (XmlNode col in columns)
            {
                string colName;
                if (col.Attributes != null)
                    colName = col.Attributes[0].Value;
                else
                    colName = col.Name;

                var cells = col.ChildNodes;
                var cellValues = new List<string>();
                foreach (XmlNode cell in cells)
                {
                    var cellValue = cell.InnerText;
                    cellValues.Add(cellValue);
                }

                dict.Add(colName, cellValues);
            }

            int maxRowLength = this.MaxRowLength(dict);

            DataRow[] rows = new DataRow[maxRowLength];
            for (int j = 0; j < maxRowLength; j++)
                rows[j] = xml.NewRow();

            foreach (string key in dict.Keys)
                xml.Columns.Add(key, typeof(string));

            int i = 0;
            foreach (var key in dict.Keys)
            {
                int j = 0;
                foreach (var value in dict[key])
                    rows[j++][i] = value;
                i++;
            }

            foreach (DataRow row in rows)
                xml.Rows.Add(row);
        }

        public void BindGrid(DataGridView dataGrid)
        {
            dataGrid.DataSource = this.xml;
        }

        private int MaxRowLength(Dictionary<string, List<string>> dict)
        {
            int max = 0;
            foreach (string key in dict.Keys)
            {
                if (dict[key].Count > max)
                    max = dict[key].Count;
            }

            return max;
        }

        public void Save(DataGridView dataGrid, string filename)
        {
            ;
        }
    }
}
