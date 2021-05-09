using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MB2_Map
{
    class TownList
    {
        public class Town
        {
            public string Name { get; }
            public PointF Location { get; }

            public Town(string name, PointF location)
            {
                Name = name;
                Location = location;
            }
            public Town(string name, float locationX, float locationY)
            {
                Name = name;
                Location = new PointF(locationX, locationY);
            }
        }
        private readonly Dictionary<string, Town> _townDictionary = new();
        private readonly Dictionary<(string, string), float> _trueDistance = new();
        public List<string> TownsList { get; } = new();
        public void AddTown(string name, PointF loc)
        {
            _townDictionary.Add(name, new Town(name, loc));
            TownsList.Add(name);
        }
        public void AddTown(string name, float locationX, float locationY)
        {
            _townDictionary.Add(name, new Town(name, locationX, locationY));
            TownsList.Add(name);
        }

        public void AddTrueLocation(string town1, string town2, float distance)
        {
            string[] townArray = { town1, town2 };
            Array.Sort(townArray);
            _trueDistance.Add((townArray[0], townArray[1]), distance);
        }

        public PointF GetTownPoint(string name)
        {
            try
            {
                return _townDictionary[name].Location;
            }
            catch (KeyNotFoundException)
            {
                return PointF.Empty;
            }
        }
        public float GetTownsDistance(string town1, string town2)
        {
            try
            {
                //Debugger.Break();
                string[] townArray = { town1, town2 };
                Array.Sort(townArray);
                return _trueDistance[(townArray[0], townArray[1])];
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    var town1Obj = _townDictionary[town1];
                    var town2Obj = _townDictionary[town2];
                    return (float)Math.Sqrt(Math.Pow(town2Obj.Location.X - town1Obj.Location.X, 2) + Math.Pow(town2Obj.Location.Y - town1Obj.Location.Y, 2));
                }
                catch (KeyNotFoundException)
                {
                    return default;
                }
            }
        }
    }
}
