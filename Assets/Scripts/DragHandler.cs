using UnityEngine;
using System.Collections;

namespace MineSweeper
{
    public class DragHandler : MonoBehaviour
    {
        private float m_DefaultPosition;

        private float m_DragPosition;
        private bool m_AllowDrag = true;

        private void Start()
        {
            GameManager.Instance.GameOverEvent += OnGameOver;
            GameManager.Instance.GameResetEvent += OnGameReset;

            m_DefaultPosition = transform.position.z;
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
            //Start Pressing
            if (Input.GetMouseButtonDown(0) && m_AllowDrag)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    m_DragPosition = hit.point.z;
                }
            }

            //While pressing
            if (Input.GetMouseButton(0) && m_AllowDrag)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    float currentZ = hit.point.z;
                    float diff = (currentZ - m_DragPosition);

                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + diff);

                    m_DragPosition = currentZ;
                }
            }
        }

        private void OnGameOver()
        {
            m_AllowDrag = false;
        }

        private void OnGameReset()
        {
            m_AllowDrag = true;

            transform.position = new Vector3(transform.position.x, transform.position.y, m_DefaultPosition);
        }
    }
}
