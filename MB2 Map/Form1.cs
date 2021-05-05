using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MB2_Map
{
    public partial class Form1 : Form
    {
        private TownList _towns;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _towns = new TownList();
            listBox1.Items.Clear();
            var loadFilesThread = new Thread(Load_Files_Into_ListBox);
            loadFilesThread.Start();
        }
        private void Load_Files_Into_ListBox()
        {
            var wd = @$"{Directory.GetCurrentDirectory()}\Towns";
            var di = new DirectoryInfo(wd);
            //Debugger.Break();
            var files = di.GetFiles("*.txt");
            foreach (var file in files)
            {
                if (file.DirectoryName == null)
                    continue;
                using var sr = new StreamReader(file.FullName);
                var line = sr.ReadLine();
                if (line == null)
                    continue;
                var splitLine = line.Split(",");
                if (splitLine.Length != 2)
                    continue;
                Invoke(new MethodInvoker(delegate
                {
                    var extRemoved = file.Name.Remove(file.Name.Length - file.Extension.Length);
                    _towns.AddTown(extRemoved, int.Parse(splitLine[0]), int.Parse(splitLine[1]));
                    listBox1.Items.Add(extRemoved);
                    listBox2.Items.Add(extRemoved);
                }));
            }
            wd = @$"{Directory.GetCurrentDirectory()}\TrueDistance.txt";
            using var sr2 = new StreamReader(wd);
            while (!sr2.EndOfStream)
            {
                var line = sr2.ReadLine();
                if (line == null)
                    continue;
                var splitLine = line.Split(",");
                if (splitLine.Length != 3)
                    continue;
                Invoke(new MethodInvoker(delegate
                {
                    _towns.AddTrueLocation(splitLine[0], splitLine[1], uint.Parse(splitLine[2]));
                }));
            }

            Invoke(new MethodInvoker(delegate
            {
                listBox1.Sorted = true;
                listBox2.Sorted = true;
                listBox1.SelectedIndex = 0;
                listBox2.SelectedIndex = 1;
                Enabled = true;
            }));
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null || listBox2.SelectedItem == null)
                return;
            label1.Text = _towns.GetTownsDistance((string)listBox1.SelectedItem, (string)listBox2.SelectedItem).ToString();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null || listBox2.SelectedItem == null)
                return;
            label1.Text = _towns.GetTownsDistance((string)listBox1.SelectedItem, (string)listBox2.SelectedItem).ToString();
        }
    }
}
