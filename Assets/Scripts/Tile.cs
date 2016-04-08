using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MineSweeper
{
    public class Tile
    {
        private int m_Value = 0; //-1 = bomb, 0 = nothing, 1+ are values
        public int Value
        {
            get { return m_Value; }
        }

        private int m_Row = 0;
        public int Row
        {
            get { return m_Row; }
        }

        private bool m_IsDiscovered = false;
        public bool IsDiscovered
        {
            get { return m_IsDiscovered; }
            set { m_IsDiscovered = value; }
        }

        //private bool m_HasFlag = false;
        //public bool HasFlag
        //{
        //    get { return m_HasFlag; }
        //}

        private List<Tile> m_Neighbours;
        private VisualTile m_VisualTile;
        public VisualTile VisualTile
        {
            get { return m_VisualTile; }
            set { m_VisualTile = value; }
        }

        public Tile(int row)
        {
            m_Neighbours = new List<Tile>();
            m_Row = row;
        }

        public void AddNeighbour(Tile neighbour)
        {
            m_Neighbours.Add(neighbour);
        }

        public void CalculateValue()
        {
            //If we're a bomb, there's nothing to calculate
            if (IsBomb())
                return;

            int value = 0;
            foreach (Tile neighBour in m_Neighbours)
            {
                if (neighBour.IsBomb())
                    ++value;
            }

            m_Value = value;
        }

        public void SetBomb()
        {
            m_Value = -1;
        }

        public bool IsBomb()
        {
            return (m_Value == -1);
        }

        public void Reset()
        {
            m_Value = 0;
            m_IsDiscovered = false;
            //m_HasFlag = false;
        }

        public List<VisualTile> GetVisualNeighbours()
        {
            List<VisualTile> visualNeighbours = new List<VisualTile>();

            foreach (Tile neighbour in m_Neighbours)
            {
                VisualTile visualTile = neighbour.VisualTile;

                if (visualTile != null)
                    visualNeighbours.Add(visualTile);
            }

            return visualNeighbours;
        }
    }
}
