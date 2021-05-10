using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

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
            _towns = new TownList(listBox1);
            listBox1.Items.Clear();
            var loadFilesThread = new Thread(Load_Files_Into_ListBox);
            loadFilesThread.Start();
        }

        private void Load_Files_Into_ListBox()
        {
            try
            {
                LoadTrueList();
            }
            catch (Exception)
            {
                /* ignored */
            }

            var wd = @$"{Directory.GetCurrentDirectory()}\Towns";
            var di = new DirectoryInfo(wd);
            try
            {
                var files = di.GetFiles("*.txt");
                var rx = new Regex(@"\d+\.?\d+");
                foreach (var file in files)
                {
                    if (file.DirectoryName == null)
                        continue;
                    using var sr = new StreamReader(file.FullName);
                    var line = sr.ReadLine();
                    if (line == null)
                        continue;
                    var splitLine = rx.Matches(line);
                    if (splitLine.Count != 2)
                        continue;
                    var extRemoved = file.Name.Remove(file.Name.Length - file.Extension.Length);
                    Invoke(new MethodInvoker(delegate
                    {
                        _towns.AddTown(extRemoved, float.Parse(splitLine[0].Value),
                            float.Parse(splitLine[1].Value));
                    }));
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"An Error has occurred!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.ExitThread();
                Invoke(new MethodInvoker(Application.Exit));
                Application.ExitThread();
            }

            Invoke(new MethodInvoker(delegate
            {
                //listBox1.Sorted = true;
                //listBox2.Sorted = true;
                //Debugger.Break();
                listBox1.DataSource = new List<TownList.Town>(_towns.TownsList);
                listBox1.SelectedIndex = 0;
                //Debugger.Break();
                listBox2.DataSource = new List<TownList.Town>(_towns.TownsList);
                Enabled = true;
            }));
            //Debugger.Break();
        }

        private void LoadTrueList()
        {
            var wd = @$"{Directory.GetCurrentDirectory()}\TrueDistance.txt";
            var rx = new Regex(@"\s*(\w+ *\w+)\s*,\s*(\w+ ?\w+)\s*, (\d+\.?\d+)");
            using var sr2 = new StreamReader(wd);
            while (!sr2.EndOfStream)
            {
                var line = sr2.ReadLine();
                if (line == null)
                    continue;
                var splitLine = rx.Matches(line);
                //var splitLine = line.Split(",");
                if (splitLine.Count != 1 && splitLine[0].Groups.Count != 4)
                    continue;
                Invoke(new MethodInvoker(delegate
                {
                    _towns.AddTrueLocation(splitLine[0].Groups[1].Value, splitLine[0].Groups[2].Value, float.Parse(splitLine[0].Groups[3].Value));
                }));
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var towns = _towns.TownsList.Select(str => str.ToString()).ToList();
            //listBox2.DataSource = new List<TownList.Town>(_towns.TownsList);
            var currentItem = listBox2.SelectedItem;
            listBox2.DataSource =
                new List<TownList.Town>(_towns.TownsList.Where(town1 =>
                    town1.Name.ToLower().Contains(textBox2.Text.ToLower())));
            listBox2.SelectedItem = currentItem;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.DataSource =
                new List<TownList.Town>(_towns.TownsList.Where(town1 =>
                    town1.Name.ToLower().Contains(textBox1.Text.ToLower())));
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            var currentItem = listBox2.SelectedItem;
            listBox2.DataSource =
                new List<TownList.Town>(_towns.TownsList.Where(town1 =>
                    town1.Name.ToLower().Contains(textBox2.Text.ToLower())));
            listBox2.SelectedItem = currentItem;
        }
    }
}