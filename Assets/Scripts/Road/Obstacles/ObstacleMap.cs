using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    // it is pretty much just a wrapper over list
    // it does not have any add value yet
    public class ObstacleMap: IEnumerable
    {

        List<ObstacleFigure> obstacles;
        public ObstacleMap(List<ObstacleFigure> obstacleList)
        {
            obstacles = obstacleList;
        }

        // check if given point is field with obstacle
        public bool IsObstacle(int row, int col)
        {
            foreach (ObstacleFigure obs in obstacles)
            {
                int rowStart = obs.Row;
                int rowEnd = rowStart + obs.Width;
                int colStart = obs.Column;
                int colEnd = colStart + obs.Length;

                if (row >= rowStart && row < rowEnd)
                {
                    if (col >= colStart && col < colEnd)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public int CountObstacles()
        {
            return obstacles.Count;
        }

        public ObstacleFigure GetObstacle(int index)
        {
            return obstacles[index];
        }

        public IEnumerator GetEnumerator()
        {
            return obstacles.GetEnumerator();
        }

    }
}