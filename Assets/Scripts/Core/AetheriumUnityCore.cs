/*
 * Copyright (c) 2026 Xenko / StudioName. All Rights Reserved.
 * Game: Aetherium (Unity Isometric Sci-Fi RPG Space Opera & Fantasy Isekai)
 * Repository: xenkoaki-png/aetherium
 * Unauthorized copying, decompilation, asset extraction, modification, or
 * redistribution of this file is strictly prohibited unless permitted by LICENSE.txt.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aetherium.Core
{
    public enum SpeciesType
    {
        HumanoidMercenary,
        Corebound,
        AmorphXenomorph,
        Plasmoid,
        ThriKreen,
        Giff,
        Hadozee,
        Autognome
    }

    public enum SkillCategory
    {
        Racial,
        Job,
        Background,
        CyberTech,
        AmorphBiologics
    }

    public enum AcousticMedium
    {
        Atmo,
        Vessel,
        Vacuum
    }

    public enum AirEnvelopeQuality
    {
        Fresh,
        Foul,
        Deadly
    }

    public enum GameStage
    {
        EarlyGame_ScrapyardAwakening,
        MidGame_WildspaceMercenary,
        LateGame_DreadnoughtCommand,
        EndGame_TartarusHiveRaid,
        PostGame_AetheriumAscendance
    }

    [Serializable]
    public struct SkillNodeDefinition : IEquatable<SkillNodeDefinition>
    {
        public string Id;
        public string DisplayName;
        public SkillCategory Category;
        public int Cost;
        public Vector2 UiPosition;

        public SkillNodeDefinition(string id, string displayName, SkillCategory category, int cost, Vector2 uiPosition)
        {
            Id = id;
            DisplayName = displayName;
            Category = category;
            Cost = cost;
            UiPosition = uiPosition;
        }

        public bool Equals(SkillNodeDefinition other)
        {
            return string.Equals(Id, other.Id, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is SkillNodeDefinition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id != null ? StringComparer.Ordinal.GetHashCode(Id) : 0;
        }
    }

    [Serializable]
    public struct OriginScenario
    {
        public int ScenarioId;
        public string Title;
        public string StartingSector;
        public SpeciesType RecommendedSpecies;
        public string StartingPerkId;
    }

    [Serializable]
    public class AetheriumPlayerStats
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";
        public const int MaxActiveGeneticTraits = 4;
        public const int MaxActiveJobSkills = 6;
        public const int MaxCyberwareLoad = 100;

        public SpeciesType Species = SpeciesType.HumanoidMercenary;

        public int Strength = 10;
        public int Agility = 10;
        public int Endurance = 10;
        public int Intelligence = 10;
        public int AetherResonance = 10;
        public int ProcessingPower = 10;

        public int Level = 1;
        public int ExperiencePoints = 0;
        public int SkillPoints = 3;
        public int Biomass = 0;
        public int EvolutionStage = 1;
        public int PsychicEnergy = 100;
        public int CyberwareLoad = 0;
        public int MagicResonance = 0;

        public bool IsDisguisedAsHuman = false;
        public bool PlasmoidSqueezeActive = false;
        public bool IsAssimilatedWithShip = false;

        public GameStage CurrentStage { get; private set; } = GameStage.EarlyGame_ScrapyardAwakening;

        private readonly HashSet<string> _unlockedNodes = new HashSet<string>();

        public int GetModifier(int attributeScore)
        {
            return Mathf.FloorToInt((attributeScore - 10) / 2f);
        }

        public int MaxIntegrityHp => (Endurance * 2) + 10 + (Species == SpeciesType.Corebound ? 20 : 0);

        public int MaxVoidEnergy => (Intelligence * 2) + AetherResonance;

        public float GetPhysicsMassMultiplier()
        {
            return Species == SpeciesType.Giff ? 2.5f : 1.0f;
        }

        public float GetColliderHeightBlocks()
        {
            if (Species == SpeciesType.Plasmoid && PlasmoidSqueezeActive)
            {
                return 0.125f;
            }

            if (Species == SpeciesType.AmorphXenomorph && EvolutionStage >= 3)
            {
                return 3.5f;
            }

            return IsDisguisedAsHuman ? 1.8f : 1.95f;
        }

        public bool VacuumOrDeadlyAirImmune => Species == SpeciesType.Corebound || Species == SpeciesType.Autognome;

        public bool CanInstallCyberware(int additionalLoad)
        {
            return MagicResonance == 0 && (CyberwareLoad + additionalLoad) <= MaxCyberwareLoad;
        }

        public bool TryUnlockSkillNode(SkillNodeDefinition node)
        {
            if (node.Id == null || _unlockedNodes.Contains(node.Id))
            {
                return false;
            }

            if (SkillPoints < node.Cost)
            {
                return false;
            }

            SkillPoints -= node.Cost;
            _unlockedNodes.Add(node.Id);
            return true;
        }

        public bool FuseGeneticTraits(string traitA, string traitB, string fusedOutput)
        {
            if (string.IsNullOrEmpty(traitA) || string.IsNullOrEmpty(traitB) || string.IsNullOrEmpty(fusedOutput))
            {
                return false;
            }

            if (!_unlockedNodes.Contains(traitA) || !_unlockedNodes.Contains(traitB))
            {
                return false;
            }

            _unlockedNodes.Remove(traitA);
            _unlockedNodes.Remove(traitB);
            _unlockedNodes.Add(fusedOutput);
            return true;
        }

        public GameStage EvaluateProgressionStage()
        {
            if (IsAssimilatedWithShip && EvolutionStage >= 3)
            {
                CurrentStage = GameStage.LateGame_DreadnoughtCommand;
                return CurrentStage;
            }

            if (Level >= 12 || EvolutionStage >= 4)
            {
                CurrentStage = GameStage.EndGame_TartarusHiveRaid;
                return CurrentStage;
            }

            if (Level >= 6 || EvolutionStage >= 3)
            {
                CurrentStage = GameStage.MidGame_WildspaceMercenary;
                return CurrentStage;
            }

            if (Level >= 2)
            {
                CurrentStage = GameStage.EarlyGame_ScrapyardAwakening;
                return CurrentStage;
            }

            CurrentStage = GameStage.EarlyGame_ScrapyardAwakening;
            return CurrentStage;
        }
    }

    public class AirEnvelopeComponent : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float Toxicity = 0.05f;

        public float VolumeCubicMeters = 250f;

        public AirEnvelopeQuality Quality
        {
            get
            {
                if (Toxicity <= 0.30f)
                {
                    return AirEnvelopeQuality.Fresh;
                }

                if (Toxicity <= 0.70f)
                {
                    return AirEnvelopeQuality.Foul;
                }

                return AirEnvelopeQuality.Deadly;
            }
        }

        public static float ComputeMergedToxicity(AirEnvelopeComponent a, AirEnvelopeComponent b)
        {
            if (a == null && b == null)
            {
                return 0f;
            }

            if (a == null)
            {
                return b.Toxicity;
            }

            if (b == null)
            {
                return a.Toxicity;
            }

            float volumes = a.VolumeCubicMeters + b.VolumeCubicMeters;
            float merged = ((a.Toxicity * a.VolumeCubicMeters) + (b.Toxicity * b.VolumeCubicMeters)) / Mathf.Max(1f, volumes);
            return Mathf.Clamp01(merged);
        }
    }

    public static class AcousticMediumHelper
    {
        public static AcousticMedium DetectMedium(Vector3 worldPos, bool hasOverheadHullWithin12m, bool isSpaceOrOrbitDimension)
        {
            if (hasOverheadHullWithin12m)
            {
                return AcousticMedium.Vessel;
            }

            if (isSpaceOrOrbitDimension || worldPos.y > 280f)
            {
                return AcousticMedium.Vacuum;
            }

            return AcousticMedium.Atmo;
        }

        public static string BuildSoundKey(string weaponSoundEvent, AcousticMedium medium)
        {
            string suffix = medium switch
            {
                AcousticMedium.Atmo => "atmo",
                AcousticMedium.Vessel => "vessel",
                AcousticMedium.Vacuum => "vacuum",
                _ => "atmo"
            };

            return "lasermod:" + weaponSoundEvent + "." + suffix;
        }
    }

    public interface IShipPhysicsBridge
    {
        Vector3 ComputeBilateralGravityVector(Transform ship, Vector3 worldPosition, float deckGravityAcceleration);
    }

    public class RigidBodyShipPhysicsBridge : IShipPhysicsBridge
    {
        public Vector3 ComputeBilateralGravityVector(Transform ship, Vector3 worldPosition, float deckGravityAcceleration)
        {
            if (ship == null)
            {
                return Vector3.zero;
            }

            Vector3 localPos = ship.InverseTransformPoint(worldPosition);
            if (localPos.y >= 0f)
            {
                return -ship.up * deckGravityAcceleration;
            }

            return ship.up * deckGravityAcceleration;
        }
    }

    public class AetheriumIsometricController : MonoBehaviour
    {
        public Vector3 IsoForward => new Vector3(1f, 0f, 1f).normalized;
        public Vector3 IsoRight => new Vector3(1f, 0f, -1f).normalized;

        public CharacterController Controller;
        public float MoveSpeed = 6f;

        private void Awake()
        {
            if (Controller == null)
            {
                Controller = GetComponent<CharacterController>();
            }
        }

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            if (Controller == null)
            {
                return;
            }

            Vector3 move = (IsoRight * h + IsoForward * v).normalized * MoveSpeed * Time.deltaTime;
            Controller.Move(move);
        }

        public bool FireCoherentPlasmaCannon(Vector3 aimDirection, bool isSpaceDimension, out RaycastHit hitInfo, out string triggeredSoundKey)
        {
            hitInfo = default;

            bool hasOverheadHullWithin12m = Physics.Raycast(transform.position, Vector3.up, 12f);
            AcousticMedium medium = AcousticMediumHelper.DetectMedium(transform.position, hasOverheadHullWithin12m, isSpaceDimension);
            triggeredSoundKey = AcousticMediumHelper.BuildSoundKey("cannon.airy_random", medium);

            Vector3 firingDirection = aimDirection.normalized;
            Vector3 origin = transform.position + (Vector3.up * 0.5f);
            Ray ray = new Ray(origin, firingDirection);

            return Physics.Raycast(ray, out hitInfo, 128f);
        }

        [ContextMenu("Run 5-Agent DevDriver Self-Test")]
        public void RunDevDriverAudit()
        {
            Debug.Log("AETHERIUM_MECH_2026_CLEANROOM_SIG");

            var stats = new AetheriumPlayerStats
            {
                Species = SpeciesType.Plasmoid,
                PlasmoidSqueezeActive = true
            };

            Debug.Log("SpeciesStats: " + stats.Species + " / colliderHeight=" + stats.GetColliderHeightBlocks());
            Debug.Log("VacuumSoundKey: " + AcousticMediumHelper.BuildSoundKey("cannon.airy_random", AcousticMedium.Vacuum));
            Debug.Log("Y320VacuumSoundKey: " + AcousticMediumHelper.BuildSoundKey("cannon.airy_random", AcousticMediumHelper.DetectMedium(new Vector3(0f, 320f, 0f), false, true)));
            Debug.Log("AuthorFingerprint: " + AetheriumPlayerStats.AuthorFingerprint);
        }
    }
}
