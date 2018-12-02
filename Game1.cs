using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Midi;
using MonoGameTest;

namespace MonoGameTest

{
    public class DeviceSignal

    {
         int n = 0;
        InputDevice inputDevice;
        public DeviceSignal(int id)
        {
         inputDevice = InputDevice.InstalledDevices[id];
            inputDevice.Open();
            inputDevice.StartReceiving(null);
        }
        public int GetSignal()
        {
           
            void NoteOn(NoteOnMessage msg)
            {
                if ((int)msg.Pitch >= (int)Pitch.C4 && (int)msg.Pitch <= (int)Pitch.E5)
                {
                    n = (int)msg.Pitch;
                }
            }
              inputDevice.NoteOn += new InputDevice.NoteOnHandler(NoteOn);
             
               return n;
            }  // Note events will be received in another thread
                                                // Pitch[] pitches = { Pitch.C4, Keys.D2, Keys.W, Keys.D3, Keys.E, Keys.R, Keys.D5, Keys.T, Keys.D6, Keys.Y, Keys.D7, Keys.U, Keys.I, Keys.D9, Keys.O, Keys.D0, Keys.P };


            // Console.ReadKey();  // This thread waits for a keypress
            
        }

    
   



    class chord
    {
        public bool isactive = false;
        public int plaingtime = 0;
        public string name;
        public chord(string name)
        {
            this.name = name;
        }
    }

    class exployd
    {
        public bool isplay = false;
        public int Y;
        public Point currentFrame = new Point(0, 0);// если менять то не забудь изменит в draw
        public int frameWidth = 36;
        public int frameHeight = 36;
        public exployd()
        {

        }
    }
    class ship
    {   
        public int timeafterdead = 0;
        public int Y;
        public Point currentFrame = new Point(0, 0);// если менять то не забудь изменит в draw
        public int frameWidth = 36;
        public int frameHeight = 45;
        public bool isdead = true;

        public ship()
        {

        }
    }
    class laser
    {
        public Point currentFrame = new Point(32, 0);// если менять то не забудь изменит в draw
        public int frameWidth = 32;
        public int frameHeight = 400;
        public laser()
        {

        }
    }
    class bpk //piano key
    {
        public bool isActive;
        public int Y;
        public int X;
        public Point currentFrame = new Point(0, 0);// если менять то не забудь изменит в draw

        public int frameWidth = 18;
        public int frameHeight = 64;
        public bpk(Keys k, ship ship)
        {
            key = k;
            this.ship = ship;
        }

        public Keys key;
        public ship ship;

    }
    //black piano key


    class pk //piano key
    {
        public bool isblack;
        public bool isActive;
        public int Y;
        public int X;
        public Point currentFrame = new Point(0, 0);// если менять то не забудь изменит в draw

        public int frameWidth = 18;
        public int frameHeight = 64;
        public pk(Keys k, ship ship, bool ib)
        {
            isblack = ib;
            key = k;
            this.ship = ship;
        }
        public int getY()
        {
            return Y;
        }

        public Keys key;
        public ship ship;
    }



    public class Game1 : Game
    {
        int speed = 5;
        Random rand = new Random();

        int temp;
        int scale = 2;// увелич размер калв
        int LASERscale = 1;
        int plusX = 37;// cдвиг клавиш вправо
        chord Ch = new chord("G");
        GraphicsDeviceManager graphics;


        SpriteBatch spriteBatch;
        Texture2D whitekey;
        Texture2D laser;
        Texture2D texship1;
        Texture2D explouse;
        laser lasersprite = new laser();
        DeviceSignal signal = new DeviceSignal(0);

        List<ship> ships = new List<ship>();

        int score = 0;
        exployd exp = new exployd();

        List<pk> keys = new List<pk>();
        List<pk> Blackkeys = new List<pk>();

       
        static int Y = 356; //клава вниз
        static int X = 100; //клава в бок // если менять то не забудь изменит в draw
        Vector2 position = new Vector2(X, Y);
        Vector2 laserposition = new Vector2(X, Y - 90);
        int n;
           
        public Game1()
        {   
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 60);
          
            Keys[] keyboard = { Keys.Q, Keys.D2, Keys.W, Keys.D3,Keys.E, Keys.R, Keys.D5,Keys.T, Keys.D6,Keys.Y, Keys.D7,Keys.U,Keys.I,  Keys.D9,  Keys.O,  Keys.D0,   Keys.P};

