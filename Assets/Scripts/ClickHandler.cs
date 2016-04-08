using UnityEngine;
using System.Collections;

namespace MineSweeper
{
    public class ClickHandler : MonoBehaviour
    {
        private bool m_AllowClick = true;

        private void Start()
        {
            GameManager.Instance.GameOverEvent += OnGameOver;
            GameManager.Instance.GameResetEvent += OnGameReset;
        }

        private void OnDestroy()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager != null)
            {
                gameManager.GameOverEvent -= OnGameOver;
                gameManager.GameResetEvent -= OnGameReset;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && m_AllowClick)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    IClickable clickableObject = hit.transform.gameObject.GetComponent<IClickable>();

                    if (clickableObject != null)
                    {
                        clickableObject.OnClick();
                    }
                }
            }
        }

        private void OnGameOver()
        {
            m_AllowClick = false;
        }

        private void OnGameReset()
        {
            m_AllowClick = true;
        }
    }
}
