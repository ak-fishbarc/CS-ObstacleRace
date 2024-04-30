using LevelManager;
using Player;
using Controler;
using System;
using System.ComponentModel.Design;
using Obstacle;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;


namespace Player
{
    class PlayerCar
    {
        public int pos_x { get; set; }
        public int pos_y { get; set; }
        public string? type { get; set; }
    }

}

namespace Obstacle
{
    class ObstacleCar
    {
        public int pos_x { get; set; }
        public int pos_y { get; set; }

        public string? type { get; set; }
    }
}

namespace Controler
{
    class CheckPositions
    {
        /* 
           Check for an object in the given direction. 0 = Empty space, 1 = Walls 2 = Obstacle 
           When checking Left or Right on Car objects, add/subtract 1 from position y. That's due to how objects are displayed.
           As positions x and y are a single point and the car is displayed as   *
                                                                                ***
                                                                                ***
                                                                                    object, if you check directly from car's posx and posy
           it'll give you wrong answer. Also, when checking down, you don't need to add/subtract anything extra. When checking up it might need
           more than one check.
           This could be resolved by simply storing information about how many positions the object takes. Each object can store an array of tuples
           with coordinates. In collision detection we could run detection from each of the outer coordinates to see if the object is touching anything.
           I might change that later to give myself some extra refactoring work, but for now I'm trying to keep with this method for practice.
        */
        public static bool Check_position(int[][] level, int posx, int posy, string direction, int check_for)
        {
            bool empty = false;

            try
            {
                switch (direction)
                {
                    // Check if the space where we want to move is empty. 
                    case "right":
                        if (posy + 1 < level[posx].Length && level[posx][posy + 1] == check_for)
                        {
                            empty = true;
                        }
                        break;
                    case "left":
                        if (0 < posy - 1 && level[posx][posy - 1] == check_for)
                        {
                            empty = true;
                        }
                        break;
                    case "down":
                        if (posx + 1 < level.Length && level[posx + 1][posy] == check_for)
                        { 
                            empty = true;
                        }
                        break;
                    case "up":
                        if (0 <= posx - 1 && level[posx - 1][posy] == check_for)
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
        public static (int, int) Move_player(int[][] level, int posx, int posy, string? key)
        {
            switch (key)
            {
                case "a":
                    if (CheckPositions.Check_position(level, posx, (posy-1), "left", 0))
                    {
                        posy +=  (-1);
                    }
                    break;
                case "d":
                    if (CheckPositions.Check_position(level, posx, posy+1, "right", 0))
                    {
                        posy += 1;
                    }
                    break;
            }
            
            return (posx, posy);
        }
    }

    class ObstacleControl
    {
        // Obstacle car's movement.
        public static (int, int) Move_obstacle(int[][] level, int posx, int posy)
        {
            if (CheckPositions.Check_position(level, posx, posy, "down", 0))
            {            
                posx += 1;
            }

            return (posx, posy);         
        }
    }

    // Create an illusion that the Obstacle Car is disappearing at the bottom of the map.
    class BorderControl
    {
        public string Disappear_object(int[][] level, int posx, string obj_type)
        {
            if (posx + 1 == level.Length && obj_type == "car")
            {
                return "disappearing_car_1";
            }
            else if (posx + 1 == level.Length && obj_type == "disappearing_car_1")
            {
                return "disappearing_car_2";
            }
            else
            {
                return obj_type;
            }
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
                    if (index_in_row == 0 || index_in_row == (y-1))
                    {
                        level_map[row_no][index_in_row] = 1;
                    }
                    else if (index_in_row > 0 && index_in_row < (y-1))
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
                if (level[obj_posx][obj_posy] == 0)
                {
                    level[obj_posx][obj_posy] = 2;
                    if (CheckPositions.Check_position(level, obj_posx, obj_posy, "left", 0) && CheckPositions.Check_position(level, obj_posx, obj_posy, "right", 0))
                    {
                        level[obj_posx][obj_posy + 1] = 2;
                        level[obj_posx][obj_posy - 1] = 2;
                    }
                }
                if (CheckPositions.Check_position(level, obj_posx, obj_posy, "up", 0))
                {
                    level[obj_posx - 1][obj_posy] = 2;
                    if (CheckPositions.Check_position(level, (obj_posx - 1), obj_posy, "left", 0) && CheckPositions.Check_position(level, (obj_posx - 1), obj_posy, "right", 0))
                    {
                        level[obj_posx - 1][obj_posy + 1] = 2;
                        level[obj_posx - 1][obj_posy - 1] = 2;
                    }
                }
                if (CheckPositions.Check_position(level, obj_posx - 1, obj_posy, "up", 0))
                {
                    level[obj_posx - 2][obj_posy] = 2;
                }
            }
            else if (obj_type == "disappearing_car_1")
            {
                if (CheckPositions.Check_position(level, obj_posx, obj_posy, "up", 0))
                {
                    level[obj_posx][obj_posy] = 2;
                    if (CheckPositions.Check_position(level, (obj_posx - 1), obj_posy, "left", 0) && CheckPositions.Check_position(level, (obj_posx - 1), obj_posy, "right", 0))
                    {
                        level[obj_posx][obj_posy + 1] = 2;
                        level[obj_posx][obj_posy - 1] = 2;
                    }
                }
                if (CheckPositions.Check_position(level, obj_posx, obj_posy, "up", 0))
                {
                    level[obj_posx - 1][obj_posy] = 2;
                }
            }
            else if (obj_type == "disappearing_car_2")
            {
                level[obj_posx][obj_posy] = 2;
            }

            return level;
        }

        // Clear objects from the level. Needs improvement as this is a quick fix; I had no time to work on this more.
        public int[][] Clear_object(int[][] level, int obj_posx, int obj_posy, string obj_type)
        {
            if (obj_type == "car")
            {
                if (level[obj_posx][obj_posy] == 2)
                {
                    level[obj_posx][obj_posy] = 0;
                    if (CheckPositions.Check_position(level, obj_posx, obj_posy, "left", 2) && CheckPositions.Check_position(level, obj_posx, obj_posy, "right", 2))
                    {
                        level[obj_posx][obj_posy + 1] = 0;
                        level[obj_posx][obj_posy - 1] = 0;
                    }
                }
                if (CheckPositions.Check_position(level, obj_posx, obj_posy, "up", 2))
                {
                    level[obj_posx - 1][obj_posy] = 0;
                    if (CheckPositions.Check_position(level, (obj_posx - 1), obj_posy, "left", 2) && CheckPositions.Check_position(level, (obj_posx - 1), obj_posy, "right", 2))
                    {
                        level[obj_posx - 1][obj_posy + 1] = 0;
                        level[obj_posx - 1][obj_posy - 1] = 0;
                    }
                }
                if (CheckPositions.Check_position(level, obj_posx - 1, obj_posy, "up", 2))
                {
                    level[obj_posx - 2][obj_posy] = 0;
                }
            }
            else if (obj_type == "disappearing_car_1")
            {
                if (level[obj_posx][obj_posy] == 2)
                {
                    level[obj_posx][obj_posy] = 0;
                    if (CheckPositions.Check_position(level, obj_posx, obj_posy, "left", 2) && CheckPositions.Check_position(level, obj_posx, obj_posy, "right", 2))
                    {
                        level[obj_posx][obj_posy + 1] = 0;
                        level[obj_posx][obj_posy - 1] = 0;
                    }
                }
                if (CheckPositions.Check_position(level, obj_posx, obj_posy, "up", 2))
                {
                    level[obj_posx - 1][obj_posy] = 0;
                    if (CheckPositions.Check_position(level, (obj_posx - 1), obj_posy, "left", 2) && CheckPositions.Check_position(level, (obj_posx - 1), obj_posy, "right", 2))
                    {
                        level[obj_posx - 1][obj_posy + 1] = 0;
                        level[obj_posx - 1][obj_posy - 1] = 0;
                    }
                }
                if (CheckPositions.Check_position(level, obj_posx - 1, obj_posy, "up", 2))
                {
                    level[obj_posx - 2][obj_posy] = 0;
                }
            }
            else if (obj_type == "disappearing_car_2")
            {
                level[obj_posx][obj_posy] = 0;
            }
            return level;
        }
    }
}

namespace GameRun
{

