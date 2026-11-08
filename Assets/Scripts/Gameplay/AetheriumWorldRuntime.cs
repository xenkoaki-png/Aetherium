using System.Collections.Generic;
using UnityEngine;

namespace Aetherium.Gameplay
{
    public class AetheriumWorldRuntime : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public static AetheriumWorldRuntime Instance { get; private set; }

        [Header("Player + Systems")]
        public AetheriumPlayerController Player;
        public AetheriumInventorySystem Inventory;
        public AetheriumSurvivalManager Survival;
        public AetheriumMissionSystem Mission;
        public AetheriumWaveManager WaveManager;

        [Header("World State")]
        public int ScrapCollected;
        public int ThreatLevel = 1;
        public float WorldTimeMinutes;
        public int CurrentWave = 1;

        private readonly List<AetheriumEnemyController> _enemies = new List<AetheriumEnemyController>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            WorldTimeMinutes += Time.deltaTime / 60f;
            ThreatLevel = Mathf.Clamp(1 + Mathf.FloorToInt(WorldTimeMinutes / 3.5f), 1, 10);
        }

        public void RegisterPlayer(AetheriumPlayerController newPlayer)
        {
            Player = newPlayer;
        }

        public void RegisterInventory(AetheriumInventorySystem newInventory)
        {
            Inventory = newInventory;
        }

        public void RegisterSurvival(AetheriumSurvivalManager newSurvival)
        {
            Survival = newSurvival;
        }

        public void RegisterMission(AetheriumMissionSystem newMission)
        {
            Mission = newMission;
        }

        public void RegisterWaveManager(AetheriumWaveManager newWaveManager)
        {
            WaveManager = newWaveManager;
        }

        public void RegisterEnemy(AetheriumEnemyController enemy)
        {
            if (enemy == null || _enemies.Contains(enemy))
            {
                return;
            }

            _enemies.Add(enemy);
        }

        public void UnregisterEnemy(AetheriumEnemyController enemy)
        {
            if (enemy == null)
            {
                return;
            }

            _enemies.Remove(enemy);
        }

        public void AddScrap(int amount)
        {
            ScrapCollected = Mathf.Max(0, ScrapCollected + amount);
            if (Mission != null)
            {
                Mission.RegisterScrap(amount);
            }
        }

        public void RegisterEnemyDefeat()
        {
            if (Mission != null)
            {
                Mission.RegisterEnemyDefeat();
            }
        }

        public int ActiveEnemyCount => _enemies.Count;

        public void OnWaveCleared(int waveNumber)
        {
            CurrentWave = Mathf.Max(1, waveNumber);
            ThreatLevel = Mathf.Clamp(1 + CurrentWave, 1, 12);

            if (Mission != null)
            {
                Mission.RegisterWaveClear();
            }
        }
    }
}
