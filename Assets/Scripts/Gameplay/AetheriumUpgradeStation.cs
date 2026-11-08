using UnityEngine;

namespace Aetherium.Gameplay
{
    public class AetheriumUpgradeStation : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public int ScrapCost = 12;
        public int HealthBonus = 15;
        public float MoveSpeedBonus = 0.75f;
        public float DamageBonus = 5f;

        private bool _playerNearby;
        private AetheriumPlayerController _player;

        private void Update()
        {
            if (!_playerNearby || _player == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                TryUpgrade();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            AetheriumPlayerController player = other.GetComponent<AetheriumPlayerController>();
            if (player == null)
            {
                return;
            }

            _playerNearby = true;
            _player = player;
            Debug.Log("Upgrade station access granted. Press E to upgrade.");
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<AetheriumPlayerController>() == null)
            {
                return;
            }

            _playerNearby = false;
            _player = null;
        }

        private void TryUpgrade()
        {
            if (_player == null)
            {
                return;
            }

            AetheriumInventorySystem inventory = _player.GetComponent<AetheriumInventorySystem>();
            if (inventory == null)
            {
                return;
            }

            if (!inventory.ConsumeItem("scrap", ScrapCost))
            {
                Debug.Log("Not enough scrap for an upgrade.");
                return;
            }

            _player.MaxHealth += HealthBonus;
            _player.CurrentHealth = Mathf.Min(_player.MaxHealth, _player.CurrentHealth + HealthBonus);
            _player.BaseMoveSpeed += MoveSpeedBonus;
            _player.CombatSystem.DamagePerSwing = Mathf.RoundToInt(_player.CombatSystem.DamagePerSwing + DamageBonus);

            Debug.Log("Upgrade applied: damage, speed, and survivability increased.");
        }
    }
}
