using UnityEngine;

namespace Aetherium.Gameplay
{
    public class AetheriumMissionSystem : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public int ScrapGoal = 25;
        public int EnemiesDefeatedGoal = 3;
        public int WaveGoal = 3;

        public int ScrapCollected { get; private set; }
        public int EnemiesDefeated { get; private set; }
        public int WavesCleared { get; private set; }

        public bool MissionComplete => ScrapCollected >= ScrapGoal && EnemiesDefeated >= EnemiesDefeatedGoal && WavesCleared >= WaveGoal;

        public string ObjectiveSummary
        {
            get
            {
                return $"Scrap {Mathf.Min(ScrapCollected, ScrapGoal)}/{ScrapGoal}  |  Kills {Mathf.Min(EnemiesDefeated, EnemiesDefeatedGoal)}/{EnemiesDefeatedGoal}  |  Waves {Mathf.Min(WavesCleared, WaveGoal)}/{WaveGoal}";
            }
        }

        public void RegisterScrap(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            ScrapCollected += amount;
            Debug.Log("Mission scrap progress: " + ScrapCollected + "/" + ScrapGoal);
        }

        public void RegisterEnemyDefeat()
        {
            EnemiesDefeated++;
            Debug.Log("Mission enemy progress: " + EnemiesDefeated + "/" + EnemiesDefeatedGoal);
        }

        public void RegisterWaveClear()
        {
            WavesCleared++;
            Debug.Log("Mission wave progress: " + WavesCleared + "/" + WaveGoal);
        }
    }
}
