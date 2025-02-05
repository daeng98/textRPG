using System.Data.Common;
using System.Dynamic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using static TextRPG.Program;

namespace TextRPG
{
    internal class Program
    {
        class Player
        {
            public int Level { get; set; }
            public string Name { get; }
            public string Class { get; }
            public int Attack { get; set; }
            public int Defense { get; set; }
            public int Health { get; set; }
            public int Gold { get; set; }

            public Player(string name)
            {
                Level = 1;
                Name = name;
                Class = "전사";
                Attack = 10;
                Defense = 5;
                Health = 100;
                Gold = 3000;
            }
            public Player(string name, int attack, int defense, int gold)        // <- 아이템 장착이랑 골드를 썼을때 쓰려고 만들었는데, 거기까지 못했음....
            {
                Level = 1;
                Name = name;
                Class = "전사";
                Attack = attack;
                Defense = defense;
                Health = 100;
                Gold = gold;
            }
        }

        class Item
        {
            public string itemName { get; set; }
            public string itemType { get; set; }
            public string itemInfo { get; set; }
            public int itemStat { get; set; }
            public int itemGold { get; set; }
            public string itemSell { get; set; }    


            public Item(string name, string type, string info, int stat, int gold, string sell)
            {
                itemName = name;
                itemType = type;
                itemInfo = info;
                itemStat = stat;
                itemGold = gold;
                itemSell = sell;
            }
        }


        class Inventory
        {
            List<Item> invenItems = new List<Item>();

            public Inventory()
            {

            }

            public void addItem(Item item)
            {
                invenItems.Add(item);
            }

            public void printInvenList()
            {
                for (int i = 0; i < invenItems.Count; i++)
                {
                    Console.WriteLine($"- {invenItems[i].itemName} | {invenItems[i].itemType} + {invenItems[i].itemStat} | {invenItems[i].itemInfo}");
                }
                Console.WriteLine();
            }

            
        }

        class Shop
        {
            static List<Item> shopItems = new List<Item>();

            public Shop()
            {

            }

            public void settingShop()                // <-단순히 생각해서 상점에 먼저 아이템이 있어야될 것 같아서 여기에서 아이템 넣기를 해버림. 이러면 인벤토리랑 장착에서 문제가 생길지 궁금함.
            {
                Item item1 = new Item("수련자 갑옷", "방어력", "수련에 도움을 주는 갑옷입니다.", 5, 1000, "");
                Item item2 = new Item("무쇠 갑옷", "방어력", "무쇠로 만들어져 튼튼한 갑옷입니다.", 9, 2000, "");
                Item item3 = new Item("스파르타 갑옷", "방어력", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 15, 3500, "");
                Item item4 = new Item("낡은 검", "공격력", "쉽게 볼 수 있는 낡은 검 입니다.", 2, 600, "");
                Item item5 = new Item("청동 도끼", "공격력", "어디선가 사용됐던거 같은 도끼입니다.", 5, 1500, "");
                Item item6 = new Item("스파르타 창", "공격력", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 7, 3000, "");

                if (shopItems.Count < 6)
                {
                    shopItems.Add(item1);
                    shopItems.Add(item2);
                    shopItems.Add(item3);
                    shopItems.Add(item4);
                    shopItems.Add(item5);
                    shopItems.Add(item6);
                }
            }

            public void printShopList(string s)
            {
                Player player = new Player(s);

                Console.Clear();

                Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine($"보유 골드 : {player.Gold} g\n");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < shopItems.Count; i++)
                {
                    Console.WriteLine($"- {shopItems[i].itemName}  |  {shopItems[i].itemType} + {shopItems[i].itemStat}  |  {shopItems[i].itemInfo}  |  {shopItems[i].itemGold} g  |  {shopItems[i].itemSell}");
                }
                Console.WriteLine();
                Console.WriteLine("1. 아이템 구매\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
            }
 

            public void printBuyList(string s)            // <- printShopList랑 똑같은데 단지 앞에 번호를 써주고 싶어서 하나 더 작성했음. 2개인데 거의 똑같은 코드라서 다른 방법이 있는지 궁금함.
            {
                Player player = new Player(s);

                Console.Clear();

                Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine($"보유 골드 : {player.Gold} g\n");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < shopItems.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {shopItems[i].itemName}  |  {shopItems[i].itemType} + {shopItems[i].itemStat}  |  {shopItems[i].itemInfo}  |  {shopItems[i].itemGold} g  |  {shopItems[i].itemSell}");
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
            }

