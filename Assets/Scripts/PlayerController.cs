using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LFA
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator = null;
        [SerializeField] private float m_JumpForce = 5f;
        [Space]
        [SerializeField] private Vector2 m_CapsuleSlideSize = Vector2.zero;
        [Space]
        [SerializeField] private UnityEvent OnStartGame = null;
        [SerializeField] private UnityEvent OnWinGame = null;
        [SerializeField] private UnityEvent OnDead = null;

        private Vector2 initialCapsuleSize = Vector2.zero;
        private Rigidbody2D m_Rigidbody = null;

        private CapsuleCollider2D m_Capsule = null;
        private SpriteRenderer m_Sprite = null;

        private void Start()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();

            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Capsule = GetComponent<CapsuleCollider2D>();
            m_Sprite = GetComponentInChildren<SpriteRenderer>();

            m_Sprite.enabled = false;
            initialCapsuleSize = m_Capsule.size;

            Time.timeScale = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Time.timeScale = 1;
                    m_Sprite.enabled = true;
                    OnStartGame.Invoke();

                }

                return;
            }

            m_Animator.SetBool("SPACE", false);
            m_Animator.SetBool("GROUND", IsGrounded());

            if (Input.GetKeyDown(KeyCode.S))
            {
                m_Animator.SetTrigger("S_PRESSED");
            }

            if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
                ScaleCapsule(m_CapsuleSlideSize);
            else
                ScaleCapsule(initialCapsuleSize);

            if (Input.GetKeyUp(KeyCode.S))
            {
                m_Animator.SetTrigger("S_RELEASED");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    m_Animator.SetBool("SPACE", true);
                    m_Animator.SetBool("GROUND", false);
                    m_Rigidbody.velocity = Vector2.up * m_JumpForce;
                }

                if (m_Rigidbody.isKinematic)
                    SceneManager.LoadScene("Game");
                
            }

        }

        private bool IsGrounded()
        {
            if (Physics2D.OverlapCircle(transform.position + Vector3.down * 0.015f, 0.01f, Physics2D.AllLayers))
                return true;

            return false;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Obstacle"))
            {
                m_Animator.SetTrigger("GEF");
                m_Rigidbody.isKinematic = true;
                m_Rigidbody.velocity = Vector2.zero;
                OnDead.Invoke();
            }
            else if (collision.CompareTag("Win"))
            {
                m_Animator.SetTrigger("GEV");
                m_Rigidbody.isKinematic = true;
                m_Rigidbody.velocity = Vector2.zero;
                OnWinGame.Invoke();
            }
        }

        private void ScaleCapsule(Vector2 size)
        {
            if (m_Capsule == null || m_Capsule.size == size)
                return;

            m_Capsule.size = size;

            Vector2 offset = m_Capsule.offset;
            offset.y = size.y * 0.5f;

            m_Capsule.offset = offset;
        }
    }
}