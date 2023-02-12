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
    public class Mission4 : GameState
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Player c;
        public static int w;
        public static int h;
        public static int frame = 0 , killedEnem;
        public static Texture2D expText;
        public static Matrix tran = Matrix.CreateTranslation(0, 0, 0f);
        public static ContentManager con;
        public static string s = "";

        List<Sprite> sprites;
        List<Bullet> bullets;

        public Mission4(ContentManager content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
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
                    int xx = i - 8, yy = j - 8, d = 0;
                    var text = con.Load<Texture2D>("clouds");

                    int r = Globals.GetRand(4);
                    if (r == 1)
                    {
                        d = 1;
                    }
                    else if (r == 2)
                    {
                        d = 1;
                    }
                    else if (r == 3)
                    {
                        text = con.Load<Texture2D>("heavy_clouds4");
                    }

                    Background back = new Background(text, new Vector2(1000 * xx, 1000 * yy), 1000, 1000, 0, 0, 0, 0, i * 256, j * 256);
                    if (d == 1) back.rotRad = 1.57f;
                    sprites.Add(back);
                }
            }
        }

        public void Reset()
        {
            sprites = new List<Sprite>();
            bullets = new List<Bullet>();
            Globals.toBeAdded = new List<Sprite>();

            frame = killedEnem = 0;
            Globals.enemyCnt = 0;

            w = graphics.PreferredBackBufferWidth;
            h = graphics.PreferredBackBufferHeight;

            Pre();

            var text = con.Load<Texture2D>("planes/plane2");
            c = new Player(text, new Vector2(0, 0), 100, 100, 0, 0, 0, 0, 30);
            c.bulletName = "bullet2";
            c.bulletSound = "fire_rocket";
            c.bulletColor = Color.Red;
            sprites.Add(c);

            HealthBar hlBar = new HealthBar(c);
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

            int cnt2 = Globals.GetRand(100000);
            if (Globals.enemyCnt < 30 && Main.cnt == 0 && cnt2 % 120 == 0)
            {
                Globals.enemyCnt++;

                Texture2D text = con.Load<Texture2D>("planes/plane5");

                Vector2 pos = new Vector2(0, 0);
                int r = Globals.GetRand(200), r2 = Globals.GetRand(2);

                if (r2 == 0) pos.X = c.pos.X + -750 - r;
                else pos.X = c.pos.X + 750 + r;

                r = Globals.GetRand(200); r2 = Globals.GetRand(2);
                if (r2 == 0) pos.Y = c.pos.Y - 450 - r;
                else pos.Y = c.pos.Y + 450 + r;

                Sprite sp = new Enemy4(text, pos, 100, 100, 0, 0, 0, 0);

                r = Globals.GetRand(4);
                if (r == 0) sp.color = Color.White;
                else if (r == 1) sp.color = Color.SlateGray;
                else if (r == 2) sp.color = Color.Silver;
                else sp.color = Color.Brown;

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
            Globals.device.Clear(Color.Cyan);

            spriteBatch.Begin(transformMatrix: tran);

            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Draw(spriteBatch, false);
            }


            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch, false);
            }

            if (frame > 240)
            {
                s = "Score: " + killedEnem.ToString();
                spriteBatch.DrawString(Globals.font, s, new Vector2(c.pos.X - 700, c.pos.Y - 400), Color.Black, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.DrawString(Globals.font, s, c.pos, Color.Black, 0, new Vector2(0, 0), 3f, SpriteEffects.None, 1);
            }

            spriteBatch.End();
        }
    }
}