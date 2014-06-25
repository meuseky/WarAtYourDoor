using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Home_Front
{
    //Handles all File I/O
    static class IOHandler
    {      
        #region Variables

        #endregion

        #region Methods
        //Constructor
        static IOHandler()
        {
        }

        //Checks to see if a File exists
        public static void CheckFile(string fileName)
        {
            if (File.Exists(fileName))
            {

            }
            else
            {
                File.Create(fileName);
                FileStream scoreFile = new FileStream(fileName, FileMode.Open, FileAccess.Write);
                BinaryWriter newWriter = new BinaryWriter(scoreFile);
                newWriter.Write(0);
                newWriter.Flush();
                newWriter.Close();
            }
        }

        //Resets file to Empty state
        public static void ResetFile(string fileName)
        {
            FileStream scoreFile = new FileStream(fileName, FileMode.Open, FileAccess.Write);
            BinaryWriter newWriter = new BinaryWriter(scoreFile);
            newWriter.Write(0);
            newWriter.Flush();
            newWriter.Close();
        }

        //Retreives the Highscore table from file
        public static int[] GetHighScores()
        {
            int[] number = new int[5];
        start:
            FileStream scoreFile = new FileStream("scores.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader scoreReader = new BinaryReader(scoreFile);
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    number[i] = scoreReader.ReadInt32();
                }
            }
            catch (EndOfStreamException)
            {
                BinaryWriter newWriter = new BinaryWriter(scoreFile);
                for (int i = 0; i < 5; i++)
                {
                    newWriter.Write(0);
                }
                newWriter.Flush();
                newWriter.Close();
                goto start;
            }
            scoreFile.Close();
            scoreReader.Close();
            return number;
        }

        //Retreives the Achievement table from file
        public static int[] GetAchievements()
        {
            int[] number = new int[4];
        start:
            FileStream scoreFile = new FileStream("achiev.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader scoreReader = new BinaryReader(scoreFile);
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    number[i] = scoreReader.ReadInt32();
                }
            }
            catch (EndOfStreamException)
            {
                BinaryWriter newWriter = new BinaryWriter(scoreFile);
                for (int i = 0; i < 4; i++)
                {
                    newWriter.Write(0);
                }
                newWriter.Flush();
                newWriter.Close();
                goto start;
            }
            scoreFile.Close();
            scoreReader.Close();
            return number;
        }

        //Writes Highscores to File
        public static void WriteHighScore(int[] playerscore)
        {
            FileStream scoreFile = new FileStream("scores.dat", FileMode.Create, FileAccess.Write);
            BinaryWriter scoreWriter = new BinaryWriter(scoreFile);
            for (int i = 0; i < 5; i++)
            {
                scoreWriter.Write(playerscore[i]);
            }
            scoreWriter.Flush();
            scoreWriter.Close();
        }

        //Writes Achievements to File
        public static void WriteAchievements(int[] achievements)
        {
            FileStream scoreFile = new FileStream("achiev.dat", FileMode.Create, FileAccess.Write);
            BinaryWriter scoreWriter = new BinaryWriter(scoreFile);
            for (int i = 0; i < 4; i++)
            {
                scoreWriter.Write(achievements[i]);
            }
            scoreWriter.Flush();
            scoreWriter.Close();
        }
        #endregion
    }
}


