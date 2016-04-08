using UnityEngine;
using System.Collections;
using System;

namespace MineSweeper
{
    public class GameManager : Singleton<GameManager>
    {
        private event Action m_GameStartEvent;
        public Action GameStartEvent
        {
            get { return m_GameStartEvent; }
            set { m_GameStartEvent = value; }

        }

        private event Action m_GameResetEvent;
        public Action GameResetEvent
        {
            get { return m_GameResetEvent; }
            set { m_GameResetEvent = value; }

        }

        private event Action m_GameOverEvent;
        public Action GameOverEvent
        {
            get { return m_GameOverEvent; }
            set { m_GameOverEvent = value; }
        }

        private event Action<Tile> m_TileDiscoveredEvent;
        public Action<Tile> TileDiscoveredEvent
        {
            get { return m_TileDiscoveredEvent; }
            set { m_TileDiscoveredEvent = value; }
        }

        private void Start()
        {
            RestartGame();
        }

        private void Update()
        {
            //For debug purposes
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }

        private void StartGame()
        {
            //The game starts when the first tile was hit!

            if (m_GameStartEvent != null)
                m_GameStartEvent();
        }

        private void GameOver()
        {
            if (m_GameOverEvent != null)
                m_GameOverEvent();
        }

        public void RestartGame()
        {
            //Reset everything
            if (m_GameResetEvent != null)
                m_GameResetEvent();

            StartGame();
        }

        public void OnTimeUp()
        {
            GameOver();
        }

        public void OnTileDiscovered(Tile tile)
        {
            //If it was a bomb, we're dead!
            if (tile.IsBomb())
            {
                GameOver();
                return;
            }

            //Let the world know!
            if (m_TileDiscoveredEvent != null)
                m_TileDiscoveredEvent(tile);
        }
    }

}
