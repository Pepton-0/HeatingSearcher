using System;
using System.Text;
using Maze;
using OpenCLforNet.Function;
using OpenCLforNet.PlatformLayer;

namespace heatingsearcher
{
    public class ParallelHeatingSearcher
    {
        private readonly Map originalMap;

        public ParallelHeatingSearcher (Map originalMap)
        {
            this.originalMap = originalMap;

            // シンプルなデバイス情報取得
            foreach (var platformInfo in Platform.PlatformInfos)
            {
                Console.WriteLine("\nPlatformIndex: " + platformInfo.Index);

                Console.WriteLine("   PlatformInfo");
                foreach(var key in platformInfo.Keys)
                {
                    Console.WriteLine("      "+key + ":" + platformInfo.GetValueAsString(key));
                }

                Console.WriteLine("   DeviceInfo");
                foreach(var deviceInfo in platformInfo.DeviceInfos)
                {
                    Console.WriteLine("      DeviceIndex: " + deviceInfo.Index);
                    foreach (var key in deviceInfo.Keys)
                        Console.WriteLine("         "+ key + ":" + deviceInfo.GetValueAsString(key));
                }
            }
        }
    }
}