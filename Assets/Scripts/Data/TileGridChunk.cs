using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MineSweeper
{
    public class TileGridChunk
    {
        private List<Tile> m_Tiles;
        private TileGridChunk m_BottomNeighbour;

        private int m_Width = 0;
        public int Width
        {
            get { return m_Width; }
        }

        private int m_Height = 0;
        public int Height
        {
            get { return m_Height; }
        }

        //Events
        private Action m_UpdateDataEvent;
        public Action UpdateDataEvent
        {
            get { return m_UpdateDataEvent; }
            set { m_UpdateDataEvent = value; }
        }

        public TileGridChunk(int width, int height, TileGridChunk bottomNeighbour)
        {
            m_Width = width;
            m_Height = height;
            m_BottomNeighbour = bottomNeighbour;

            GenerateChunk();
        }

        private void GenerateChunk()
        {
            m_Tiles = new List<Tile>();

            for (int i = 0; i < (int)(m_Width * m_Height); ++i)
            {
                int row = (i / m_Width) + 1;

                if (m_BottomNeighbour != null)
                    row += m_BottomNeighbour.GetTopRow();

                Tile newTile = new Tile(row);

                m_Tiles.Add(newTile);

                //Add neighbours of the row below
                if (i >= m_Width || m_BottomNeighbour != null)
                {
                    //Bottom Left
                    if ((i % m_Width) > 0)
                    {
                        Tile bottomLeftTile;

                        if (i - m_Width - 1 >= 0) { bottomLeftTile = m_Tiles[i - m_Width - 1]; }
                        else                      { bottomLeftTile = m_BottomNeighbour.GetTileInTopRow(i - 1); }
                        
                        newTile.AddNeighbour(bottomLeftTile);
                        bottomLeftTile.AddNeighbour(newTile);
                    }

                    //Bottom Middle
                    Tile bottomMiddleTile;
                        
                    if (i - m_Width >= 0) { bottomMiddleTile = m_Tiles[i - m_Width]; }
                    else                  { bottomMiddleTile = m_BottomNeighbour.GetTileInTopRow(i); }

                    newTile.AddNeighbour(bottomMiddleTile);
                    bottomMiddleTile.AddNeighbour(newTile);

                    //Bottom Right
                    if ((i % m_Width) < (m_Width - 1))
                    {
                        Tile bottomRightTile;

                        if (i - m_Width + 1 >= 0) { bottomRightTile = m_Tiles[i - m_Width + 1]; }
                        else                      { bottomRightTile = m_BottomNeighbour.GetTileInTopRow(i + 1); }

                        newTile.AddNeighbour(bottomRightTile);
                        bottomRightTile.AddNeighbour(newTile);
                    }
                }

                //Add the left neighbour
                if ((i % m_Width) > 0)
                {
                    Tile leftTile = m_Tiles[i - 1];
                    newTile.AddNeighbour(leftTile);
                    leftTile.AddNeighbour(newTile);
                }
            }
        }

        public void PlaceBombs(int numberOfBombs)
        {
            //Clear values
            foreach (Tile tile in m_Tiles)
            {
                tile.Reset();
            }

            //We place all the bombs
            int bombsGenerated = 0;

            //No bombs on the first row
            while (bombsGenerated < numberOfBombs)
            {
                for (int y = 1; y < m_Height; ++y)
                {
                    int bombsOnRow = 0;
                    for (int x = 0; x < m_Width; ++x)
                    {
                        int rand = UnityEngine.Random.Range(0, 1000);

                        if (rand > 900)
                        {
                            int id = (y * m_Width) + x;
                            m_Tiles[id].SetBomb();
                            ++bombsOnRow;
                            ++bombsGenerated;
                        }

                        //Max 2 bombs per row
                        if (bombsOnRow > 1)
                            break;

                        if (bombsGenerated >= numberOfBombs)
                            return;
                    }
                }
            }
        }

        public void CalculateValues()
        {
            foreach (Tile tile in m_Tiles)
            {
                tile.CalculateValue();
            }

            if (m_UpdateDataEvent != null)
                m_UpdateDataEvent();
        }

        public int GetBottomRow()
        {
            int bottomRow = 0;

            //Get top row of our bottom neighbour
            if (m_BottomNeighbour != null)
                bottomRow = m_BottomNeighbour.GetTopRow();

            return bottomRow;
        }

        public int GetTopRow()
        {
            return (GetBottomRow() + m_Height);
        }

        public Tile GetTileInTopRow(int id)
        {
            //0 is the most left of the top row.
            int tileID = m_Tiles.Count - m_Width + id;
            return m_Tiles[tileID];
        }

        public Tile GetTile(int id)
        {
            if (id < 0 || id >= m_Tiles.Count)
                return null;

            return m_Tiles[id];
        }
    }
}
