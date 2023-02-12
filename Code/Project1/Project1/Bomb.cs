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
    public class Bomb : Sprite
    {
        SoundEffect snd;
        Plane parent;
        float explosionTime;
        public Bomb(SoundEffect snd , Texture2D text , Plane parent , float expTime , Vector2 pos) 
        {
            this.snd = snd;
            this.parent = parent;
            explosionTime = expTime;
            this.pos = pos;
            height = width = 100;
            height = 50;
            texture = text;
            vel = 2;
            rotRad = this.parent.rotRad;
            org = new Vector2(text.Width / 2f, text.Height / 2f);
            Build();
            RotatePoly(rotRad, false);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Bullet> bullets, ref int idx)
        {
            explosionTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (explosionTime <= 0f)
            {
                Explosion2 exp = new Explosion2(pos, 600, 600);
                Globals.toBeAdded.Add(exp);
                sprites.RemoveAt(idx);
                idx--;

                for(int i = 0; i < sprites.Count; i++)
                {
                    if ( (parent is Friend && sprites[i] is Enemy) || (parent is Enemy && sprites[i] is Friend) )
                    {
                        if (Globals.DsSq(pos , sprites[i].pos) <= 90000)
                        {
                            ((Plane)sprites[i]).health -= 20;
                        }
                    }
                }
            }

            pos = ChangePos(1);
            UpdatePoly(1);
            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, bool drawPol = false)
        {
            //Rectangle rec2 = new Rectangle(0, 0, texture.Width, texture.Height); //mloosh lazmla
            spriteBatch.Draw(texture, rec, null, Color.White, rotRad, org, SpriteEffects.None, 0f);

            if (drawPol == true)
            {
                for (int i = 0; i < pol.Count; i++)
                {
                    Rectangle recc = new Rectangle((int)pol[i].X, (int)pol[i].Y, 5, 5);
                    spriteBatch.Draw(Globals.red, recc, Color.White);
                }
            }
        }
    }
}
