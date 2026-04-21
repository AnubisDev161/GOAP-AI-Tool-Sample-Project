using GOAP.Tree;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class GOAPPlanner
    {
        private GOAPTree tree;

        private List<GOAPAction> availableActions;
        public GOAPPlanner()
        {
            tree = new GOAPTree();
            // ---------------------------
            // ACTION NAMES
            // ---------------------------
            string gatherWood = "gatherWood";
            string makeFire = "makeFire";
            string collectBurningTorch = "collectBurningTorch";
            string prayForALightning = "prayForALightning";
            string useMagic = "useMagic";
            string wanderInRandomDirection = "wanderInRandomDirection";
            string wanderEvenFurtherAway = "wanderEvenFurtherAway";

            // ---------------------------
            // STATE NAMES
            // ---------------------------
            string hasWood = "hasWood";
            string hasFire = "hasFire";

            string isPagen = "isPagen";
            string isNearTown = "isNearTown";
            string isStupid = "isStupid";
            string isRetarded = "isRetarded";


            // ---------------------------
            // AVAILABLE ACTIONS
            // ---------------------------
            availableActions = new List<GOAPAction>();

            // ---------------------------
            // 1.Gather wood
            // ---------------------------
            Dictionary<string, bool> gatherWoodPre = new Dictionary<string, bool>();

            Dictionary<string, bool> gatherWoodEff = new Dictionary<string, bool>();
            gatherWoodEff.Add(hasWood, true);

            availableActions.Add(new GOAPAction(gatherWoodPre, gatherWoodEff, gatherWood, 1));



            // ---------------------------
            // 2.Make fire
            // ---------------------------
            Dictionary<string, bool> makeFirePre = new Dictionary<string, bool>();
            makeFirePre.Add(hasWood, true);

            Dictionary<string, bool> makeFireEff = new Dictionary<string, bool>();
            makeFireEff.Add(hasFire, true);

            availableActions.Add(new GOAPAction(makeFirePre, makeFireEff, makeFire, 1));

            // ---------------------------
            // 2. collectBurningTorch
            // ---------------------------
            Dictionary<string, bool> collectBurningTorchePre = new Dictionary<string, bool>();
            collectBurningTorchePre.Add(isNearTown, true);

            Dictionary<string, bool> collectBurningTorchEff = new Dictionary<string, bool>();
            collectBurningTorchEff.Add(hasFire, true);

            availableActions.Add(new GOAPAction(collectBurningTorchePre, collectBurningTorchEff, collectBurningTorch, 1));

            // ---------------------------
            // 2. prayForALightning
            // ---------------------------
            Dictionary<string, bool> prayForALightningPre = new Dictionary<string, bool>();
            prayForALightningPre.Add(isPagen, true);

            Dictionary<string, bool> prayForALightningEff = new Dictionary<string, bool>();
            prayForALightningEff.Add(hasFire, true);

            availableActions.Add(new GOAPAction(prayForALightningPre, prayForALightningEff, prayForALightning, 1));

            // ---------------------------
            // 2. useMagic
            // ---------------------------
            Dictionary<string, bool> useMagicPre = new Dictionary<string, bool>();

            Dictionary<string, bool> useMagicEff = new Dictionary<string, bool>();
            useMagicEff.Add(hasFire, true);

            availableActions.Add(new GOAPAction(useMagicPre, useMagicEff, useMagic, 1));

            // ---------------------------
            // 2. wanderInRandomDirection
            // ---------------------------
            Dictionary<string, bool> wanderInRandomDirectionPre = new Dictionary<string, bool>();
            wanderInRandomDirectionPre.Add(hasWood, true);
            Dictionary<string, bool> wanderInRandomDirectionEff = new Dictionary<string, bool>();
            wanderInRandomDirectionEff.Add(isStupid, true);

            availableActions.Add(new GOAPAction(wanderInRandomDirectionPre, wanderInRandomDirectionEff, wanderInRandomDirection, 1));

            // ---------------------------
            // 2. wanderEvenFurtherAway
            // ---------------------------
            Dictionary<string, bool> wanderEvenFurtherAwayPre = new Dictionary<string, bool>();
            wanderEvenFurtherAwayPre.Add(isStupid, true);

            Dictionary<string, bool> wanderEvenFurtherAwayEff = new Dictionary<string, bool>();
            wanderEvenFurtherAwayEff.Add(isRetarded, true);

            availableActions.Add(new GOAPAction(wanderEvenFurtherAwayPre, wanderEvenFurtherAwayEff, wanderEvenFurtherAway, 1));

            //string hasWood = "hasWood";
            //string hasFire = "hasFire";
            //string isPagen = "isPagen";
            //string isNearTown = "isNearTown";
            //string hasFlint = "hasFlint";
            //string hasSteel = "hasSteel";
            //string hasDryGrass = "hasDryGrass";
            //string hasTorch = "hasTorch";
            //string isRaining = "isRaining";
            //string hasMagicCrystal = "hasMagicCrystal";
            //string isAngryGod = "isAngryGod";
            //string hasOil = "hasOil";
            //string hasLantern = "hasLantern";
            //string lanternFilled = "lanternFilled";
            //string lanternLit = "lanternLit";

            //ACTION NAMES
            // ---------------------------
            //string gatherWood = "gatherWood";
            //string makeFire = "makeFire";
            //string collectBurningTorch = "collectBurningTorch";
            //string prayForALightning = "prayForALightning";
            //string useMagic = "useMagic";

            //availableActions = new List<GOAPAction>();

            //Dictionary<string, bool> gatherWoodPre = new Dictionary<string, bool>();

            //Dictionary<string, bool> gatherWoodEff = new Dictionary<string, bool>();
            //gatherWoodEff.Add(hasWood, true);

            //availableActions.Add(new GOAPAction(gatherWoodPre, gatherWoodEff, gatherWood, 100));

            //Dictionary<string, bool> makeFirePre = new Dictionary<string, bool>();
            //makeFirePre.Add(hasWood, true);

            //Dictionary<string, bool> makeFireEff = new Dictionary<string, bool>();
            //makeFireEff.Add(hasFire, true);

            //availableActions.Add(new GOAPAction(makeFirePre, makeFireEff, makeFire, 100));

            //Dictionary<string, bool> collectBurningTorchePre = new Dictionary<string, bool>();
            //collectBurningTorchePre.Add(isNearTown, true);

            //Dictionary<string, bool> collectBurningTorchEff = new Dictionary<string, bool>();
            //collectBurningTorchEff.Add(hasFire, true);

            //availableActions.Add(new GOAPAction(collectBurningTorchePre, collectBurningTorchEff, collectBurningTorch, 50));

            //Dictionary<string, bool> prayForALightningPre = new Dictionary<string, bool>();
            //prayForALightningPre.Add(isPagen, true);

            //Dictionary<string, bool> prayForALightningEff = new Dictionary<string, bool>();
            //prayForALightningEff.Add(hasFire, true);

            //availableActions.Add(new GOAPAction(prayForALightningPre, prayForALightningEff, prayForALightning, 10));

            //Dictionary<string, bool> useMagicPre = new Dictionary<string, bool>();
            //useMagicPre.Add(isAngryGod, false);

            //Dictionary<string, bool> useMagicEff = new Dictionary<string, bool>();
            //useMagicEff.Add(hasFire, true);

            //availableActions.Add(new GOAPAction(useMagicPre, useMagicEff, useMagic, 10));

            //Dictionary<string, bool> gatherFlintPre = new Dictionary<string, bool>();

            //Dictionary<string, bool> gatherFlintEff = new Dictionary<string, bool>();
            //gatherFlintEff.Add(hasFlint, true);

            //availableActions.Add(new GOAPAction(gatherFlintPre, gatherFlintEff, "gatherFlint", 40));

            //Dictionary<string, bool> gatherSteelPre = new Dictionary<string, bool>();

            //Dictionary<string, bool> gatherSteelEff = new Dictionary<string, bool>();
            //gatherSteelEff.Add(hasSteel, true);

            //availableActions.Add(new GOAPAction(gatherSteelPre, gatherSteelEff, "gatherSteel", 40));

            //Dictionary<string, bool> gatherDryGrassPre = new Dictionary<string, bool>();

            //Dictionary<string, bool> gatherDryGrassEff = new Dictionary<string, bool>();
            //gatherDryGrassEff.Add(hasDryGrass, true);

            //availableActions.Add(new GOAPAction(gatherDryGrassPre, gatherDryGrassEff, "gatherDryGrass", 20));

            //Dictionary<string, bool> strikeFlintSteelPre = new Dictionary<string, bool>();
            //strikeFlintSteelPre.Add(hasFlint, true);
            //strikeFlintSteelPre.Add(hasSteel, true);
            //strikeFlintSteelPre.Add(hasDryGrass, true);

            //Dictionary<string, bool> strikeFlintSteelEff = new Dictionary<string, bool>();
            //strikeFlintSteelEff.Add(hasFire, true);

            //availableActions.Add(new GOAPAction(strikeFlintSteelPre, strikeFlintSteelEff, "strikeFlintSteel", 30));

            //Dictionary<string, bool> findTorchPre = new Dictionary<string, bool>();

            //Dictionary<string, bool> findTorchEff = new Dictionary<string, bool>();
            //findTorchEff.Add(hasTorch, true);

            //availableActions.Add(new GOAPAction(findTorchPre, findTorchEff, "findTorch", 25));

            //Dictionary<string, bool> lightTorchPre = new Dictionary<string, bool>();
            //lightTorchPre.Add(hasTorch, true);
            //lightTorchPre.Add(hasFire, true); // must already have fire

            //Dictionary<string, bool> lightTorchEff = new Dictionary<string, bool>();
            //lightTorchEff.Add(hasFire, true); // redundant but valid

            //availableActions.Add(new GOAPAction(lightTorchPre, lightTorchEff, "lightTorch", 5));

            //Dictionary<string, bool> gatherOilPre = new Dictionary<string, bool>();

            //Dictionary<string, bool> gatherOilEff = new Dictionary<string, bool>();
            //gatherOilEff.Add(hasOil, true);

            //availableActions.Add(new GOAPAction(gatherOilPre, gatherOilEff, "gatherOil", 30));

            //Dictionary<string, bool> findLanternPre = new Dictionary<string, bool>();

            //Dictionary<string, bool> findLanternEff = new Dictionary<string, bool>();
            //findLanternEff.Add(hasLantern, true);

            //availableActions.Add(new GOAPAction(findLanternPre, findLanternEff, "findLantern", 30));


            //Dictionary<string, bool> fillLanternPre = new Dictionary<string, bool>();
            //fillLanternPre.Add(hasLantern, true);
            //fillLanternPre.Add(hasOil, true);

            //Dictionary<string, bool> fillLanternEff = new Dictionary<string, bool>();
            //fillLanternEff.Add(lanternFilled, true);

            //availableActions.Add(new GOAPAction(fillLanternPre, fillLanternEff, "fillLantern", 15));

            //Dictionary<string, bool> lightLanternPre = new Dictionary<string, bool>();
            //lightLanternPre.Add(lanternFilled, true);

            //Dictionary<string, bool> lightLanternEff = new Dictionary<string, bool>();
            //lightLanternEff.Add(lanternLit, true);
            //lightLanternEff.Add(hasFire, true);

            //availableActions.Add(new GOAPAction(lightLanternPre, lightLanternEff, "lightLantern", 20));

            //Dictionary<string, bool> findMagicCrystalPre = new Dictionary<string, bool>();

            //Dictionary<string, bool> findMagicCrystalEff = new Dictionary<string, bool>();
            //findMagicCrystalEff.Add(hasMagicCrystal, true);

            //availableActions.Add(new GOAPAction(findMagicCrystalPre, findMagicCrystalEff, "findMagicCrystal", 80));

            //Dictionary<string, bool> crystalFirePre = new Dictionary<string, bool>();
            //crystalFirePre.Add(hasMagicCrystal, true);

            //Dictionary<string, bool> crystalFireEff = new Dictionary<string, bool>();
            //crystalFireEff.Add(hasFire, true);

            //availableActions.Add(new GOAPAction(crystalFirePre, crystalFireEff, "crystalFire", 5));

            //Dictionary<string, bool> angerGodsPre = new Dictionary<string, bool>();

            //Dictionary<string, bool> angerGodsEff = new Dictionary<string, bool>();
            //angerGodsEff.Add(isAngryGod, true);

            //availableActions.Add(new GOAPAction(angerGodsPre, angerGodsEff, "angerGods", 1));

            //Dictionary<string, bool> calmGodsPre = new Dictionary<string, bool>();
            //calmGodsPre.Add(isAngryGod, true);

            //Dictionary<string, bool> calmGodsEff = new Dictionary<string, bool>();
            //calmGodsEff.Add(isAngryGod, false);

            //availableActions.Add(new GOAPAction(calmGodsPre, calmGodsEff, "calmGods", 1000000000000000));

        }

        public Queue<GOAPAction> GeneratePlan(WorldState blackboard, GOAPGoal goal)
        {
            return tree.GeneratePlan(blackboard, goal, availableActions);
        }
    }
}
    