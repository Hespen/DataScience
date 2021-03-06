﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace HashMap
{
    internal class DataProcessor
    {
        private String _filePath = @"../../userItem.data";
        private Char _splitChar = ',';
        private readonly Dictionary<int, UserPreference> _map;
        public static HashSet<int> ArticleIds;

        public DataProcessor()
        {
            _map = new Dictionary<int, UserPreference>();
            ArticleIds = new HashSet<int>();
        }

        public void setFilePath(int path)
        {
            switch (path)
            {
                case Constants.ds1:
                    _filePath = Constants.dataSet1;
                    _splitChar = Constants.dataSplit;
                    break;
                case Constants.ds2:
                    _filePath = Constants.dataSet2;
                    _splitChar = Constants.dataSplit;
                    break;
                case Constants.ml:
                    _filePath = Constants.movieLensDataSet;
                    _splitChar = Constants.movieLensSplit;
                    break;

            }

        }

        public Dictionary<int, UserPreference> ReadDataFromFile()
        {
            try
            {
                using (var sr = new StreamReader(_filePath))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        ProcessLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            return _map;
        }

        private void ProcessLine(String line)
        {
            String[] data = line.Split(_splitChar);
            UserPreference currentPreference;
            int userId = Convert.ToInt16(data[0]);
            int articleId = Convert.ToInt16(data[1]);
            ArticleIds.Add(articleId);
            float rating = float.Parse(data[2], CultureInfo.InvariantCulture.NumberFormat);
            if (!_map.ContainsKey(userId))
            {
                currentPreference = new UserPreference();
                _map.Add(userId, currentPreference);
            }
            else
            {
                currentPreference = _map[userId];
            }
            currentPreference.AddNewRecord(articleId, rating);
        }
    }
}