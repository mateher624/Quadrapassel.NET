using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Quadrapassel
{
    [Serializable]
    public class HighScoresTable
    {
        public ICollection<HighScore> HighScoresList { get; }

        public HighScoresTable()
        {
            HighScoresList = new Collection<HighScore>();
        }

        public static HighScoresTable LoadScores()
        {
            try
            {
                using (Stream stream = File.Open("scores.dat", FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return (HighScoresTable)binaryFormatter.Deserialize(stream);
                }
            }
            catch (Exception)
            {
                return new HighScoresTable();
            }
        }

        public static void SaveScore(HighScoresTable instance)
        {
            using (var stream = File.Open("scores.dat", FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, instance);
                stream.Close();
            }
        }

        public void AddScore(long score)
        {
            HighScoresList.Add(new HighScore
            {
                Date = DateTime.Now,
                Score = score
            });
        }
    }

    [Serializable]
    public class HighScore
    {
        public DateTime Date { get; set; }
        public long Score { get; set; }
    }
}
