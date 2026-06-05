using TinyRPG.Core;
using UnityEngine;


namespace TinyRPG.Gameplay
{
    [RequireComponent(typeof(Health))]
    public class Player : MonoBehaviour
    {
        public float MoveSpeed = 5f;
        public float RotationSpeed = 10f;
        
        public Health PlayerHealth { get; private set; }


        private void Awake()
        {
            PlayerHealth = GetComponent<Health>();
            PlayerHealth.Initialize(100f);
            
            ServiceLocator.Register<Player>(this);
        }


        private void OnDestroy()
        {
            ServiceLocator.Unregister<Player>();
        }
        

        private void Update()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = Vector3.zero;

            // Deplacement relatif à la camera
            if (Camera.main != null)
            {
                Vector3 camForward = Camera.main.transform.forward;
                Vector3 camRight = Camera.main.transform.right;

                camForward.y = 0f;
                camRight.y = 0f;
                
                camForward.Normalize();
                camRight.Normalize();

                moveDir = (camForward * moveZ + camRight * moveX).normalized;
            }
            else
            {
                moveDir = new Vector3(moveX, 0f, moveZ).normalized;
            }

            transform.position += moveDir * MoveSpeed * Time.deltaTime;

            // Rotation du player
            if (moveDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            }
        }
    }
}
