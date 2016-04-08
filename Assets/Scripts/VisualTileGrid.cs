using UnityEngine;
using System.Collections;

namespace MineSweeper
{
    public class VisualTileGrid : MonoBehaviour
    {
        [SerializeField]
        private Vector2 m_ChunkSize;

        [SerializeField]
        private int m_BombsInChunk;

        [SerializeField]
        private int m_BombIncreaseRate;

        [SerializeField]
        private Pool m_VisualTilePool;
        private TileGrid m_TileGridData;

        private void Start()
        {
            m_TileGridData = new TileGrid((int)m_ChunkSize.x, (int)m_ChunkSize.y);

            GameManager.Instance.GameResetEvent += OnGameReset;
            GameManager.Instance.TileDiscoveredEvent += OnTileDiscovered;
        }

        private void OnDestroy()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager != null)
            {
                gameManager.GameResetEvent -= OnGameReset;
                gameManager.TileDiscoveredEvent -= OnTileDiscovered;
            }  
        }

        private void ClearGrid()
        {
            m_VisualTilePool.ResetAll();
        }

        private void AddChunk()
        {
            
            m_TileGridData.FillChunk(m_BombsInChunk);

            int totalSize = (int)(m_ChunkSize.x * m_ChunkSize.y);

            for (int i = 0; i < totalSize; ++i)
            {
                Vector3 pos = new Vector3((int)(i % m_ChunkSize.x), 0.0f, (int)(i / m_ChunkSize.x));
                VisualTile visualTile = m_VisualTilePool.ActivateAvailableObject(pos, Quaternion.identity) as VisualTile;
                Tile tile = m_TileGridData.GetTile(i);

                visualTile.SetTileData(tile);
                tile.VisualTile = visualTile;

                //Enable the first row of tiles
                if (i < m_ChunkSize.x)
                    visualTile.Enable();
            }
        }

        private void OnGameReset()
        {
            ClearGrid();
        }

        private void OnTileDiscovered(Tile tile)
        {

        }
    }
}
