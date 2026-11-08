/*
 * Copyright (c) 2026 Xenko / StudioName. All Rights Reserved.
 * Game: Aetherium (Unity Isometric Sci-Fi RPG Space Opera & Fantasy Isekai)
 * Repository: xenkoaki-png/aetherium
 * Unauthorized copying, decompilation, asset extraction, modification, or
 * redistribution of this file is strictly prohibited unless permitted by LICENSE.txt.
 */

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Aetherium.Core
{
    [Serializable]
    public class SpeciesProfile
    {
        public SpeciesType Species;
        public string DisplayName;
        public float MassMultiplier;
        public float DefaultHeight;
        public float MorphedHeight;
        public bool VacuumImmune;
        public int BonusArmor;
        public int ExtraWeaponLimbs;
    }

    public class AetheriumOriginAndCharacterCreation : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public string OperativeName = "Aetherian";
        public SpeciesType SelectedSpecies = SpeciesType.HumanoidMercenary;
        public int SelectedScenarioIndex = 0;

        private readonly List<SpeciesProfile> _speciesProfiles = new List<SpeciesProfile>();
        private readonly List<OriginScenario> _allScenarios = new List<OriginScenario>();

        public IReadOnlyList<SpeciesProfile> SpeciesProfiles => _speciesProfiles;
        public IReadOnlyList<OriginScenario> AllScenarios => _allScenarios;

        private void Awake()
        {
            InitializeSpeciesProfiles();
            InitializeAll69OriginScenarios();
        }

        public void InitializeSpeciesProfiles()
        {
            _speciesProfiles.Clear();

            _speciesProfiles.Add(new SpeciesProfile
            {
                Species = SpeciesType.Corebound,
                DisplayName = "Corebound (Crystalline AI Core)",
                MassMultiplier = 1.2f,
                DefaultHeight = 1.85f,
                MorphedHeight = 2.40f,
                VacuumImmune = true,
                BonusArmor = 20,
                ExtraWeaponLimbs = 0
            });

            _speciesProfiles.Add(new SpeciesProfile
            {
                Species = SpeciesType.AmorphXenomorph,
                DisplayName = "Amorph / Xenomorph (Apex Bio-Horror)",
                MassMultiplier = 1.35f,
                DefaultHeight = 1.95f,
                MorphedHeight = 3.50f,
                VacuumImmune = false,
                BonusArmor = 15,
                ExtraWeaponLimbs = 1
            });

            _speciesProfiles.Add(new SpeciesProfile
            {
                Species = SpeciesType.Plasmoid,
                DisplayName = "Plasmoid (Amorphous Osmosis Slime)",
                MassMultiplier = 0.85f,
                DefaultHeight = 1.75f,
                MorphedHeight = 0.125f,
                VacuumImmune = false,
                BonusArmor = 5,
                ExtraWeaponLimbs = 1
            });

            _speciesProfiles.Add(new SpeciesProfile
            {
                Species = SpeciesType.ThriKreen,
                DisplayName = "Thri-Kreen (Six-Limbed Chitin Raider)",
                MassMultiplier = 1.1f,
                DefaultHeight = 1.90f,
                MorphedHeight = 1.90f,
                VacuumImmune = false,
                BonusArmor = 12,
                ExtraWeaponLimbs = 2
            });

            _speciesProfiles.Add(new SpeciesProfile
            {
                Species = SpeciesType.Giff,
                DisplayName = "Giff (Heavy Siege Mercenary)",
                MassMultiplier = 2.5f,
                DefaultHeight = 2.30f,
                MorphedHeight = 2.30f,
                VacuumImmune = false,
                BonusArmor = 18,
                ExtraWeaponLimbs = 0
            });

            _speciesProfiles.Add(new SpeciesProfile
            {
                Species = SpeciesType.Hadozee,
                DisplayName = "Hadozee (Wing-Membrane Deck Glider)",
                MassMultiplier = 0.9f,
                DefaultHeight = 1.80f,
                MorphedHeight = 1.80f,
                VacuumImmune = false,
                BonusArmor = 4,
                ExtraWeaponLimbs = 1
            });

            _speciesProfiles.Add(new SpeciesProfile
            {
                Species = SpeciesType.Autognome,
                DisplayName = "Autognome (Clockwork-Aether Engineer)",
                MassMultiplier = 1.4f,
                DefaultHeight = 1.25f,
                MorphedHeight = 1.25f,
                VacuumImmune = true,
                BonusArmor = 16,
                ExtraWeaponLimbs = 0
            });

            _speciesProfiles.Add(new SpeciesProfile
            {
                Species = SpeciesType.HumanoidMercenary,
                DisplayName = "Humanoid Mercenary (Isekai Wildspacer)",
                MassMultiplier = 1.0f,
                DefaultHeight = 1.80f,
                MorphedHeight = 1.80f,
                VacuumImmune = false,
                BonusArmor = 8,
                ExtraWeaponLimbs = 0
            });
        }

        public void InitializeAll69OriginScenarios()
        {
            _allScenarios.Clear();

            string[] sectors =
            {
                "khar_vosh",
                "xylos_prime",
                "neo_veridia",
                "aetherium_nexus",
                "tartarus_hive"
            };

            string[] archetypes =
            {
                "White Room Isekai Transfer",
                "Derelict Freighter Stowaway",
                "Scrapyard Drone Awakening",
                "Grakkan Imperial Deserter",
                "Containment Lab Breach",
                "Syndicate Vent Infiltrator",
                "Orbital Drop Survivor",
                "Shattered Ring Salvager",
                "Astral Helm Apprentice",
                "Deep-Hive Bio-Specimen",
                "Rogue AI Core Conduit",
                "Krynnish Void Privateer",
                "Cryo-Pod Amnesiac",
                "Aether-Crystal Resonator"
            };

            SpeciesType[] speciesCycle =
            {
                SpeciesType.HumanoidMercenary,
                SpeciesType.Corebound,
                SpeciesType.AmorphXenomorph,
                SpeciesType.Plasmoid,
                SpeciesType.ThriKreen,
                SpeciesType.Giff,
                SpeciesType.Hadozee,
                SpeciesType.Autognome
            };

            for (int i = 0; i < 69; i++)
            {
                string archetype = archetypes[i % archetypes.Length];
                string sector = sectors[i % sectors.Length];
                SpeciesType recommended = speciesCycle[i % speciesCycle.Length];

                _allScenarios.Add(new OriginScenario
                {
                    ScenarioId = i + 1,
                    Title = "#" + (i + 1).ToString("D2") + " - " + archetype,
                    StartingSector = sector,
                    RecommendedSpecies = recommended,
                    StartingPerkId = "perk_" + archetype.Replace(" ", "_").ToLowerInvariant()
                });
            }

            string directory = Path.Combine(Application.dataPath, "Resources", "Data");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string savePath = Path.Combine(directory, "origin_scenarios_69.json");
            OriginScenario[] scenarioArray = _allScenarios.ToArray();
            string json = JsonUtility.ToJson(new OriginScenarioListWrapper { scenarios = scenarioArray }, true);
            File.WriteAllText(savePath, json);
        }

        public void ApplyCharacterSelection()
        {
            if (SelectedScenarioIndex < 0 || SelectedScenarioIndex >= _allScenarios.Count)
            {
                SelectedScenarioIndex = 0;
            }

            OriginScenario selected = _allScenarios[SelectedScenarioIndex];
            Debug.Log("Selected origin: " + selected.Title + " | species: " + selected.RecommendedSpecies + " | sector: " + selected.StartingSector);
        }

        private void OnGUI()
        {
            Rect rect = new Rect(40f, 40f, 520f, 720f);
            GUI.Box(rect, "Aetherium Character Creation");

            GUILayout.BeginArea(rect);
            OperativeName = GUILayout.TextField(OperativeName, GUILayout.Width(300f));

            GUILayout.Label("Species:");
            foreach (SpeciesProfile profile in _speciesProfiles)
            {
                if (GUILayout.Button(profile.DisplayName, GUILayout.Width(420f)))
                {
                    SelectedSpecies = profile.Species;
                }
            }

            GUILayout.Space(10f);
            GUILayout.Label("Origin Scenario:");
            int scenarioCount = _allScenarios.Count;
            SelectedScenarioIndex = Mathf.Clamp(SelectedScenarioIndex, 0, Mathf.Max(0, scenarioCount - 1));
            SelectedScenarioIndex = Mathf.RoundToInt(GUILayout.HorizontalSlider(SelectedScenarioIndex, 0f, Mathf.Max(0, scenarioCount - 1), GUILayout.Width(420f)));

            if (SelectedScenarioIndex >= 0 && SelectedScenarioIndex < _allScenarios.Count)
            {
                OriginScenario scenario = _allScenarios[SelectedScenarioIndex];
                GUILayout.Label("Selected: " + scenario.Title);
                GUILayout.Label("Sector: " + scenario.StartingSector);
                GUILayout.Label("Recommended Species: " + scenario.RecommendedSpecies);
            }

            if (GUILayout.Button("Awaken in Aetherium (Apply Build & Spawn)", GUILayout.Width(420f)))
            {
                ApplyCharacterSelection();
            }

            GUILayout.EndArea();
        }

        [Serializable]
        private class OriginScenarioListWrapper
        {
            public OriginScenario[] scenarios = new OriginScenario[0];
        }
    }
}
