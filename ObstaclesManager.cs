using GameObjects;
using System;

class ObstaclesManager
{
    private List<Car> obstacles = [];

    public void Add_Obstacles(Car Obstacle)
    {
        obstacles.Add(Obstacle);
    }

    public void Spawn_Obstacles(int[][] level)
    {
        Random number = new Random();

        bool placed = false;
        int chance_to_spawn = number.Next(1, 8);

        foreach (Car obstacle in obstacles)
        {
            obstacle.Access_posx = 0;
            if (chance_to_spawn > 4)
            {
                while (!placed)
                {
                    int posx = number.Next(1, level.Length);
                    int posy = number.Next(1, level[posx].Length);

                    if (level[posx][posy] == 0 && level[posx][posy - 1] == 0 && level[posx][posy + 1] == 0)
                    {
                        obstacle.Access_posy = posy;
                        placed = true;
                    }
                }
                placed = false;
            }
        }

    }
}
 
