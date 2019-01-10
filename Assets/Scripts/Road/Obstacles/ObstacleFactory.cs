using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Obstacles
{
    public class ObstacleFactory
    {
        public static ObstacleFigure Get(ObstacleType type)
        {
            switch (type)
            {
                case ObstacleType.Cube:
                    return new ObstacleFigure(1, 1, 1, type);
                default:
                    return null;
            }
        }
    }
}
