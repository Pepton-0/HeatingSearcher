using System;
using System.Collections.Generic;
using Maze;

namespace depthfirstsearch
{
    /// <summary>
    /// 深さ優先探索
    /// </summary>
    public class DepthFirstSearch
    {
        private struct Depth
        {
            public Vec2 pos;
            public int depth;

            public Depth (Vec2 pos, int depth)
            {
                this.pos = pos;
                this.depth = depth;
            }
        }

        private readonly Map map;
        private List<Depth> depths = new List<Depth>();

        public DepthFirstSearch (Map map)
        {
            this.map = map;
        }

        public Map Solve ()
        {
            depths.Add(new Depth(map.startPos, 0));

            Vec2[] dirs = map.GetPaths(map.startPos.x, map.startPos.y);
            bool found = false;
            foreach (var dir in dirs)
            {
                if (!found)
                    found = Process(map.startPos, dir, 0);
            }

            if (found)
            {
                Console.WriteLine("経路を求められました.");
                Vec2 lastPos = depths[depths.Count - 1].pos;
                int lastDepth = 0;

                map[lastPos.x, lastPos.y] = Map.ROUTE;

                for (int i = depths.Count - 2; i >= 0; i--)
                {
                    //Console.WriteLine(lastPos);
                    Depth dp = depths[i];
                    if ((lastPos - dp.pos).SqrDistance == 1 && lastDepth - dp.depth == 1)
                    {
                        lastDepth = dp.depth;
                        lastPos = dp.pos;
                        map[lastPos.x, lastPos.y] = Map.ROUTE;
                    }
                }

                return map;
            }
            else
            {
                Console.WriteLine("経路を求められませんでした.");
                return null;
            }
        }

        private bool Process (Vec2 pos, Vec2 dir, int depth)
        {
            depths.Add(new Depth(pos, depth));

            if (map.goalPos == pos)
                return true;


            depth++;

            sbyte up = map[pos.x, pos.y + 1];
            if (dir * -1 != Vec2.up && up == Map.PATH && Process(pos + Vec2.up, Vec2.up, depth))
                return true;

            sbyte left = map[pos.x - 1, pos.y];
            if (dir * -1 != Vec2.left && left == Map.PATH && Process(pos + Vec2.left, Vec2.left, depth))
                return true;

            sbyte down = map[pos.x, pos.y - 1];
            if (dir * -1 != Vec2.down && down == Map.PATH && Process(pos + Vec2.down, Vec2.down, depth))
                return true;

            sbyte right = map[pos.x + 1, pos.y];
            if (dir * -1 != Vec2.right && right == Map.PATH && Process(pos + Vec2.right, Vec2.right, depth))
                return true;

            return false;
        }
    }
}
