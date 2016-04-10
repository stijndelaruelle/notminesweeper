using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        private int m_MaxDataChunks;

        [SerializeField]
        private int m_MaxVisualChunks;

        [SerializeField]
        private VisualTileGridChunk m_ChunkPrefab;

        [SerializeField]
        private Pool m_VisualTilePool;

        private TileGrid m_TileGridData;
        private List<VisualTileGridChunk> m_VisualTileGridChunk;

        private int m_BottomChunkID = 0;
        private int m_TopChunkID = 0;

        private void Start()
        {
            GameManager.Instance.GameResetEvent += OnGameReset;

            GenerateGrid();
        }

        private void OnDestroy()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager != null)
            {
                gameManager.GameResetEvent -= OnGameReset;
            }

            foreach (VisualTileGridChunk chunk in m_VisualTileGridChunk)
            {
                chunk.AlmostOnScreenEvent -= OnAlmostOnScreenEvent;
            }
        }

        private void GenerateGrid()
        {
            //Generate the data
            m_TileGridData = new TileGrid((int)m_ChunkSize.x, (int)m_ChunkSize.y, m_MaxDataChunks);

            //Generate the visuals
            m_VisualTileGridChunk = new List<VisualTileGridChunk>();

            m_BottomChunkID = 0;
            m_TopChunkID = m_MaxVisualChunks - 1;

            for (int i = 0; i < m_MaxVisualChunks; ++i)
            {
                VisualTileGridChunk chunk = GameObject.Instantiate(m_ChunkPrefab) as VisualTileGridChunk;
                chunk.VisualTilePool = m_VisualTilePool;
                chunk.SetChunkData(m_TileGridData.GetChunk(i));

                chunk.gameObject.transform.parent = this.transform;

                chunk.AlmostOnScreenEvent += OnAlmostOnScreenEvent;

                m_VisualTileGridChunk.Add(chunk);
            }
        }

        private void ResetGrid()
        {
            m_VisualTilePool.ResetAll();
            m_TileGridData.FillGrid(m_BombsInChunk, m_BombIncreaseRate);

            m_BottomChunkID = 0;
            m_TopChunkID = m_MaxVisualChunks - 1;

            for (int i = 0; i < m_MaxVisualChunks; ++i)
            {
                m_VisualTileGridChunk[i].SetChunkData(m_TileGridData.GetChunk(i));
            }
        }

        private void OnGameReset()
        {
            ResetGrid();
        }

        private void OnAlmostOnScreenEvent(VisualTileGridChunk chunk, bool top)
        {
            //If the top of a chunk is almost on screen, we need to place another one above it.
            if (top && m_VisualTileGridChunk[m_VisualTileGridChunk.Count - 1] == chunk)
            {
                TileGridChunk tileGrid = m_TileGridData.GetChunk(m_TopChunkID + 1);

                if (tileGrid != null)
                {
                    m_TopChunkID += 1;
                    m_BottomChunkID += 1;

                    VisualTileGridChunk bottomChunk = m_VisualTileGridChunk[0];
                    bottomChunk.SetChunkData(tileGrid);

                    m_VisualTileGridChunk.RemoveAt(0);
                    m_VisualTileGridChunk.Add(bottomChunk);
                }
            }

            if (!top && m_VisualTileGridChunk[0] == chunk)
            {
                TileGridChunk tileGrid = m_TileGridData.GetChunk(m_BottomChunkID - 1);

                if (tileGrid != null)
                {
                    m_TopChunkID -= 1;
                    m_BottomChunkID -= 1;

                    VisualTileGridChunk topChunk = m_VisualTileGridChunk[m_VisualTileGridChunk.Count - 1];
                    topChunk.SetChunkData(tileGrid);

                    m_VisualTileGridChunk.RemoveAt(m_VisualTileGridChunk.Count - 1);
                    m_VisualTileGridChunk.Insert(0, topChunk);
                }
            }
        }
    }
}
