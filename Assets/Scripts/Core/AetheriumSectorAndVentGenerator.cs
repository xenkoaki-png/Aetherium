/*
 * Copyright (c) 2026 Xenko / StudioName. All Rights Reserved.
 * Game: Aetherium (Unity Isometric Sci-Fi RPG Space Opera & Fantasy Isekai)
 * Repository: xenkoaki-png/aetherium
 * Unauthorized copying, decompilation, asset extraction, modification, or
 * redistribution of this file is strictly prohibited unless permitted by LICENSE.txt.
 */

using System.Collections.Generic;
using UnityEngine;

namespace Aetherium.Core
{
    public enum SectorDimensionId
    {
        Wildspace,
        KharVosh_ScrapyardMoon,
        XylosPrime_ChitinWastes,
        NeoVeridia_CyberSpire,
        AetheriumNexus_ShatteredRing,
        TartarusHive_XenomorphWorld
    }

    public enum TileCellType
    {
        Empty,
        WalkableGround,
        ResonantBulkheadWall,
        CyberSpireVentilationGrate,
        BuddingAetheriumCrystal,
        BridgeAssimilationConsoleBlock,
        AirEnvelopeScrubberBlock,
        BilateralGravityProjectorBlock,
        Debris,
        HazardZone
    }

    [System.Serializable]
    public class GalacticTradeRouteEdge
    {
        public SectorDimensionId From;
        public SectorDimensionId To;
        public int TradeValue;
        public bool IsPirateBlockade;
    }

    public class AetheriumSectorAndVentGenerator : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        public const int GridSize = 24;
        public TileCellType[,] Cells = new TileCellType[GridSize, GridSize];
        public List<GalacticTradeRouteEdge> MacroEconomyGraph = new List<GalacticTradeRouteEdge>();

        private void Awake()
        {
            GenerateSectorGrid();
            InitializeMacroEconomyGraph();
        }

        public void GenerateSectorGrid()
        {
            for (int x = 0; x < GridSize; x++)
            {
                for (int z = 0; z < GridSize; z++)
                {
                    Cells[x, z] = TileCellType.Empty;
                }
            }

            for (int i = 0; i < GridSize; i++)
            {
                Cells[0, i] = TileCellType.ResonantBulkheadWall;
                Cells[GridSize - 1, i] = TileCellType.ResonantBulkheadWall;
                Cells[i, 0] = TileCellType.ResonantBulkheadWall;
                Cells[i, GridSize - 1] = TileCellType.ResonantBulkheadWall;
            }

            for (int x = 1; x < GridSize - 1; x++)
            {
                for (int z = 1; z < GridSize - 1; z++)
                {
                    if (x == 2 || z == 2)
                    {
                        Cells[x, z] = TileCellType.CyberSpireVentilationGrate;
                    }
                    else if ((x + z) % 7 == 0)
                    {
                        Cells[x, z] = TileCellType.BuddingAetheriumCrystal;
                    }
                    else if ((x + z) % 11 == 0)
                    {
                        Cells[x, z] = TileCellType.WalkableGround;
                    }
                    else if (x == GridSize / 2 && z == GridSize / 2)
                    {
                        Cells[x, z] = TileCellType.BridgeAssimilationConsoleBlock;
                    }
                    else
                    {
                        Cells[x, z] = TileCellType.WalkableGround;
                    }
                }
            }

            for (int x = 3; x < GridSize - 3; x++)
            {
                for (int z = 3; z < GridSize - 3; z++)
                {
                    if ((x * z) % 13 == 0)
                    {
                        Cells[x, z] = TileCellType.AirEnvelopeScrubberBlock;
                    }
                }
            }
        }

        public bool CanEntityTraverseCell(TileCellType cellType, AetheriumPlayerStats playerStats)
        {
            if (cellType == TileCellType.ResonantBulkheadWall)
            {
                return false;
            }

            if (cellType == TileCellType.CyberSpireVentilationGrate)
            {
                if (playerStats == null)
                {
                    return false;
                }

                return playerStats.GetColliderHeightBlocks() <= 0.25f;
            }

            return cellType != TileCellType.HazardZone;
        }

        public void ScrubAirEnvelope(AirEnvelopeComponent envelope, float aetherEnergyProvided)
        {
            if (envelope == null)
            {
                return;
            }

            envelope.Toxicity = Mathf.Clamp01(envelope.Toxicity - (0.15f * aetherEnergyProvided));
        }

        public void InitializeMacroEconomyGraph()
        {
            MacroEconomyGraph.Clear();

            SectorDimensionId[] sectors =
            {
                SectorDimensionId.Wildspace,
                SectorDimensionId.KharVosh_ScrapyardMoon,
                SectorDimensionId.XylosPrime_ChitinWastes,
                SectorDimensionId.NeoVeridia_CyberSpire,
                SectorDimensionId.AetheriumNexus_ShatteredRing,
                SectorDimensionId.TartarusHive_XenomorphWorld
            };

            for (int i = 0; i < sectors.Length - 1; i++)
            {
                MacroEconomyGraph.Add(new GalacticTradeRouteEdge
                {
                    From = sectors[i],
                    To = sectors[i + 1],
                    TradeValue = 250 + (i * 75),
                    IsPirateBlockade = i % 2 == 0
                });
            }
        }
    }
}
