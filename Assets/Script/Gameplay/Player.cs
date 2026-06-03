using TinyRPG.Core;
using UnityEngine;

namespace TinyRPG.Gameplay
{
    public class Player : MonoBehaviour
    {
        public float MoveSpeed = 5f;

        private void Awake()
        {
            // On s'enregistre dans le ServiceLocator pour que les ennemis puissent nous trouver !
            ServiceLocator.Register<Player>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<Player>();
        }

        private void Update()
        {
            // Déplacement très basique
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;
            transform.position += moveDir * MoveSpeed * Time.deltaTime;
        }
    }
}
