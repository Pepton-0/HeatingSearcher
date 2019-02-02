using System;
using static Maze.Map;

namespace Maze
{
    public class MazeCreator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">5以上の奇数</param>
        /// <param name="height">5以上の奇数</param>
        /// <param name="startPos">各値ｈａ、2以上の偶数</param>
        /// <returns></returns>
        public static Map Create (int width, int height, Vec2 startPos)
        {
            Map map = new Map(width, height, startPos.x, startPos.y);

            startPos = new Vec2(startPos.x - 1, startPos.y - 1);

            Vec2 goalPos = startPos;
            Dig(map, startPos, ref goalPos);
            map.goalPos = goalPos;

            return map;
        }

        private static void Dig (Map map, Vec2 pos, ref Vec2 goalPos)
        {
            map[pos.x, pos.y] = PATH;

            foreach (Vec2 dir in Vec2.ShuffledDirs())
            {
                Vec2 nextDigPos = pos + dir * 2;

                if (map[nextDigPos.x, nextDigPos.y] == WALL)
                {
                    map[pos.x + dir.x, pos.y + dir.y] = PATH;
                    Dig(map, nextDigPos, ref goalPos);
                }
                else // 行き止まり
                {
                    if ((pos - map.startPos).SqrDistance > (goalPos - map.startPos).SqrDistance)
                        goalPos = pos;
                }
            }
        }
    }
}