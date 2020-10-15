using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LFA
{
    public class LevelMovement : MonoBehaviour
    {
        private Rigidbody2D m_Rigidbody = null;

        [SerializeField] private float m_LevelSpeed = 2f;

        // Start is called before the first frame update
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            StartMove();
        }

        public void StopMove()
        {
            m_Rigidbody.velocity = Vector2.zero;
        }

        public void StartMove()
        {
            m_Rigidbody.velocity = Vector2.left * m_LevelSpeed;
        }
    }
}