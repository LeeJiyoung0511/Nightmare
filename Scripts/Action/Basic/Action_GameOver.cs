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
                ASCIIManager.DisplayAlignASCIIArt("BadEnd", Align.Center, VerticalAlign.Top);

                var endTexts = new List<string>();
                endTexts.Add("당신은 영원히 악몽속에 갇히게 되었습니다.");
                endTexts.Add("1.다시하기");
                endTexts.Add("2.게임 종료");

                ASCIIManager.AlignText(endTexts.ToArray(), Align.Center, VerticalAlign.Bottom, 18);

                UtilityManager.InputNumberInRange(1, 2, ReStart, null, "");
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
