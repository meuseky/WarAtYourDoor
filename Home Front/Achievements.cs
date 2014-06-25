using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    /*Keeps track of Player Achievements
    0 - New Game
    1 - Finish first level
    2 - Finish Game
    3 - Earn 10,00 Points
    */
    static class Achievements
    {
        #region Variables
        //Holds y/n state of achievements
        private static int[] achievements;
        #endregion

        #region Properties
        public static int[] Achievement
        {
            get { return achievements; }
        }
        #endregion

        #region Methods
        //Constructor
        static Achievements()
        {
            achievements = new int[4];
            achievements = IOHandler.GetAchievements();
        }

        //Sets value to 1 if achievement is triggered
        static public void UpdateAchievements(int achieveID)
        {
            achievements[achieveID] = 1;
        }

        //Resets in SETTINGS menu
        static public void ResetAll()
        {
            for (int i = 0; i < 4; i++)
            {
                achievements[i] = 0;
            }
        }
        #endregion
    }
}
