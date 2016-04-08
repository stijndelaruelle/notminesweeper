using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MineSweeper
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField]
        private Text m_ScoreLabel;

        [SerializeField]
        private Text m_HighscoreLabel;

        private int m_Score;
        private int m_Highscore;

        private void Start()
        {
            GameManager gameManager = GameManager.Instance;

            gameManager.GameResetEvent += OnGameReset;
            gameManager.TileDiscoveredEvent += OnTileDiscovered;
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

        private void OnGameReset()
        {
            UpdateScore(0);
        }

        private void OnTileDiscovered(Tile tile)
        {
            if (tile.Row > m_Score)
                UpdateScore(tile.Row);
        }

        private void UpdateScore(int score)
        {
            m_Score = score;

            if (score > m_Highscore)
                m_Highscore = score;

            m_ScoreLabel.text = m_Score.ToString();
            m_HighscoreLabel.text = "Best: " + m_Highscore;
        }
    }
}
