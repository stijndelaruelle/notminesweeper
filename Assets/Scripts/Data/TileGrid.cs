using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MineSweeper
{
    public class TileGrid
    {
        private List<TileGridChunk> m_TileChunks;

        public TileGrid(int width, int height, int maxChuncks)
        {
            m_TileChunks = new List<TileGridChunk>();
            GenerateGrid(width, height, maxChuncks);
        }

        private void GenerateGrid(int width, int height, int maxChuncks)
        {
            m_TileChunks.Clear();

            TileGridChunk bottomNeighbour = null;

            for (int i = 0; i < maxChuncks; ++i)
            {
                TileGridChunk newChunk = new TileGridChunk(width, height, bottomNeighbour);
                m_TileChunks.Add(newChunk);

                bottomNeighbour = newChunk;
            }
        }

        public void FillGrid(int numberOfBombs, int bombIncreaseRate)
        {
            //Place all the bombs
            for (int i = 0; i < m_TileChunks.Count; ++i)
            {
                m_TileChunks[i].PlaceBombs(numberOfBombs + (bombIncreaseRate * i));
            }

            //Afterwards we can calculate all our values
            for (int i = 0; i < m_TileChunks.Count; ++i)
            {
                m_TileChunks[i].CalculateValues();
            }
        }

        public TileGridChunk GetChunk(int id)
        {
            if (id < 0 || id >= m_TileChunks.Count)
                return null;

            return m_TileChunks[id];
        }

    }
}
