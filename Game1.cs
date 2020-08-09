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
using System.IO;
using System.Diagnostics;

namespace Stick_Hero
{
    /// <summary>
    /// This is the main type for your game
    /// </summary> 
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region globalVars
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public static FileStream file = new FileStream(@"score.txt" , FileMode.Append);
        public static StreamWriter writer = new StreamWriter(file);

        public static int finalScore = 0;
        public static int highScore = 0;
        public static bool gameOver = false;

        public static Random rand;

        // sounds
        public static SoundEffect effect;
        public static SoundEffectInstance soundEffectInstance;

        public static SoundEffect beep;
        public static SoundEffectInstance beepInstance;

        public static SoundEffect robot;
        public static SoundEffectInstance robotInstance;

        public static bool speakerState;

        // mouse detecting
        public static MouseState oldState, currentState;

        //1 for cow , 2 for zebra , 3 for monkey
        public static int currentCharacter;
        #endregion

       public enum GameState
        {
            intro,
            characterMenu,
            gameScene,
            scoreScene
        }

       public static GameState _state;

        // start main structure
        public struct Main
        {

            // start Characters's menu structure
            public struct Characters
            {
                public Texture2D background, cow, zebra, monkey, previous;
                public SpriteFont font;

                public void draw(SpriteBatch sp)
                {
                    sp.Draw(background, new Vector2(0, 0), Color.White);

                    sp.Draw(cow, new Vector2(50, 300), Color.White);
                    sp.Draw(zebra, new Vector2(50, 50), Color.White);
                    sp.Draw(monkey, new Vector2(550, 270), Color.White);

                    sp.Draw(previous, new Vector2(650, 50), Color.White);

                    switch (currentCharacter)
                    {
                        case 1:
                            sp.DrawString(font, "current : Cow", new Vector2(400, 20), Color.Black);
                            break;
                        case 2:
                            sp.DrawString(font, "current : Zebra", new Vector2(400, 20), Color.Black);
                            break;
                        case 3:
                            sp.DrawString(font, "current : monkey", new Vector2(400, 20), Color.Black);
                            break;
                    }
                }

                //which character user selectes
                public void choiceCharacter()
                {

                    Rectangle cowArea = new Rectangle(100, 350, 100, 100);
                    Rectangle zebraArea = new Rectangle(100, 100, 100, 100);
                    Rectangle monkeyArea = new Rectangle(600, 300, 100, 100);

                    var mousePosition = new Point(currentState.X, currentState.Y);

                    if (zebraArea.Contains(mousePosition) & oldState.LeftButton == ButtonState.Released & currentState.LeftButton == ButtonState.Pressed)
                    {
                        currentCharacter = 2;

                    }
                    else if (monkeyArea.Contains(mousePosition) & oldState.LeftButton == ButtonState.Released & currentState.LeftButton == ButtonState.Pressed)
                    {
                        currentCharacter = 3;

                    }
                    else if (cowArea.Contains(mousePosition) & oldState.LeftButton == ButtonState.Released & currentState.LeftButton == ButtonState.Pressed)
                    {
                        currentCharacter = 1;
                    }

                }

                public void backButton()
                {

                    var mousePosition = new Point(currentState.X, currentState.Y);
                    Rectangle area = new Rectangle(660, 60, 100, 100);

                    if (area.Contains(mousePosition) & oldState.LeftButton == ButtonState.Released & currentState.LeftButton == ButtonState.Pressed)
                    {

                        Game1._state = GameState.intro;
                       
                    }

                }
            }
            // end Characters's menu structure

            // start Intro structure
            public struct Intro
            {
                static Vector2 speakerPos = new Vector2(50, 50);
                static Vector2 startGameBtnPos = new Vector2(50, 150);
                static Vector2 CharacterMenuBtnPos = new Vector2(50, 250);
                public Texture2D background, speakerOn, speakerOff, startGameBtn , CharacterMenuBtn , superman , telegram , instagram;

                public static Rectangle instagramArea = new Rectangle(400, 370, 60, 60),
                                        telegramArea = new Rectangle(320 , 370 , 60 , 60);

                // for set random background in intro
                public string randomForBg;

                //for change speaker background when mouse clicked
                public int counterSp;


