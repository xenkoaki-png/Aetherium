using UnityEngine;

namespace Aetherium.Gameplay
{
    public class AetheriumLootPickup : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public int ScrapValue = 5;
        public string PickupId = "scrap";

        private void OnTriggerEnter(Collider other)
        {
            AetheriumPlayerController player = other.GetComponent<AetheriumPlayerController>();
            if (player == null)
            {
                return;
            }

            AetheriumInventorySystem inventory = GetComponentInParent<AetheriumInventorySystem>();
            if (inventory == null)
            {
                inventory = player.GetComponent<AetheriumInventorySystem>();
            }

            if (inventory != null)
            {
                inventory.AddItem(PickupId, ScrapValue);
            }

            if (AetheriumWorldRuntime.Instance != null)
            {
                AetheriumWorldRuntime.Instance.AddScrap(ScrapValue);
            }

            Destroy(gameObject);
        }
    }
}
