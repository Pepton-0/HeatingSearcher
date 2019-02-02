using System;
using System.Diagnostics;
using Maze;
using depthfirstsearch;
using breadthfirstsearch;
using dijkstra;
using heatingsearcher;

/// <summary>
/// 計測する物:
/// 1. CPU使用率
/// 2. 処理時間(Solve()にかかる時間. その他の、迷路生成やアルゴリズムのコンストラクタにかかる時間は計測しない)
/// </summary>
namespace AlgorithmRunner
{
    class Runner
    {
        private static readonly Stopwatch stopwatch = new Stopwatch();

        public static void Main (string[] args)
        {
            Console.WriteLine("Runner was activated.\n");
            Timer.Start();

            try
            {
                Run();
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            // Keep the console open
            Console.WriteLine("Press \"e\" to exit the console.");
            while (Console.ReadKey().KeyChar != 'e')
                Console.WriteLine();
            // Exit
        }

        private static void Run ()
        {
            /*
            int testTime = 4;
            double allTime = 0f;
            for (int i = 0; i < testTime; i++)
            {
                // Test the algorithm
                Console.WriteLine("迷路生成中...");
                var map = MazeCreator.Create(51, 51, new Vec2(2, 2));
                Console.WriteLine("    生成完了.\n");
                var algorithm = new HeatingSearcher(map);

                double elapsedMs1 = Timer.ElapsedMilliSec();
                algorithm.Solve();
                double elapsedMs2 = Timer.ElapsedMilliSec();

                allTime += elapsedMs2 - elapsedMs1;
            }
            Console.WriteLine("Average Time: " + allTime / (double)testTime);*/


            var map = MazeCreator.Create(21, 21, new Vec2(2, 2));
            map.Dump();
            Console.WriteLine();
            double t1 = Timer.ElapsedMilliSec();
            var algorithm = new Dijkstra(map);
            double t2 = Timer.ElapsedMilliSec();
            
            Console.WriteLine("Elapsed Time: " + (t2- t1));
            //var searcher = new ParallelHeatingSearcher(map);
        }
    }
}