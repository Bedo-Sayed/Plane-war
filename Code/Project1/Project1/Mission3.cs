using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Project1
{
    public class Mission3 : GameState
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Player c;
        public static Sprite red;
        public static int w;
        public static int h;
        public static int frame = 0;
        public static string s = "";
        public static Texture2D expText;
        public static Matrix tran = Matrix.CreateTranslation(0, 0, 0f);
        public static ContentManager con;
        public static Sprite icePlanet;
        public Sprite myFriend;

        List<Sprite> sprites;
        List<Bullet> bullets;

        public Mission3(ContentManager content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            con = content;
            this.spriteBatch = spriteBatch;
            this.graphics = graphics;

            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 680;
            graphics.ApplyChanges();

            Reset();
        }

        public void Pre()
        {
            expText = con.Load<Texture2D>("pngegg");

            Globals.topLeft = new Vector2(-w / 4f, -h / 4f);
            Globals.botRight = new Vector2(w / 4f, h / 4f);

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    int xx = i - 8, yy = j - 8 , d = 0;
                    var text = con.Load<Texture2D>("heavy_clouds");
                    Rectangle recFromPic = new Rectangle();
                    if (Globals.GetRand(2) == 1)
                    {
                        text = con.Load<Texture2D>("heavy_clouds4");
                        recFromPic = new Rectangle(50, 100, 700, 680);
                        d = 1;
                    }

                    Background back = new Background(text, new Vector2(1000 * xx, 1000 * yy), 1000, 1000, 0, 0, 0, 0, i * 256, j * 256);
                    sprites.Add(back);


                    if (d == 1)
                    {
                        back.recFromPic = recFromPic;
                        back.width = 2500;
                        back.height = 2500;
                    }
                }
            }
        }

        public void Reset()
        {
            sprites = new List<Sprite>();
            bullets = new List<Bullet>();
            Globals.toBeAdded = new List<Sprite>();

            frame = 0;
            Globals.enemyCnt = Globals.friendCnt = 0;

            w = graphics.PreferredBackBufferWidth;
            h = graphics.PreferredBackBufferHeight;

            Pre();


            icePlanet = new EnemyBoss3(con.Load<Texture2D>("ice_planet"), new Vector2(500, 300), 500, 500
                , 0, 0, 0, 0, 500);
            sprites.Add(icePlanet);

            var text = con.Load<Texture2D>("planes/plane10");
            c = new Player(text, new Vector2(0, 0), 100, 100, 0, 0, 0, 0, 30);
            c.bulletName = "bullet2";
            c.bulletSound = "fire_rocket";
            c.bulletColor = Color.DarkRed;
            sprites.Add(c);

            HealthBar hlBar = new HealthBar(c);
            sprites.Add(hlBar);

            hlBar = new HealthBar((EnemyBoss3)icePlanet);
            sprites.Add(hlBar);

            Globals.camX = c.pos.X;
            Globals.camY = c.pos.Y;
            SetCamera(ref tran);

            Globals.backgroundSound = con.Load<SoundEffect>("sounds/three_two_one_fight");
            Globals.backSnd = Globals.backgroundSound.CreateInstance();
            Globals.backSnd.Play();
        }

        public override void Update(GameTime gameTime)
        {
            frame++;
            if (frame < 240)
            {
                int tmp = frame / 60 + 1;
                if (tmp < 4)
                {
                    tmp = 4 - tmp;
                    s = tmp.ToString();
                }
                else s = "Go";
                return;
            }

            if (frame == 240)
            {
                Globals.backgroundSound = con.Load<SoundEffect>("sounds/smooth_cold_wind");
                Globals.backSnd = Globals.backgroundSound.CreateInstance();
                Globals.backSnd.Play();
            }

            s = "";

            if (Globals.backSnd.State == SoundState.Stopped && Main.cnt == 0)
                Globals.backSnd.Play();

            int cnt = Globals.GetRand(100000);
            if (cnt % 120 == 0 )
            {
                if (Globals.enemyCnt < 21)
                {
                    Globals.enemyCnt++;
                    Texture2D text = con.Load<Texture2D>("planes/plane14");
                   
                    Vector2 pos = new Vector2(0, 0);
                    int r = Globals.GetRand(200), r2 = Globals.GetRand(2);

                    if (r2 == 0) pos.X = c.pos.X + -750 - r;
                    else pos.X = c.pos.X + 750 + r;

                    r = Globals.GetRand(200); r2 = Globals.GetRand(2);
                    if (r2 == 0) pos.Y = c.pos.Y - 450 - r;
                    else pos.Y = c.pos.Y + 450 + r;

                    
                    Sprite sp = new Enemy3(text, pos, 100, 100, 0, 0, 0, 0 , 1);
                    
                    sp.vel = 5;
                    
                    sprites.Add(sp);
                    
                }
                if (Globals.friendCnt < 5)
                {
                    Globals.friendCnt++;
                    Texture2D text = con.Load<Texture2D>("planes/plane15");

                    Vector2 pos = new Vector2(0, 0);
                    int r = Globals.GetRand(200), r2 = Globals.GetRand(2);

                    if (r2 == 0) pos.X = c.pos.X + -750 - r;
                    else pos.X = c.pos.X + 750 + r;

                    r = Globals.GetRand(200); r2 = Globals.GetRand(2);
                    if (r2 == 0) pos.Y = c.pos.Y - 450 - r;
                    else pos.Y = c.pos.Y + 450 + r;

                    Sprite sp = new Friend3(text, pos, 100, 100, 0, 0, 0, 0 , 1);
                    sp.color = Color.Brown;

                    sp.vel = 3;
                    sprites.Add(sp);
                }
            }


            Globals.toBeAdded.Clear();
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(gameTime, sprites, bullets, ref i);
            }

            bool found = false;
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Update(gameTime, sprites, bullets, ref i);

                if (sprites[i] is Player)
                    found = true;
            }

            for (int i = 0; i < Globals.toBeAdded.Count; i++)
            {
                Sprite sp = Globals.toBeAdded[i];
                sprites.Add(sp);
            }

            SetCamera(ref tran);

            found = true;
            if (!found)
                Reset();
        }
        public static void SetCamera(ref Matrix mat)
        {
            mat = Matrix.CreateTranslation(-Globals.camX, -Globals.camY, 0f);
            mat *= Matrix.CreateScale(0.8f, 0.8f, 0);
            mat *= Matrix.CreateTranslation(w / 2f, h / 2f, 0);
        }
        public override void Draw(GameTime gameTime)
        {
            Globals.device.Clear(Color.WhiteSmoke);

            spriteBatch.Begin(transformMatrix: tran);

            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Draw(spriteBatch, false);
            }


            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch, false);
            }

            //s = Globals.friendCnt.ToString() + " " + Globals.enemyCnt.ToString();
            //spriteBatch.DrawString(Globals.font, s, c.pos, Color.Black);
            spriteBatch.DrawString(Globals.font, s, c.pos, Color.Black, 0, new Vector2(0, 0), 3f, SpriteEffects.None, 1);

            spriteBatch.End();
        }

    }
}