            foreach (var key in keyboard)
            {
                if (48 <= (int)key && (int)key <= 57)
                {
                    keys.Add(new pk(key, new ship(), true));
                }
                else keys.Add(new pk(key, new ship(),false));
                
            }
        

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
            laser = Content.Load<Texture2D>("laser");
            whitekey = Content.Load<Texture2D>("whitekey");
            texship1 = Content.Load<Texture2D>("ship1");
            explouse = Content.Load<Texture2D>("explouse");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
             
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            n += signal.GetSignal();
            //проверка на нажатую белую клавишу вссе работает не трогай

            foreach (var key in keys)
            {
                if (Keyboard.GetState().IsKeyDown(key.key))
                {
                    key.ship.isdead = true;

                    key.currentFrame.X = key.frameWidth;
                    key.isActive = true;
                    exp.isplay = false;
                }

                if (Keyboard.GetState().IsKeyUp(key.key))
                {
                    key.currentFrame.X = 0;
                    key.isActive = false;
                }
            }
            // проверка на черную
           

            {/*
                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    ship1obj.isdead = true;

                    C1.currentFrame.X = C1.frameWidth;
                    C1.isActive = true;
                    exp.isplay = false;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Q))
                {
                    C1.currentFrame.X = 0;
                    C1.isActive = false;
                }



                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    ship2obj.isdead = true;
                    D1.currentFrame.X = D1.frameWidth;
                    D1.isActive = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.W))
                {
                    D1.currentFrame.X = 0;
                    D1.isActive = false;
                }



                if (Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    ship3obj.isdead = true;
                    E1.currentFrame.X = E1.frameWidth;
                    E1.isActive = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.E))
                {
                    E1.currentFrame.X = 0;
                    E1.isActive = false;
                }



                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    ship4obj.isdead = true;
                    F1.currentFrame.X = F1.frameWidth;
                    F1.isActive = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.R))
                {
                    F1.currentFrame.X = 0;
                    F1.isActive = false;
                }



                if (Keyboard.GetState().IsKeyDown(Keys.T))
                {
                    ship5obj.isdead = true;
                    G1.currentFrame.X = G1.frameWidth;
                    G1.isActive = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.T))
                {
                    G1.currentFrame.X = 0;
                    G1.isActive = false;
                }



                if (Keyboard.GetState().IsKeyDown(Keys.Y))
                {
                    ship6obj.isdead = true;
                    A1.currentFrame.X = A1.frameWidth;
                    A1.isActive = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.Y))
                {
                    A1.currentFrame.X = 0;
                    A1.isActive = false;
                }



                if (Keyboard.GetState().IsKeyDown(Keys.U))
                {
                    ship7obj.isdead = true;
                    B1.currentFrame.X = B1.frameWidth;
                    B1.isActive = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.U))
                {
                    B1.currentFrame.X = 0;
                    B1.isActive = false;
                }



                if (Keyboard.GetState().IsKeyDown(Keys.I))
                {
                    ship8obj.isdead = true;
                    C2.currentFrame.X = C2.frameWidth;
                    C2.isActive = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.I))
                {
                    C2.currentFrame.X = 0;
                    C2.isActive = false;
                }



                if (Keyboard.GetState().IsKeyDown(Keys.O))
                {
                    ship9obj.isdead = true;
                    D2.currentFrame.X = D2.frameWidth;
                    D2.isActive = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.O))
                {
                    D2.currentFrame.X = 0;
                    D2.isActive = false;
                }



                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    ship10obj.isdead = true;
                    E2.currentFrame.X = E2.frameWidth;
                    E2.isActive = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.P))
                {
                    E2.currentFrame.X = 0;
                    E2.isActive = false;
                }

            */
                base.Update(gameTime);
            }
           

