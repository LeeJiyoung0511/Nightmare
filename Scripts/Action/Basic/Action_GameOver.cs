namespace Nightmare
{
    public partial class GameManager
    {
        public class Action_GameOver : ActionBase
        {
            public Action_GameOver(int number) : base(number) { }

            public override ActionType Type => ActionType.GameOver;

            protected override Dictionary<int, ActionBase> CreateNextActionDic()
            {
                return new Dictionary<int, ActionBase>()
                {
                    { 0,  new Action_Return(0) },
                };
            }

            public override void OnEnter()
            {
                SoundManager.PlayBGM("BadEnding");
                Console.Clear();
                //플레이한 직업 삭제
                DisPlay();
            }

            protected override void DisPlay()
            {
                Console.WriteLine("게임 오버!");

                Console.WriteLine("\n1.다시하기");
                Console.WriteLine("2.게임 종료");

                UtilityManager.InputNumberInRange(1, 2, ReStart, null, "원하시는 행동을 입력해주세요");
            }

            private void ReStart(int num)
            {
                if (num == 1)
                {
                    Instance.GameStart();
                }
                else
                {
                    Environment.Exit(0);
                }
            }

        }
    }
}
