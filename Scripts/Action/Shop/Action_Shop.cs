namespace Nightmare
{
    public partial class GameManager
    {
        public class Action_Shop : ActionBase
        {
            public Action_Shop(int number) : base(number) { }

            public override ActionType Type => ActionType.Shop;

            protected override Dictionary<int, ActionBase> CreateNextActionDic()
            {
                return new Dictionary<int, ActionBase>()
                {
                    { 1, new Action_Buy(1) },
                    { 2, new Action_Sell(2) },
                    { 0, new Action_Return(0) },
                };
            }

            protected void PrintErrorMessage(int num)
            {
                UtilityManager.PrintErrorMessage();
            }
            protected override void DisPlay()
            {
                OnInputInvalidActionNumber = PrintErrorMessage;
                Console.Clear();

                var Weaponlines = ASCIIManager.Getlines("Weapon");

                ASCIIManager.DisplayAlignASCIIArt(Weaponlines, Align.Center, VerticalAlign.Top);
                Console.WriteLine();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");                

                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{GameManager.Instance.Player.Gold.PlayerGold} G");

                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");

                foreach (var item in DataManager.Instance.ItemDatas.Where(item => item.Key >= 1 && item.Key <= 10))
                {
                    Console.WriteLine($"- {item.Value.ShowShopItem()}");
                }

            }
        }
    }
}
