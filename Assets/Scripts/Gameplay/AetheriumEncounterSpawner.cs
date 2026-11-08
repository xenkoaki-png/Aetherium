using UnityEngine;

namespace Aetherium.Gameplay
{
    public class AetheriumEncounterSpawner : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public int EnemyCount = 4;
        public int PickupCount = 6;
        public Vector2 SpawnRadius = new Vector2(8f, 12f);

        private bool _spawned;

        private void Start()
        {
            if (_spawned)
            {
                return;
            }

            SpawnEnemies();
            SpawnPickups();
            _spawned = true;
        }

        private void SpawnEnemies()
        {
            var player = FindObjectOfType<AetheriumPlayerController>();
            if (player == null)
            {
                return;
            }

            for (int i = 0; i < EnemyCount; i++)
            {
                Vector3 spawnPosition = player.transform.position + new Vector3(
                    Random.Range(-SpawnRadius.x, SpawnRadius.x),
                    0.5f,
                    Random.Range(-SpawnRadius.y, SpawnRadius.y));

                GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                enemy.name = "AetheriumEnemy";
                enemy.transform.position = spawnPosition;

                var health = enemy.AddComponent<AetheriumEntityHealth>();
                health.MaxHealth = 60;
                health.CurrentHealth = 60;

                var controller = enemy.AddComponent<AetheriumEnemyController>();
                controller.Target = player.transform;
                controller.DetectionRange = 12f;
                controller.AttackRange = 1.6f;
                controller.MoveSpeed = 2.4f;
                controller.DamagePerHit = 12;
            }
        }

        private void SpawnPickups()
        {
            var player = FindObjectOfType<AetheriumPlayerController>();
            if (player == null)
            {
                return;
            }

            for (int i = 0; i < PickupCount; i++)
            {
                Vector3 spawnPosition = player.transform.position + new Vector3(
                    Random.Range(-SpawnRadius.x, SpawnRadius.x),
                    0.4f,
                    Random.Range(-SpawnRadius.y, SpawnRadius.y));

                GameObject pickup = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                pickup.name = "ScrapPickup";
                pickup.transform.position = spawnPosition;
                pickup.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

                var collider = pickup.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.isTrigger = true;
                }

                pickup.AddComponent<AetheriumLootPickup>();
            }
        }
    }
}
