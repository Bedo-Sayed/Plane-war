using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project1
{
    public class BackgroundBuilder
    {
        Background back;

        public BackgroundBuilder(Texture2D text)
        {
            back = new Background(text);
        }

        public BackgroundBuilder SetPos(Vector2 pos)
        {
            back.pos = pos;
            return this;
        }

        public BackgroundBuilder SetWidth(int width)
        {
            back.width = width;
            return this;
        }

        public BackgroundBuilder SetHeight(int height)
        {
            back.height = height;
            return this;
        }

        public BackgroundBuilder SetOffUp(float offUp)
        {
            back.offUp = offUp;
            return this;
        }

        public BackgroundBuilder SetOffDown(float offDown)
        {
            back.offDown = offDown;
            return this;
        }

        public BackgroundBuilder SetOffLeft(float offLeft)
        {
            back.offLeft = offLeft;
            return this;
        }

        public BackgroundBuilder SetOffRight(float offRight)
        {
            back.offRight = offRight;
            return this;
        }

        public BackgroundBuilder SetStX(int stX)
        {
            back.stX = stX;
            return this;
        }

        public BackgroundBuilder SetStY(int stY)
        {
            back.stY = stY;
            return this;
        }

        public BackgroundBuilder SetTakeX(int takeX)
        {
            back.takeX = takeX;
            return this;
        }

        public BackgroundBuilder SetTakeY(int takeY)
        {
            back.takeY = takeY;
            return this;
        }
        public BackgroundBuilder SetOpacity(float opacity)
        {
            back.opacity = opacity;
            return this;
        }

        public Background Build()
        {
            back.Build();
            return back;
        }
    }
}
