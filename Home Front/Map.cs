using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Home_Front
{
    //Holds Map Grid information
    class MapCell
    {
        #region Variables

        //Map tile data
        private int[,] m_mapCells;

        //Size of Map
        private int m_mapWidth;
        private int m_mapHeight;

        #endregion

        #region Properties
        public int[,] MapTile
        {
            get { return m_mapCells; }
        }

        public int Width
        {
            get { return m_mapWidth; }
        }
        public int Height
        {
            get { return m_mapHeight; }
        }
        #endregion

        #region Methods
        //Constructor
        public MapCell(int[,] mapCells)
        {
            m_mapCells = mapCells;
            m_mapWidth = m_mapCells.GetLength(1);
            m_mapHeight = m_mapCells.GetLength(0);

        }
        #endregion
    }

    //Holds Level Map & Overlay arrays
    static class Map
    {
        #region Variables

        //Holds map tiles for all Levels
        public static MapCell[] m_Maps;

        //Holds map tile overlays
        public static MapCell[] m_staticOverlay;

        //Holds player Starting Positions for each level
        public static Point[] startingPoints;

        #endregion

        #region Methods
        //Constructor
        static Map()
        {           
        }

        //Draws Map & Overlay onscreen
        public static void Draw(SpriteBatch spriteBatch)
        {
                            
            for (int i = 0; i < m_Maps[InPlay.currentLevel].Width; i++)
            {
                for (int j = 0; j < m_Maps[InPlay.currentLevel].Height; j++)
                {
                    //Adjust position relative to player
                    Vector2 tileVector = new Vector2(Player.screenVector.X - Player.mapVector.X + (j * Tile.tileWidth),
                                                        Player.screenVector.Y - Player.mapVector.Y + (i * Tile.tileHeight));
                    Tile.DrawMap(spriteBatch, tileVector, m_Maps[InPlay.currentLevel].MapTile[j, i]);
                    Tile.DrawOverlay(spriteBatch, tileVector, m_staticOverlay[InPlay.currentLevel].MapTile[j, i]);
                }
            }
        }

        //Initialize Values for level Maps
        public static void SetMap()
        {
            m_Maps = new MapCell[6];
            //Place Map Tile Data hereabouts mi hermano
            int[,] map01 = new int[,] {      
                                         {15, 00, 17, 17, 21, 17, 00, 15, 15, 15, 00, 17},
                                         {13, 08, 01, 01, 07, 01, 10, 01, 22, 15, 00, 13},
                                         {15, 00, 15, 15, 00, 15, 00, 17, 17, 15, 00, 17},
                                         {17, 00, 20, 17, 00, 17, 03, 01, 01, 01, 06, 15},
                                         {17, 00, 17, 17, 00, 17, 17, 15, 15, 15, 00, 15},
                                         {01, 09, 01, 01, 06, 13, 02, 01, 01, 01, 06, 15},
                                         {17, 17, 17, 17, 00, 17, 00, 17, 17, 17, 00, 16},
                                         {11, 01, 07, 01, 04, 15, 00, 15, 21, 15, 15, 15},
                                         {17, 17, 00, 17, 13, 15, 08, 01, 01, 01, 07, 01},
                                         {17, 17, 00, 17, 17, 15, 00, 15, 15, 15, 00, 15},
                                         {17, 01, 10, 01, 01, 01, 10, 01, 01, 01, 06, 13},
                                         {15, 12, 00, 15, 15, 15, 00, 15, 15, 15, 00, 15}
                                      };

            int[,] map02 = new int[,] {                                        
                                         {15, 00, 17, 17, 21, 17, 00, 15, 15, 15, 00, 17},
                                         {13, 08, 01, 01, 07, 01, 10, 01, 22, 15, 00, 13},
                                         {15, 00, 15, 15, 00, 15, 00, 17, 17, 15, 00, 17},
                                         {17, 00, 20, 17, 00, 17, 03, 01, 01, 01, 06, 15},
                                         {17, 00, 17, 17, 00, 17, 17, 15, 15, 15, 00, 15},
                                         {01, 09, 01, 01, 06, 13, 02, 01, 01, 01, 06, 15},
                                         {17, 17, 17, 17, 00, 17, 00, 17, 17, 17, 00, 16},
                                         {11, 01, 07, 01, 04, 15, 00, 15, 21, 15, 15, 15},
                                         {17, 17, 00, 17, 13, 15, 08, 01, 01, 01, 07, 01},
                                         {17, 17, 00, 17, 17, 15, 00, 15, 15, 15, 00, 15},
                                         {17, 01, 10, 01, 01, 01, 10, 01, 01, 01, 06, 13},
                                         {15, 12, 00, 15, 15, 15, 00, 15, 15, 15, 00, 15}
                                      };
            int[,] map03 = new int[,] {
                                        {12, 17, 00, 15, 00, 17, 17, 00, 17, 15, 15, 17},
                                        {01, 07, 09, 01, 10, 01, 01, 10, 01, 01, 05, 13},
                                        {15, 00, 17, 15, 00, 15, 15, 00, 15, 15, 00, 15},
                                        {15, 00, 17, 17, 00, 17, 12, 00, 17, 21, 00, 15},
                                        {15, 03, 07, 01, 09, 07, 01, 10, 01, 01, 10, 01},
                                        {17, 17, 00, 15, 15, 00, 15, 00, 17, 15, 00, 15},
                                        {17, 17, 00, 15, 17, 00, 15, 00, 22, 17, 00, 17},
                                        {01, 01, 10, 01, 01, 06, 15, 00, 17, 17, 00, 17},
                                        {19, 17, 00, 17, 17, 08, 01, 09, 01, 01, 06, 17},
                                        {17, 15, 00, 17, 15, 00, 17, 17, 17, 17, 00, 15},
                                        {01, 01, 09, 07, 01, 09, 01, 07, 01, 01, 06, 15},
                                        {17, 15, 15, 00, 17, 17, 15, 00, 15, 15, 00, 15}
                                      };

            int[,] map04 = new int[,] {
                                        {15, 15, 17, 19, 00, 15, 17, 15, 17, 15, 00, 17},
                                        {01, 01, 07, 01 ,09, 07, 01, 07, 01, 01, 06, 15},
                                        {17, 15, 00, 15, 17, 00, 17, 00, 17, 17, 00, 15},
                                        {17, 15, 00, 17, 17, 00, 13, 00, 17, 17, 08, 01},
                                        {01, 07, 09, 01 ,07, 09, 01, 09, 01, 01, 06, 15},
                                        {15, 00, 17, 15, 00, 17, 15, 15, 17, 17, 00, 15},
                                        {15, 00, 15, 17, 00, 15, 02, 01, 07, 01, 10, 01},
                                        {01, 09, 01, 01 ,09, 07, 04, 17, 00, 12, 00, 17},
                                        {17, 15, 15, 17, 15, 00, 17, 17, 00, 22, 00, 17},
                                        {01, 01, 01, 07 ,01, 09, 07, 01, 09, 07, 09, 01},
                                        {14, 18, 18, 00, 18, 15, 00, 18, 14, 00, 15, 15},
                                        {18, 18, 18, 00, 18, 15, 00, 18, 18, 00, 13, 15}
                                      };

            int[,] map05 = new int[,] {
                                        {12, 13, 11, 00, 21, 15, 00, 15, 17, 00, 12, 25},
                                        {02, 01, 01, 10, 01, 07, 09, 01, 07, 09, 01, 05},
                                        {00, 21, 17, 00, 24, 00, 19, 16, 00, 23, 14, 00},
                                        {00, 20, 18, 00, 16, 08, 01, 01, 06, 22, 16, 00},
                                        {08, 01, 07, 09, 01, 06, 17, 14, 00, 17, 19, 00},
                                        {04, 18, 00, 15, 23, 08, 01, 01, 06, 15, 20, 00},
                                        {17, 16, 00, 22, 14, 00, 17, 17, 08, 01, 01, 04},
                                        {02, 01, 09, 07, 01, 10, 01, 01, 06, 14, 11, 16},
                                        {00, 20, 16, 00, 22, 00, 14, 14, 08, 01, 05, 12},
                                        {00, 11, 12, 08 ,01, 06, 13, 18, 00, 15, 00, 17},
                                        {03, 01, 01, 06, 21, 08, 01, 01, 10, 01, 06, 25},
                                        {15, 13, 16, 00, 19, 00, 12, 11, 00, 17, 00, 15}
                                      };

            int[,] map06 = new int[,] {
                                        {15, 15, 02, 01, 04, 17, 00, 18, 18, 18, 18, 18},
                                        {02, 01, 04, 15, 15, 17, 08, 01, 05, 18, 00, 18},
                                        {00, 17, 15, 15, 02, 01, 06, 17, 00, 18, 00, 18},
                                        {00, 15, 01, 01, 04, 15, 00, 17, 08, 01, 06, 18},
                                        {00, 15, 18, 17, 17, 15, 00, 17, 00, 18, 00, 18},
                                        {03, 05, 18, 02, 01, 01, 06, 17, 00, 18, 00, 18},
                                        {17, 00, 18, 00, 17, 15, 03, 01, 04, 18, 03, 05},
                                        {02, 04, 18, 00, 17, 15, 17, 18, 18, 15, 18, 00},
                                        {00, 15, 02, 09, 05, 15, 02, 01, 05, 21, 18, 00},
                                        {00, 15, 00, 15, 08, 01, 06, 17, 03, 05, 18, 00},
                                        {03, 01, 06, 17, 00, 17, 00, 17, 17, 03, 01, 04},
                                        {18, 18, 03, 01, 09, 01, 04, 18, 18, 18, 18, 18}
                                      };
            m_Maps[0] = new MapCell(map01);
            m_Maps[1] = new MapCell(map02);
            m_Maps[2] = new MapCell(map03);
            m_Maps[3] = new MapCell(map04);
            m_Maps[4] = new MapCell(map05);
            m_Maps[5] = new MapCell(map06);
        }

        //Initialize Values for level Overlays
        public static void SetStaticOverlay()
        {
            m_staticOverlay = new MapCell[6];

            int[,] over01 = new int[,] {
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0}
                                       };

            int[,] over02 = new int[,] {
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0}
                                       };

            int[,] over03 = new int[,] {
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0}
                                       };

            int[,] over04 = new int[,] {
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0}
                                       };

            int[,] over05 = new int[,] {
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0}
                                       };

            int[,] over06 = new int[,] {
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0},
                                          {0,0,0,0,0,0,0,0,0,0,0,0}
                                       };

            m_staticOverlay[0] = new MapCell(over01);
            m_staticOverlay[1] = new MapCell(over02);
            m_staticOverlay[2] = new MapCell(over03);
            m_staticOverlay[3] = new MapCell(over04);
            m_staticOverlay[4] = new MapCell(over05);
            m_staticOverlay[5] = new MapCell(over06);
        }

        //Initialize player starting positions
        public static void SetStartingPoints()
        {
            startingPoints = new Point[6];
            startingPoints[0] = new Point(5, 6);
            startingPoints[1] = new Point(6, 10);
            startingPoints[2] = new Point(1, 0);
            startingPoints[3] = new Point(0, 4);
            startingPoints[4] = new Point(0, 9);
            startingPoints[5] = new Point(1, 10);
        }
        #endregion
    }

    //Hold Map Art Assets
    static class Tile
    {
        #region Variables

        static public Texture2D tileTexture;
        static public Texture2D overlayTexture;

        //Map border area tiles
        static public Texture2D[] borderTile;

        static public int tileWidth = 100;
        static public int tileHeight = 100;

        #endregion

        public static void DrawMap(SpriteBatch spriteBatch, Vector2 screenPos, int tileNum)
        {
            spriteBatch.Draw(tileTexture, screenPos, new Rectangle(tileNum * 100, 0, tileWidth, tileHeight), Color.White);
        }

        public static void DrawOverlay(SpriteBatch spriteBatch, Vector2 screenPos, int tileNum)
        {
            spriteBatch.Draw(overlayTexture, screenPos, new Rectangle(tileNum * 100, 0, tileWidth, tileHeight), Color.White);
        }

        public static void DrawBorder(SpriteBatch spriteBatch)
        {
            int borderTileNum = 0;

            Vector2 borderVector = new Vector2(Player.screenVector.X - Player.mapVector.X + 100,
                                                        Player.screenVector.Y - Player.mapVector.Y + 100);

            for (int j = 0; j < 26; j++)
            {
                borderTileNum = (borderTileNum + 1) % 4;
                for (int k = 0; k < 20; k++)
                {
                    borderTileNum = (borderTileNum + 1) % 4;
                    spriteBatch.Draw(borderTile[borderTileNum], new Rectangle((int)borderVector.X - 700 + (j * 100), (int)borderVector.Y - 400 + (k * 100), 100, 100), Color.White);
                }
            }
        }

        //Is the map tile passable
        public static bool IsPassable(int tileNum)
        {
            if (tileNum <= 11)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
