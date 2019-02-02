using System;

namespace heatingsearcher
{
    public static class Mathf
    {
        /// <summary>
        /// HACK 計算が合っているかどうか、自身がない.
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="fol"></param>
        /// <returns>そのブロックに初めて熱が伝わったかどうか</returns>
        public static bool gatherHeat (HeatMap.Block prev, HeatMap.Block fol)
        {
            float folTemp = 0f;
            folTemp += prev.temp * HeatingSearcher.partialTime / HeatingSearcher.blockSize * (1f - HeatingSearcher.k * prev.byBlocks.Count);
            foreach (var byPrev in prev.byBlocks)
                folTemp += byPrev.temp * HeatingSearcher.partialTime / HeatingSearcher.blockSize * HeatingSearcher.k;

            fol.temp = folTemp;

            return !prev.heatedOnce && fol.heatedOnce;
        }
    }
}