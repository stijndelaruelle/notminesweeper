using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MineSweeper
{
    public delegate void VisualTileGridChunkBoolDelegate(VisualTileGridChunk chunk, bool top);

    public class VisualTileGridChunk : MonoBehaviour
    {
        private Pool m_VisualTilePool;
        public Pool VisualTilePool
        {
            get { return m_VisualTilePool; }
            set { m_VisualTilePool = value; }
        }

        private TileGridChunk m_ChunkData;
        private List<VisualTile> m_VisualTiles;

        private bool m_IsEnabled = false;
        public bool IsEnabled
        {
            get { return m_IsEnabled; }
        }

        private VisualTileGridChunkBoolDelegate m_AlmostOnScreenEvent;
        public VisualTileGridChunkBoolDelegate AlmostOnScreenEvent
        {
            get { return m_AlmostOnScreenEvent; }
            set { m_AlmostOnScreenEvent = value; }
        }

        private void Awake()
        {
            m_VisualTiles = new List<VisualTile>();
        }

        private void OnDestroy()
        {
            if (m_ChunkData != null)
                m_ChunkData.UpdateDataEvent -= OnChunkDataUpdated;
        }

        private void Update()
        {
            //DisableIfOffScreen();
            CheckIfAlmostOnScreen();
        }

        private void DisableIfOffScreen()
        {
            if (m_VisualTiles.Count == 0)
                return;

            //Take the last tile in our list (top row and check if that one is offscreen)
            Vector3 bottomLeftPos = Camera.main.WorldToViewportPoint(m_VisualTiles[0].transform.position);
            Vector3 topRightPos = Camera.main.WorldToViewportPoint(m_VisualTiles[m_VisualTiles.Count - 1].transform.position);

            if (topRightPos.x < 0.0f || bottomLeftPos.x > 1.1f || topRightPos.y < 0.0f || bottomLeftPos.y > 1.1f)
            {
                Deinitialize();
            }
        }

        private void CheckIfAlmostOnScreen()
        {
            if (m_VisualTiles.Count == 0)
                return;

            //Take the last tile in our list (top row and check if that one is offscreen)
            Vector3 bottomLeftPos = Camera.main.WorldToViewportPoint(m_VisualTiles[0].transform.position);
            Vector3 topRightPos = Camera.main.WorldToViewportPoint(m_VisualTiles[m_VisualTiles.Count - 1].transform.position);

            if (bottomLeftPos.y > -0.5f && bottomLeftPos.y < 0.0f)
            {
                if (m_AlmostOnScreenEvent != null)
                    m_AlmostOnScreenEvent(this, false);
            }

            if (topRightPos.y > 1.0f && topRightPos.y < 1.5f)
            {
                if (m_AlmostOnScreenEvent != null)
                    m_AlmostOnScreenEvent(this, true);
            }
        }

        public void SetChunkData(TileGridChunk chunkData)
        {
            Deinitialize();

            if (m_ChunkData != null)
                m_ChunkData.UpdateDataEvent -= OnChunkDataUpdated;

            m_ChunkData = chunkData;
            m_ChunkData.UpdateDataEvent += OnChunkDataUpdated;

            Initialize();
        }

        private void Initialize()
        {
            m_VisualTiles.Clear();

            int totalSize = m_ChunkData.Width * m_ChunkData.Height;
            int startRow = m_ChunkData.GetBottomRow();

            transform.position = new Vector3(0.0f, 0.0f, startRow);

            for (int i = 0; i < totalSize; ++i)
            {
                Vector3 pos = new Vector3((i % m_ChunkData.Width), 0.0f, startRow + (i / m_ChunkData.Width));
                VisualTile visualTile = m_VisualTilePool.ActivateAvailableObject(pos, Quaternion.identity) as VisualTile;
                Tile tile = m_ChunkData.GetTile(i);

                visualTile.SetTileData(tile);
                tile.VisualTile = visualTile;

                m_VisualTiles.Add(visualTile);

                //Enable the first row of tiles
                if (startRow == 0 && i < m_ChunkData.Width)
                    visualTile.Enable();
            }

            m_IsEnabled = true;
        }

        private void Deinitialize()
        {
            foreach(VisualTile tile in m_VisualTiles)
            {
                tile.Deactivate();
            }

            m_VisualTiles.Clear();
            m_IsEnabled = false;
        }

        private void OnChunkDataUpdated()
        {
            Initialize();
        }
    }
}
