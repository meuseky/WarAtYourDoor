using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Home_Front
{
    //Hold info for individual Enemy
    //EnemyHandler class stores Enemy array
    class Enemy
    {
        #region Variables

        //Current Map Tile
        private Point gridPosition;

        //Vector Relative to Map origin
        private Vector2 mapVector;

        //Vector of screen position
        private Vector2 screenVector;

        //Represents facing direction: Up=0,Right=1,Down=2,Left=3
        private int faceDirection;

        //List of Points to Patrol to (must be linear)
        private Point[] patrolPoints;

        //Which direction patroling in.
        private bool ascendingPatrol;

        //Current point on patrol
        public int currentPatrolPoint;

        //The number of points that needs to be traversed
        //Used for Loop type Enemy
        private int patrolLength;

        //Direction Travelling In
        private Vector2 headingVector;

        //Used by Guard type Enemy
        //Holds original position
        private Vector2 guardPosition;

        //Used to conceal player (or not)
        public bool smokeInSight;

        //Used to move guard (or not)
        public bool fireNearby;

        //Position of firework headed towards
        public Point firePos;

        //Animation
        private int fps;
        private int totalFrame;
        private int currentFrame;
        private float updateTrigger;
        private float guardingTimer;

        //Patrol type
        private PatrolType patrolType;
        public PatrolState patrolState;

        #endregion

        #region Properties
        public int CurrentFrame
        {
            get { return currentFrame; }
        }
        public int FaceDirection
        {
            get { return faceDirection; }
        }
        public Vector2 ScreenVector
        {
            get { return screenVector; }
        }
        public Point GridPosition
        {
            get { return gridPosition; }
        }
        public Vector2 MapVector
        {
            get { return mapVector; }
        }
        public int PatrolPoint
        {
            get { return currentPatrolPoint; }
        }
        #endregion

        #region Methods
        //Constructor
        public Enemy(Point[] movePoints, PatrolType enemyPatrol)
        {
            fps = 6;
            totalFrame = 6;
            currentFrame = 0;
            updateTrigger = 0;
            faceDirection = 2;

            patrolPoints = movePoints;
            ascendingPatrol = true;
            currentPatrolPoint = 0;
            patrolLength = patrolPoints.Count() - 1;
            headingVector = Vector2.Zero;
            smokeInSight = false;
            fireNearby = false;

            gridPosition = patrolPoints[currentPatrolPoint];
            mapVector = new Vector2(gridPosition.X * 100, gridPosition.Y * 100);
            guardPosition = mapVector;

            patrolType = enemyPatrol;
            patrolState = PatrolState.Stopped;
            guardingTimer = 0;
        }

        //Main enemy update method
        public void Update(GameTime gameTime)
        {
            LookForPlayer();
            UpdateMovement(gameTime);
            UpdateAnimation(gameTime);
            UpdateScreenPosition();
            LookForFirework();
        }

        //Updates draw position on-screen
        private void UpdateScreenPosition()
        {
            screenVector = new Vector2(Player.screenVector.X - Player.mapVector.X + mapVector.X,
                                                        Player.screenVector.Y - Player.mapVector.Y + mapVector.Y);
        }

        //Updates Enemy position & direction, depending on Patrol type
        public void UpdateMovement(GameTime gameTime)
        {
            switch (patrolType)
            {
                case PatrolType.Guard:
                    UpdateGuard(gameTime);
                    break;

                case PatrolType.PatrolLine:
                    UpdatePatrolLine(gameTime);
                    break;

                case PatrolType.PatrolLoop:
                    UpdatePatrolLoop(gameTime);
                    break;
            }
        }

        //Updates animation frame
        public void UpdateAnimation(GameTime gameTime)
        {
            updateTrigger += (float)gameTime.ElapsedGameTime.TotalSeconds * fps;

            if (updateTrigger >= 1)
            {
                updateTrigger = 0;

                currentFrame = (currentFrame + 1) % totalFrame;
            }
        }

        //Updates movement for PatrolLine type Enemy
        public void UpdatePatrolLine(GameTime gameTime)
        {
            //Stopped, Moving, or GoToFire
            switch (patrolState)
            {
                //If stopped find next Patrol Point
                case PatrolState.Stopped:

                    int nextPosition;

                    mapVector.X = patrolPoints[currentPatrolPoint].X * 100;
                    mapVector.Y = patrolPoints[currentPatrolPoint].Y * 100;

                    //If at end or start of patrol Line, switch direction
                    if (currentPatrolPoint == patrolLength)
                    {
                        ascendingPatrol = false;
                    }
                    if (currentPatrolPoint == 0)
                    {
                        ascendingPatrol = true;
                    }

                    //Move to next line Point
                    if (ascendingPatrol)
                    {
                        nextPosition = currentPatrolPoint + 1;
                    }
                    else
                    {
                        nextPosition = currentPatrolPoint - 1;
                    }

                    //Update current point;
                    headingVector.X = (patrolPoints[nextPosition].X - patrolPoints[currentPatrolPoint].X) * 100;
                    headingVector.Y = (patrolPoints[nextPosition].Y - patrolPoints[currentPatrolPoint].Y) * 100;

                    //Updates current point & starts Moving
                    currentPatrolPoint = nextPosition;
                    patrolState = PatrolState.Moving;
                    break;

                //If moving update position
                case PatrolState.Moving:

                    if (Math.Abs(headingVector.X) > 5)
                    {
                        if (headingVector.X > 0)
                        {
                            mapVector.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 1;
                        }
                        if (headingVector.X < 0)
                        {
                            mapVector.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 3;
                        }
                    }

                    if (Math.Abs(headingVector.Y) > 5)
                    {
                        if (headingVector.Y > 0)
                        {
                            mapVector.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 2;
                        }
                        if (headingVector.Y < 0)
                        {
                            mapVector.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 0;
                        }
                    }

                    //If close to destination point, stop at point
                    if (Math.Abs(headingVector.X) < 5 && Math.Abs(headingVector.Y) < 5)
                    {
                        patrolState = PatrolState.Stopped;
                    }

                    break;

                //If firework sighted, move towards
                case PatrolState.GoToFire:

                    //Updates heading vector
                    headingVector.X = ((int)firePos.X * 100) - mapVector.X;
                    headingVector.Y = ((int)firePos.Y * 100) - mapVector.Y;

                    //If close to firework, stop moving. Otherwise continue moving
                    if (headingVector.Length() < 50)
                    {

                    }
                    else
                    {
                        if (headingVector.X > 0)
                        {
                            mapVector.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 1;
                        }
                        if (headingVector.X < 0)
                        {
                            mapVector.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 3;
                        }
                        if (headingVector.Y > 0)
                        {
                            mapVector.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 2;
                        }
                        if (headingVector.Y < 0)
                        {
                            mapVector.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 0;
                        }
                    }
                    break;
            }
        }

        //Updates movement for PatrolLoop type Enemy
        public void UpdatePatrolLoop(GameTime gameTime)
        {
            //Stopped, Moving, or GoToFire
            switch (patrolState)
            {
                //If stopped find next Patrol Point
                case PatrolState.Stopped:

                    //If stopped find next Patrol Point
                    int nextPosition;

                    mapVector.X = patrolPoints[currentPatrolPoint].X * 100;
                    mapVector.Y = patrolPoints[currentPatrolPoint].Y * 100;

                    //If at end of point array, loop to frst point in patrol
                    nextPosition = (currentPatrolPoint + 1) % (patrolLength + 1);

                    //Update current point;
                    headingVector.X = (patrolPoints[nextPosition].X - patrolPoints[currentPatrolPoint].X) * 100;
                    headingVector.Y = (patrolPoints[nextPosition].Y - patrolPoints[currentPatrolPoint].Y) * 100;

                    currentPatrolPoint = nextPosition;
                    patrolState = PatrolState.Moving;
                    break;

                //If moving update position
                case PatrolState.Moving:

                    if (Math.Abs(headingVector.X) > 5)
                    {
                        if (headingVector.X > 0)
                        {
                            mapVector.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 1;
                        }
                        if (headingVector.X < 0)
                        {
                            mapVector.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 3;
                        }
                    }

                    if (Math.Abs(headingVector.Y) > 5)
                    {
                        if (headingVector.Y > 0)
                        {
                            mapVector.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 2;
                        }
                        if (headingVector.Y < 0)
                        {
                            mapVector.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 0;
                        }
                    }

                    if (Math.Abs(headingVector.X) < 5 && Math.Abs(headingVector.Y) < 5)
                    {
                        patrolState = PatrolState.Stopped;
                    }

                    break;

                //If firework sighted, move towards
                case PatrolState.GoToFire:

                    //Updates heading vector
                    headingVector.X = ((int)firePos.X * 100) - mapVector.X;
                    headingVector.Y = ((int)firePos.Y * 100) - mapVector.Y;

                    //If close to firework, stop moving. Otherwise continue moving
                    if (headingVector.Length() < 50)
                    {

                    }
                    else
                    {
                        if (headingVector.X > 0)
                        {
                            mapVector.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 1;
                        }
                        if (headingVector.X < 0)
                        {
                            mapVector.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 3;
                        }
                        if (headingVector.Y > 0)
                        {
                            mapVector.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 2;
                        }
                        if (headingVector.Y < 0)
                        {
                            mapVector.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 0;
                        }
                    }
                    break;
            }
        }

        //Updates movement for Guard type Enemy
        public void UpdateGuard(GameTime gameTime)
        {
            //Stopped, Moving, or GoToFire
            switch (patrolState)
            {
                //If stopped slowly spin in circle
                case PatrolState.Stopped:

                if (guardingTimer >= 1)
                {
                    faceDirection = (faceDirection + 1) % 4;
                    guardingTimer = 0;
                }
                else
                {
                    guardingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                    break;

                //Used to return to startiong position after following firework
                case PatrolState.Moving:
                   
                    if (Math.Abs(headingVector.X) > 5)
                    {
                        if (headingVector.X > 0)
                        {
                            mapVector.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 1;
                        }
                        if (headingVector.X < 0)
                        {
                            mapVector.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 3;
                        }
                    }

                    if (Math.Abs(headingVector.Y) > 5)
                    {
                        if (headingVector.Y > 0)
                        {
                            mapVector.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 2;
                        }
                        if (headingVector.Y < 0)
                        {
                            mapVector.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            headingVector.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 0;
                        }
                    }

                    if (Math.Abs(headingVector.X) < 5 && Math.Abs(headingVector.Y) < 5)
                    {
                        patrolState = PatrolState.Stopped;
                        mapVector = guardPosition;
                    }

                    break;

                //Move towards firework
                case PatrolState.GoToFire:

                    if (fireNearby == false)
                    {
                        headingVector = guardPosition - mapVector;
                        patrolState = PatrolState.Moving;
                        break;
                    }

                    headingVector.X = ((int)firePos.X * 100) - mapVector.X;
                    headingVector.Y = ((int)firePos.Y * 100) - mapVector.Y;

                    if (headingVector.Length() < 50)
                    {

                    }
                    else
                    {
                        if (headingVector.X > 0)
                        {
                            mapVector.X += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 1;
                        }
                        if (headingVector.X < 0)
                        {
                            mapVector.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 3;
                        }
                        if (headingVector.Y > 0)
                        {
                            mapVector.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 2;
                        }
                        if (headingVector.Y < 0)
                        {
                            mapVector.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
                            faceDirection = 0;
                        }
                    }
                    break;
            }
        }

        //If enemy facing firework & in range switch patrol state to move towards firework
        public void LookForFirework()
        {           
            switch (faceDirection)
            {
                case 0:

                    foreach (FireWork fw in Items.activeFire)
                    {
                        if ((fw.spritePosition.Y - screenVector.Y < 0
                            && fw.spritePosition.Y - screenVector.Y > -500
                            && fw.spritePosition.X == screenVector.X) && (Items.fireIsActive == true))
                        {
                            patrolState = PatrolState.GoToFire;
                            fireNearby = true;
                            firePos = fw.location;
                            break;
                        }
                        else
                        {
                            fireNearby = false;
                            if (patrolState == PatrolState.GoToFire)
                            {
                                ResetHeading();
                            }
                        }
                    }
                    if (Items.activeFire.Count() == 0)
                    {
                        fireNearby = false;
                        if (patrolState == PatrolState.GoToFire)
                        {
                            ResetHeading();
                        }
                    }
                    break;

                case 1:

                    foreach (FireWork fw in Items.activeFire)
                    {
                        if ((fw.spritePosition.X - screenVector.X > 0
                            && fw.spritePosition.X - screenVector.X < 500
                            && fw.spritePosition.Y == screenVector.Y) && (Items.fireIsActive == true))
                        {
                            patrolState = PatrolState.GoToFire;
                            fireNearby = true;
                            firePos = fw.location;
                            break;
                        }
                        else
                        {
                            fireNearby = false;
                            if (patrolState == PatrolState.GoToFire)
                            {
                                ResetHeading();
                            }
                        }
                    }
                    if (Items.activeFire.Count() == 0)
                    {
                        fireNearby = false;
                        if (patrolState == PatrolState.GoToFire)
                        {
                            ResetHeading();
                        }
                    }
                    break;

                case 2:

                    foreach (FireWork fw in Items.activeFire)
                    {
                        if ((fw.spritePosition.Y - screenVector.Y > 0
                            && fw.spritePosition.Y - screenVector.Y < 500
                            && fw.spritePosition.X == screenVector.X) && (Items.fireIsActive == true))
                        {
                            patrolState = PatrolState.GoToFire;
                            fireNearby = true;
                            firePos = fw.location;
                            break;
                        }
                        else
                        {
                            fireNearby = false;
                            if (patrolState == PatrolState.GoToFire)
                            {
                                ResetHeading();
                            }
                        }
                    }
                    if (Items.activeFire.Count() == 0)
                    {
                        fireNearby = false;
                        if (patrolState == PatrolState.GoToFire)
                        {
                            ResetHeading();
                        }
                    }
                    break;

                case 3:
                    foreach (FireWork fw in Items.activeFire)
                    {
                        if ((fw.spritePosition.X - screenVector.X < 0
                            && fw.spritePosition.X - screenVector.X > -500
                            && fw.spritePosition.Y == screenVector.Y) && (Items.fireIsActive == true))
                        {
                            patrolState = PatrolState.GoToFire;
                            fireNearby = true;
                            firePos = fw.location;
                            break;
                        }
                        else
                        {
                            fireNearby = false;
                            if (patrolState == PatrolState.GoToFire)
                            {
                                ResetHeading();
                            }
                        }
                    }
                    if (Items.activeFire.Count() == 0)
                    {
                        fireNearby = false;
                        if (patrolState == PatrolState.GoToFire)
                        {
                            ResetHeading();
                        }
                    }
                    break;
            }
        }        

        //Changes heading direction, used in LookForFirework()
        public void ResetHeading()
        {
            patrolState = PatrolState.Moving;
            headingVector.X = (patrolPoints[currentPatrolPoint].X * 100) - mapVector.X;
            headingVector.Y = (patrolPoints[currentPatrolPoint].Y * 100) - mapVector.Y;
        }

        //If player is in facing direction & in range, kill player
        //Unless smokebomb is in use
        public void LookForPlayer()
        {
            switch (faceDirection)  //Up=0,Right=1,Down=2,Left=3
            {
                case 0:
                    foreach (SmokeBomb sb in Items.activeSmoke)
                    {
                        if ((sb.spritePosition.Y - screenVector.Y < 0
                            && sb.spritePosition.Y - screenVector.Y > -150
                            && sb.spritePosition.X == screenVector.X) && (Items.smokeIsActive == true))
                        {
                            smokeInSight = true;
                        }
                    }
                    if ((Player.screenVector.Y - screenVector.Y < 0
                        && Player.screenVector.Y - screenVector.Y > -150
                        && Player.screenVector.X == screenVector.X) && (smokeInSight == false))
                    {
                        AudioHandler.PlayEffectInstance(7);
                        AudioHandler.PlayEffectInstance(0);
                        Player.Alive = false;
                    }
                    break;
                case 1:
                    foreach (SmokeBomb sb in Items.activeSmoke)
                    {
                        if ((sb.spritePosition.X - screenVector.X > 0
                            && sb.spritePosition.X - screenVector.X < 150
                            && sb.spritePosition.Y == screenVector.Y) && (Items.smokeIsActive == true))
                        {
                            smokeInSight = true;
                        }
                    }
                    if ((Player.screenVector.X - screenVector.X > 0
                        && Player.screenVector.X - screenVector.X < 150
                        && Player.screenVector.Y == screenVector.Y) && (smokeInSight == false))
                    {
                        AudioHandler.PlayEffectInstance(7);
                        AudioHandler.PlayEffectInstance(0);
                        Player.Alive = false;
                    }
                    break;
                case 2:
                    foreach (SmokeBomb sb in Items.activeSmoke)
                    {
                        if ((sb.spritePosition.Y - screenVector.Y > 0
                            && sb.spritePosition.Y - screenVector.Y < 150
                            && sb.spritePosition.X == screenVector.X) && (Items.smokeIsActive == true))
                        {
                            smokeInSight = true;
                        }
                    }
                    if ((Player.screenVector.Y - screenVector.Y > 0
                        && Player.screenVector.Y - screenVector.Y < 150
                        && Player.screenVector.X == screenVector.X) && (smokeInSight == false))
                    {
                        AudioHandler.PlayEffectInstance(7);
                        AudioHandler.PlayEffectInstance(0);
                        Player.Alive = false;
                    }
                    break;
                case 3:
                    foreach (SmokeBomb sb in Items.activeSmoke)
                    {
                        if ((sb.spritePosition.X - screenVector.X < 0
                            && sb.spritePosition.X - screenVector.X > -150
                            && sb.spritePosition.Y == screenVector.Y) && (Items.smokeIsActive == true))
                        {
                            smokeInSight = true;
                        }
                    }
                    if ((Player.screenVector.X - screenVector.X < 0
                        && Player.screenVector.X - screenVector.X > -150
                        && Player.screenVector.Y == screenVector.Y) && (smokeInSight == false))
                    {
                        AudioHandler.PlayEffectInstance(7);
                        AudioHandler.PlayEffectInstance(0);
                        Player.Alive = false;
                    }
                    break;
                default:
                    break;
            }

        }
        #endregion
    }

    //Holds enemy sprite assets
    class EnemySprite
    {
        static public Texture2D spriteTexture;
        static public int tileWidth = 100;
        static public int tileHeight = 100;

        //Draws sprite on-screen
        public static void Draw(SpriteBatch spriteBatch, Vector2 screenPos, int tileFrame, int tileDirection)
        {
            spriteBatch.Draw(spriteTexture, screenPos, new Rectangle(tileFrame * 100, tileDirection * 100, tileWidth, tileHeight), Color.White);
        }
    }

    static class EnemyHandler
    {
        #region Variables
        public static Enemy[] gameEnemy;
        //Holds enemy # by level
        public static int[,] firstlastByLevel;
        #endregion

        #region Methods
        static EnemyHandler()
        {

        }

        public static void InitializeEnemies()
        {
            firstlastByLevel = new int[6, 2];
            firstlastByLevel[0, 0] = 0;
            firstlastByLevel[0, 1] = 2;
            firstlastByLevel[1, 0] = 3;
            firstlastByLevel[1, 1] = 5;
            firstlastByLevel[2, 0] = 6;
            firstlastByLevel[2, 1] = 8;
            firstlastByLevel[3, 0] = 9;
            firstlastByLevel[3, 1] = 10;
            firstlastByLevel[4, 0] = 11;
            firstlastByLevel[4, 1] = 15;
            firstlastByLevel[5, 0] = 16;
            firstlastByLevel[5, 1] = 20;

            gameEnemy = new Enemy[21];

            Point[] temp = new Point[3];
            temp[0] = new Point(0, 1);
            temp[1] = new Point(1, 1);
            temp[2] = new Point(1, 4);
            gameEnemy[0] = new Enemy(temp, PatrolType.PatrolLine);
            gameEnemy[3] = new Enemy(temp, PatrolType.PatrolLine);

            temp = new Point[2];
            temp[0] = new Point(1, 4);
            temp[1] = new Point(5, 4);
            gameEnemy[1] = new Enemy(temp, PatrolType.PatrolLine);
            gameEnemy[4] = new Enemy(temp, PatrolType.PatrolLine);

            temp = new Point[4];
            temp[0] = new Point(10, 6);
            temp[1] = new Point(8, 6);
            temp[2] = new Point(8, 10);
            temp[3] = new Point(10, 10);
            gameEnemy[2] = new Enemy(temp, PatrolType.PatrolLoop);
            gameEnemy[5] = new Enemy(temp, PatrolType.PatrolLine);

            temp = new Point[2];
            temp[0] = new Point(0, 7);
            temp[1] = new Point(8, 7);
            gameEnemy[6] = new Enemy(temp, PatrolType.PatrolLine);

            temp = new Point[2];
            temp[0] = new Point(10, 0);
            temp[1] = new Point(10, 10);
            gameEnemy[7] = new Enemy(temp, PatrolType.PatrolLine);

            temp = new Point[4];
            temp[0] = new Point(4, 2);
            temp[1] = new Point(4, 5);
            temp[2] = new Point(7, 5);
            temp[3] = new Point(7, 2);
            gameEnemy[8] = new Enemy(temp, PatrolType.PatrolLoop);

            temp = new Point[3];
            temp[0] = new Point(4, 0);
            temp[1] = new Point(4, 10);
            temp[2] = new Point(9, 10);
            gameEnemy[9] = new Enemy(temp, PatrolType.PatrolLine);

            temp = new Point[6];
            temp[0] = new Point(7, 5);
            temp[1] = new Point(7, 6);
            temp[2] = new Point(6, 6);
            temp[3] = new Point(6, 8);
            temp[4] = new Point(9, 8);
            temp[5] = new Point(9, 5);
            gameEnemy[10] = new Enemy(temp, PatrolType.PatrolLoop);

            temp = new Point[1];
            temp[0] = new Point(1, 0);
            gameEnemy[11] = new Enemy(temp, PatrolType.Guard);

            temp = new Point[1];
            temp[0] = new Point(4, 0);
            gameEnemy[12] = new Enemy(temp, PatrolType.Guard);

            temp = new Point[1];
            temp[0] = new Point(6, 11);
            gameEnemy[13] = new Enemy(temp, PatrolType.Guard);

            temp = new Point[4];
            temp[0] = new Point(1, 8);
            temp[1] = new Point(3, 8);
            temp[2] = new Point(3, 5);
            temp[3] = new Point(1, 5);
            gameEnemy[14] = new Enemy(temp, PatrolType.PatrolLoop);

            temp = new Point[4];
            temp[0] = new Point(7, 3);
            temp[1] = new Point(9, 3);
            temp[2] = new Point(9, 5);
            temp[3] = new Point(7, 5);
            gameEnemy[15] = new Enemy(temp, PatrolType.PatrolLoop);

            temp = new Point[1];
            temp[0] = new Point(0, 6);
            gameEnemy[16] = new Enemy(temp, PatrolType.Guard);

            temp = new Point[1];
            temp[0] = new Point(9, 2);
            gameEnemy[17] = new Enemy(temp, PatrolType.Guard);

            temp = new Point[2];
            temp[0] = new Point(1, 8);
            temp[1] = new Point(6, 8);
            gameEnemy[18] = new Enemy(temp, PatrolType.PatrolLine);

            temp = new Point[2];
            temp[0] = new Point(11, 2);
            temp[1] = new Point(11, 6);
            gameEnemy[19] = new Enemy(temp, PatrolType.PatrolLine);

            temp = new Point[18];
            temp[0] = new Point(8, 8);
            temp[1] = new Point(9, 8);
            temp[2] = new Point(9, 9);
            temp[3] = new Point(10, 9);
            temp[4] = new Point(10, 11);
            temp[5] = new Point(6, 11);
            temp[6] = new Point(6, 10);
            temp[7] = new Point(3, 10);
            temp[8] = new Point(3, 8);
            temp[9] = new Point(6, 8);
            temp[10] = new Point(6, 6);
            temp[11] = new Point(5, 6);
            temp[12] = new Point(5, 3);
            temp[13] = new Point(8, 3);
            temp[14] = new Point(8, 4);
            temp[15] = new Point(9, 4);
            temp[16] = new Point(9, 6);
            temp[17] = new Point(8, 6);
            gameEnemy[20] = new Enemy(temp, PatrolType.PatrolLoop);
        }

        public static void Update(GameTime gameTime)
        {
            for (int i = firstlastByLevel[InPlay.currentLevel, 0]; i < firstlastByLevel[InPlay.currentLevel, 1] + 1; i++)
            {
                gameEnemy[i].Update(gameTime);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int i = firstlastByLevel[InPlay.currentLevel, 0]; i < firstlastByLevel[InPlay.currentLevel, 1] + 1; i++)
            {
                //**Get screen position relative to player & pass in
                EnemySprite.Draw(spriteBatch, gameEnemy[i].ScreenVector, gameEnemy[i].CurrentFrame, gameEnemy[i].FaceDirection);
            }
        }

        public static void Reset()
        {
            foreach (Enemy enemies in gameEnemy)
            {
                enemies.currentPatrolPoint = 0;
                enemies.patrolState = PatrolState.Stopped;
            }
        }
        #endregion
    }
}
