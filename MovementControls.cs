using CollisionManager;
using System;

class PlayerControl
{
    // Player movement.
    public static (int, int) Move_player(int[][] level, (int, int)[] shape, int posx, int posy, string? key)
    {
        switch (key)
        {
            // Move to the left or right even when there's an obstacle car in the way so that you can smash in to it and lose the game.
            case "a":
                if (CheckPositions.Check_position(level, shape, 0, (-1), 0) || CheckPositions.Check_position(level, shape, 0, (-1), 2))
                {
                    posy += (-1);
                }
                break;
            case "d":
                if (CheckPositions.Check_position(level, shape, 0, 1, 0) || CheckPositions.Check_position(level, shape, 0, 1, 2))
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
    public static (int, int) Move_obstacle(int[][] level, (int, int)[] shape, int posx, int posy)
    {
        if (CheckPositions.Check_position(level, shape, 1, 0, 0))
        {
            posx += 1;
        }

        return (posx, posy);
    }
}
