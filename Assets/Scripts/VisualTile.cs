using UnityEngine;
using System.Collections;

namespace MineSweeper
{
    public class VisualTile : PoolableObject
    {
        [SerializeField]
        private VisualTileCover m_Cover;

        [SerializeField]
        private MeshRenderer m_TextRenderer;

        private Tile m_TileData;
        
        public void SetTileData(Tile tileData)
        {
            m_TileData = tileData;

            //Set the number/trap
            float xOffset = 0.0f;
            float yOffset = 0.0f;
            bool enabled = false;

            if (m_TileData.IsBomb())
            {
                xOffset = 0.66f;
                enabled = true;
            }
            else if (m_TileData.Value > 0)
            {
                xOffset = ((m_TileData.Value - 1) % 3) * 0.33f;
                yOffset = 0.66f - ((m_TileData.Value - 1) / 3) * 0.33f;
                enabled = true;
            }

            m_TextRenderer.material.mainTextureOffset = new Vector2(xOffset, yOffset);
            m_TextRenderer.enabled = enabled;

            //Place the cover if required
            if (tileData.IsDiscovered)
            {

            }

            //Place the flag if required

        }

        #region PoolableObject

        public override void Initialize()
        {

        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            gameObject.transform.position = pos;
            gameObject.transform.rotation = rot;

            gameObject.SetActive(true);
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public override bool IsAvailable()
        {
            return (!gameObject.activeSelf);
        }

        #endregion
    }
}
