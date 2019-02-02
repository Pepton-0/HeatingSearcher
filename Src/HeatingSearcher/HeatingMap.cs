using System.Collections.Generic;

namespace heatingsearcher
{
    /// <summary>
    /// 熱移動の計算では、両者間の熱の移動がちゃんと現実と同じになるよう、計算は一括してやらないといけない.
    /// </summary>
    public class HeatMap
    {
        public class Block
        {
            public readonly Vec2 pos;
            private float _temp;
            public float temp
            {
                get
                {
                    return _temp;
                }
                set
                {
                    if (isFixedTemp)
                        return;

                    if (!heatedOnce)
                        if (_temp < value)
                            heatedOnce = true;
                    _temp = value;
                }
            }
            public bool isGoal;
            public readonly bool isFixedTemp = false;
            public bool heatedOnce { get; private set; } = false;

            public Block up, left, down, right;
            public Queue<Block> byBlocks;

            public Block (Vec2 v, float temp, bool isGoal, bool isFixedTemp)
            {
                pos = v;
                this._temp = temp;
                this.isGoal = isGoal;
                this.isFixedTemp = isFixedTemp;
                byBlocks = new Queue<Block>();
            }
        }

        public readonly int width;
        public readonly int height;

        public readonly List<Block> blocks = new List<Block>();
        public readonly Block startBlock;
        public readonly Block goalBlock;

        public HeatMap (Maze.Map originalMap, float startBlockTemp)
        {
            this.width = originalMap.width;
            this.height = originalMap.height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (originalMap[x, y] == Maze.Map.PATH)
                    {
                        Vec2 pos = new Vec2(x, y);
                        var block = new Block(pos, pos == originalMap.startPos ? startBlockTemp : HeatingSearcher.defTemp,
                            pos == originalMap.goalPos, pos == originalMap.startPos);
                        blocks.Add(block);

                        if (block.pos == originalMap.startPos)
                            this.startBlock = block;
                        if (block.pos == originalMap.goalPos)
                            this.goalBlock = block;
                    }
                }
            }

            foreach (var block in blocks)
            {
                foreach (var b in blocks)
                {
                    Vec2 diffuse = b.pos - block.pos;
                    if (diffuse == Vec2.up)
                    {
                        block.up = b;
                        block.byBlocks.Enqueue(b);
                    }
                    else if (diffuse == Vec2.left)
                    {
                        block.left = b;
                        block.byBlocks.Enqueue(b);
                    }
                    else if (diffuse == Vec2.down)
                    {
                        block.down = b;
                        block.byBlocks.Enqueue(b);
                    }
                    else if (diffuse == Vec2.right)
                    {
                        block.right = b;
                        block.byBlocks.Enqueue(b);
                    }
                }
            }
        }
    }
}