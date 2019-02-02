using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Maze;

namespace dijkstra
{
    /// <summary>
    /// ダイクストラ法
    /// </summary>
    public class Dijkstra
    {
        private readonly Map originalMap;
        private readonly DMap dmap;
        private readonly Queue<Vec2> passed;

        public Dijkstra (Map originalMap)
        {
            this.originalMap = originalMap;
            dmap = new DMap(originalMap);
            passed = new Queue<Vec2>();
        }

        public Map Solve ()
        {
            return null;
        }
    }

    /// <summary>
    /// ダイクストラ法用のマップ
    /// </summary>
    class DMap
    {
        public class Path
        {
            /// <summary>
            /// start地点から
            /// </summary>
            public readonly Vec2 start;
            /// <summary>
            /// ゴール地点に近い方
            /// </summary>
            public readonly Vec2 end;
            /// <summary>
            /// start, endとつながっているもの
            /// </summary>
            public readonly List<Hub> connectedHubs = new List<Hub>();

            protected internal bool isGoableToGoal = false;
            protected internal bool isSearched = false;

            public Path (Vec2 start, Vec2 end)
            {
                this.start = start;
                this.end = end;
            }

            public bool Contains (Vec2 point)
            {
                return start == point || end == point;
            }

            public bool Contains(Vec2 point1, Vec2 point2)
            {
                if (start == point1 && end == point2)
                    return true;
                if (end == point1 && start == point2)
                    return true;

                return false;
            }

            public Vec2 GetAnother(Vec2 one)
            {
                if (start == one)
                    return end;
                if (end == one)
                    return start;

                throw new AlgorithmException("");
            }
        }

        public class Hub
        {
            public readonly Vec2 pos;
            public readonly List<Path> connectedPaths = new List<Path>();

            protected internal bool isGoableToGoal;

            public Hub (Vec2 pos)
            {
                this.pos = pos;
            }
        }

        public readonly Hub startPosHub;
        public readonly Hub goalPosHub;

        public readonly Hub[] hubs;
        public readonly Path[] paths;

        private readonly Map originalMap;

        public DMap (Map originalMap)
        {
            this.originalMap = originalMap;

            List<Hub> draftHubs = new List<Hub>();
            List<Path> draftPaths = new List<Path>();

            for(int x = 0; x < originalMap.width; x++)
            {
                for(int y = 0; y < originalMap.height; y++)
                {
                    if(originalMap[x, y] == Map.PATH)
                    {
                        Vec2 pos = new Vec2(x, y);
                        Vec2[] dirs = originalMap.GetPaths(x, y);

                        bool settableHub = dirs.Length >= 3;

                        if (!settableHub && (pos == originalMap.goalPos || pos == originalMap.startPos))
                            settableHub = true;

                        if (!settableHub && dirs.Length==2)
                        {
                            if (new Vec2(0, 0) - dirs[0] != dirs[1])
                                settableHub = true;
                        }

                        if (settableHub)
                        {
                            var hub = new Hub(pos);
                            draftHubs.Add(hub);
                            if (hub.pos == originalMap.startPos)
                                startPosHub = hub;
                            if (hub.pos == originalMap.goalPos)
                                goalPosHub = hub;


                            foreach (var dir in dirs)
                            {
                                Vec2 corner = originalMap.GetCorner(x, y, dir);
                                bool notContain = true;
                                foreach (var path in draftPaths)
                                {
                                    if (!path.Contains(pos, corner))
                                    {
                                        notContain = false;
                                        break;
                                    }
                                }
                                if (notContain)
                                    draftPaths.Add(new Path(pos, corner));
                            }
                        }
                    }
                }
            }

            foreach(var hub in draftHubs)
            {
                foreach(var path in draftPaths)
                {
                    if (path.Contains(hub.pos))
                    {
                        hub.connectedPaths.Add(path);
                        path.connectedHubs.Add(hub);
                    }
                }
            }

            Console.WriteLine("DraftHubs: "+draftHubs.Count + "  DraftPaths:" + draftPaths.Count +
                "  Start: " + (startPosHub!=null ? startPosHub.pos.ToString() : "null") +
                "  Goal: " + (goalPosHub != null ? goalPosHub.pos.ToString() : "null"));

            List<Path> goablePaths = new List<Path>();
            List<Hub> goableHubs = new List<Hub>();
            SelectPathsGoableToGoal(goalPosHub, goablePaths, goableHubs);

            if (!goableHubs.Contains(goalPosHub))
            {
                goableHubs.Add(goalPosHub);
                goalPosHub.isGoableToGoal = true;
            }

            this.hubs = goableHubs.ToArray();
            this.paths = goablePaths.ToArray();

            Dump();
        }

        private void SelectPathsGoableToGoal(Hub from, List<Path> goablePaths, List<Hub> goableHubs)
        {
            foreach (Path path in from.connectedPaths)
            {
                if (!path.isSearched)
                {
                    path.isSearched = true;
                    if (path.Contains(originalMap.goalPos) || 
                        path.connectedHubs.Exists(hub => hub.connectedPaths.Exists(p => p.isGoableToGoal))
                        )
                    {
                        path.isGoableToGoal = true;
                        goablePaths.Add(path);

                        foreach (var neighborHub in path.connectedHubs)
                        {
                            if (!neighborHub.isGoableToGoal)
                            {
                                goableHubs.Add(neighborHub);
                                neighborHub.isGoableToGoal = true;
                            }
                            SelectPathsGoableToGoal(neighborHub, goablePaths, goableHubs);
                        }
                    }
                }
            }
        }

        private void Dump ()
        {
            StringBuilder mapBuilder = new StringBuilder();
            StringBuilder lineBuilder = new StringBuilder();

            Console.WriteLine();
            foreach (var h in hubs)
                Console.Write(h.pos);

            for(int y = originalMap.height-1; y >=0; y--)
            {
                for(int x = 0; x < originalMap.width; x++)
                {
                    if (hubs.Any(hub=>hub.pos.x == x && hub.pos.y == y))
                        lineBuilder.Append("●");
                    else if (originalMap[x, y] == Map.WALL)
                        lineBuilder.Append("■");
                    else
                        lineBuilder.Append("□");
                }

                mapBuilder.AppendLine(lineBuilder.ToString());
                lineBuilder.Clear();
            }

            Console.WriteLine();
            Console.WriteLine(mapBuilder.ToString());
            Console.WriteLine();
        }
    }
}