    class Test
    {
        static void Main()
        {
            LevelCreator LevelBuilder = new LevelCreator();
            int [][] level1 = LevelBuilder.Create_Level(12, 12);
            PlayerCar Player = new PlayerCar();
            Player.pos_x = 11;
            Player.pos_y = 5;
            Player.type = "car";

            ObstacleCar Obstacle = new ObstacleCar();
            Obstacle.pos_x = 0;
            Obstacle.pos_y = 5;
            Obstacle.type = "car";

            LevelDisplay LevelRenderer = new LevelDisplay();
            BorderControl B_Control = new BorderControl();
            while (true)
            {             
                // Draw and show the level map.     
                level1 = LevelRenderer.Display_object(level1, Player.pos_x, Player.pos_y, Player.type);
                level1 = LevelRenderer.Display_object(level1, Obstacle.pos_x, Obstacle.pos_y, Obstacle.type);
                Console.WriteLine(LevelRenderer.Display_level(level1));

               
                // Player controls.
                string? move_key = Console.ReadLine();
                var player_new_position = PlayerControl.Move_player(level1, Player.pos_x, Player.pos_y, move_key);
                var obstacle_new_position = ObstacleControl.Move_obstacle(level1, Obstacle.pos_x, Obstacle.pos_y);

                // Clear out map before redrawing it.
                level1 = LevelRenderer.Clear_object(level1, Player.pos_x, Player.pos_y, Player.type);
                level1 = LevelRenderer.Clear_object(level1, Obstacle.pos_x, Obstacle.pos_y, Obstacle.type);
                if (Obstacle.type == "disappearing_car_2")
                {
                    Obstacle.pos_x = 0;
                    Obstacle.type = "car";
                } else
                {
                    Obstacle.pos_x = obstacle_new_position.Item1;
                    Obstacle.pos_y = obstacle_new_position.Item2;
                }

                Player.pos_x = player_new_position.Item1;
                Player.pos_y = player_new_position.Item2;
                
                
                string Check_if_obstacle_reached_the_end = B_Control.Disappear_object(level1, Obstacle.pos_x, Obstacle.type);
                Obstacle.type = Check_if_obstacle_reached_the_end;
            }
        }
    }
}
