using UnityEngine;

namespace Aetherium.Gameplay
{
    public class AetheriumHUD : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public AetheriumSurvivalManager Survival;
        public AetheriumInventorySystem Inventory;
        public AetheriumWorldRuntime Runtime;

        private void Update()
        {
            if (Survival == null && AetheriumWorldRuntime.Instance != null)
            {
                Survival = AetheriumWorldRuntime.Instance.Survival;
            }

            if (Inventory == null && AetheriumWorldRuntime.Instance != null)
            {
                Inventory = AetheriumWorldRuntime.Instance.Inventory;
            }

            if (Runtime == null)
            {
                Runtime = AetheriumWorldRuntime.Instance;
            }
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(16f, 16f, 300f, 180f), "Aetherium");

            string healthText = Runtime != null && Runtime.Player != null ? $"Health: {Runtime.Player.CurrentHealth}/{Runtime.Player.MaxHealth}" : "Health: --";
            string oxygenText = Survival != null ? $"Oxygen: {Survival.CurrentOxygen:0}/{Survival.MaxOxygen:0}" : "Oxygen: --";
            string staminaText = Survival != null ? $"Stamina: {Survival.CurrentStamina:0}/{Survival.MaxStamina:0}" : "Stamina: --";
            string hungerText = Survival != null ? $"Hunger: {Survival.CurrentHunger:0}/{Survival.MaxHunger:0}" : "Hunger: --";
            string scrapText = Inventory != null ? $"Scrap: {Inventory.Scrap}" : "Scrap: --";
            string threatText = Runtime != null ? $"Threat: {Runtime.ThreatLevel}" : "Threat: --";
            string waveText = Runtime != null ? $"Wave: {Runtime.CurrentWave}" : "Wave: --";
            string enemiesText = Runtime != null ? $"Enemies: {Runtime.ActiveEnemyCount}" : "Enemies: --";
            string objectiveText = AetheriumWorldRuntime.Instance != null && AetheriumWorldRuntime.Instance.Mission != null ? AetheriumWorldRuntime.Instance.Mission.ObjectiveSummary : "Objective: --";

            GUI.Label(new Rect(28f, 32f, 260f, 20f), healthText);
            GUI.Label(new Rect(28f, 52f, 260f, 20f), oxygenText);
            GUI.Label(new Rect(28f, 72f, 260f, 20f), staminaText);
            GUI.Label(new Rect(28f, 92f, 260f, 20f), hungerText);
            GUI.Label(new Rect(28f, 112f, 260f, 20f), scrapText);
            GUI.Label(new Rect(28f, 132f, 260f, 20f), waveText);
            GUI.Label(new Rect(28f, 152f, 260f, 20f), enemiesText);
            GUI.Label(new Rect(16f, 198f, 320f, 24f), objectiveText);
        }
    }
}
