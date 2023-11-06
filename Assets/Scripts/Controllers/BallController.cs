using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Controls the movement and jumping behavior of a ball object with physics.
    /// </summary>
    public class BallController : MonoBehaviour
    {
        /// <summary>Speed at which the ball moves.</summary>
        public float moveSpeed = 5f;
        
        /// <summary>Strength of the jump applied to the ball.</summary>
        public float jumpForce = 5f;
        
        /// <summary>Speed at which the ball rotates.</summary>
        public float rotationSpeed = 100f;

        // Reference to the Rigidbody component attached to the ball.
        private Rigidbody _rb;
        
        // Flag indicating whether the ball is touching the ground.
        private bool _isGrounded;

 
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }


        private void Update()
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                Jump();
            }
        }

        /// <summary>
        /// Moves the ball based on player input.
        /// </summary>
        private void Move()
        {
            var h = Input.GetAxis("Horizontal");
            var v = -Input.GetAxis("Vertical");

            var movement = new Vector3(v, 0.0f, h);
            _rb.AddForce(movement * moveSpeed);

            if (movement == Vector3.zero) return;
            
            var rotationDirection = Vector3.Cross(Vector3.up, movement);
            _rb.AddTorque(rotationDirection * (rotationSpeed * Time.deltaTime));
        }

        /// <summary>
        /// Makes the ball jump if it is grounded.
        /// </summary>
        private void Jump()
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isGrounded = false; 
        }

        /// <summary>
        /// Detects collision with the ground and sets the ball as grounded.
        /// </summary>
        /// <param name="other">The collision event data.</param>
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _isGrounded = true;
            }
        }
    }
}
