namespace Nightmare
{
    public partial class GameManager
    {
        public class Action_EquipItem : Action_Inventory
        {
            public Action_EquipItem(int number) : base(number) { }
            public override ActionType Type => ActionType.Equip;

            protected override Dictionary<int, ActionBase> CreateNextActionDic()
            {
                return new Dictionary<int, ActionBase>()
                {
                    { 0,  new Action_Return(0) },
                };
            }
            protected void PrintErrorMsg(int num)
            {
                if (num >= 0 && num <= DataManager.Instance.HaveItems.Count) // 유효한 아이템 번호인지 체크
                {
                    EquipItem(num);  // 아이템을 장착

                }
                else
                {
                    UtilityManager.PrintErrorMessage();  // 잘못된 입력 처리
                }
            }

            // 화면을 표시하고 액션을 처리하는 메서드
            protected override void DisPlay()
            {

                OnInputInvalidActionNumber = PrintErrorMsg;  // 잘못된 입력시 PrintErrorMsg 호출
                Console.Clear();
                UtilityManager.ColorWriteLine("인벤토리 - 장착 관리", ConsoleColor.Green);
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

                var Baglines = ASCIIManager.Getlines("BagA");

                ASCIIManager.DisplayAlignASCIIArt(Baglines, Align.Center, VerticalAlign.Top);

                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                int i = 0;
                foreach (Item item in DataManager.Instance.HaveItems)
                {
                    Console.WriteLine($" - {i+1}. {item.SelectItem()}");
                    i++;
                }
                Console.WriteLine();
            }

            // 아이템 장착 처리
            private void EquipItem(int num)
            {
                Item selectItem = DataManager.Instance.HaveItems[num-1];  // 유효한 인덱스를 사용해 아이템 선택
                selectItem.OnEquipEvent = DisPlay;
                if (selectItem != null)
                {
                    selectItem.Equip(num-1);  // Item 클래스의 Equip() 호출 (num-1로 인덱스 조정)
                }


            }
        }
    }
            
}
