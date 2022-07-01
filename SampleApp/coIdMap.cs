using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SampleApp.SomfyRts;

namespace SomfyRtsApp
{
    public static class coIdMap
    {
        public static Dictionary<uint, Tuple<int, SomfyRtsButton>> Map =
            new Dictionary<uint, Tuple<int, SomfyRtsButton>>()
            {
                { 1, new Tuple<int, SomfyRtsButton>(1, SomfyRtsButton.UpDown) },
                { 2, new Tuple<int, SomfyRtsButton>(1, SomfyRtsButton.My) },
                { 3, new Tuple<int, SomfyRtsButton>(1, SomfyRtsButton.Prog) },
                { 4, new Tuple<int, SomfyRtsButton>(2, SomfyRtsButton.UpDown) },
                { 5, new Tuple<int, SomfyRtsButton>(2, SomfyRtsButton.My) },
                { 6, new Tuple<int, SomfyRtsButton>(2, SomfyRtsButton.Prog) },                
                { 7, new Tuple<int, SomfyRtsButton>(3, SomfyRtsButton.UpDown) },
                { 8, new Tuple<int, SomfyRtsButton>(3, SomfyRtsButton.My) },
                { 9, new Tuple<int, SomfyRtsButton>(3, SomfyRtsButton.Prog) },                
                { 10, new Tuple<int, SomfyRtsButton>(4, SomfyRtsButton.UpDown) },
                { 11, new Tuple<int, SomfyRtsButton>(4, SomfyRtsButton.My) },
                { 12, new Tuple<int, SomfyRtsButton>(4, SomfyRtsButton.Prog) },                
                { 13, new Tuple<int, SomfyRtsButton>(5, SomfyRtsButton.UpDown) },
                { 14, new Tuple<int, SomfyRtsButton>(5, SomfyRtsButton.My) },
                { 15, new Tuple<int, SomfyRtsButton>(5, SomfyRtsButton.Prog) },                
                { 16, new Tuple<int, SomfyRtsButton>(6, SomfyRtsButton.UpDown) },
                { 17, new Tuple<int, SomfyRtsButton>(6, SomfyRtsButton.My) },
                { 18, new Tuple<int, SomfyRtsButton>(6, SomfyRtsButton.Prog) },
                { 19, new Tuple<int, SomfyRtsButton>(7, SomfyRtsButton.UpDown) },
                { 20, new Tuple<int, SomfyRtsButton>(7, SomfyRtsButton.My) },
                { 21, new Tuple<int, SomfyRtsButton>(7, SomfyRtsButton.Prog) },                
                { 22, new Tuple<int, SomfyRtsButton>(8, SomfyRtsButton.UpDown) },
                { 23, new Tuple<int, SomfyRtsButton>(8, SomfyRtsButton.My) },
                { 24, new Tuple<int, SomfyRtsButton>(8, SomfyRtsButton.Prog) },                
                { 25, new Tuple<int, SomfyRtsButton>(9, SomfyRtsButton.UpDown) },
                { 26, new Tuple<int, SomfyRtsButton>(9, SomfyRtsButton.My) },
                { 27, new Tuple<int, SomfyRtsButton>(9, SomfyRtsButton.Prog) },                
                { 28, new Tuple<int, SomfyRtsButton>(10, SomfyRtsButton.UpDown) },
                { 29, new Tuple<int, SomfyRtsButton>(10, SomfyRtsButton.My) },
                { 30, new Tuple<int, SomfyRtsButton>(10, SomfyRtsButton.Prog) },
            };

        public static Tuple<int, SomfyRtsButton> Get(uint coId)
        {
            return Map[coId];
        }

        public static bool Contains(uint coId)
        {
            return Map.ContainsKey(coId);
        }
    }
}
