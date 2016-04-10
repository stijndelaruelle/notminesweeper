using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MineSweeper
{
    public class VisualTile : PoolableObject
    {
        [SerializeField]
        private VisualTileCover m_Cover;

        [SerializeField]
        private MeshRenderer m_TextRenderer;

        private Tile m_TileData;
        
        private void Awake()
        {
            m_Cover.TriggerEvent += OnCoverRemoved;
        }

        private void OnDestroy()
        {
            if (m_Cover != null)
                m_Cover.TriggerEvent -= OnCoverRemoved;
        }

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
            m_Cover.gameObject.SetActive(!tileData.IsDiscovered);
            if (!tileData.IsDiscovered)
            {
                m_Cover.Reset();
            }

            if (m_TileData.IsEnabled)
                Enable();

            //Place the flag if required

        }

        public void Enable()
        {
            m_TileData.IsEnabled = true;
            m_Cover.SetEnabled(true);
        }

        public void Reveal()
        {
            if (!m_TileData.IsDiscovered)
                m_Cover.TriggerAnimation();
        }

        private void OnCoverRemoved()
        {
            if (m_TileData.IsDiscovered)
                return;

            m_TileData.IsDiscovered = true;
            GameManager.Instance.OnTileDiscovered(m_TileData);

            if (m_TileData.IsBomb())
                return;

            List<VisualTile> visualNeighbours = m_TileData.GetVisualNeighbours();

            //If our value was 0, automatically reveal all our neighbours!
            if (m_TileData.Value == 0)
            {
                StartCoroutine(RevealNeighboursRoutine(visualNeighbours));
            }

            //Make our neighbours clickable
            else
            {
                foreach (VisualTile visualNeighbour in visualNeighbours)
                {
                    visualNeighbour.Enable();
                }
            }
        }

        private IEnumerator RevealNeighboursRoutine(List<VisualTile> visualNeighbours)
        {
            foreach (VisualTile visualNeighbour in visualNeighbours)
            {
                visualNeighbour.Reveal();
                yield return new WaitForSeconds(0.05f);
            }

            yield return null;
        }

        #region PoolableObject

        public override void Initialize()
        {

        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            gameObject.transform.localPosition = pos;
            gameObject.transform.localRotation = rot;

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
