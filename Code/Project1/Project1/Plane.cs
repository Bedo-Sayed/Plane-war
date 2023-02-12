using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Net.Security;
//using System.Numerics;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace Project1
{
    public class Plane : Sprite
    {
        public int health;
        public int startingHealth;
        public int frame;
        public double lifeTime;
        public Vector2 lastMove;
        protected bool idle;
        public Plane(Texture2D texture, Vector2 pos, int width, int height, float ofUp, float ofDw, float ofLf, float ofRt)
            : base(texture, pos, width, height, ofUp, ofDw, ofLf, ofRt)
        {
            lifeTime = 100000;
            health = 100000;
            idle = true;
            vel = 3;
        }

        protected void KillMe(List<Sprite> sprites , List<Bullet> bullets, ref int idx , SoundEffect snd)
        {
            Sprite exp;
            exp = new Explosion(snd, Globals.expText, sprites[idx].pos, width, height, 0.2f, 0.2f, 0.2f, 0.2f , 5 , 5);
            sprites.RemoveAt(idx);
            idx--;
            Globals.toBeAdded.Add(exp);
        }
    }
}
