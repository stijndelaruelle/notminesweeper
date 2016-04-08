using UnityEngine;
using System.Collections;

namespace MineSweeper
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_MainMenuPanel;

        [SerializeField]
        private GameObject m_GameOverPanel;


        private void Start()
        {
            GameManager.Instance.GameStartEvent += OnGameStart;
            GameManager.Instance.GameOverEvent += OnGameOver;
            GameManager.Instance.GameResetEvent += OnGameReset;
        }

        private void OnDestroy()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager != null)
            {
                gameManager.GameStartEvent -= OnGameStart;
                gameManager.GameOverEvent -= OnGameOver;
                gameManager.GameResetEvent -= OnGameReset;
            }
        }

        private void OnGameStart()
        {
            m_MainMenuPanel.SetActive(false);
            m_GameOverPanel.SetActive(false);
        }

        private void OnGameOver()
        {
            m_MainMenuPanel.SetActive(false);
            m_GameOverPanel.SetActive(true);
        }

        private void OnGameReset()
        {
            m_MainMenuPanel.SetActive(true);
            m_GameOverPanel.SetActive(false);
        }
    }
}