                public void update()
                {

                    Rectangle speakerArea = new Rectangle(50, 50, 60, 60);
                    Rectangle startGameBtnArea = new Rectangle(50, 150, 60, 60);
                    Rectangle characterBtnArea = new Rectangle(50, 250, 60, 60);
                    var position = new Point(currentState.X, currentState.Y);

                    // for speaker
                    if (speakerArea.Contains(position) & oldState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed)
                    {
                        counterSp++;
                    }

                    if (counterSp % 2 == 0)
                    {
                        Game1.soundEffectInstance.Pause();
                        speakerState = false;
                    }
                    else
                    {
                        Game1.soundEffectInstance.Play();
                        speakerState = true;
                    }

                    // for start game button
                    if (startGameBtnArea.Contains(position) & oldState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed)
                    {
                        Game1._state = GameState.gameScene;
                        Game1.soundEffectInstance.Stop();
                    }

                    // for character button
                    if (characterBtnArea.Contains(position) & oldState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed)
                    {
                        Game1._state = GameState.characterMenu;
                    }

                    // for social buttons
                    if (telegramArea.Contains(position) & oldState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed){
                        Process.Start("http://telegram.com");
                    }

                    if (instagramArea.Contains(position) & oldState.LeftButton == ButtonState.Released && currentState.LeftButton == ButtonState.Pressed){
                        Process.Start("http://instagram.com");
                    }
                }

                public void draw(SpriteBatch sp)
                {
                    sp.Draw(background, Vector2.Zero, Color.White);
                    sp.Draw(startGameBtn, startGameBtnPos, Color.White);
                    sp.Draw(CharacterMenuBtn, CharacterMenuBtnPos, Color.White);
                    sp.Draw(superman , new Vector2(250 , 30) , Color.White);
                    sp.Draw(telegram , telegramArea , Color.White);
                    sp.Draw(instagram, instagramArea, Color.White);

                    // draw speaker
                    if (speakerState)
                    {
                        sp.Draw(speakerOn, speakerPos, Color.White);
                    }
                    else
                    {
                        sp.Draw(speakerOff, speakerPos, Color.White);
                    }
                }
            }
            // end Intro structure

            //start Game structure
            public struct Game
            {
               
                // for draw target Rects and some animations
                public static Rectangle drawStage(int x)
                {
                    Random r = new Random();
                    int stageWidth = r.Next(16, 200);
                    int recStart = 50;
                    int recEnd = 380;
                    int startPos = r.Next(recStart, recEnd);
                    sunPos = new Rectangle(r.Next(50, 700), r.Next(30, 100), 160, 160);
                    cloud1Pos = new Rectangle(r.Next(100, 500), r.Next(20, 50), 160, 160);
                    cloud2Pos = new Rectangle(r.Next(10, 200), r.Next(60, 90), 160, 160);
                    return new Rectangle(x + startPos, 440, stageWidth, 100);
                }

                #region vars
                public int animationSpeed;
                public bool boardIsReverse;
                public static int height = 0;
            
                public Texture2D blackStage, characterTexture, scoreBg, sun, cloud1, cloud2, cloud3 , background;
                public Texture2D cow, monkey, zebra;

                public SpriteFont font;

                public static  float degree = (float)Math.PI;

                public static int score = 0;

                public static Rectangle first = new Rectangle(0, 440, 160, 100), last = drawStage(16);

                public static Rectangle
                                         temp = new Rectangle(0, 0, 0, 0),
                                         currentBoard = new Rectangle(0, 0, 0, 0),
                                         oldBoard = new Rectangle(0, 0, 0, 0),
                                         character = new Rectangle(0, 403, 40, 40),
                                         sunPos = new Rectangle(500, 50, 0, 0),
                                         cloud1Pos = new Rectangle(200, 70, 0, 0),
                                         cloud2Pos = new Rectangle(300, 30, 0, 0);

                public static bool moving = false;
                #endregion vars

                public void update()
                {
                    gameOver = false;
                    if (!moving)
                    {
                        if (currentState.RightButton == ButtonState.Pressed) { drawBoard(); }

                    }

                    if (currentState.RightButton == ButtonState.Released && oldState.RightButton== ButtonState.Pressed)
                    {
                        moving = true;

                    }
                    if (moving)
                    {

                        if (degree > Math.PI / 2)
                        {
                            degree -= (float)Math.PI / 40;

                            if (Game1.speakerState)
                            {
                                Game1.robotInstance.Play();
                            }
                        }
                        else if ((last.Left - first.Right) > currentBoard.Height || (last.Right - first.Right) < currentBoard.Height)
                        {


                            if (character.X < first.Right + currentBoard.Height - 5)
                            {
                                character.X += 6;
                                
                                if ( Game1.speakerState)
                                {
                                    Game1.beepInstance.Play();
                                }
                            }else
                            {
                                if (character.Y < 480)
                                {
                                    character.Y += 16;
                                }
                                else
                                {
                                    Game1.finalScore = score;

                                    try
                                    {
                                        writer.WriteLine(finalScore.ToString());

                                    }catch(Exception e)
                                    {
                                        Debug.WriteLine(e.ToString());
                                    }
                                    finally
                                    {
                                        writer.Close();
                                    }

                                    Game1.gameOver = true;

                                    Game1._state = GameState.scoreScene;
                                }
                            }
                        }
                        else if (character.X < last.Right - 35)
                        {

                            character.X += 6;
                            if (Game1.speakerState)
                            {
                                Game1.beepInstance.Play();
                            }
                        }
                        else if (last.X > 0)
                        {

                            if (temp.X == 0)
                            {
                                temp = drawStage(last.Right);
                            }

                            oldBoard.X -= animationSpeed;
                            last.X -= animationSpeed;
                            first.X -= animationSpeed;
                            currentBoard.X -= animationSpeed;
                            temp.X -= animationSpeed;
                            character.X -= animationSpeed;
                        }
                        else
                        {
                            oldBoard = currentBoard;
                            last.X = 0;
                            first = last;
                            last = temp;
                            temp = new Rectangle(0, 0, 0, 0);

                            degree = (float)Math.PI;
                            currentBoard = new Rectangle(0, 0, 0, 0);
                            moving = false;
                            score++;
                        }
                    }
                }

