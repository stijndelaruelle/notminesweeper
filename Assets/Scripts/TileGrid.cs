using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MineSweeper
{
    public class TileGrid
    {
        private List<Tile> m_Tiles;

        public TileGrid()
        {
            m_Tiles = new List<Tile>();
        }

        public TileGrid(int width, int height)
        {
            m_Tiles = new List<Tile>();
            GenerateGrid(width, height);
        }

        private void GenerateGrid(int width, int height)
        {
            m_Tiles.Clear();
            AddChunck(width, height);
        }

        public void AddChunck(int width, int height)
        {
            int startID = m_Tiles.Count - 1;
            int endID = startID + (int)(width * height);

            for (int i = startID; i < endID; ++i)
            {
                int row = (i / width) + 1;
                Tile newTile = new Tile(row);

                m_Tiles.Add(newTile);

                //Add neighbours of the row below
                if (i >= width)
                {
                    //Bottom Left
                    if ((i % width) > 0)
                    {
                        Tile topLeftTile = m_Tiles[i - width - 1];
                        newTile.AddNeighbour(topLeftTile);
                        topLeftTile.AddNeighbour(newTile);
                    }

                    //Bottom Middle
                    Tile topMiddleTile = m_Tiles[i - width];
                    newTile.AddNeighbour(topMiddleTile);
                    topMiddleTile.AddNeighbour(newTile);

                    //Bottom Right
                    if ((i % width) < (width - 1))
                    {
                        Tile topLeftTile = m_Tiles[i - width + 1];
                        newTile.AddNeighbour(topLeftTile);
                        topLeftTile.AddNeighbour(newTile);
                    }
                }

                //Add the left neighbour
                if ((i % width) > 0)
                {
                    Tile leftTile = m_Tiles[i - 1];
                    newTile.AddNeighbour(leftTile);
                    leftTile.AddNeighbour(newTile);
                }
            }
        }

        public void FillChunk(int numberOfBombs)
        {
            //Clear values
            foreach (Tile tile in m_Tiles)
            {
                tile.Reset();
            }

            //We place all the bombs
            int bombsGenerated = 0;

            while (bombsGenerated < numberOfBombs)
            {
                int rand = Random.Range(0, m_Tiles.Count - 1);

                if (m_Tiles[rand].IsBomb() == false)
                {
                    m_Tiles[rand].SetBomb();
                    ++bombsGenerated;
                }
            }

            //Recalculateall the other values
            foreach (Tile tile in m_Tiles)
            {
                tile.CalculateValue();
            }
        }

        public Tile GetTile(int id)
        {
            if (id > -1 && id < m_Tiles.Count)
                return m_Tiles[id];
            else
                return null;
        }
    }
}
