using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Home_Front
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //General Assets
        public static bool exitGame;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Art Assets
        Texture2D tiles;
        Texture2D staticOverlay;
        Texture2D mouseCursor;

        //Texture2D animOverlay;
        Texture2D playerSprite;
        Texture2D enemySprite;
        Texture2D[] InPlayTxr;
        Texture2D[] TransitionTxr;
        Texture2D[] MenuTxr;
        Texture2D[] ButtonTxr;
        Texture2D[] PopUpTxr;
        Texture2D[] borderTiles;

        //Sound Assets
        Song[] gameSongs;
        SoundEffect[] gameSFX;

        //String Assets
        SpriteFont debugFont;
        SpriteFont[] popUpFonts;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1366;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            exitGame = false;
            this.IsMouseVisible = false;

            //Initialize Textures
            InPlayTxr = new Texture2D[13];
            TransitionTxr = new Texture2D[11];
            MenuTxr = new Texture2D[7];
            ButtonTxr = new Texture2D[8];
            PopUpTxr = new Texture2D[23];
            borderTiles = new Texture2D[4];

            //Initialize Sounds
            gameSongs = new Song[6];
            gameSFX = new SoundEffect[14];

            //Initialize Fonts
            popUpFonts = new SpriteFont[4];

            EnemyHandler.InitializeEnemies();

            Map.SetMap();
            Map.SetStaticOverlay();
            Map.SetStartingPoints();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load map tiles
            tiles = Content.Load<Texture2D>(".\\Images\\InPlay\\Tiles");
            staticOverlay = Content.Load<Texture2D>(".\\Images\\InPlay\\Overlays");
            borderTiles[0] = Content.Load<Texture2D>(".\\Images\\Border\\BorderBuilding01");
            borderTiles[1] = Content.Load<Texture2D>(".\\Images\\Border\\BorderBuilding02");
            borderTiles[2] = Content.Load<Texture2D>(".\\Images\\Border\\BorderBuilding03");
            borderTiles[3] = Content.Load<Texture2D>(".\\Images\\Border\\BorderBuilding04");

            //Load sprites
            playerSprite = Content.Load<Texture2D>(".\\Images\\InPlay\\SpritePlayer");
            enemySprite = Content.Load<Texture2D>(".\\Images\\InPlay\\SpriteEnemy");
            
            //Load textures
            InPlayTxr[0] = Content.Load<Texture2D>(".\\Images\\InPlay\\InPlayBack");
            InPlayTxr[1] = Content.Load<Texture2D>(".\\Images\\InPlay\\Inventory");
            InPlayTxr[2] = Content.Load<Texture2D>(".\\Images\\InPlay\\StatusBar");
            InPlayTxr[3] = Content.Load<Texture2D>(".\\Images\\InPlay\\HungerBox");
            InPlayTxr[4] = Content.Load<Texture2D>(".\\Images\\InPlay\\HungerBar");
            InPlayTxr[5] = Content.Load<Texture2D>(".\\Images\\InPlay\\LoseBack");
            InPlayTxr[6] = Content.Load<Texture2D>(".\\Images\\InPlay\\WinBack");
            InPlayTxr[7] = Content.Load<Texture2D>(".\\Images\\InPlay\\InfoBox");
            InPlayTxr[8] = Content.Load<Texture2D>(".\\Images\\InPlay\\Firework");
            InPlayTxr[9] = Content.Load<Texture2D>(".\\Images\\InPlay\\SmokeBomb");
            InPlayTxr[10] = Content.Load<Texture2D>(".\\Images\\InPlay\\Fire");
            InPlayTxr[11] = Content.Load<Texture2D>(".\\Images\\InPlay\\Smoke");
            InPlayTxr[12] = Content.Load<Texture2D>(".\\Images\\InPlay\\QuestMarker");

            //Load transition textures
            TransitionTxr[0] = Content.Load<Texture2D>(".\\Images\\Transition\\Intro");
            TransitionTxr[1] = Content.Load<Texture2D>(".\\Images\\Transition\\Outro");
            TransitionTxr[2] = Content.Load<Texture2D>(".\\Images\\Transition\\LoadingBack");
            TransitionTxr[3] = Content.Load<Texture2D>(".\\Images\\Transition\\SpinBorder");
            TransitionTxr[4] = Content.Load<Texture2D>(".\\Images\\Transition\\Spin1");
            TransitionTxr[5] = Content.Load<Texture2D>(".\\Images\\Transition\\Spin2");
            TransitionTxr[6] = Content.Load<Texture2D>(".\\Images\\Transition\\Spin3");
            TransitionTxr[7] = Content.Load<Texture2D>(".\\Images\\Transition\\LoadingBar");
            TransitionTxr[8] = Content.Load<Texture2D>(".\\Images\\Transition\\LoadingFill");
            TransitionTxr[9] = Content.Load<Texture2D>(".\\Images\\Transition\\Bomber");
            TransitionTxr[10] = Content.Load<Texture2D>(".\\Images\\Transition\\Bomb");

            //Load menu textures
            MenuTxr[0] = Content.Load<Texture2D>(".\\Images\\Menu\\MenuBack");
            MenuTxr[1] = Content.Load<Texture2D>(".\\Images\\Menu\\HighScoreBox");
            MenuTxr[2] = Content.Load<Texture2D>(".\\Images\\Menu\\AchievementBox");
            MenuTxr[3] = Content.Load<Texture2D>(".\\Images\\Menu\\InstructionBox");
            MenuTxr[4] = Content.Load<Texture2D>(".\\Images\\Menu\\SettingBox");
            MenuTxr[5] = Content.Load<Texture2D>(".\\Images\\Menu\\MenuTitle");
            MenuTxr[6] = Content.Load<Texture2D>(".\\Images\\Menu\\Achievements");

            //Load button textures
            ButtonTxr[0] = Content.Load<Texture2D>(".\\Images\\Button\\NewGame");
            ButtonTxr[1] = Content.Load<Texture2D>(".\\Images\\Button\\HighScore");
            ButtonTxr[2] = Content.Load<Texture2D>(".\\Images\\Button\\Settings");
            ButtonTxr[3] = Content.Load<Texture2D>(".\\Images\\Button\\QuitGame");
            ButtonTxr[4] = Content.Load<Texture2D>(".\\Images\\Button\\MainMenu");
            ButtonTxr[5] = Content.Load<Texture2D>(".\\Images\\Button\\Return");
            ButtonTxr[6] = Content.Load<Texture2D>(".\\Images\\Button\\ButtonProto");
            ButtonTxr[7] = Content.Load<Texture2D>(".\\Images\\Button\\Continue");

            //Load popup textures
            PopUpTxr[0] = Content.Load<Texture2D>(".\\Images\\PopUp\\PopUpBack");
            PopUpTxr[1] = Content.Load<Texture2D>(".\\Images\\PopUp\\PopUpBox");
            PopUpTxr[2] = Content.Load<Texture2D>(".\\Images\\PopUp\\01");
            PopUpTxr[3] = Content.Load<Texture2D>(".\\Images\\PopUp\\02");
            PopUpTxr[4] = Content.Load<Texture2D>(".\\Images\\PopUp\\03");
            PopUpTxr[5] = Content.Load<Texture2D>(".\\Images\\PopUp\\04");
            PopUpTxr[6] = Content.Load<Texture2D>(".\\Images\\PopUp\\05");
            PopUpTxr[7] = Content.Load<Texture2D>(".\\Images\\PopUp\\06");
            PopUpTxr[8] = Content.Load<Texture2D>(".\\Images\\PopUp\\07");
            PopUpTxr[9] = Content.Load<Texture2D>(".\\Images\\PopUp\\08");
            PopUpTxr[10] = Content.Load<Texture2D>(".\\Images\\PopUp\\09");
            PopUpTxr[11]= Content.Load<Texture2D>(".\\Images\\PopUp\\10");
            PopUpTxr[12] = Content.Load<Texture2D>(".\\Images\\PopUp\\11");
            PopUpTxr[13] = Content.Load<Texture2D>(".\\Images\\PopUp\\12");
            PopUpTxr[14] = Content.Load<Texture2D>(".\\Images\\PopUp\\13");
            PopUpTxr[15] = Content.Load<Texture2D>(".\\Images\\PopUp\\14");
            PopUpTxr[16] = Content.Load<Texture2D>(".\\Images\\PopUp\\15");
            PopUpTxr[17] = Content.Load<Texture2D>(".\\Images\\PopUp\\16");
            PopUpTxr[18] = Content.Load<Texture2D>(".\\Images\\PopUp\\17");
            PopUpTxr[19] = Content.Load<Texture2D>(".\\Images\\PopUp\\18");
            PopUpTxr[20] = Content.Load<Texture2D>(".\\Images\\PopUp\\19");
            PopUpTxr[21] = Content.Load<Texture2D>(".\\Images\\PopUp\\20");
            PopUpTxr[22] = Content.Load<Texture2D>(".\\Images\\PopUp\\20");
           
            //Error handling in case of No Audio Device
            try
            {
                gameSongs[0] = Content.Load<Song>(".\\Music\\Music01");
                gameSongs[1] = Content.Load<Song>(".\\Music\\Music02");
                gameSongs[2] = Content.Load<Song>(".\\Music\\Music03");
                gameSongs[3] = Content.Load<Song>(".\\Music\\Music04");
                gameSongs[4] = Content.Load<Song>(".\\Music\\Music05");
                gameSongs[5] = Content.Load<Song>(".\\Music\\Music06");
            }
            catch (NoAudioHardwareException)
            {
            }
            catch (InvalidOperationException)
            {
            }

            //Error handling in case of No Audio Device
            try
            {
                gameSFX[0] = Content.Load<SoundEffect>(".\\SFX\\Death");
                gameSFX[1] = Content.Load<SoundEffect>(".\\SFX\\Explosions");
                gameSFX[2] = Content.Load<SoundEffect>(".\\SFX\\Firework");
                gameSFX[3] = Content.Load<SoundEffect>(".\\SFX\\FootStep");
                gameSFX[4] = Content.Load<SoundEffect>(".\\SFX\\Notification");
                gameSFX[5] = Content.Load<SoundEffect>(".\\SFX\\Quest");
                gameSFX[6] = Content.Load<SoundEffect>(".\\SFX\\SmokeBomb");
                gameSFX[7] = Content.Load<SoundEffect>(".\\SFX\\Bullet");
                gameSFX[8] = Content.Load<SoundEffect>(".\\SFX\\Logo");
                gameSFX[9] = Content.Load<SoundEffect>(".\\SFX\\Outro");
                gameSFX[10] = Content.Load<SoundEffect>(".\\SFX\\SmokeStart");
                gameSFX[11] = Content.Load<SoundEffect>(".\\SFX\\FireStart");
                gameSFX[11] = Content.Load<SoundEffect>(".\\SFX\\FireStart");
                gameSFX[12] = Content.Load<SoundEffect>(".\\SFX\\LoadingNoise");
                gameSFX[13] = Content.Load<SoundEffect>(".\\SFX\\LoadingEnd");
            }
            catch (NoAudioHardwareException)
            {
            }
            catch (InvalidOperationException)
            {
            }

            //Load fonts
            debugFont = Content.Load<SpriteFont>(".\\Fonts\\DebugFont");
            popUpFonts[0] = Content.Load<SpriteFont>(".\\Fonts\\Arial20");
            popUpFonts[1] = Content.Load<SpriteFont>(".\\Fonts\\Arial14");
            popUpFonts[2] = Content.Load<SpriteFont>(".\\Fonts\\Xirod32");
            popUpFonts[3] = Content.Load<SpriteFont>(".\\Fonts\\Xirod20");

            //Load mouse cursor
            mouseCursor = Content.Load<Texture2D>(".\\Images\\Menu\\Cursor");

            //Send tile textures to static class
            Tile.tileTexture = tiles;
            Tile.overlayTexture = staticOverlay;
            Tile.borderTile = borderTiles;

            //Send tile textures to classes
            PlayerSprite.spriteTexture = playerSprite;
            EnemySprite.spriteTexture = enemySprite;

            //Send additional textures to static classes
            InPlay.inPlayTexture = InPlayTxr;
            Transition.transitionTexture = TransitionTxr;
            Menu.menuTexture = MenuTxr;
            PopUp.popUpTxr = PopUpTxr;
            ButtonManager.buttonTexture = ButtonTxr;
            ButtonManager.InitButtons();

            //Send Music & SFX to static class
            AudioHandler.songs = gameSongs;
            AudioHandler.sfxs = gameSFX;
            AudioHandler.CreateEffectInstances();

            //Send font to classes
            Debug.debugFont = debugFont;
            PopUp.popUpFonts = popUpFonts;
        }

        protected override void UnloadContent()
        {
            //Not used
        }

        protected override void Update(GameTime gameTime)
        {
            //If outro finished, exit program
            if (exitGame)
            {
                Exit();
            }

            //Used for debug
            Debug.Update(gameTime);

            //Update Input
            InputHandler.Update();

            //Update Game State
            StateHandler.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //Draw Game Screen
            StateHandler.Draw(spriteBatch);

            //Draws mouse cursor
            int[] mousePos = InputHandler.GetMousePos();
            spriteBatch.Draw(mouseCursor, new Vector2(mousePos[0], mousePos[1]), Color.White);

            //Draws debug information
            Debug.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
