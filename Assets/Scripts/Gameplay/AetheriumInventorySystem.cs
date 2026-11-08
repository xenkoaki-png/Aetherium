using UnityEngine;

namespace Aetherium.Gameplay
{
    public class AetheriumInventorySystem : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public int Scrap;
        public int Components;
        public int Medkits;

        public void AddItem(string itemId, int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            switch (itemId)
            {
                case "scrap":
                    Scrap += amount;
                    break;
                case "component":
                    Components += amount;
                    break;
                case "medkit":
                    Medkits += amount;
                    break;
                default:
                    Scrap += amount;
                    break;
            }
        }

        public bool ConsumeItem(string itemId, int amount = 1)
        {
            if (amount <= 0)
            {
                return true;
            }

            switch (itemId)
            {
                case "scrap":
                    if (Scrap < amount)
                    {
                        return false;
                    }
                    Scrap -= amount;
                    return true;
                case "component":
                    if (Components < amount)
                    {
                        return false;
                    }
                    Components -= amount;
                    return true;
                case "medkit":
                    if (Medkits < amount)
                    {
                        return false;
                    }
                    Medkits -= amount;
                    return true;
                default:
                    return false;
            }
        }
    }
}
