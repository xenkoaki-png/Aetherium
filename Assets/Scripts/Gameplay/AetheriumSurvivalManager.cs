using UnityEngine;
using Aetherium.Core;

namespace Aetherium.Gameplay
{
    public class AetheriumSurvivalManager : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        [Header("Vitals")]
        public float MaxOxygen = 100f;
        public float CurrentOxygen = 100f;
        public float MaxStamina = 100f;
        public float CurrentStamina = 100f;
        public float MaxHunger = 100f;
        public float CurrentHunger = 100f;

        [Header("Drain")]
        public float OxygenDrainPerSecond = 1.4f;
        public float HungerDrainPerSecond = 0.18f;
        public float StaminaDrainPerSecond = 15f;
        public float StaminaRecoveryPerSecond = 22f;

        [Header("References")]
        public AetheriumPlayerStats Stats;
        public AirEnvelopeComponent ActiveAirEnvelope;

        private void Start()
        {
            if (Stats == null)
            {
                Stats = new AetheriumPlayerStats();
            }

            if (ActiveAirEnvelope == null)
            {
                ActiveAirEnvelope = GetComponentInChildren<AirEnvelopeComponent>();
            }

            if (AetheriumWorldRuntime.Instance != null)
            {
                AetheriumWorldRuntime.Instance.RegisterSurvival(this);
            }
        }

        private void Update()
        {
            float oxygenMultiplier = 1f;
            if (ActiveAirEnvelope != null)
            {
                switch (ActiveAirEnvelope.Quality)
                {
                    case AirEnvelopeQuality.Foul:
                        oxygenMultiplier = 1.75f;
                        break;
                    case AirEnvelopeQuality.Deadly:
                        oxygenMultiplier = 3f;
                        break;
                    default:
                        oxygenMultiplier = 1f;
                        break;
                }
            }

            CurrentOxygen = Mathf.Clamp(CurrentOxygen - OxygenDrainPerSecond * oxygenMultiplier * Time.deltaTime, 0f, MaxOxygen);
            CurrentHunger = Mathf.Clamp(CurrentHunger - HungerDrainPerSecond * Time.deltaTime, 0f, MaxHunger);

            bool sprinting = Input.GetKey(KeyCode.LeftShift);
            if (sprinting && CurrentStamina > 0f)
            {
                CurrentStamina = Mathf.Clamp(CurrentStamina - StaminaDrainPerSecond * Time.deltaTime, 0f, MaxStamina);
            }
            else
            {
                CurrentStamina = Mathf.Clamp(CurrentStamina + StaminaRecoveryPerSecond * Time.deltaTime, 0f, MaxStamina);
            }

            if (CurrentOxygen <= 0f)
            {
                // Placeholder but usable damage loop for the first playable slice.
                Debug.Log("Asphyxiation warning: vital signs falling.");
            }

            if (CurrentHunger <= 0f)
            {
                Debug.Log("Hunger warning: need recovery before the next encounter.");
            }
        }
    }
}
