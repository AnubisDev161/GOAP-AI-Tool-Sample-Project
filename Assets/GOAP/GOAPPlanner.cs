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

            // action names
            string collectWater = "collectWater";
            string collectFromSupermarketWater = "collectFromSupermarketWater";
            string boilWater = "boilWater";
            string brewTea = "brewTea";
            string findWaterBottle = "findWaterBottle";
            string getWaterBottleFromGod = "getWaterBottleFromGod";

            // state names
            string hasKettle = "hasKettle";
            string hasWater = "hasWater";
            string hasTea = "hasTea";
            string KettleBoiled = "KettleBoiled";

            availableActions = new List<GOAPAction>();



            ////////////////////////////// // OLD TEST
            //{
            //    availableActions = new List<GOAPAction>();

            //    // collect water from supermarket
            //    Dictionary<string, bool> collectWaterFromSupermarketPreconditions = new Dictionary<string, bool>();
            //    collectWaterFromSupermarketPreconditions.Add(hasKettle, true);

            //    Dictionary<string, bool> collectWaterFromSupermarketEffects = new Dictionary<string, bool>();
            //    collectWaterFromSupermarketEffects.Add(hasWater, true);
            //    availableActions.Add(new GOAPAction(collectWaterFromSupermarketPreconditions, collectWaterFromSupermarketEffects, collectFromSupermarketWater, 100));

            //    // collect water
            //    Dictionary<string, bool> collectWaterPreconditions = new Dictionary<string, bool>();
            //    collectWaterPreconditions.Add(hasKettle, true);

            //    Dictionary<string, bool> collectWaterEffects = new Dictionary<string, bool>();
            //    collectWaterEffects.Add(hasWater, true);
            //    availableActions.Add(new GOAPAction(collectWaterPreconditions, collectWaterEffects, collectWater));



            //    // boil water
            //    Dictionary<string, bool> boilWaterPreconditions = new Dictionary<string, bool>();
            //    boilWaterPreconditions.Add(hasWater, true);

            //    Dictionary<string, bool> boilWaterEffects = new Dictionary<string, bool>();
            //    boilWaterEffects.Add(KettleBoiled, true);
            //    availableActions.Add(new GOAPAction(boilWaterPreconditions, boilWaterEffects, boilWater, 2));

            //    // brew tea 
            //    Dictionary<string, bool> brewTeaPreconditions = new Dictionary<string, bool>();
            //    brewTeaPreconditions.Add(KettleBoiled, true);

            //    Dictionary<string, bool> brewTeaEffects = new Dictionary<string, bool>();
            //    brewTeaEffects.Add(hasTea, true);
            //    availableActions.Add(new GOAPAction(brewTeaPreconditions, brewTeaEffects, brewTea));


            //    // findWaterBottle
            //    Dictionary<string, bool> findWaterBottlePre = new Dictionary<string, bool>();


            //    Dictionary<string, bool> findWaterBottleEff = new Dictionary<string, bool>();
            //    findWaterBottleEff.Add(hasWater, true);

            //    availableActions.Add(new GOAPAction(findWaterBottlePre, findWaterBottleEff, findWaterBottle, 100));


            //    // findWaterBottle
            //    Dictionary<string, bool> getWaterBottleFromGodPre = new Dictionary<string, bool>();


            //    Dictionary<string, bool> getWaterBottlFromGodEff = new Dictionary<string, bool>();
            //    getWaterBottlFromGodEff.Add(hasWater, true);

            //    availableActions.Add(new GOAPAction(getWaterBottleFromGodPre, getWaterBottlFromGodEff, getWaterBottleFromGod, 97));
            //}

            // INITIAL WORLD STATE
            Dictionary<string, bool> startState = new Dictionary<string, bool>()
{
    { hasKettle, true },
    { hasWater, false },
    { KettleBoiled, false },
    { hasTea, false }
};

            // GOAL
            Dictionary<string, bool> goal = new Dictionary<string, bool>()
{
    { hasTea, true }
};

            // ACTIONS (your exact syntax)
            availableActions = new List<GOAPAction>();

            // collect water from supermarket (expensive)
            var collectWaterFromSupermarketPre = new Dictionary<string, bool>() { { hasKettle, true } };
            var collectWaterFromSupermarketEff = new Dictionary<string, bool>() { { hasWater, true } };
            availableActions.Add(new GOAPAction(collectWaterFromSupermarketPre, collectWaterFromSupermarketEff, collectFromSupermarketWater, 100));

            // collect water (cheap)
            var collectWaterPre = new Dictionary<string, bool>() { { hasKettle, true } };
            var collectWaterEff = new Dictionary<string, bool>() { { hasWater, true } };
            availableActions.Add(new GOAPAction(collectWaterPre, collectWaterEff, collectWater, 1));

            // boil water
            var boilWaterPre = new Dictionary<string, bool>() { { hasWater, true } };
            var boilWaterEff = new Dictionary<string, bool>() { { KettleBoiled, true } };
            availableActions.Add(new GOAPAction(boilWaterPre, boilWaterEff, boilWater, 2));

            // brew tea
            var brewTeaPre = new Dictionary<string, bool>() { { KettleBoiled, true } };
            var brewTeaEff = new Dictionary<string, bool>() { { hasTea, true } };
            availableActions.Add(new GOAPAction(brewTeaPre, brewTeaEff, brewTea, 1));

            // findWaterBottle (very expensive)
            var findWaterBottlePre = new Dictionary<string, bool>();
            var findWaterBottleEff = new Dictionary<string, bool>() { { hasWater, true } };
            availableActions.Add(new GOAPAction(findWaterBottlePre, findWaterBottleEff, findWaterBottle, 100));

            // getWaterBottleFromGod (medium expensive)
            var getWaterBottleFromGodPre = new Dictionary<string, bool>();
            var getWaterBottleFromGodEff = new Dictionary<string, bool>() { { hasWater, true } };
            availableActions.Add(new GOAPAction(getWaterBottleFromGodPre, getWaterBottleFromGodEff, getWaterBottleFromGod, 97));
        }
        public Queue<GOAPAction> GeneratePlan(GOAPBlackboard blackboard, GOAPGoal goal)
        {
            var actionsPlan = new Queue<GOAPAction>();
            var nodesPlan = tree.GeneratePlan(blackboard, goal, availableActions);

            while (nodesPlan.Count > 0)
            {
                var currentNode = nodesPlan.Dequeue();
                actionsPlan.Enqueue(currentNode.action);
            }

            return actionsPlan;
        }
    }
}
    