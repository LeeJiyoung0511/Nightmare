namespace Nightmare
{
    public partial class GameManager
    {
        public class Action_RecoveryItem : ActionBase
        {
            public Action_RecoveryItem(int number) : base(number) { }


            public override ActionType Type => ActionType.RecoveryItem;

            protected override Dictionary<int, ActionBase> CreateNextActionDic()
            {
                return new Dictionary<int, ActionBase>()
                {
                    { 0,  new Action_Return(0) },

                };
            }
            private void PrintErrorMsg(int number)
            {

                if (number >= 0 && number < 2)
                {
                    var selectPortion = DataManager.Instance.PortionDatas[number-1];
                    selectPortion.OnUsePotionEvent = DisPlay;

                    selectPortion.UsePotion();
                }
                else
                {
                    UtilityManager.PrintErrorMessage();
                }
            }

            protected override void DisPlay()
            {
                Console.Clear();


              //  OnInputInvalidActionNumber = PrintErrorMsg;
               
                while (true)
                {
                    Console.WriteLine("회복");
                    Console.WriteLine("포션을 사용하여 HP나 MP를 회복할 수 있습니다.");

                    Console.WriteLine("[포션 목록]");
                    Console.WriteLine();
                    Console.WriteLine("0. 돌아가기");

                    Console.WriteLine();
                    Show();
                    Console.WriteLine("번호를 입력해주세요");
                    int number = int.Parse(Console.ReadLine());
                    if (number == 0)
                    {
                        Instance.MoveNextAction(ActionType.Village);
                    }
                    else if (number == 1)
                    {
                        if (DataManager.Instance.HealthConsumableItems.Count() == 0)
                        {
                            Console.WriteLine("포션이 없습니다.");
                            continue;
                        }
                        else
                        {
                            DataManager.Instance.HealthConsumableItems[0].UsePotion();
                            DataManager.Instance.HealthConsumableItems.RemoveAt(0);
                        }
                    }
                    else if (number == 2)
                    {
                        if (DataManager.Instance.ManaConsumableItems.Count() == 0)
                        {
                            Console.WriteLine("포션이 없습니다.");
                            continue;
                        }
                        else
                        {
                            DataManager.Instance.ManaConsumableItems[0].UsePotion();
                            DataManager.Instance.ManaConsumableItems.RemoveAt(0);
                        }
                    }
                    else if (number == 3)
                    {
                        if (DataManager.Instance.LoveConsumableItems.Count() == 0)
                        {
                            Console.WriteLine("포션이 없습니다.");
                            continue;
                        }
                        else
                        {
                            DataManager.Instance.LoveConsumableItems[0].UsePotion();
                            DataManager.Instance.LoveConsumableItems.RemoveAt (0);
                        }
                    }
                    else
                    {
                        UtilityManager.PrintErrorMessage();
                        continue;
                    }

                    Thread.Sleep(1000);
                    Console.Clear();
                }



            }

            public void Show()
            {

                if (DataManager.Instance.HealthConsumableItems.Count() != 0)
                    Console.WriteLine($"1. 앨리스의 쿠키 | HP +20 | Eat Me! 라는 꼬리표가 달려있다 | (개수 : {DataManager.Instance.HealthConsumableItems.Count()} / 3 )");
                if (DataManager.Instance.ManaConsumableItems.Count() != 0)
                    Console.WriteLine($"2. 앨리스의 음료 | MP +10 | Drink Me! 라는 꼬리표가 달려있다. | (개수 : {DataManager.Instance.ManaConsumableItems.Count()} / 3 )");
                if (DataManager.Instance.LoveConsumableItems.Count() != 0)
                    Console.WriteLine($"3. 사랑의 정수 | HP +100, MP +50 | 모든 것은 사랑의 힘으로부터 | (개수 : {DataManager.Instance.LoveConsumableItems.Count()} / 4 )");
            }
        }
    }
}
