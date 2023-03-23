namespace MarkupParser
{
    public partial class mainForm : Form
    {

        JsonParser json;
        CsvParser csv;
        XmlParser xml;

        public mainForm()
        {
            InitializeComponent();
        }

        private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ���������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new()
            {
                Filter = "���� �������� (*.json;*.xml;*.csv)|*.json;*.xml;*.csv",
                CheckFileExists = true,
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = fileDialog.FileName;
                if (filename.EndsWith(".json"))
                {
                    json = new(filename);
                    json.BindGrid(dataGrid);
                }
                else if (filename.EndsWith(".csv"))
                {
                    csv = new(filename);
                    csv.BindGrid(dataGrid);
                }
                else if (filename.EndsWith(".xml"))
                {
                    xml = new(filename);
                    xml.BindGrid(dataGrid);
                }
            }
        }

        private void jsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new()
            {
                DefaultExt = ".json",
                AddExtension = true,
                OverwritePrompt = true,
                Filter = "���� �������� (*.json)|*.json"
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = fileDialog.FileName;
                json.Save(dataGrid, filename);
            }
        }

        private void csvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new()
            {
                DefaultExt = ".csv",
                AddExtension = true,
                OverwritePrompt = true,
                Filter = "���� �������� (*.csv)|*.csv"
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = fileDialog.FileName;
                csv.Save(dataGrid, filename);
            }
        }

        private void xmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new()
            {
                DefaultExt = ".xml",
                AddExtension = true,
                OverwritePrompt = true,
                Filter = "���� �������� *.xml|*.xml"
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = fileDialog.FileName;
                xml.Save(dataGrid, filename);
            }
        }
    }
}