            public void BuyItem(Player player, int a)
            {
                Inventory inventory = new Inventory();

                if (shopItems[a - 1].itemGold <= player.Gold)
                {
                    player.Gold -= shopItems[a - 1].itemGold;       // <- 구매하고 player.gold를 깍아주고 싶었는데 전달이 안되는 것 같음.
                    shopItems[a - 1].itemSell = "구매완료";             // <- 이런 방식으로 해도 괜찮을지 모르겠음. 판매가 된지를 판단하지 못할 것 같음.

                    inventory.addItem(shopItems[a - 1]);             // <- 구매하면 인벤토리에 additem을 써서 넣어주고 싶었는데 잘 안됨. 골드랑 마찬가지로 전달이 안되는 것 같음.
                }
            }
        }

        static void printInfo(string s)            // <- 초기 스탯은 그대로 잘 나오는데, 상점에서 골드를 썻을때 골드가 차감이 안되고 초기 상태 그대로 나옴.
        {
            Player player = new Player(s);

            Console.Clear();

            Console.WriteLine("상태 보기\n캐릭터의 정보가 표시됩니다.\n");
            Console.WriteLine($"레벨 : {player.Level}");
            Console.WriteLine($"이름 : {player.Name}");
            Console.WriteLine($"직업 : {player.Class}");
            Console.WriteLine($"공격력 : {player.Attack}");
            Console.WriteLine($"방어력 : {player.Defense}");
            Console.WriteLine($"체 력 : {player.Health}");
            Console.WriteLine($"소지금 : {player.Gold} g");
            Console.WriteLine("\n\n0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
        }

        static void printInven()             // <- 구매한 아이템을 여기로 띄어주고 싶은데 그게 잘 안됫음.
        {
            Inventory inventory = new Inventory();

            Console.Clear();

            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");
            inventory.printInvenList();
            Console.WriteLine("1. 장착 관리\n0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
        }

        static void Main(string[] args)
        {
            Player player;
            Shop shop = new Shop();
            Inventory inventory = new Inventory();
            Item item;

            bool isPlaying = true;


            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n원하시는 이름을 설정해주세요.");
            Console.Write(">> ");
            string playerName = Console.ReadLine();

            player = new Player(playerName);

            while (isPlaying)
            {
                bool isNum = true;

                Console.Clear();

                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine("1. 상태보기\n2. 인벤토리\n3. 상점\n\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string s = Console.ReadLine();

                if (int.TryParse(s, out int number))
                {
                    while (isNum)
                    {
                        switch (number)
                        {
                            case 1:
                                printInfo(playerName);

                                string n1 = Console.ReadLine();

                                if (int.TryParse(n1, out int num1))
                                {
                                    if (num1 == 0)
                                    {
                                        Console.Clear();
                                        isNum = false;
                                    }
                                }
                                else
                                    Console.WriteLine("잘못된 입력입니다.");      // <- 전체적으로 잘못 입력되었다는 텍스트가 안나옴.
                                break;

                            case 2:
                                printInven();

                                string n2 = Console.ReadLine();

                                if (int.TryParse(n2, out int num2))
                                {
                                    if (num2 == 0)
                                    {
                                        Console.Clear();
                                        isNum = false;
                                    }
                                    else if( num2 == 1)                               // <- 상점을 먼저 하려다 보니까 장착 관련은 하지 못했음.
                                    {

                                    }
                                }
                                else
                                    Console.WriteLine("잘못된 입력입니다.");     
                                break;

                            case 3:
                                shop.settingShop();

                                shop.printShopList(playerName);

                                string n3 = Console.ReadLine();

                                if (int.TryParse(n3, out int num3))
                                {
                                    if (num3 == 0)
                                    {
                                        isNum = false;
                                        break;
                                    }
                                    else if (num3 == 1)
                                    {
                                        shop.printBuyList(playerName);

                                        string n4 = Console.ReadLine();

                                        if (int.TryParse(n4, out int num4))     // <- 뭔가 너무 길다고 느껴짐. 더 줄일 수 있을지 모르겠음
                                        {
                                            if(num4 == 0)
                                            {
                                                isNum = false;
                                                break;
                                            }
                                            else if(num4 == 1)
                                            {
                                                shop.BuyItem(player, num4);     
                                            }
                                            else if (num4 == 2)
                                            {
                                                shop.BuyItem(player, num4);
                                            }
                                            else if (num4 == 3)
                                            {
                                                shop.BuyItem(player, num4);
                                            }
                                            else if (num4 == 4)
                                            {
                                                shop.BuyItem(player, num4);
                                            }
                                            else if (num4 == 5)
                                            {
                                                shop.BuyItem(player, num4);
                                            }
                                            else if (num4 == 6)
                                            {
                                                shop.BuyItem(player, num4);
                                            }
                                            else
                                            {
                                                Console.WriteLine("잘못된 입력입니다.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("잘못된 입력입니다.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("잘못된 입력입니다.");
                                }
                                break;

                            default:
                                Console.WriteLine("잘못된 입력입니다.");
                                isNum = false;
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
    }
}
