using UnityEngine;

namespace Aetherium.Gameplay
{
    public class AetheriumEntityHealth : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        [Header("Vitals")]
        public int MaxHealth = 100;
        public int CurrentHealth = 100;
        public bool DestroyOnDeath = true;

        public bool IsDead => CurrentHealth <= 0;

        public void ApplyDamage(int amount)
        {
            if (IsDead)
            {
                return;
            }

            CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);

            if (IsDead)
            {
                OnDeath();
            }
        }

        public void Heal(int amount)
        {
            if (IsDead)
            {
                return;
            }

            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        }

        protected virtual void OnDeath()
        {
            if (DestroyOnDeath)
            {
                Destroy(gameObject, 0.1f);
            }
        }
    }
}
