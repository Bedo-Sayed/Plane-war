using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Audio;

namespace Project1
{
    public class Player : Friend
    {  
        int rotDeg;
        bool rel , relRt;
        public Color bulletColor;
        public string bulletName, bulletSound;

        public Player(Texture2D texture, Vector2 pos, int width, int height , float ofUp , float ofDw , float ofLf , float ofRt , int hl)
            : base(texture, pos, width, height , ofUp, ofDw, ofLf, ofRt , hl)
        {
            rel = relRt = true;
            health = startingHealth = hl;
            org = new Vector2(texture.Width / 2f, texture.Height / 2f);
            vel = 5;
            rotVel = 2;
            bulletColor = Color.White;
        }
        void add()
        {
            RotatePoly(rotVel);

            totRot += rotVel;
            rotDeg = (rotDeg + (int)rotVel)%360;
        }

        void sub()
        {
            RotatePoly(-rotVel);

            totRot -= rotVel;
            rotDeg = (rotDeg - (int)rotVel + 360)%360;
        }

        public override void Update(GameTime gameTime , List<Sprite> sprites , List<Bullet> bullets, ref int idx)
        {
            lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
            frame++;
            lastMove = new Vector2(0, 0);

            if ( (lifeTime <= 0 || health <= 0) && Main.cnt == 0)
            {
                Main.cnt = 600;
                Globals.stateSnd = Globals.lose;
                Globals.backSnd.Stop();
                KillMe(sprites, bullets, ref idx, Globals.exp);
                for (int i = 0; i < sprites.Count; i++)
                {
                    if (sprites[i] is Friend)
                        ((Friend)sprites[i]).health = 0;
                }
                return;
            }

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && rel == true)
            {
                GenerateBullet(bulletName, bulletSound, bullets, 100, 30, 1, Globals.soundVol);
                bullets[bullets.Count - 1].color = bulletColor;
                rel = false;                
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                rel = true;
            }
            
            /*
            if (mouseState.RightButton == ButtonState.Pressed && relRt == true)
            {
                relRt = false;
                SoundEffect snd = Globals.con.Load<SoundEffect>("sounds/long_explosion");
                Texture2D text = Globals.con.Load<Texture2D>("rockets/nuclear");
                Bomb bmb = new Bomb(snd , text , this , 3 , pos);
                sprites.Add(bmb);
            }
            else if (mouseState.RightButton == ButtonState.Released)
            {
                relRt = true;
            }
            */

            KeyboardState state = Keyboard.GetState();
            bool forward = false, backward = false;

            if (state.IsKeyDown(Keys.A))
                sub();
            else if (state.IsKeyDown(Keys.D))
                add();
            if (state.IsKeyDown(Keys.W))
            {
                lastMove = ChangePos(1) - pos;
                pos += lastMove;
                forward = true;
            }
            else if (state.IsKeyDown(Keys.S))
            {
                lastMove = ChangePos(-1) - pos;
                pos += lastMove;
                backward = true;
            }

            if (forward || backward)
            {
                if (forward)
                {
                    UpdatePoly(1);
                }
                else
                {
                    UpdatePoly(-1);
                }

                rec.X = (int)pos.X;
                rec.Y = (int)pos.Y;
            }
            rotRad = MathHelper.ToRadians(rotDeg);

            Globals.camX = pos.X;
            Globals.camY = pos.Y;


            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] is not Enemy) continue;
                if (Collision(this, sprites[i]))
                {
                    health -= 50;
                    return;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch , bool drawPol = false)
        {
            Rectangle rec2 = new Rectangle(0, 0, texture.Width, texture.Height); //mloosh lazmla
            spriteBatch.Draw(texture, rec , rec2, Color.White, rotRad, org, SpriteEffects.None, 0f);

            if (drawPol == true)
            {
                for(int i=0; i<pol.Count; i++)
                {
                    Rectangle recc = new Rectangle((int)pol[i].X, (int)pol[i].Y, 5, 5);
                    spriteBatch.Draw(Globals.red , recc , Color.White);
                }
            }
        }
    }
}