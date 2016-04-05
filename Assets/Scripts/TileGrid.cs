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

        public void GenerateGrid(int width, int height)
        {
            m_Tiles.Clear();

            for (int i = 0; i < (int)(width * height); ++i)
            {
                Tile newTile = new Tile();
                m_Tiles.Add(newTile);
                //Add neighbours of the row above
                if (i >= width)
                {
                    //Top Left
                    if ((i % width) > 0)
                    {
                        Tile topLeftTile = m_Tiles[i - width - 1];
                        newTile.AddNeighbour(topLeftTile);
                        topLeftTile.AddNeighbour(newTile);
                    }

                    //Top Middle
                    Tile topMiddleTile = m_Tiles[i - width];
                    newTile.AddNeighbour(topMiddleTile);
                    topMiddleTile.AddNeighbour(newTile);

                    //Top Right
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

        public void FillGrid(int numberOfBombs)
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
