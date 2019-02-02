using System;
using System.Collections.Generic;
using Maze;

namespace breadthfirstsearch
{
    /// <summary>
    /// 幅優先探索
    /// </summary>
    public class BreadthFirstSearch
    {
        private const sbyte VISITED = 20;

        private readonly Map original;
        private readonly Map draft;
        private readonly List<Vec2> queue;
        private int visitingIndex_start;
        private int visitingsCount;

        /// <summary>
        /// まだ通っていない道の総量
        /// </summary>
        private int leftSavageCount;

        public BreadthFirstSearch (Map map)
        {
            this.original = map;
            draft = this.original.Clone();
            queue = new List<Vec2>();

            for (int x = 0; x < this.original.width; x++)
                for (int y = 0; y < this.original.height; y++)
                    if (this.original[x, y] == Map.PATH)
                        leftSavageCount++;
        }

        public Map Solve ()
        {
            queue.Add(original.startPos);
            original[original.startPos.x, original.startPos.y] = VISITED;
            visitingIndex_start = 0;
            visitingsCount = 1;
            leftSavageCount--;

            int nowVisitedCount = 0;

            while (leftSavageCount >= 0)
            {
                // これで、途中で追加しても大丈夫
                for (int i = visitingIndex_start; i < visitingIndex_start + visitingsCount; i++)
                {
                    Vec2 current = queue[i];

                    foreach (var dir in Vec2.dirs)
                    {
                        Vec2 next = current + dir;
                        if (next == draft.goalPos)
                        {
                            // 終える
                            queue.Add(next);
                            leftSavageCount--;
                            nowVisitedCount++;
                            goto FINISHED;
                        }

                        sbyte value = draft[next.x, next.y];

                        if (value == Map.PATH)
                        {
                            queue.Add(next);
                            leftSavageCount--;
                            nowVisitedCount++;
                            draft[next.x, next.y] = VISITED;
                        }
                    }
                }

                visitingIndex_start += visitingsCount;
                visitingsCount = nowVisitedCount;
                nowVisitedCount = 0;
            }

            FINISHED:;
            if (queue[queue.Count - 1] == draft.goalPos)
            {
                // ゴール地点が見つかったら、ルートの作成を開始
                Vec2 lastPos;
                lastPos = original.goalPos;
                for (int i = queue.Count - 2; i >= 0; i--)
                {
                    Vec2 pos = queue[i];
                    if ((lastPos - pos).SqrDistance == 1)
                    {
                        original[pos.x, pos.y] = Map.ROUTE;
                        lastPos = pos;
                    }
                }

                return original;
            }
            else
            {
                // ゴールが見つからなかったら、失敗報告
                Console.WriteLine("経路を求められませんでした. leftSavageCount: " + leftSavageCount);
                return draft;
            }
        }
    }
}