using System;

namespace Nightmare
{
    public partial class GameManager
    {
        public class Action_Buy : ActionBase
        {

            public Action_Buy(int number) : base(number) { }

            public override ActionType Type => ActionType.Buy;

            protected override Dictionary<int, ActionBase> CreateNextActionDic()
            {
                return new Dictionary<int, ActionBase>()
                {
                    { 0,  new Action_Return(0) },
                };
            }


            protected void PrintErrorMsg(int number)
            {
                if (number >= 0 && number < 11)
                {
                    Item selectItem = DataManager.Instance.ItemDatas[number];

                    //아이템이 구매한 적이 없다면 
                    if (selectItem.IsPurchase == false)
                    {   //아이템가격보다 플레이어의 보유골드가 더 많다면
                        if (GameManager.Instance.Player.Gold.PlayerGold >= selectItem.Cost)
                        {
                            Console.WriteLine("구매가 완료되었습니다.");
                            selectItem.IsPurchase = true;
                            GameManager.Instance.Player.Gold.PlayerGold -= selectItem.Cost;
                            DataManager.Instance.HaveItems.Add(selectItem);
                            DisPlay();
                        }
                        else
                        {
                            Console.WriteLine("골드가 부족합니다 !! ");
                        }
                    }
                    else
                    {
                        Console.WriteLine("이미 보유한 아이템입니다.");
                    }
                }
                else 
                { 
                    UtilityManager.PrintErrorMessage();
                }
            }
            protected override void DisPlay()
            {
                OnInputInvalidActionNumber = PrintErrorMsg;
                Console.Clear();

                Console.WriteLine();
                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{GameManager.Instance.Player.Gold.PlayerGold} G");

                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                int i = 1;
                foreach (var item in DataManager.Instance.ItemDatas.Where(item => item.Key >= 1 && item.Key <= 10))
                {

                    Console.WriteLine($"- {i}. {item.Value.ShowShopItem()}");
                    i++;
                }
                Console.WriteLine();


            }
        }
    }
}
