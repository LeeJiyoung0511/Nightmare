namespace Nightmare
{
    public partial class GameManager
    {
        public class Action_Inventory : ActionBase
        {
            public Action_Inventory(int number) : base(number) { }

            public override ActionType Type => ActionType.Inventory;

            protected override Dictionary<int, ActionBase> CreateNextActionDic()
            {
                return new Dictionary<int, ActionBase>()
                {
                    { 1,  new Action_EquipItem(1) },
                    { 0,  new Action_Return(0) },
                };
            }
            protected void PrintErrorMsg(int num)
            {
                UtilityManager.PrintErrorMessage();
            }
            protected override void DisPlay()
            {
                OnInputInvalidActionNumber = PrintErrorMsg;
                Console.Clear();
                var Baglines = ASCIIManager.Getlines("BagA");

                ASCIIManager.DisplayAlignASCIIArt(Baglines, Align.Center, VerticalAlign.Top);
                Console.WriteLine("인벤토리");
                DisPlayInventory();
            }

            public void DisPlayInventory()
            {
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");               

                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                
                foreach (Item item in DataManager.Instance.HaveItems)
                {
                    Console.WriteLine($" - {item.SelectItem()}");
                }

                Console.WriteLine();
            }
        }
    }
}
