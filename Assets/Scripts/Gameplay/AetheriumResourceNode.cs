using UnityEngine;

namespace Aetherium.Gameplay
{
    public class AetheriumResourceNode : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public int ScrapValue = 6;
        public string NodeType = "scrap";

        private void OnTriggerEnter(Collider other)
        {
            AetheriumPlayerController player = other.GetComponent<AetheriumPlayerController>();
            if (player == null)
            {
                return;
            }

            AetheriumInventorySystem inventory = player.GetComponent<AetheriumInventorySystem>();
            if (inventory != null)
            {
                inventory.AddItem(NodeType, ScrapValue);
            }

            if (AetheriumWorldRuntime.Instance != null)
            {
                AetheriumWorldRuntime.Instance.AddScrap(ScrapValue);
                if (AetheriumWorldRuntime.Instance.Mission != null)
                {
                    AetheriumWorldRuntime.Instance.Mission.RegisterScrap(ScrapValue);
                }
            }

            Destroy(gameObject);
        }
    }
}
