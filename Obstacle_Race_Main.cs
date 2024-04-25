using LevelManager;
using System;
using System.ComponentModel.Design;
namespace Player
{
    class PlayerCar
    {
        public int pos_x { get; set; }
        public int pos_y { get; set; }
        public string type { get; set; }

    }

}

namespace Obstacle
{
    class ObstacleCar
    {
        public int pos_x { get; set; }
        public int pos_y { get; set; }

        public string type { get; set; }
    }
}

namespace Controler
{
    class CheckPositions
    {
        public static bool Check_position(int[][] level, int posx, int posy, string direction)
        {
            bool empty = false;

            try
            {
                switch (direction)
                {
                    // Check if the space where we want to move is empty. 
                    case "right":
                        if (posy + 1 < level[posx].Length && level[posx][posy + 1] == 0)
                        {
                            empty = true;
                        }
                        break;
                    case "left":
                        if (0 < posy - 1 && level[posx][posy - 1] == 0)
                        {
                            empty = true;
                        }
                        break;
                    case "down":
                        if (posx + 1 < level.Length && level[posx + 1][posy] == 0)
                        {
                            empty = true;
                        }
                        break;
                }
                return empty;
            } 
            catch (IndexOutOfRangeException e)
            {
                Console.Write(e.Message);
                throw new ArgumentOutOfRangeException("Wrong size of the level or wrong position of object!", e);
            }
        }

    }

    class PlayerControl
    {
        // Player movement.
        public static (int, int) Move_player(int[][] level, int posx, int posy, string key)
        {
            switch (key)
            {
                case "a":
                    if (CheckPositions.Check_position(level, posx, (posy-1), "left"))
                    {
               
                        posy +=  (-1);
                    }
                    break;
                case "d":
                    if (CheckPositions.Check_position(level, posx, (posy+1), "right"))
                    {
                        posy += 1;
                    }
                    break;
            }
            
            return (posx, posy);
        }
    }

}

namespace LevelManager
{
    class LevelCreator
    {
        // Create array as a level map.
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
        // Create a string out of array of integers to represent the game map.
        public string Display_level(int[][] level)
        {
            string level_display = "";
            for (int row_no = 0; row_no < level.Length; row_no++)
            {
                for (int index_in_row = 0; index_in_row < level[row_no].Length; index_in_row++)
                {
                    // 0 = Road.
                    if (level[row_no][index_in_row] == 0)
                    {

                        level_display += " ";

                    }
                    // 1 = Side of the road.
                    else if (level[row_no][index_in_row] == 1)
                    {

                        level_display += "#";

                    }
                    // 2 = Car object.
                    else if (level[row_no][index_in_row] == 2)
                    {
                        level_display += "*";
                    }
                }
                level_display += Environment.NewLine;
            }

            return level_display;
        }


        // Show objects on the map. Different objects can have different id_number so that they are
        // displayed with different symbols.
        public int[][] Display_object(int[][] level, int obj_posx, int obj_posy, string obj_type)
        {
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

        // Clear objects from the level.
        public int[][] Clear_object(int[][] level, int obj_posx, int obj_posy, string obj_type)
        {
            if (obj_type == "car")
            {
                level[obj_posx][obj_posy] = 0;
                level[obj_posx][obj_posy + 1] = 0;
                level[obj_posx][obj_posy - 1] = 0;
                level[obj_posx - 1][obj_posy] = 0;
                level[obj_posx - 2][obj_posy] = 0;
                level[obj_posx - 1][obj_posy + 1] = 0;
                level[obj_posx - 1][obj_posy - 1] = 0;
            }
            return level;
        }
    }
}

namespace GameRun
{
    using LevelManager;
    using Player;
    using Controler;
    class Test
    {
        static void Main()
        {
            LevelCreator LevelBuilder = new LevelCreator();
            int [][] level1 = LevelBuilder.Create_Level(12, 12);
            PlayerCar Player = new PlayerCar();
            Player.pos_x = 11;
            Player.pos_y = 9;
            Player.type = "car";

            LevelDisplay LevelRenderer = new LevelDisplay();
            

            while (true)
            {
                // Draw and show the level map.
                level1 = LevelRenderer.Display_object(level1, Player.pos_x, Player.pos_y, Player.type);
                Console.WriteLine(LevelRenderer.Display_level(level1));

                // Clear out map before redrawing it.
                level1 = LevelRenderer.Clear_object(level1, Player.pos_x, Player.pos_y, Player.type);

                // Player controls.
                string move_key = Console.ReadLine();
                var player_new_position = PlayerControl.Move_player(level1, Player.pos_x, Player.pos_y, move_key);
                Player.pos_x = player_new_position.Item1;
                Player.pos_y = player_new_position.Item2;
            }
        }
    }
}
