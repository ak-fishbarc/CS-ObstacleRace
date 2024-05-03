using System;
using CollisionManager;
using GameObjects;
using DisplayManager; 


namespace GameRun
{

    class Test
    {
        static void Main()
        {

            bool game_start = true;

            LevelCreator LevelBuilder = new LevelCreator();
            int [][] level1 = LevelBuilder.Create_Level(12, 24);
            PlayerCar Player = new PlayerCar();
            Player.Access_posx = 11;
            Player.Access_posy = 5;
            Player.Access_shape = Player.Access_shape;

            ObstacleCar Obstacle = new ObstacleCar();
            Obstacle.Access_posx = 2;
            Obstacle.Access_posy = 5;
            Obstacle.Access_shape = Player.Access_shape;

            LevelDisplay LevelRenderer = new LevelDisplay();
            ObstaclesManager O_Control = new ObstaclesManager();
            O_Control.Add_Obstacles(Obstacle);;


            while (game_start)
            {
                
                // Draw and show the level map.     
                level1 = LevelRenderer.Display_object(level1, Player.Access_shape);
                level1 = LevelRenderer.Display_object(level1, Obstacle.Access_shape);

                Console.WriteLine(LevelRenderer.Display_level(level1));
           
                // Player controls.
                string? move_key = Console.ReadLine();
                var player_new_position = PlayerControl.Move_player(level1, Player.Access_shape, Player.Access_posx, Player.Access_posy, move_key);
                var obstacle_new_position = ObstacleControl.Move_obstacle(level1, Obstacle.Access_shape, Obstacle.Access_posx, Obstacle.Access_posy);


                // Clear out map before redrawing it.
                level1 = LevelRenderer.Clear_object(level1, Player.Access_shape);
                level1 = LevelRenderer.Clear_object(level1, Obstacle.Access_shape);

                Obstacle.Access_posx = obstacle_new_position.Item1;
                Obstacle.Access_posy = obstacle_new_position.Item2;
                Obstacle.Access_shape = Obstacle.Access_shape;

                Console.WriteLine(player_new_position.Item1);
                Console.WriteLine(player_new_position.Item2);

                Player.Access_posx = player_new_position.Item1;
                Player.Access_posy = player_new_position.Item2;
                Player.Access_shape = Player.Access_shape;
            }

        }
    }
}

