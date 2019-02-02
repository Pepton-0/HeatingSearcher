using System;
using System.Collections.Generic;
using System.Text;
using Maze;

namespace heatingsearcher
{
    /// <summary>
    /// CPU上で動く熱式探索アルゴリズム
    /// </summary>
    public class HeatingSearcher
    {
        /// <summary>
        /// 1フレームは、仮想的に何秒かかった計算となるか
        /// </summary>
        public const float partialTime = 1f;
        /// <summary>
        /// 熱伝導率 0.25未満にすべし。でないと全ての熱が、周囲に逃げてしまうという現象が起きる.
        /// </summary>
        public const float k = 0.23f;
        /// <summary>
        /// 一ブロックの一辺の長さ
        /// </summary>
        public const float blockSize = 1f;
        /// <summary>
        /// 初期の温度
        /// </summary>
        public const float defTemp = 0f;

        private readonly Map original;
        private HeatMap hMapPrev; // 変更元
        private HeatMap hMapFol; // 変更先

        private Queue<HeatMap.Block> heatedBlocks;

        public HeatingSearcher (Map map)
        {
            this.original = map;
            this.hMapPrev = new HeatMap(original, 300f);
            this.hMapFol = new HeatMap(original, 300f);
            this.heatedBlocks = new Queue<HeatMap.Block>();
        }

        public Map Solve ()
        {
            heatedBlocks.Enqueue(hMapPrev.startBlock);

            double indexTime = 0f;
            double calcuTime = 0f;

            bool foundGoal = false;
            int count = 0;
            while (true)
            {
                count++;
                for (int i = 0; i < hMapPrev.blocks.Count; i++)
                {
                    double t1 = Timer.ElapsedMilliSec();
                    var prev = hMapPrev.blocks[i];
                    var fol = hMapFol.blocks[i];

                    double t2 = Timer.ElapsedMilliSec();
                    indexTime += t2 - t1;

                    bool transferredFirst = Mathf.gatherHeat(prev, fol);
                    if (transferredFirst)
                    {
                        heatedBlocks.Enqueue(fol); // 入れるものは、PrevのとFolのとで、交互になるが、ルート生成時には距離で判断するので、多分問題にはならない

                        if (fol.isGoal)
                        {
                            // 修了する
                            foundGoal = true;
                            goto FINISHED;
                        }

                    }
                    double t3 = Timer.ElapsedMilliSec();
                    calcuTime += t3 - t2;
                }

                // 反転させる
                var tmpMap = hMapPrev;
                hMapPrev = hMapFol;
                hMapFol = tmpMap;
            }

            FINISHED:;

            Console.WriteLine("結果報告");
            Console.WriteLine("     総フレーム数: " + count);
            Console.WriteLine("     インデクス: " + indexTime / (double)count);
            Console.WriteLine("     計算: " + calcuTime / (double)count);

            if (foundGoal)
            {
                Console.WriteLine("経路を求められました.");

                var heatedBlocks = this.heatedBlocks.ToArray();
                HeatMap.Block lastBlock = heatedBlocks[heatedBlocks.Length - 1];
                for (int i = heatedBlocks.Length - 2; i >= 0; i--)
                {
                    var block = heatedBlocks[i];
                    if ((lastBlock.pos - block.pos).SqrDistance == 1)
                    {
                        original[block.pos.x, block.pos.y] = Map.ROUTE;
                        lastBlock = block;
                    }
                }

                return original;
            }
            else
            {
                Console.WriteLine("経路を求められませんでした.\n作業内容を表示します.\n");
                Dump();

                return null;
            }
        }

        private void Dump ()
        {
            StringBuilder mapBuilder = new StringBuilder();
            StringBuilder lineBuilder = new StringBuilder(0, original.width);

            for (int y = original.height - 1; y >= 0; y--)
            {
                for (int x = 0; x < original.width; x++)
                {
                    HeatMap.Block block = null;
                    if (original[x, y] == Map.PATH)
                    {
                        foreach (var b in hMapFol.blocks)
                        {
                            if (x == b.pos.x && y == b.pos.y)
                            {
                                block = b;
                                break;
                            }
                        }
                        if (block != null)
                            lineBuilder.Append(block.heatedOnce ? "▲" : "△");
                        else
                            lineBuilder.Append("●");
                    }
                    else
                    {
                        lineBuilder.Append("■");
                    }
                }

                mapBuilder.AppendLine(lineBuilder.ToString());
                lineBuilder.Clear();
            }

            Console.WriteLine("\n");
            Console.WriteLine(mapBuilder.ToString());
        }
    }
}