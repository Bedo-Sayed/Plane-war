using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace Project1
{
    public class Enemy : Plane
    {
        public Enemy(Texture2D texture, Vector2 pos, int width, int height, float ofUp, float ofDw, float ofLf, float ofRt)
            : base(texture, pos, width, height, ofUp, ofDw, ofLf, ofRt)
        {
            vel = 3;
            frame = 0;
            lifeTime = 300;
            idle = true;
            rec = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }

        protected Friend GetRandFriend(List<Sprite> sprites)
        {
            int cnt = 0;
            for(int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] is Friend)
                    cnt++;
            }

            if (cnt == 0)
                return null;

            int r = Globals.GetRand(cnt);

            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] is Friend)
                {
                    if (r == 0)
                    {
                        return (Friend)sprites[i];
                    }
                    else r--;
                }
            }
            return null;
        }

        protected Player GetPlayer(List<Sprite> sprites)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] is Player)
                    return (Player)sprites[i];
            }
            return null;
        }
    }
}
