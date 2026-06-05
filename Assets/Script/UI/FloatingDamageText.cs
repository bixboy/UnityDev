using UnityEngine;
using TMPro;
using TinyRPG.Core;

namespace TinyRPG.UI
{

    public class FloatingDamageText : MonoBehaviour, IPoolable
    {
        public TextMeshPro TextComponent;
        public float MoveSpeed = 2f;
        public float FadeSpeed = 2f;
        public float LifeTime = 1f;

        private float _timer;
        private Color _originalColor;
        private Transform _mainCameraTransform;

        private void Awake()
        {
            if (TextComponent != null)
            {
                _originalColor = TextComponent.color;
            }
            
            if (Camera.main != null)
            {
                _mainCameraTransform = Camera.main.transform;
            }
        }

        public void Setup(float damageAmount)
        {
            if (TextComponent != null)
            {
                TextComponent.text = Mathf.RoundToInt(damageAmount).ToString();
                TextComponent.color = _originalColor;
            }
        }

        public void OnSpawned()
        {
            _timer = 0f;
            
            if (TextComponent != null)
            {
                TextComponent.color = _originalColor;
            }
        }

        public void OnDespawned()
        {
        }

        private void Update()
        {
            transform.position += Vector3.up * MoveSpeed * Time.deltaTime;

            if (_mainCameraTransform != null)
            {
                transform.rotation = Quaternion.LookRotation(transform.position - _mainCameraTransform.position);
            }

            if (TextComponent != null)
            {
                Color c = TextComponent.color;
                c.a -= FadeSpeed * Time.deltaTime;
                TextComponent.color = c;
            }

            _timer += Time.deltaTime;
            if (_timer >= LifeTime)
            {
                PoolManager pool = ServiceLocator.Get<PoolManager>();
                if (pool != null)
                {
                    pool.Despawn("FloatingText", gameObject); 
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
