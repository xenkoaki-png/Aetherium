using UnityEngine;

namespace Aetherium.Gameplay
{
    public class AetheriumWaveManager : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        [Header("Wave Control")]
        public int CurrentWave = 1;
        public int BaseEnemiesPerWave = 4;
        public float SpawnRadius = 12f;
        public float TimeBetweenWaves = 2.5f;
        public int MaxConcurrentEnemies = 12;

        [Header("Runtime")]
        public bool AutoStart = true;

        private float _nextWaveTime;
        private bool _waveActive;
        private int _remainingEnemies;

        private void Start()
        {
            if (AetheriumWorldRuntime.Instance != null)
            {
                AetheriumWorldRuntime.Instance.RegisterWaveManager(this);
            }

            if (AutoStart)
            {
                BeginWave();
            }
        }

        private void Update()
        {
            if (!_waveActive)
            {
                if (Time.time >= _nextWaveTime)
                {
                    BeginWave();
                }

                return;
            }

            if (_remainingEnemies <= 0 && FindObjectsOfType<AetheriumEnemyController>().Length == 0)
            {
                CompleteWave();
            }
        }

        public void BeginWave()
        {
            _waveActive = true;
            _remainingEnemies = Mathf.Min(BaseEnemiesPerWave + (CurrentWave - 1) * 2, MaxConcurrentEnemies);

            var player = FindObjectOfType<AetheriumPlayerController>();
            if (player == null)
            {
                return;
            }

            int enemiesToSpawn = _remainingEnemies;
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Vector3 offset = new Vector3(
                    Random.Range(-SpawnRadius, SpawnRadius),
                    0.5f,
                    Random.Range(-SpawnRadius, SpawnRadius));

                GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                enemy.name = "AetheriumWaveEnemy";
                enemy.transform.position = player.transform.position + offset;

                var health = enemy.AddComponent<AetheriumEntityHealth>();
                health.MaxHealth = 45 + CurrentWave * 12;
                health.CurrentHealth = health.MaxHealth;

                var controller = enemy.AddComponent<AetheriumEnemyController>();
                controller.Target = player.transform;
                controller.DetectionRange = 12f + CurrentWave;
                controller.AttackRange = 1.8f;
                controller.MoveSpeed = 2.3f + CurrentWave * 0.15f;
                controller.DamagePerHit = 10 + CurrentWave * 3;
                controller.HitFlashColor = new Color(1f, 0.7f, 0.5f, 1f);
            }

            Debug.Log("Wave " + CurrentWave + " begins with " + enemiesToSpawn + " enemies.");
        }

        public void CompleteWave()
        {
            _waveActive = false;
            CurrentWave++;
            _nextWaveTime = Time.time + TimeBetweenWaves;

            if (AetheriumWorldRuntime.Instance != null)
            {
                AetheriumWorldRuntime.Instance.OnWaveCleared(CurrentWave - 1);
            }

            Debug.Log("Wave cleared. Next wave: " + CurrentWave);
        }
    }
}
