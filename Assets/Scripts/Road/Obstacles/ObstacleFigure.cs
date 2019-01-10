using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Obstacles
{
    public class ObstacleFigure
    {
        private int width; // z axis
        private int length; // x axis
        private int height;  // y axis

        private int row;
        private int column;

        private ObstacleType type;


        public int Width { get { return width; } }
        public int Length { get { return length; } }
        public int Height { get { return height; } }

        public int Row { get { return row; } }
        public int Column { get { return column; } }

        public ObstacleType Type { get { return type; } }

        public ObstacleFigure(int width, int length, int height, ObstacleType type)
        {
            this.width = width;
            this.height = height;
            this.length = length;
            this.type = type;
        }

        public void Place(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
}