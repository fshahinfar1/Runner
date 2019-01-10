using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    public class ObstacleGenretor
    {
        private int mapWidth;  // x axis
        private int mapHeight;  // z axis

        public ObstacleGenretor(int width, int height)
        {
            mapWidth = width;
            mapHeight = height;
        }

        public ObstacleMap Generate()
        {
            List<ObstacleFigure> obstacles = new List<ObstacleFigure>();
            for (int row=0; row < mapHeight; row++)
            {
                for (int column = 0; column < mapWidth; column++)
                {
                    int tmp = Random.Range(0, 2);
                    if (tmp == 1)
                    {
                        continue;
                    }
                    ObstacleType type = (ObstacleType)tmp;
                    ObstacleFigure obs = ObstacleFactory.Get(type);
                    obs.Place(row, column);
                    obstacles.Add(obs);
                }
            }
            ObstacleMap map = new ObstacleMap(obstacles);
            return map;
        }
    }
}