using MergeAndFight.Merge;
using System.Collections.Generic;

namespace MergeAndFight.Fight
{
    public static class CastleParametrsFormula
    {
        public static int GetCastleHealth(List<MergeObject> mergeObjects, int levelCompleteTime)
        {
            int attackSum = 0;
            mergeObjects.ForEach(mergeObject => attackSum += mergeObject.Warrior.AttackPower);

            return attackSum * levelCompleteTime;
        }

        public static int GetCastleAttackPower(List<MergeObject> mergeObjects, int castleWarriorsCount, int levelFailTime, int maxAttackPower)
        {
            int healthSum = 0;
            mergeObjects.ForEach(mergeObject => healthSum += mergeObject.Warrior.Health);

            var attackPower = castleWarriorsCount != 0 && healthSum / (castleWarriorsCount * (levelFailTime / 60f)) > 0 ? (int)(healthSum / (castleWarriorsCount * (levelFailTime / 60f))) : 1;
            return attackPower > maxAttackPower ? maxAttackPower : attackPower;
        }
    }
}
