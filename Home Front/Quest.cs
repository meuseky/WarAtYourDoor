using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    /// <summary>
    /// First create Lists of 'Choices' for 'SubQuests'
    /// List<Choice> choices01 = List<Choice>()
    /// {
    ///     new Choice("first message", "first outcome", 86)
    ///     new Choice("second message", "second outcome", 87)
    ///     new Choice("third message", "third outcome", 88)
    /// }
    /// List<Choice> choices02 = List<Choice>()
    /// {
    ///     ...
    /// }
    /// ...etc.
    /// 
    /// Then create Lists of 'SubQuest' classes & pass in the 'Choice' Lists
    /// List<SubQuest> subquests01 = List<SubQuest>()
    /// {
    ///     new SubQuest(new Point(6,8), "PopUp Message", choices01)
    ///     new SubQuest(new Point(3,4), "Another Message", choices02)
    ///     ...etc. for all subquests in 'Quest'
    /// }
    /// 
    /// Then create a List of 'Quest' classes and pass in the 'SubQuest' Lists
    /// List<Quest> quests = List<Quest>()
    /// {
    ///     new Quest(23, 37, subquests01)
    ///     new Quest(
    /// }
    /// 
    /// </summary> 

    class Quest
    {
        #region Variables

        //Id number for each quest
        private int m_questId;
        //Checks if there is a follow-on quest. 0 if none (level over), otherwise put in quest id#.
        private int[] m_nextQuest;
        //List of constituent SubQuests
        private SubQuest[] m_subQuests;
        //Quest name
        private string m_questName;
        //Quest description
        private string m_questDesr;
        //Is quest active?
        private bool m_active;
        //The current subquest running
        private int m_currSubquest;
        //The level the quest is on
        private int m_questLevel;
        #endregion

        #region Properties
        public int QuestID
        {
            get { return m_questId; }
        }
        public bool Active
        {
            get { return m_active; }
            set { m_active = value; }
        }
        public int Level
        {
            get { return m_questLevel; }
        }
        public SubQuest[] SubQuests
        {
            get { return m_subQuests; }
        }
        public int CurrentSubQ
        {
            get{ return m_currSubquest; }
            set { m_currSubquest = value; }
        }
        public int[] NextQuests
        {
            get { return m_nextQuest; }
        }
        public string QuestName
        {
            get { return m_questName; }
        }
        public string QuestDescription
        {
            get { return m_questDesr; }
        }
        #endregion

        #region Methods
        //Constructor Method
        public Quest(bool active, int questId, int questLevel, int[] nextQuest, 
                        List<SubQuest> subQuests, string questName, string questDesr)
        {
            m_active = active;
            m_questId = questId;
            m_nextQuest = nextQuest;
            m_subQuests = subQuests.ToArray();
            m_questName = questName;
            m_questDesr = questDesr;
            m_currSubquest = 0;
            m_questLevel = questLevel;
        }

        public void Update()
        {
            if (m_currSubquest >= m_subQuests.Count())
            {
                m_active = false;
            }            
            if (Player.gridPosition == m_subQuests[m_currSubquest].MapPosition)
            {
                m_currSubquest++;
                //Need something to display the popup info, (static class?)

            }
        }

        public void Draw()
        {

        }
        #endregion
    }

    class SubQuest
    {
        #region Variables
        //Point at which the quest is triggered
        private Point m_point;
        //Message triggered at point
        private string m_message;
        //List of possible choices
        private Choice[] m_choice;
        //Event ID for PopUp Box
        private int m_popUpID;
        #endregion

        #region Properties
        public Choice[] Choices
        {
            get { return m_choice; }
        }
        public Point MapPosition
        {
            get { return m_point; }
        }
        public string Message
        {
            get { return m_message; }
        }
        public int PopUpID
        {
            get { return m_popUpID; }
        }
        #endregion

        #region Methods
        public SubQuest(int popUpID, Point point, string message, List<Choice> choice)
        {
            m_point = point;
            m_message = message;
            m_choice = choice.ToArray();
            m_popUpID = popUpID;
        }
        #endregion
    }

    class Choice
    {
        #region Variables
        //Text of choice
        private string m_choiceText;
        //Text given if choice chosen
        private string m_outcomeText;
        //For managing outcome (rewards, etc.)
        private int m_outcomeID;
        #endregion

        #region Properties
        public string ChoiceText
        {
            get { return m_choiceText; }
        }
        public string OutcomeText
        {
            get { return m_outcomeText; }
        }
        public int ID
        {
            get { return m_outcomeID; }
        }
        #endregion

        #region Methods
        public Choice(string choiceText, string outcomeText, int outcomeID)
        {
            m_choiceText = choiceText;
            m_outcomeText = outcomeText;
            m_outcomeID = outcomeID;
        }
        #endregion
    }
}
