using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MineSweeper
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField]
        private float m_StartTime;

        [SerializeField]
        private float m_BonusTimePerTile;

        [SerializeField]
        private Text m_Text;

        private float m_Time;
        private bool m_IsTicking = false;

        private void Start()
        {
            GameManager gameManager = GameManager.Instance;

            gameManager.GameStartEvent += OnGameStart;
            gameManager.GameOverEvent += OnGameOver;
            gameManager.GameResetEvent += OnGameReset;
            gameManager.TileDiscoveredEvent += OnTileDiscovered;
        }

        private void OnDestroy()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager != null)
            {
                gameManager.GameStartEvent -= OnGameStart;
                gameManager.GameOverEvent -= OnGameOver;
                gameManager.GameResetEvent -= OnGameReset;
                gameManager.TileDiscoveredEvent -= OnTileDiscovered;
            }
        }

        private void Update()
        {
            if (!m_IsTicking)
                return;

            m_Time -= Time.deltaTime;

            if (m_Time < 0.0f)
            {
                m_Time = 0.0f;
                GameManager.Instance.OnTimeUp();
            }

            m_Text.text = ((int)m_Time).ToString();
        }

        private void OnGameStart()
        {
            m_IsTicking = true;
        }

        private void OnGameOver()
        {
            m_IsTicking = false;
        }

        private void OnGameReset()
        {
            m_Time = m_StartTime;
        }

        private void OnTileDiscovered(Tile tile)
        {
            m_Time += tile.Value;
        }
    }
}
