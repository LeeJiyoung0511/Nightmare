namespace Nightmare
{
    public partial class GameManager
    {
        public class Action_Sell : ActionBase
        {
            public Action_Sell(int number) : base(number) { }


            public override ActionType Type => ActionType.Sell;

            protected override Dictionary<int, ActionBase> CreateNextActionDic()
            {
                return new Dictionary<int, ActionBase>()
                {
                    { 0,  new Action_Return(0) },
                };
            }

            private void PrintErrorMsg(int number)
            {
                if (number >= 0 && number < DataManager.Instance.HaveItems.Count)
                {


                    Item selectItem = DataManager.Instance.HaveItems[number - 1];

                    //아이템이 판매한 적이 없다면 
                    if (selectItem.IsSold == false)
                    {

                        Console.WriteLine("판매가 완료되었습니다.");
                        selectItem.IsSold = true;
                        //소수점 계산이 있어서 int가 아닌 float이나 double의 형태로 나중에 수정할 것
                        GameManager.Instance.Player.Gold.PlayerGold += (int)(selectItem.Cost * 0.85f);
                        //인벤토리 딕셔너리에서 삭제
                        DataManager.Instance.HaveItems.Remove(selectItem);
                        //상점 아이템 구매 목록에 다시 살수 있도록 IsPurchase == false로 바꾸기
                        selectItem.IsPurchase = false;
                        selectItem.IsEquip = false;
                        DisPlay();

                    }
                    else
                    {
                        Console.WriteLine("이미 판매한 아이템입니다.");
                    }
                }
                else
                {
                    UtilityManager.PrintErrorMessage();
                }
            }
            protected override void DisPlay()
            {
                Console.Clear();
                OnInputInvalidActionNumber = PrintErrorMsg;
                Console.WriteLine();
                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("이 곳에서는 보유한 아이템을 판매할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{GameManager.Instance.Player.Gold.PlayerGold} G");

                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < DataManager.Instance.HaveItems.Count; i++)
                {
                    Console.WriteLine($"- {i + 1}.  {DataManager.Instance.HaveItems[i].ShowSellItem()}");
                }

                Console.WriteLine();


            }
        }
    }

}