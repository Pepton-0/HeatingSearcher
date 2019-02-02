using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Maze
{
    /// <summary>
    /// 2次元のマップ
    /// width(x), height(y)の2つから成る
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Out of range
        /// </summary>
        public static sbyte VOID = -1;
        public static sbyte WALL = 0;
        public static sbyte PATH = 1;
        public static sbyte ROUTE = 2;

        private readonly sbyte[,] byteMap;
        public readonly int width;
        public readonly int height;
        public readonly Vec2 startPos;
        public Vec2 goalPos = new Vec2(-1, -1); // 初めは領域外を指定する. 後で、本当の値を指定する

        public int count;

        public sbyte this[int x, int y]
        {
            get
            {
                if (IsOutOfRange(x, y))
                    return VOID;
                else
                    return byteMap[x, y];
            }

            set
            {
                if (IsOutOfRange(x, y))
                {
                    throw new AlgorithmException("byteMap外の座標にアクセスしようとしました. byteMap[" + width + "," + height + "]  pos(" + x + "," + y + "]");
                }

                byteMap[x, y] = value;
            }
        }
        /// <summary>
        /// 配列としては分かりにくい表現なので、内部のみでしか使わないようにしよう
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private sbyte this[Vec2 pos]
        {
            get
            {
                return this[pos.x, pos.y];
            }

            set
            {
                this[pos.x, pos.y] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">5以上の奇数</param>
        /// <param name="height">5以上の奇数</param>
        /// <param name="startX">2以上の偶数</param>
        /// <param name="startY">2以上の偶数</param>
        public Map (int width, int height, int startX, int startY)
        {
            if (width % 2 == 0 || height % 2 == 0)
                throw new AlgorithmException("幅や高さは、奇数で設定してください. width[" + width + "], height[" + height + "]");

            if (startX < 2 || startY < 2 || startX % 2 == 1 || startY % 2 == 1)
                throw new AlgorithmException("開始地点の座標は、2以上の偶数で設定してください. startX[" + startX + "], startY[" + startY + "]");

            byteMap = new sbyte[width, height]; // 壁(0)で埋もれている状態
            this.width = width;
            this.height = height;

            startPos = new Vec2(startX - 1, startY - 1);
        }

        public Vec2[] GetPaths (int x, int y)
        {
            List<Vec2> list = new List<Vec2>();

            if (this[x + 1, y] == PATH)
                list.Add(Vec2.right);
            if (this[x - 1, y] == PATH)
                list.Add(Vec2.left);
            if (this[x, y + 1] == PATH)
                list.Add(Vec2.up);
            if (this[x, y - 1] == PATH)
                list.Add(Vec2.down);

            return list.ToArray();
        }

        /// <summary>
        /// dirの方向まで行って壁にめりこんだところの一歩前の位置を取得する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public Vec2 GetCorner (int x, int y, Vec2 dir)
        {
            int length = 0;
            Vec2 starting = new Vec2(x, y);
            do { length++; }
            while (this[starting + dir * length] == PATH);

            return starting + dir * (length - 1);
        }

        /// <summary>
        /// マップのビジュアル化出力
        /// </summary>
        public void Dump ()
        {
            // Header
            Console.WriteLine(string.Concat("Dumped the map[", width, ",", height, "]"));

            // Content
            StringBuilder mapBuilder = new StringBuilder();
            StringBuilder lineBuilder = new StringBuilder(0, width);

            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == startPos.x && y == startPos.y)
                        lineBuilder.Append("◇"); // スタート地点は装飾を
                    else if (x == goalPos.x && y == goalPos.y)
                        lineBuilder.Append("◎"); // ゴール地点にも装飾を
                    else
                    {
                        sbyte value = this[x, y];
                        if (value == PATH)
                            lineBuilder.Append("□");
                        else if (value == WALL)
                            lineBuilder.Append("■");
                        else if (value == ROUTE)
                            lineBuilder.Append("・");
                        else
                            lineBuilder.Append("▲");
                    }
                }
                mapBuilder.AppendLine(lineBuilder.ToString());
                lineBuilder.Clear();
            }

            Console.WriteLine(mapBuilder.ToString());
        }

        /// <summary>
        /// byteMap外に、座標(x,y)はあるかどうか
        /// </summary>
        /// <returns></returns>
        public bool IsOutOfRange (int x, int y)
        {
            return x >= width || y >= height || x < 0 || y < 0 ? true : false;
        }

        /// <summary>
        /// 同じマップを、複数のアルゴリズムで使用したい時に
        /// </summary>
        /// <returns></returns>
        public Map Clone ()
        {
#pragma warning disable IDE0017 // オブジェクトの初期化を簡略化します
            Map clone = new Map(width, height, startPos.x + 1, startPos.y + 1);
#pragma warning restore IDE0017 // オブジェクトの初期化を簡略化します
            clone.goalPos = this.goalPos;
            Array.Copy(this.byteMap, clone.byteMap, this.byteMap.Length);

            return clone;
        }
    }
}