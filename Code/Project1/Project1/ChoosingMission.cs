using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class ChoosingMission : GameState
    {
        SpriteBatch spriteBatch;
        List<Button> btnList;
        Texture2D background;

        public ChoosingMission(ContentManager content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            Pre(content, spriteBatch, graphics);
        }

        private void Pre(ContentManager content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            this.spriteBatch = spriteBatch;
            var text = Globals.font;
            btnList = new List<Button>();

            Button btn = new Button(new Vector2(25, 350));
            btn.rec.Width = 210;
            btn.rec.Height = 200;
            btn.s = "     Mission 1";
            btn.description = "\n\n Protect the big spaceship\n until reaches the earth planet.\n" +
                " the enemies attack you and\n attack the big plane.";
            btnList.Add(btn);

            btn = new Button(new Vector2(325, 350));
            btn.rec.Width = 210;
            btn.rec.Height = 200;
            btn.s = "     Mission 2";
            btn.description = "\n\n Destroy the big plane before\n reaches the flag.\n" +
                " the enemies attack you. the\n bullets of enemies only affect\n you. If you collide with " +
                "an\n enemy or a big plane you will\n die.";
            btnList.Add(btn);

            btn = new Button(new Vector2(625, 350));
            btn.rec.Width = 210;
            btn.rec.Height = 200;
            btn.s = "     Mission 3";
            btn.description = "\n\n Fight with your team against\n the enemies team and\n destroy the ice planet.\n Enemies are the gray" +
                " team.";
            btnList.Add(btn);

            btn = new Button(new Vector2(925, 350));
            btn.rec.Width = 210;
            btn.rec.Height = 200;
            btn.s = "     Mission 4";
            btn.description = "\n\n Just free playing. Kill as\n enemies as you can to\n " +
                "achieve higher score.";
            btnList.Add(btn);

            background = content.Load<Texture2D>("back_main_menu");
        }
        public override void Update(GameTime gameTime)
        {
            foreach (Button btn in btnList)
            {
                btn.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, 1200, 680), Color.White);
            foreach (Button btn in btnList)
            {
                btn.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
