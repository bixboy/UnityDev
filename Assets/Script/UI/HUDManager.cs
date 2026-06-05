using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TinyRPG.Core;
using TinyRPG.Gameplay;


namespace TinyRPG.UI
{
    public class HUDManager : MonoBehaviour
    {
        [Header("UI Elements")]
        public Slider HealthBar;
        public TextMeshProUGUI TimerText;

        private float _surviveTime = 0f;
        private Player _player;


        private void Start()
        {
            _player = ServiceLocator.Get<Player>();
            
            if (_player != null && _player.PlayerHealth != null)
            {
                if (HealthBar != null)
                {
                    HealthBar.maxValue = _player.PlayerHealth.MaxHealth;
                    HealthBar.value = _player.PlayerHealth.CurrentHealth;
                }

                _player.PlayerHealth.OnTakeDamage += HandlePlayerTakeDamage;
            }
        }


        private void Update()
        {
            _surviveTime += Time.deltaTime;
            
            if (TimerText != null)
            {
                int minutes = Mathf.FloorToInt(_surviveTime / 60f);
                int seconds = Mathf.FloorToInt(_surviveTime % 60f);
                TimerText.text = $"{minutes:00}:{seconds:00}";
            }
        }


        private void HandlePlayerTakeDamage(float damage)
        {
            if (_player != null && _player.PlayerHealth != null && HealthBar != null)
            {
                HealthBar.value = _player.PlayerHealth.CurrentHealth;
            }
        }
        

        private void OnDestroy()
        {
            if (_player != null && _player.PlayerHealth != null)
            {
                _player.PlayerHealth.OnTakeDamage -= HandlePlayerTakeDamage;
            }
        }
    }
}
