using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flyingChess
{
    class Program
    {
        //地图数组
        static int[] Maps = new int[100];
        //玩家坐标
        static int[] PlayerPos = new int[2];
        //玩家姓名
        static string[] PlayerName = new string[2];
        //玩家标记,默认false
        static bool[] Flags = new bool[2];

        static void Main(string[] args)
        {
            Login();
            Console.ReadKey();
        }

        /*登录*/
        public static void Login()
        {
            GameShow();
            Console.WriteLine("请输入玩家A的姓名");
            PlayerName[0] = Console.ReadLine();
            while (PlayerName[0] == "")
            {
                Console.WriteLine("玩家A的姓名不能为空，请重新输入");
                PlayerName[0] = Console.ReadLine();
            }
            Console.WriteLine("请输入玩家B的姓名");
            PlayerName[1] = Console.ReadLine();
            while (PlayerName[1] == "" || PlayerName[1] == PlayerName[0])
            {
                Console.WriteLine("玩家B的姓名不能为空且不能与A相同，请重新输入");
                PlayerName[1] = Console.ReadLine();
            }

            Console.Clear();
            GameShow();
            Console.WriteLine("玩家{0}用A表示", PlayerName[0]);
            Console.WriteLine("玩家{0}用B表示", PlayerName[1]);
            InitMap();
            DrawMap();

            StartGame();
        }

        /*游戏开始*/
        public static void StartGame()
        {
            while (PlayerPos[0] < 99 && PlayerPos[1] < 99)
            {
                if (Flags[0] == false)
                {
                    PlayingGame(0);
                }
                else
                {
                    Flags[0] = false;
                }

                if (Flags[1] == false)
                {
                    PlayingGame(1);
                }
                else
                {
                    Flags[1] = false;
                }
            }
        }

        /*进行游戏*/
        public static void PlayingGame(int playerNum)
        {
            Random r = new Random();
            int rNum = r.Next(1, 7);
            Console.WriteLine("{0}按任意键开始掷骰子", PlayerName[playerNum]);
            Console.ReadKey(true);
            Console.WriteLine("{0}掷出了{1}", PlayerName[playerNum], rNum);
            PlayerPos[playerNum] += rNum;
            Console.ReadKey(true);
            Console.WriteLine("{0}按任意键开始行动", PlayerName[playerNum]);
            Console.ReadKey(true);
            Console.WriteLine("{0}行动完了", PlayerName[playerNum]);
            Console.ReadKey(true);

            /* 游戏规则
                * 玩家A踩到玩家B, 玩家B退6格.
                * 玩家踩到方块, 神马都不发生
                * 玩家踩到地雷，退六格
                * 玩家踩到幸运轮盘 1.交换位置 2.轰炸指定玩家（指定玩家退6格）
                * 玩家踩到暂停，停止一回合
                * 玩家踩到时空隧道，前进十格
                */
            if (PlayerPos[playerNum] == PlayerPos[1 - playerNum])
            {
                Console.WriteLine("玩家{0}踩到玩家{1}，玩家{2}退6格", PlayerName[playerNum], PlayerName[1 - playerNum], PlayerName[1 - playerNum]);
                PlayerPos[1 - playerNum] -= 6;
                Console.ReadKey(true);
            }
            else
            {
                switch (Maps[PlayerPos[playerNum]])
                {
                    case 0:
                        Console.WriteLine("玩家{0}踩到方块，神马都没发生！", PlayerName[playerNum]);
                        Console.ReadKey(true);
                        break;
                    case 1:
                        Console.WriteLine("玩家{0}踩到幸运轮盘，请选择 1.交换位置 2.轰炸对方（使对方退6格）", PlayerName[playerNum]);
                        string input = Console.ReadLine();
                        while (true)
                        {
                            if (input == "1")
                            {
                                Console.WriteLine("玩家{0}选择跟玩家{1}交换位置", PlayerName[playerNum], PlayerName[1 - playerNum]);
                                Console.ReadKey(true);
                                int temp = PlayerPos[playerNum];
                                PlayerPos[playerNum] = PlayerPos[1 - playerNum];
                                PlayerPos[1 - playerNum] = temp;
                                Console.WriteLine("交换完成，按任意键继续游戏！");
                                break;
                            }
                            else if (input == "2")
                            {
                                Console.WriteLine("玩家{0}选择轰炸玩家{1}，玩家{2}退6格", PlayerName[playerNum], PlayerName[1 - playerNum], PlayerName[1 - playerNum]);
                                Console.ReadKey(true);
                                PlayerPos[1 - playerNum] -= 6;
                                Console.WriteLine("玩家{0}退了6格", PlayerName[1 - playerNum]);
                                Console.ReadKey(true);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("只能输入1或者2 1--交换位置 2--轰炸对方");
                                input = Console.ReadLine();
                            }
                        }
                        Console.ReadKey(true);
                        break;
                    case 2:
                        Console.WriteLine("玩家{0}踩到地雷，退6格！", PlayerName[playerNum]);
                        Console.ReadKey(true);
                        PlayerPos[playerNum] -= 6;
                        break;
                    case 3:
                        Console.WriteLine("玩家{0}踩到暂停，暂停一回合！", PlayerName[playerNum]);
                        Flags[playerNum] = true;
                        Console.ReadKey(true);
                        break;
                    case 4:
                        Console.WriteLine("玩家{0}踩到时空隧道，前进十格！", PlayerName[playerNum]);
                        PlayerPos[playerNum] += 10;
                        Console.ReadKey(true);
                        break;
                }
            }

            ChangePos();
            Console.Clear();
            DrawMap();
        }

        /*游戏标题*/
        public static void GameShow()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("******************");
            Console.WriteLine("******************");
            Console.WriteLine("****  飞行棋  ****");
            Console.WriteLine("******************");
            Console.WriteLine("******************");
        }

        /*初始化地图*/
        public static void InitMap()
        {
            //幸运轮盘
            int[] luckyTurn = { 6, 23, 40, 55, 69, 83 };
            for (int i = 0; i < luckyTurn.Length; i++)
            {
                Maps[luckyTurn[i]] = 1;
            }

            //地雷
            int[] landMine = { 5, 13, 17, 33, 38, 50, 64, 80, 94 };
            for (int i = 0; i < landMine.Length; i++)
            {
                Maps[landMine[i]] = 2;
            }

            //暂停
            int[] pause = { 9, 27, 60, 93 };
            for (int i = 0; i < pause.Length; i++)
            {
                Maps[pause[i]] = 3;
            }

            //时空隧道
            int[] timeTunnel = { 20, 25, 45, 63, 72, 88, 90 };
            for (int i = 0; i < timeTunnel.Length; i++)
            {
                Maps[timeTunnel[i]] = 4;
            }
        }

        /*绘制地图*/
        public static void DrawMap()
        {
            //图形含义提示
            Console.WriteLine("图例: 幸运轮盘:★  地雷:※  暂停:▼  时空隧道:卍");

            //第一横行
            for (int i = 0; i < 30; i++)
            {
                judgeMap(i);
            }

            //第一竖行
            for (int i = 30; i < 35; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < 29; j++)
                {
                    Console.Write("  ");
                }
                judgeMap(i);
            }

            //第二横行
            Console.WriteLine();
            for (int i = 64; i >= 35; i--)
            {
                judgeMap(i);
            }

            //第二竖行
            for (int i = 65; i < 70; i++)
            {
                Console.WriteLine();
                judgeMap(i);
            }

            //第三横行
            Console.WriteLine();
            for (int i = 70; i < 100; i++)
            {
                judgeMap(i);
            }

            Console.WriteLine();
        }

        /*公用判断方法判断*/
        public static void judgeMap(int i)
        {
            //如果玩家一和玩家二坐标相同，并且都在地图内
            if (PlayerPos[0] == PlayerPos[1] && PlayerPos[1] == i)
            {
                Console.Write("<>");
            }
            else if (PlayerPos[0] == i)
            {
                Console.Write("Ａ");
            }
            else if (PlayerPos[1] == i)
            {
                Console.Write("Ｂ");
            }
            else
            {
                switch (Maps[i])
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("□");
                        break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("★");
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("※");
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("▼");
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("卍");
                        break;
                }
            }
        }

        /*玩家坐标发生变化时检查是否符合规范*/
        public static void ChangePos()
        {
            for (int i = 0; i < 2; i++)
            {
                if (PlayerPos[i] < 0)
                {
                    PlayerPos[i] = 0;
                }
                if (PlayerPos[i] > 99)
                {
                    PlayerPos[i] = 99;
                }
            }
        }
    }
}