            {

                /*if (Keyboard.GetState().IsKeyDown(Keys.D2))
                {
                    C1b.currentFrame.X = C1.frameWidth;
                    C1b.isActive = true;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.D2))
                {
                    C1b.currentFrame.X = 0;
                    C1b.isActive = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D3))
                {
                    D1b.currentFrame.X = C1.frameWidth;
                    D1b.isActive = true;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.D3))
                {
                    D1b.currentFrame.X = 0;
                    D1b.isActive = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D5))
                {
                    F1b.currentFrame.X = C1.frameWidth;
                    F1b.isActive = true;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.D5))
                {
                    F1b.currentFrame.X = 0;
                    F1b.isActive = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D6))
                {
                    G1b.currentFrame.X = C1.frameWidth;
                    G1b.isActive = true;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.D6))
                {
                    G1b.currentFrame.X = 0;
                    G1b.isActive = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D7))
                {
                    A1b.currentFrame.X = C1.frameWidth;
                    A1b.isActive = true;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.D7))
                {
                    A1b.currentFrame.X = 0;
                    A1b.isActive = false;

                }

                if (Keyboard.GetState().IsKeyDown(Keys.D9))
                {
                    C2b.currentFrame.X = C1.frameWidth;
                    C2b.isActive = true;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.D9))
                {
                    C2b.currentFrame.X = 0;
                    C2b.isActive = false;

                }


                if (Keyboard.GetState().IsKeyDown(Keys.D0))
                {
                    D2b.currentFrame.X = C1.frameWidth;
                    D2b.isActive = true;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.D0))
                {
                    D2b.currentFrame.X = 0;
                    D2b.isActive = false;

                }*/
            }
            void deathcycle(ship ship1obj, chord ch)
            {
                if (ship1obj.Y == Y - 20 - 36 - 30)
                {
                    ship1obj.isdead = true;

                }
                
                if (ship1obj.isdead) ship1obj.Y = 0;
            }

            void chordActive(chord F)
            {
                F.isactive = true;
                switch (F.name)
                {
                    case "C":
                        { //047
                            

                            keys[0].ship.isdead = keys[4].ship.isdead = keys[7].ship.isdead = true;

                           
                            break;
                        }

                    case "D":
                        { //269
                          keys[2].ship.isdead = keys[6].ship.isdead = keys[9].ship.isdead = false;
                           

                            break;
                        }
                    case "F":
                        { //5 9 12
                            keys[5].ship.isdead = keys[9].ship.isdead = keys[12].ship.isdead = false;

                         

                            break;
                        }
                    case "G":
                        { //7 11 14
                            keys[7].ship.isdead = keys[11].ship.isdead = keys[14].ship.isdead = false;

                           
                            break;
                        }
                    case "A":
                        { //9 13 16
                            keys[9].ship.isdead = keys[13].ship.isdead = keys[16].ship.isdead = false;
                           
                            break;
                        }
                    case "B":
                        { //11 15

                          keys[11].ship.isdead = keys[15].ship.isdead = false;
                           
                            break;
                        }

                }

                if (F.plaingtime > 3) F.plaingtime = 0;

            }

            int aliveShipsCount = keys.Count(key => key.ship.isdead == false);

            if (Ch.plaingtime == 0)
            {
                if (aliveShipsCount == 0)
                {
                    temp = rand.Next(1, 8);
                    switch (temp)
                    {
                        case 1: { Ch.name = "A"; break; }
                        case 2: { Ch.name = "B"; break; }
                        case 3: { Ch.name = "C"; break; }
                        case 4: { Ch.name = "D"; break; }
                        case 5: { Ch.name = "E"; break; }
                        case 6: { Ch.name = "F"; break; }
                        case 7: { Ch.name = "G"; break; }

                    }

                }


            }
            if (aliveShipsCount == 0)
            {
                Ch.plaingtime++;
                chordActive(Ch);
            }