                public void draw(SpriteBatch sp)
                {
                    sp.Draw(background, new Vector2(0, 0), Color.White);
                    sp.Draw(blackStage, first, Color.Black);
                    sp.Draw(blackStage, last, Color.Black);
                    sp.Draw(blackStage, temp, Color.Black);
                    sp.Draw(blackStage, currentBoard, null, Color.Black, -degree, Vector2.Zero, SpriteEffects.None, 0);
                    sp.Draw(blackStage, oldBoard, null, Color.Black, -(float)Math.PI / 2, Vector2.Zero, SpriteEffects.None, 0);
                    sp.Draw(scoreBg, new Vector2(40, 15), Color.White);
                    sp.Draw(sun, sunPos, Color.White);
                    sp.Draw(cloud1, cloud1Pos, Color.White);
                    sp.Draw(cloud2, cloud2Pos, Color.White);
                    sp.Draw(characterTexture, character, Color.White);
                    sp.DrawString(font, "Score : " + score, new Vector2(50, 20), Color.NavajoWhite);
                }

                // for draw board
                public void drawBoard()
                {


                    if (currentBoard.Height < 400 & !boardIsReverse)
                    {

                        if (score <= 5)
                        {
                            height = currentBoard.Height + 4;
                        }
                        else if (score > 5 & score <= 10)
                        {
                            height = currentBoard.Height + 6;

                        }
                        else if (score > 10 & score <= 15)
                        {
                            height = currentBoard.Height + 8;
                        }



                    }
                    else if (currentBoard.Height <= 400 & currentBoard.Height > 0 & boardIsReverse)
                    {

                        if (score <= 5)
                        {
                            height = currentBoard.Height - 4;
                        }
                        else if (score > 5 & score <= 10)
                        {
                            height = currentBoard.Height - 6;

                        }
                        else if (score > 10 & score <= 15)
                        {
                            height = currentBoard.Height - 8;
                        }

                    }
                    else if (currentBoard.Height == 400)
                    {
                        boardIsReverse = true;
                    }
                    else if (currentBoard.Height == 0 & boardIsReverse)
                    {
                        boardIsReverse = false;
                    }

                    currentBoard = new Rectangle(first.Right -4, 440, 4, height);

                }

                // for start game
                public static void restart()
                {
                    Main.Game.moving = false;
                    gameOver = false;
                    Main.Game.score = 0;
                    Main.Game.currentBoard = new Rectangle(0, 0, 0, 0);
                    Main.Game.oldBoard = new Rectangle(0, 0, 0, 0);
                    Main.Game.character = new Rectangle(0, 403, 40, 40);
                    finalScore = 0;
                    Main.Game.degree = (float)Math.PI;
                    Main.Game.temp = new Rectangle(0, 0, 0, 0);
                }

            }
            public struct Score
            {
                public Texture2D background;
                public SpriteFont font;

                public static Rectangle restartArea = new Rectangle(80, 420, 160, 20),
                                        menuArea = new Rectangle(370, 420, 160, 20);

                public void update()
                {
                    var mousePos = new Point(currentState.X , currentState.Y);

                    if (oldState.LeftButton == ButtonState.Released & currentState.LeftButton == ButtonState.Pressed & restartArea.Contains(mousePos))
                    {
                        Main.Game.restart();
                        Game1._state = GameState.gameScene;
                    }
                    else if (oldState.LeftButton == ButtonState.Released & currentState.LeftButton == ButtonState.Pressed & menuArea.Contains(mousePos))
                    {
                        Main.Game.restart();
                        Game1._state = GameState.intro;
                    }
                }

