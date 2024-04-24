using System;
using System.ComponentModel.Design;
namespace Player
{
    class PlayerCar
    {
        public int pos_x { get; set; }
        public int pos_y { get; set; }

    }

}

namespace LevelManager
{
    class LevelCreator
    {
        public int[][] Create_Level(int x, int y)
        {
            int[][] level_map = new int[x][];
            for (int row_no = 0; row_no < x; row_no++)
            {
                level_map[row_no] = new int[y];

                for (int index_in_row = 0; index_in_row < y; index_in_row++)
                {

                    if (index_in_row == 0 || index_in_row == (y - 1))
                    {
                        level_map[row_no][index_in_row] = 1;

                    }
                    else
                    {

                        level_map[row_no][index_in_row] = 0;

                    }

                }
            }
            return level_map;
        }

    }

    class LevelDisplay
    {
        public string Display_level(int[][] level)
        {
            string level_display = "";
            for (int row_no = 0; row_no < level.Length; row_no++)
            {
                for (int index_in_row = 0; index_in_row < level[row_no].Length; index_in_row++)
                {
                    if (level[row_no][index_in_row] == 0)
                    {

                        level_display += " ";

                    }
                    else if (level[row_no][index_in_row] == 1)
                    {

                        level_display += "#";

                    }
                    else if (level[row_no][index_in_row] == 2)
                    {
                        level_display += "*";
                    }
                }
                level_display += Environment.NewLine;
            }

            return level_display;
        }



        public int[][] Display_object(int[][] level, int obj_posx, int obj_posy, string obj_type)
        {
            level[obj_posx][obj_posy] = 2;
            if (obj_type == "car")
            {
                level[obj_posx][obj_posy] = 2;
                level[obj_posx][obj_posy + 1] = 2;
                level[obj_posx][obj_posy - 1] = 2;
                level[obj_posx - 1][obj_posy] = 2;
                level[obj_posx - 2][obj_posy] = 2;
                level[obj_posx - 1][obj_posy + 1] = 2;
                level[obj_posx - 1][obj_posy - 1] = 2;
            }
            return level;
        }
    }
}

namespace GameRun
{
    using LevelManager;
    using Player;
    class Test
    {
        static void Main()
        {
            LevelCreator LevelBuilder = new LevelCreator();
            int [][] level1 = LevelBuilder.Create_Level(12, 12);
            PlayerCar Player = new PlayerCar();
            Player.pos_x = 11;
            Player.pos_y = 5;
            

            LevelDisplay LevelRenderer = new LevelDisplay();
            LevelRenderer.Display_object(level1, Player.pos_x, Player.pos_y, "car");
            Console.WriteLine(LevelRenderer.Display_level(level1));
        }
    }
}
