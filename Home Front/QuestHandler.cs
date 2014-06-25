﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Home_Front
{
    //Implements & Manages Quests
    static class QuestHandler
    {
        #region Variables

        public static Quest[] gameQuests;

        //Keeps track of PopUp Image
        public static int popUpFlag;

        //Quest Marker Animation
        private static float animTimer;
        private static int animFrame;

        #endregion

        #region Methods
        //Constructor
        static QuestHandler()
        {
            SetAllQuests();
            popUpFlag = 0;
            animTimer = 0.5f;
            animFrame = 0;
        }

        //Update current quests
        public static void Update(GameTime gameTime)
        {
            //Update Animation
            if (animTimer > 0)
            {
                animTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                animFrame = (animFrame + 1) % 4;
                animTimer = 0.35f;
            }

            //Cycle through quests
            foreach (Quest quest in gameQuests)
            {
                //If Current level..
                if (quest.Level == InPlay.currentLevel)
                {
                    //If Quest is active...
                    if (quest.Active)
                    {
                        //If Player is in quest location...
                        if (Player.gridPosition == quest.SubQuests[quest.CurrentSubQ].MapPosition)
                        {

                            popUpFlag = quest.SubQuests[quest.CurrentSubQ].PopUpID;
                            PopUp.sqHolder = quest.SubQuests[quest.CurrentSubQ];
                            
                            quest.CurrentSubQ++;

                            //If last SubQuest in Quest finish Quest
                            if (quest.CurrentSubQ == quest.SubQuests.Length)
                            {
                                quest.Active = false;

                                //If last quest in level finish level
                                if (quest.NextQuests[0] == 0)
                                {

                                    QuestHandler.RunChoiceOutcomes(PopUp.sqHolder.Choices[0].ID);
                                    InPlay.currentLevel++;

                                    //Stop level from being out of bounds
                                    if (InPlay.currentLevel > 5)
                                    {
                                        Player.tempScore = Player.Score;
                                        Player.UpdateHighScores();
                                        StateHandler.gameState = GameState.InPlay;
                                        InPlay.inPlayState = InPlayState.Win;
                                        return;
                                    }
                                    
                                    //Remove Active Items
                                    Items.ResetItems();
                                    InPlay.inPlayState = InPlayState.NewLevel;
                                    return;
                                }
                                //Activate quests that completing this quest begins
                                ActivateNewQuests(quest.NextQuests);
                            }
                            AudioHandler.PlayEffectInstance(4);
                            InPlay.inPlayState = InPlayState.PopUpScreen;
                        }
                    }
                }
            }
        }

        //Draw quest marker animation
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Quest quest in gameQuests)
            {
                if (quest.Level == InPlay.currentLevel)
                {
                    if (quest.Active)
                    {
                        try
                        {
                            Vector2 tempPosition;
                            tempPosition = GetScreenPosition(quest.SubQuests[quest.CurrentSubQ].MapPosition);
                            spriteBatch.Draw(InPlay.inPlayTexture[12], new Rectangle((int)tempPosition.X + 10, (int)tempPosition.Y + 10, 80, 80),
                                new Rectangle(animFrame * 100, 0, 100, 100), Color.White);
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }

                    }
                }
            }
        }

        //Holds Quest/SubQuest/Choice Data
        public static void SetAllQuests()
        {
            List<Choice> choices01 = new List<Choice>();
            choices01.Add(new Choice("Thank you officer", "Go to Public Service building", 00));

            List<Choice> choices02 = new List<Choice>();
            choices02.Add(new Choice("Not what I expected but thank you.", "Wait for further announcements", 01));

            List<Choice> choices03 = new List<Choice>();
            choices03.Add(new Choice("I will help you. Hold on.", "Go to the hospital", 04));
            choices03.Add(new Choice("I don't have time for this.", "The man looks devastated by your refusal.", 03));

            List<Choice> choices04 = new List<Choice>();
            choices04.Add(new Choice("Ok", "Return the medicine to the man.", 04));

            List<Choice> choices05 = new List<Choice>();
            choices05.Add(new Choice("Time to move on.", "You feel satisfied that you could save a man's\nlife.", 05));

            List<Choice> choices06 = new List<Choice>();
            choices06.Add(new Choice("I will try to find your husband", "Check the place where the man works.", 04));
            choices06.Add(new Choice("Better take your family to the evacuation point.", "You leave the family to its fate.", 21));

            List<Choice> choices07 = new List<Choice>();
            choices07.Add(new Choice("Good luck!", "The man thanks you and rushes to meet\nhis family.", 06));


            List<Choice> choices08 = new List<Choice>();
            choices08.Add(new Choice("Thank you.", "You should pack up and head there first\nthing in the morning", 04));

            List<Choice> choices09 = new List<Choice>();
            choices09.Add(new Choice("Ok", "", 02));

            List<Choice> choices10 = new List<Choice>();
            choices10.Add(new Choice("I can try and ..hope.", "Fortunately there were some things left\nin the destroyed shop.", 08));

            List<Choice> choices11 = new List<Choice>();
            choices11.Add(new Choice("Ok", "Carry the list of medicines to the\nnearest hospital.", 04));
            choices11.Add(new Choice("Sorry I don't have time for this.", "You decide not to help in this ocasion.", 22));

            List<Choice> choices12 = new List<Choice>();
            choices12.Add(new Choice("Thanks for your help.", "Carry the medicines back to the other\nhospital.", 04));

            List<Choice> choices13 = new List<Choice>();
            choices13.Add(new Choice("Come with me.", "You take the children with you.\nThey seem to relax a bit now that someone\nis with them.", 04));

            List<Choice> choices14 = new List<Choice>();
            choices14.Add(new Choice("Can you help these children?", "Yes we have people here for such cases.", 02));

            List<Choice> choices15 = new List<Choice>();
            choices15.Add(new Choice("Ok", "", 09));

            List<Choice> choices16 = new List<Choice>();
            choices16.Add(new Choice("Ok", "", 04));

            List<Choice> choices17 = new List<Choice>();
            choices17.Add(new Choice("Ok", "", 10));

            List<Choice> choices18 = new List<Choice>();
            choices18.Add(new Choice("Ok", "", 11));

            List<Choice> choices19 = new List<Choice>();
            choices19.Add(new Choice("Ok", "", 12));

            List<Choice> choices20 = new List<Choice>();
            choices20.Add(new Choice("Ok", "", 13));

            List<Choice> choices21 = new List<Choice>();
            choices21.Add(new Choice("Ok", "", 14));

            List<Choice> choices22 = new List<Choice>();
            choices22.Add(new Choice("Try to help the man", "You manage to free the man but you are\nexhausted.", 19));
            choices22.Add(new Choice("You decide to call for help", "As you leave you hear the building collapse and\nwhen you call the man there is no answer.", 07));

            List<Choice> choices23 = new List<Choice>();
            choices23.Add(new Choice("You help the man load his truck.", "After he leaves, you search the shop and you\nfind a pack of fireworks that might be useful.", 15));

            List<Choice> choices24 = new List<Choice>();
            choices24.Add(new Choice("...", "", 04));

            List<Choice> choices25 = new List<Choice>();
            choices25.Add(new Choice("Thank God!", "As soon as you reach a safe point you collapse\nfrom exhaustion. When you regain\nconsciousness you realize you are being\ntransferred away from the city.No words can\ndescribe the way you feel. At least you are still\nalive and for this you are thankful.", 16));

            List<Choice> choices26 = new List<Choice>();
            choices26.Add(new Choice("Ok", "", 02));

            List<Choice> choices27 = new List<Choice>();
            choices27.Add(new Choice("Ok", "", 18));

            List<Choice> choices28 = new List<Choice>();
            choices28.Add(new Choice("Ok", "", 17));

            List<Choice> choices29 = new List<Choice>();
            choices29.Add(new Choice("Ok", "", 20));

            List<Choice> choices30 = new List<Choice>();
            choices30.Add(new Choice("Ok", "", 30));

            List<Choice> choices31 = new List<Choice>();
            choices31.Add(new Choice("Ok", "", 31));

            List<SubQuest> subquests01 = new List<SubQuest>();
            subquests01.Add(new SubQuest(2, new Point(3, 1), "You reach the police station where a police\nofficer tells you that the eastern part of the city\nis under attack. Go to Public Service nearby\nto learn about the evacuation plan.", choices01));

            List<SubQuest> subquests02 = new List<SubQuest>();
            subquests02.Add(new SubQuest(3, new Point(7, 1), "We are forming an evacuation plan right now\nThe evacuation point is at the southern bus\nstation. At the moment we only accept women\nand children.Wait for further anouncements by\nradio at 20:00 o'clock each day.Take these\nsupplies for now and good luck.", choices02));

            List<SubQuest> subquests03 = new List<SubQuest>();
            subquests03.Add(new SubQuest(4, new Point(10, 10), "A middle aged man with a serious heart\nproblem is in dire need of medication. Go\nto the nearest hospital and bring some medicine.", choices03));
            subquests03.Add(new SubQuest(4, new Point(1, 4), "You have reached the hospital and while there\nis much chaos and disorder you manage\nto get the required medicine. Now you have\nto return it to the man.", choices04));
            subquests03.Add(new SubQuest(4, new Point(10, 10), "Thank you very much! Now I can get out of\nthis city before it's too late.", choices05));

            List<SubQuest> subquests04 = new List<SubQuest>();
            subquests04.Add(new SubQuest(5, new Point(7, 4), "A wife with 3 children awaits her husband\nto return from work so that they can leave\nthe city by car. He should have been home by\nnow, I feel that something bad has happened\nPlease go to his work and look for him.", choices06));
            subquests04.Add(new SubQuest(5, new Point(10, 1), "After some asking and searching around you\nfind the man unconscious. He was knocked\ndown on the panic that erupted when the\npeople tried to evacuate the building. You\nare able to find a first aid kit nearby and\nhelp the man return to his family.", choices07));

            List<SubQuest> subquests05 = new List<SubQuest>();
            subquests05.Add(new SubQuest(6, new Point(7, 1), "There is an evacuation scheduled for\ntomorrow morning. Go to the bus station\nbefore then and you should be able\nto flee the city.", choices08));
            subquests05.Add(new SubQuest(6, new Point(6, 10), "You finally reach the bus station.", choices09));
            subquests05.Add(new SubQuest(6, new Point(6, 10), "", choices30));

            List<SubQuest> subquests06 = new List<SubQuest>();
            subquests06.Add(new SubQuest(7, new Point(7, 1), "The evacuation point has been moved at the\nNorthern part of the city to avoid further\nsimilar incidents. Today's departures have\nbeen cancelled though for obvious reasons.", choices09));

            List<SubQuest> subquests07 = new List<SubQuest>();
            subquests07.Add(new SubQuest(8, new Point(1, 7), "The shop seems already pillaged. With\nany luck there will be something useful\nleft in it.", choices10));

            List<SubQuest> subquests08 = new List<SubQuest>();
            subquests08.Add(new SubQuest(9, new Point(5, 6), "You pack some clothes, food and water and\nstart on the long way to the new\nevacuation point.", choices16));
            subquests08.Add(new SubQuest(9, new Point(1, 1), "You find an abandoned house that seems\nsuitable for a night stop.", choices17));
            subquests08.Add(new SubQuest(9, new Point(1, 1), "", choices30));

            List<SubQuest> subquests09 = new List<SubQuest>();
            subquests09.Add(new SubQuest(10, new Point(1, 4), "We have need for additional medication.\nCould you go to the nearest hospital and\nsee if they can help us?", choices11));
            subquests09.Add(new SubQuest(10, new Point(8, 8), "You give the list of medicines that the\nhospital requires and after a while\na nurse brings you what you need.", choices12));
            subquests09.Add(new SubQuest(10, new Point(1, 4), "Thank you very much for your\nhelp.", choices26));

            List<SubQuest> subquests10 = new List<SubQuest>();
            subquests10.Add(new SubQuest(11, new Point(1, 10), "The parents of 3 children have been missing\nsince the attack.They are scared and crying.\nYou cannot leave them like this.You decide to\ntake them to the Public Service building where\nhopefully someone will take care of them.", choices13));
            subquests10.Add(new SubQuest(11, new Point(7, 1), "Fortunately there are people here that\nwill take care of the children.", choices14));

            List<SubQuest> subquests11 = new List<SubQuest>();
            subquests11.Add(new SubQuest(12, new Point(1, 0), "The situation gets worse every day. We\nneed all the help we can get. Take these\nsupplies (one smoke bomb and some food).\nWe need you to go to the gas station and\nbring fuel for our vehicles.", choices16));
            subquests11.Add(new SubQuest(12, new Point(6, 7), "The army vehicle reaches the gas station\nand you along with a dozen civilians start\nfilling the containers. When they are full\nyou load them into the vehicle. Your task\nis done and you can proceed with any other\nbusiness.", choices29));

            List<SubQuest> subquests12 = new List<SubQuest>();
            subquests12.Add(new SubQuest(13, new Point(3, 10), "Help empty rooms at the hospital by\ntransferring some of the less wounded to\nthe building in the next block.", choices16));
            subquests12.Add(new SubQuest(13, new Point(3, 7), "The less wounded reach the newly prepared\narea. This should give the hospital a\nchance to look after more serious cases.", choices27));


            List<SubQuest> subquests13 = new List<SubQuest>();
            subquests13.Add(new SubQuest(14, new Point(7, 0), "Piles of dead bodies await proper burial.\nYou see many civilians with shovels and a\nfew army officers managing the burials.", choices26));

            List<SubQuest> subquests14 = new List<SubQuest>();
            subquests14.Add(new SubQuest(15, new Point(3, 7), "We don't need any more help here civilian.\nHowever we need people helping our officers\nat the HQs. Go east to the pick up point\nand wait our vehicles there. You don't\nhave much time.", choices16));
            subquests14.Add(new SubQuest(15, new Point(11, 3), "You reach the designated point\nof arrival. You are being transferred to\nthe new army headquarters near the front.", choices18));
            subquests14.Add(new SubQuest(15, new Point(11, 3), "", choices30));

            List<SubQuest> subquests15 = new List<SubQuest>();
            subquests15.Add(new SubQuest(16, new Point(1, 3), "All civilians available are being formed into\ngroups.The groups should search the\nsurrounding area for survivors.If you find\nany either transfer them to the medical\nunit if you can or notify the medics\nimmediately.", choices16));
            subquests15.Add(new SubQuest(16, new Point(9, 0), "Your group splits and searches the block.After\nsome time you are able to spot some survivors\ntrapped in the ruins.The only thing you can do\nis keep them calm while you ask for assistance.", choices27));

            List<SubQuest> subquests16 = new List<SubQuest>();
            subquests16.Add(new SubQuest(17, new Point(1, 3), "Our soldiers need ammunition and at the\nmoment the army cannot spare any soldiers\nfor the task.Take these bags to the\ntemporary stronghold", choices16));
            subquests16.Add(new SubQuest(17, new Point(3, 7), "You arrive safely at the base and deliver\nthe much needed ammunition.Well done", choices19));

            List<SubQuest> subquests17 = new List<SubQuest>();
            subquests17.Add(new SubQuest(18, new Point(1, 3), "Your contribution so far to the army s needs is\nvaluable civilian. I am going to assign you a\nmore dangerous task but I think you are up to\nit.You must carry the vital documents to the\nofficers at the stronghold.", choices20));
            subquests17.Add(new SubQuest(18, new Point(11, 9), "After reaching the stronghold you deliver the\ndocuments safely", choices28));
            subquests17.Add(new SubQuest(18, new Point(11, 9), "", choices30));

            List<SubQuest> subquests18 = new List<SubQuest>();
            subquests18.Add(new SubQuest(19, new Point(9, 8), "There is someone trapped inside this\nbuilding and is calling for help.He seems ok\nbut the situation looks dangerous.You can either\ntry to help him or call for help.", choices22));

            List<SubQuest> subquests19 = new List<SubQuest>();
            subquests19.Add(new SubQuest(20, new Point(8, 10), "A shop owner needs your help.He wants to take\nwhatever is left of value from his shop and\nflee the city.He offers to let you search for\nanything of use after you help him.", choices23));

            List<SubQuest> subquests20 = new List<SubQuest>();
            subquests20.Add(new SubQuest(21, new Point(1, 10), "Civilian we need your help.Our scouts are being\npinned down by an enemy flanking movement.\nYou must go to the building across their\nlocation and operate the machine gun there\nso as to provide them cover to escape.Be very\ncareful.We don't want you to kill enemies just\n provide cover to our scouts.Good luck.", choices24));
            subquests20.Add(new SubQuest(21, new Point(8, 9), "You manage to reach the machine gun with\ncaution.Soon after you start firing you can\nsee the scouts retreating.It is time for\nyou to retreat as well.", choices21));
            subquests20.Add(new SubQuest(21, new Point(8, 9), "", choices30));

            List<SubQuest> subquests21 = new List<SubQuest>();
            subquests21.Add(new SubQuest(22, new Point(2, 10), "Find the exit but be careful there are enemies\neverywhere.", choices24));
            subquests21.Add(new SubQuest(22, new Point(0, 3), "You made it! You escaped the warzone in\none piece.", choices25));
            subquests21.Add(new SubQuest(22, new Point(0, 4), "", choices31));            

            int[] nextQuests = new int[] { 1 };
            List<Quest> quests = new List<Quest>();
            nextQuests = new int[] { 2 };
            quests.Add(new Quest(true, 1, 0, nextQuests, subquests01, "War??", "Reach the police station."));
            nextQuests = new int[] { 3 };
            quests.Add(new Quest(false, 2, 0, nextQuests, subquests02, "Evacuation\nplan", "Learn details."));
            nextQuests = new int[] { 4 };
            quests.Add(new Quest(false, 3, 0, nextQuests, subquests03, "Fighting\nchance", "A man is in dire need of medication."));
            nextQuests = new int[] { 5 };
            quests.Add(new Quest(false, 4, 0, nextQuests, subquests04, "Head of\nthe family", "Find the father."));
            nextQuests = new int[] { 0 };
            quests.Add(new Quest(false, 5, 0, nextQuests, subquests05, "Leave the\nCity", "Go to the evacuation point."));


            nextQuests = new int[] { 12 };
            quests.Add(new Quest(true, 11, 1, nextQuests, subquests06, "It didn't hit\nthe mark.", "Find the new evacuation point"));
            nextQuests = new int[] { 13 };
            quests.Add(new Quest(false, 12, 1, nextQuests, subquests07, "Surviving\nlong enough", "      Look for supplies."));
            nextQuests = new int[] { 14 };
            quests.Add(new Quest(false, 13, 1, nextQuests, subquests10, "Protect\nthe young.", "Accompany the children\nto Public Service building."));
            nextQuests = new int[] { 15 };
            quests.Add(new Quest(false, 14, 1, nextQuests, subquests09, "Hospital\nsupplies.", "Bring supplies from\nthe nearest hospital."));
            nextQuests = new int[] { 0 };
            quests.Add(new Quest(false, 15, 1, nextQuests, subquests08, "Easier said\nthan done.", "Find a shelter along the\nway."));

            nextQuests = new int[] { 22 };
            quests.Add(new Quest(true, 21, 2, nextQuests, subquests11, "Bad news.", "Evacuation plan aborted. All capable\ncitizens must report to the army base."));
            nextQuests = new int[] { 23 };
            quests.Add(new Quest(false, 22, 2, nextQuests, subquests12, "Empty the\nhospital.", "Help the hospital staff empty some\nrooms for those in greater need."));
            nextQuests = new int[] { 24 };
            quests.Add(new Quest(false, 23, 2, nextQuests, subquests13, "A grave\ntask.", "All civilians must help\nwith the burials of the deceased."));
            nextQuests = new int[] { 0 };
            quests.Add(new Quest(false, 24, 2, nextQuests, subquests14, "Help our\nsoldiers.", "You must go to the new\narmy headquarters."));

            nextQuests = new int[] { 32 };
            quests.Add(new Quest(true, 31, 3, nextQuests, subquests15, "Look for\nsurvivors.", "Search the destroyed buildings for\nsurvivors."));
            nextQuests = new int[] { 33 };
            quests.Add(new Quest(false, 32, 3, nextQuests, subquests16, "Supply our\nsoldiers.", "Carry ammunition to the front line\nsoldiers."));
            nextQuests = new int[] { 34 };
            quests.Add(new Quest(false, 33, 3, nextQuests, subquests19, "Quick load.", "A shop owner needs help emptying\nhis shop before he flees the city."));
            nextQuests = new int[] { 35 };
            quests.Add(new Quest(false, 34, 3, nextQuests, subquests18, "Trapped.", "Help the trapped civilian escape the\ndestroyed building."));
            nextQuests = new int[] { 0 };
            quests.Add(new Quest(false, 35, 3, nextQuests, subquests17, "You're in the\narmy now.", "   Carry the vital documents to the\n   frontline stronghold."));


            quests.Add(new Quest(true, 41, 4, nextQuests, subquests20, "Every man\ncounts.", "Provide cover for our soldiers\nwhile they try to escape."));

            quests.Add(new Quest(true, 51, 5, nextQuests, subquests21, "A way\nout.", "Escape the battleground."));



            gameQuests = quests.ToArray();
        }

        //Reset Quest Data
        public static void ResetAllQuests()
        {
            Array.Clear(gameQuests, 0, gameQuests.Length);
            SetAllQuests();
        }

        //Change Player Stats from outcome of quest choice
        public static void RunChoiceOutcomes(int outcomeID)
        {           
            switch (outcomeID)
            {
                case 00:
                    Player.Score += 20;

                    break;
                case 01:
                    Player.Score += 30;
                    Player.Hunger += 20;

                    break;
                case 02:
                    Player.Score += 70;
                    break;
                case 03:                    
                    gameQuests[2].Active = false;                    
                    gameQuests[3].Active = true;                   

                    break;
                case 04:

                    break;
                case 05:
                    Player.Score += 30;

                    break;
                case 06:
                    Player.Score += 100;
                    Player.Hunger += 30;

                    break;
                case 07:
                    Player.Score += 50;

                    break;
                case 08:
                    Player.Score += 50;
                    Player.Hunger += 100;

                    break;
                case 09:
                    Player.Score += 70;
                    break;
                case 10:
                    Player.Score += 100;
                    break;
                case 11:
                    Player.Score += 100;
                    Player.Hunger += 100;
                    break;
                case 12:
                    Player.Score += 100;
                    Player.smokebombAmount++;

                    break;
                case 13:
                    Player.smokebombAmount++;

                    break;
                case 14:
                    Player.Score += 150;
                    break;
                case 15:
                    Player.Hunger += 50;
                    Player.fireworkAmount++;

                    break;
                case 16:
                    Player.Score += 1000;
                    
                    break;
                case 17:
                    Player.Score += 150;
                    break;
                case 18:
                    Player.Score += 100;
                    break;
                case 19:
                    Player.Score += 150;
                    Player.Hunger -= 50;
                    break;
                case 20:
                    Player.Score += 30;
                    Player.Hunger += 20;
                    Player.smokebombAmount++;
                    break;
                case 21:
                    gameQuests[3].Active = false;
                    gameQuests[4].Active = true;
                    break;
                case 22:
                    gameQuests[8].Active = false;
                    gameQuests[9].Active = true;
                    break;
                case 30:

                    break;
                case 31:
                    InPlay.inPlayState = InPlayState.Win;
                    break;
            }
        }

        //Activate new quests, after completing a quest
        public static void ActivateNewQuests(int[] nextQuests)
        {
            foreach (int questID in nextQuests)
            {
                foreach (Quest quest in gameQuests)
                {
                    if (quest.QuestID == questID)
                    {
                        quest.Active = true;
                    }
                }
            }
        }

        //Updates animation position
        private static Vector2 GetScreenPosition(Point mapPosition)
        {
            Vector2 screenPosition;
            Vector2 mapVector;
            mapVector.X = (int)mapPosition.X * 100;
            mapVector.Y = (int)mapPosition.Y * 100;
            screenPosition = new Vector2(Player.screenVector.X - Player.mapVector.X + mapVector.X,
                                                        Player.screenVector.Y - Player.mapVector.Y + mapVector.Y);
            return screenPosition;
        }
        #endregion
    }
}