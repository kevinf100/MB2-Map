using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MB2_Map
{
    class TownList
    {
        public class Town
        {
            public string Name { get; }
            public Point Location { get; }

            public Town(string name, Point location)
            {
                Name = name;
                Location = location;
            }
            public Town(string name, int locationX, int locationY)
            {
                Name = name;
                Location = new Point(locationX, locationY);
            }

        }
        private readonly Dictionary<string, Town> _townDictionary = new();
        private readonly Dictionary<(string, string), uint> _trueDistance = new();
        public List<string> TownsList { get; } = new();
        public void AddTown(string name, Point loc)
        {
            _townDictionary.Add(name, new Town(name, loc));
            TownsList.Add(name);
        }
        public void AddTown(string name, int locationX, int locationY)
        {
            _townDictionary.Add(name, new Town(name, locationX, locationY));
            TownsList.Add(name);
        }

        public void AddTrueLocation(string town1, string town2, uint distance)
        {
            string[] townArray = { town1, town2 };
            Array.Sort(townArray);
            _trueDistance.Add((townArray[0], townArray[1]), distance);
        }

        public Point GetTownPoint(string name)
        {
            try
            {
                return _townDictionary[name].Location;
            }
            catch (KeyNotFoundException)
            {
                return Point.Empty;
            }
        }
        public uint GetTownsDistance(string town1, string town2)
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
                    return (uint)Math.Sqrt(Math.Pow(town2Obj.Location.X - town1Obj.Location.X, 2) + Math.Pow(town2Obj.Location.Y - town1Obj.Location.Y, 2));
                }
                catch (KeyNotFoundException)
                {
                    return default;
                }
            }
        }
    }
}
