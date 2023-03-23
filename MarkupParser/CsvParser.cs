using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace MarkupParser
{
    internal class CsvParser
    {
        private readonly DataTable csv;

        public CsvParser(string filename) {
            string plain;
            csv = new();

            using (StreamReader sr = new StreamReader(filename))
            {
                plain = sr.ReadToEnd().Trim();
            }

            string[] rows = plain.Split("\r\n");
            foreach (string col in rows[0].Split(","))
            {
                csv.Columns.Add(col);
            }

            foreach (string r in rows.Skip(1))
            {
                DataRow row = csv.NewRow();
                string[] cells = r.Split(",");
                for (int i = 0; i < cells.Length; i++)
                {
                    row[i] = cells[i];
                }
                csv.Rows.Add(row);
            }
        }

        public void BindGrid(DataGridView dataGrid)
        {
            dataGrid.DataSource = this.csv;
        }

        public void Save(DataGridView dataGrid, string filename)
        {
            ;
        }
    }
}
