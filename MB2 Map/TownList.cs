using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MB2_Map
{
    public class TownList
    {
        public class Town
        {
            private readonly TownList _mainClass;
            public string Name { get; set; }
            public PointF Location { get; }

            public Town(TownList mainClass, string name, PointF location)
            {
                _mainClass = mainClass;
                Name = name;
                Location = location;
            }

            public Town(TownList mainClass, string name, float locationX, float locationY)
            {
                _mainClass = mainClass;
                Name = name;
                Location = new PointF(locationX, locationY);
            }

            public override string ToString()
            {
                return _mainClass._referTo.SelectedItem == null
                    ? Name
                    : $"{Name} - {_mainClass.GetTownsDistance(this, _mainClass._referTo.SelectedItem as Town)}";
            }

            public string ToString(bool getName)
            {
                return getName ? Name : ToString();
            }
        }

        //private readonly Dictionary<string, Town> _townDictionary = new();
        private readonly Dictionary<(string, string), float> _trueDistance = new();
        private readonly ListBox _referTo;

        private readonly Func<Town, Town, float> _distForm = (town1, town2) =>
            (float) Math.Sqrt(Math.Pow(town2.Location.X - town1.Location.X, 2) +
                              Math.Pow(town2.Location.Y - town1.Location.Y, 2));

        public List<Town> TownsList { get; } = new();

        public TownList(ListBox listBox)
        {
            _referTo = listBox;
        }

        public void AddTown(string name, PointF loc)
        {
            TownsList.Add(new Town(this, name, loc));
        }

        public void AddTown(string name, float locationX, float locationY)
        {
            var newTown = new Town(this, name, locationX, locationY);
            TownsList.Add(newTown);
            foreach (var town in TownsList)
            {
                string[] townArray = {name, town.ToString()};
                Array.Sort(townArray);
                var townTuple = (townArray[0], townArray[1]);
                if (!_trueDistance.ContainsKey(townTuple))
                    _trueDistance.Add(townTuple, _distForm(town, newTown));
            }
        }

        public void AddTrueLocation(string town1, string town2, float distance)
        {
            string[] townArray = {town1, town2};
            Array.Sort(townArray);
            var townTuple = (townArray[0], townArray[1]);
            if (!_trueDistance.ContainsKey(townTuple))
                _trueDistance.Add((townArray[0], townArray[1]), distance);
        }

        public float GetTownsDistance(Town town1, Town town2)
        {
            //Debugger.Break();
            string[] townArray = {town1.ToString(true), town2.ToString(true)};
            Array.Sort(townArray);
            return _trueDistance[(townArray[0], townArray[1])];
        }
    }
}