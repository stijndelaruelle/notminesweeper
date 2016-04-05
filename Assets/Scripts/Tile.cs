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

        private bool m_IsDiscovered = false;
        public bool IsDiscovered
        {
            get { return m_IsDiscovered; }
        }

        private bool m_HasFlag = false;
        public bool HasFlag
        {
            get { return m_HasFlag; }
        }

        private List<Tile> m_Neighbours;

        public Tile()
        {
            m_Neighbours = new List<Tile>();
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
            m_HasFlag = false;
        }
    }
}