            //death cycle
            {
                foreach (var key in keys)
                {
                    deathcycle(key.ship, Ch);
                }

                
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            void shipdraw(Vector2 pos, ship shipclass)
            {
                if (!shipclass.isdead)
                {
                    Vector2 Lposition = position;
                    Lposition.Y = shipclass.Y;
                    Lposition.X = position.X;
                    spriteBatch.Draw(texship1, Lposition,
       new Rectangle(
           shipclass.currentFrame.X, shipclass.currentFrame.Y,
         shipclass.frameWidth, shipclass.frameHeight),
       Color.White, 0, Vector2.Zero,
       1, SpriteEffects.None, 0);
                    shipclass.currentFrame.X += shipclass.frameWidth;
                    if (shipclass.currentFrame.X == 108)
                    {
                        shipclass.currentFrame.X = 0;
                    }
                }
                shipclass.Y += speed;
            }//рисование корабля на позиции x=position.x y=Y

            void blackshipdraw(Vector2 pos, ship shipclass)
            {
                if (!shipclass.isdead)
                {
                    Vector2 Lposition = position;
                    Lposition.Y = shipclass.Y;
                    Lposition.X = position.X+28-10 ;
                    spriteBatch.Draw(texship1, Lposition,
       new Rectangle(
           shipclass.currentFrame.X, shipclass.currentFrame.Y,
         shipclass.frameWidth, shipclass.frameHeight),
       Color.Blue, 0, Vector2.Zero,
       1, SpriteEffects.None, 0);
                    shipclass.currentFrame.X += shipclass.frameWidth;
                    if (shipclass.currentFrame.X == 108)
                    {
                        shipclass.currentFrame.X = 0;
                    }
                }
                shipclass.Y += speed;
            }

            void drawexplouse(Vector2 position, ship ship)
            {
                Vector2 Lposition = position;
                Lposition.Y = ship.Y;
                Lposition.X = position.X;
                spriteBatch.Draw(explouse, Lposition,
   new Rectangle(
       exp.currentFrame.X, exp.currentFrame.Y,
     exp.frameWidth, exp.frameHeight),
   Color.White, 0, Vector2.Zero,
   2, SpriteEffects.None, 0);
                exp.currentFrame.X += exp.frameWidth;
                if (!exp.isplay) exp.currentFrame.X = 0; //тут менять

            } //рисование взрыва
            //рисование белой


            void laserdraw(Vector2 position)
            {
                Vector2 Lposition = position;
                Lposition.Y = position.Y - 3 * keys[0].frameHeight - 200;
                Lposition.X = position.X;
                spriteBatch.Draw(laser, Lposition,
   new Rectangle(
       lasersprite.currentFrame.X, lasersprite.currentFrame.Y,
     lasersprite.frameWidth, lasersprite.frameHeight),
   Color.White, 0, Vector2.Zero,
   LASERscale, SpriteEffects.None, 0);
                lasersprite.currentFrame.X += lasersprite.frameWidth;
                if (lasersprite.currentFrame.X > 96) lasersprite.currentFrame.X = 30; //тут менять

            }

            void laserdrawblue(Vector2 position)// для диезов онли
            {
                Vector2 Lposition = position;
                Lposition.Y = position.Y - 3 * keys[0].frameHeight - 200;
                Lposition.X = position.X -18;
                spriteBatch.Draw(laser, Lposition,
   new Rectangle(
       lasersprite.currentFrame.X, lasersprite.currentFrame.Y,
     lasersprite.frameWidth, lasersprite.frameHeight),
   Color.Blue, 0, Vector2.Zero,
   LASERscale, SpriteEffects.None, 0);
                lasersprite.currentFrame.X += lasersprite.frameWidth;
                if (lasersprite.currentFrame.X > 96) lasersprite.currentFrame.X = 30; //тут менять

            }

            void keydraw(Vector2 position, pk C1)
            {
                spriteBatch.Draw(whitekey, position,
                new Rectangle(
                    C1.currentFrame.X, C1.currentFrame.Y,
                  C1.frameWidth, C1.frameHeight),
                Color.White, 0, Vector2.Zero,
                scale, SpriteEffects.None, 0);
            }

            void blackkeydraw(Vector2 position, pk C1)
            {
                Vector2 pos = position;
                pos.X = position.X + 28;
                spriteBatch.Draw(whitekey, pos,
                new Rectangle(
                    C1.currentFrame.X, C1.currentFrame.Y,
                  C1.frameWidth, C1.frameHeight),
                Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);
            }
            //c1  
            // if (exp.isplay)
            //  {
            //    drawexplouse(position, ship1obj);
            //    exp.isplay = false;
            // }


          //  shipdraw(position, ship1obj);
           // blackshipdraw(position, ship1b);


            foreach (var key in keys)
            {
                if (key.isActive&&!key.isblack) laserdraw(position);
                if (!key.isblack)
                {
                    shipdraw(position, key.ship);
                    keydraw(position, key);
                }
                if (key.isActive&&key.isblack) laserdrawblue(position);
                if (key.isblack)
                {   
                    position.X -= plusX;
                    blackkeydraw(position, key);
                    blackshipdraw(position, key.ship);
                }
                position.X += plusX;
             
               
            }

            


            position.X = 10;


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