                public void draw(SpriteBatch sp)
                {
                    gameOver = false;
                    sp.Draw(background , new Vector2(0 , 0) , Color.White);
                    sp.DrawString(font , "your score is : " + finalScore.ToString() , new Vector2(10 , 5) , Color.White);
                    
                    var scores = File.ReadAllLines(@"score.txt");

                    foreach (var score in scores)
                    {
                        if (Convert.ToInt32(score) > highScore)
                        {
                            highScore = Convert.ToInt32(score);
                        }                       
                    }

                    sp.DrawString(font, "highscore is : " + highScore.ToString(), new Vector2(10, 50), Color.Beige);
                }
            }
            // end Score structure
        }
        // end main structure

        #region structuresInstance
       public Main main;
       public Main.Characters charactersMenu;
       public Main.Intro introMenu;
       public Main.Game game;
       public Main.Score scoreMenu;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            speakerState = true;
            introMenu.counterSp = 1;
            game.animationSpeed = 5;
            game.boardIsReverse = false;
            rand = new Random();

            introMenu.randomForBg = rand.Next(1 , 5).ToString();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Intro contents
            introMenu.background = Content.Load<Texture2D>("backgrounds/" + introMenu.randomForBg);
            introMenu.speakerOn = Content.Load<Texture2D>("speaker/on");
            introMenu.speakerOff = Content.Load<Texture2D>("speaker/off");
            introMenu.startGameBtn = Content.Load<Texture2D>("buttons/Start-Game");
            introMenu.CharacterMenuBtn = Content.Load<Texture2D>("buttons/Characters");
            introMenu.superman = Content.Load<Texture2D>("superman");
            introMenu.telegram = Content.Load<Texture2D>("telegram");
            introMenu.instagram = Content.Load<Texture2D>("instagram");


            effect = Content.Load<SoundEffect>("eye of tiger");
            soundEffectInstance = effect.CreateInstance();

            soundEffectInstance.IsLooped = true;
            soundEffectInstance.Play();

            // Characters contents
            charactersMenu.background = Content.Load<Texture2D>("backgrounds/" + introMenu.randomForBg);
            charactersMenu.cow = Content.Load<Texture2D>("characters/cow");
            charactersMenu.zebra = Content.Load<Texture2D>("characters/zebra");
            charactersMenu.monkey = Content.Load<Texture2D>("characters/monkey");
            charactersMenu.previous = Content.Load<Texture2D>("buttons/Back-To-Menu");
            charactersMenu.font = Content.Load<SpriteFont>("font");

            // Game contents
            game.background = Content.Load<Texture2D>("gameBG/" + rand.Next(1, 5).ToString());
            game.blackStage = Content.Load<Texture2D>("black");
            game.characterTexture = Content.Load<Texture2D>("_cow");
            game.font = Content.Load<SpriteFont>("font");
            game.scoreBg = Content.Load<Texture2D>("score");
            game.cloud1 = Content.Load<Texture2D>("cloud");
            game.cloud2 = Content.Load<Texture2D>("cloud");
            game.cloud3 = Content.Load<Texture2D>("cloud");
            game.sun = Content.Load<Texture2D>("sun");
            game.cow = Content.Load<Texture2D>("_cow");
            game.monkey = Content.Load<Texture2D>("_monkey");
            game.zebra = Content.Load<Texture2D>("_zebra");

            beep = Content.Load<SoundEffect>("beep");
            beepInstance = beep.CreateInstance();

            // Score contents
            scoreMenu.background = Content.Load<Texture2D>("scoreBG");
            scoreMenu.font = Content.Load<SpriteFont>("font");

            robot = Content.Load<SoundEffect>("robot");
            robotInstance = robot.CreateInstance();

        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

                oldState = currentState;
                currentState = Mouse.GetState();

            switch (_state)
            {
                case GameState.intro:
                    introMenu.update();
                    break;
                case GameState.characterMenu:
                    charactersMenu.choiceCharacter();
                    charactersMenu.backButton();
                    break;
                case GameState.gameScene:
                    if (!gameOver) { game.update(); }
                    break;
                case GameState.scoreScene:
                    scoreMenu.update();
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.IsMouseVisible = true;

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch (currentCharacter)
            {
                case 1:
                    game.characterTexture = game.cow;
                    break;
                case 2:
                    game.characterTexture = game.zebra;
                    break;
                case 3:
                    game.characterTexture = game.monkey;
                    break;
            }
            
            switch(_state){
                case GameState.intro:
                    introMenu.draw(spriteBatch);
                    break;
                case GameState.characterMenu:
                    charactersMenu.draw(spriteBatch);
                    break;
                case GameState.gameScene:
                    if (!gameOver) { game.draw(spriteBatch); }
                    break;
                case GameState.scoreScene:
                    scoreMenu.draw(spriteBatch);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);

                    
                 
        }

    }
}