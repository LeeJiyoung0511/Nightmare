namespace Nightmare
{
    public partial class GameManager
    {
        public class Action_Recovery : ActionBase
        {

            public Action_Recovery(int number) : base(number) { }

            public override ActionType Type => ActionType.Recovery;

            protected override Dictionary<int, ActionBase> CreateNextActionDic()
            {
                return new Dictionary<int, ActionBase>()
                {
                    { 0,  new Action_Return(0) },
                };
            }

            protected void PrintErrorMsg(int number)
            {
                if (number >= 0 && number < 4)
                {
                    var selectPortion = DataManager.Instance.PortionDatas[number - 1];
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
                OnInputInvalidActionNumber = PrintErrorMsg;
                Thread.Sleep(800);
                Console.Clear();

                Console.WriteLine("회복");
                Console.WriteLine("포션을 사용하여 HP나 MP를 회복할 수 있습니다.");

                Console.WriteLine("[포션 목록]");
                Console.WriteLine();

                int i = 0;
                foreach (var portion in DataManager.Instance.PortionDatas)
                {
                    Console.WriteLine($" - {i + 1}. {portion.ShowPotion()}");
                    i++;
                }
            }
        }
    }
}
