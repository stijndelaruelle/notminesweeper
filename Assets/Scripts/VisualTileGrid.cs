using UnityEngine;
using System.Collections;

namespace MineSweeper
{
    public class VisualTileGrid : MonoBehaviour
    {
        [SerializeField]
        private Vector2 m_Size;

        [SerializeField]
        private Pool m_VisualTilePool;
        private TileGrid m_TileGridData;

        private void Start()
        {
            m_TileGridData = new TileGrid((int)m_Size.x, (int)m_Size.y);
        }

        private void FillGrid()
        {
            m_VisualTilePool.ResetAll();
            m_TileGridData.FillGrid(5);

            int totalSize = (int)(m_Size.x * m_Size.y);

            for (int i = 0; i < totalSize; ++i)
            {
                Vector3 pos = new Vector3((int)(i % m_Size.x), 0.0f, (int)(i / m_Size.x));
                VisualTile visualTile = m_VisualTilePool.ActivateAvailableObject(pos, Quaternion.identity) as VisualTile;

                visualTile.SetTileData(m_TileGridData.GetTile(i));
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FillGrid();
            }
        }
    }
}
