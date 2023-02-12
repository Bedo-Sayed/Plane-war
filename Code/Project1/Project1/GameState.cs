using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public abstract class GameState
    {
        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    }
}
