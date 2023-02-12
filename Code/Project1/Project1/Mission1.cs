using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Project1
{
    public class Mission1 : GameState
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
        public static Sprite earth;
        public Sprite myFriend;
        DateTime started;

        List<Sprite> sprites;
        List<Bullet> bullets;


        public Mission1(ContentManager content , SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
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
                    int xx = i - 8, yy = j - 8;
                    var text = con.Load<Texture2D>("stars");

                    Background back = new Background(text, new Vector2(1000 * xx, 1000 * yy), 1000, 1000, 0, 0, 0, 0, i * 256, j * 256);
                    sprites.Add(back);
                }
            }

            for (int i = 0; i < 100; i++)
            {
                Random rnd = new Random();
                int xx = rnd.Next(16) - 8, yy = rnd.Next(16) - 8;
                var text2 = con.Load<Texture2D>("rock");

                int r = rnd.Next(200) + 1;
                Background back2 = new BackgroundBuilder(text2).SetPos(new Vector2(500 * xx, 500 * yy))
                    .SetHeight(r).SetWidth(r).SetOpacity(0.2f).Build();

                back2.recFromPic = new Rectangle(0, 0, 2483, 2077);

                sprites.Add(back2);
            }

            earth = new BackgroundBuilder(con.Load<Texture2D>("earth_planet")).SetPos(new Vector2(6500, 6500))
                .SetHeight(600).SetWidth(600).SetOpacity(0.6f).Build();
            earth.org = new Vector2(earth.texture.Width / 2f, earth.texture.Height / 2f);

            sprites.Add(earth);
        }
        
        public void Reset()
        {
            started = DateTime.Now;

            sprites = new List<Sprite>();
            bullets = new List<Bullet>();
            Globals.toBeAdded = new List<Sprite>();

            frame = 0;
            Globals.enemyCnt = 0;

            w = graphics.PreferredBackBufferWidth;
            h = graphics.PreferredBackBufferHeight;

            Pre();

            var text = con.Load<Texture2D>("planes/plane11");
            c = new Player(text, new Vector2(-7200, -7200), 100, 100, 0, 0, 0, 0, 30);
            c.bulletName = "1";
            c.bulletSound = "light_bullet";
            sprites.Add(c);

            text = con.Load<Texture2D>("planes/plane12");
            Sprite tmp = new Friend(text, new Vector2(-7000, -7400), 300, 300, 0, 0, 0, 0, 200);
            tmp.vel = 1;
            sprites.Add(tmp);
            myFriend = tmp;

            HealthBar hlBar = new HealthBar(c);
            sprites.Add(hlBar);

            hlBar = new HealthBar((Friend)tmp);
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

            s = "";
            //s = c.pos.X.ToString() + " " + (DateTime.Now - started).ToString();

            int cnt2 = Globals.GetRand(100000);
            if (Globals.enemyCnt < 30 && Main.cnt == 0 && cnt2 % 120 == 0)
            {
                Globals.enemyCnt++;

                Texture2D text = con.Load<Texture2D>("planes/plane13");

                Vector2 pos = new Vector2(0, 0);
                int r = Globals.GetRand(200), r2 = Globals.GetRand(2);

                if (r2 == 0) pos.X = myFriend.pos.X + -750 - r;
                else pos.X = myFriend.pos.X + 750 + r;

                r = Globals.GetRand(200); r2 = Globals.GetRand(2);
                if (r2 == 0) pos.Y = myFriend.pos.Y - 450 - r;
                else pos.Y = myFriend.pos.Y + 450 + r;

                Sprite sp = new Enemy1(text, pos, 100, 100, 0, 0, 0, 0);
             
                r = Globals.GetRand(4);
                if (r == 0) sp.color = Color.White;
                else if (r == 1) sp.color = Color.SlateGray;
                else if (r == 2) sp.color = Color.Silver;
                else sp.color = Color.Red;

                sp.vel = 3;
                sprites.Add(sp);
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
            Globals.device.Clear(Color.Black);

            spriteBatch.Begin(transformMatrix: tran);


            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Draw(spriteBatch, false);
            }


            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch, false);
            }

            //spriteBatch.DrawString(Globals.font, s, c.pos, Color.White);
            spriteBatch.DrawString(Globals.font, s, c.pos, Color.White, 0, new Vector2(0, 0), 3f, SpriteEffects.None, 1);

            spriteBatch.End();
        }
    }
}