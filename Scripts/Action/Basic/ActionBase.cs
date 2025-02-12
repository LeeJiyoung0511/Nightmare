namespace Nightmare
{
    public partial class GameManager
    {
        public abstract partial class ActionBase
        {
            public abstract ActionType Type { get; }

            //선택할수있는 활동클래스 딕셔너리 생성함수
            protected abstract Dictionary<int, ActionBase> CreateNextActionDic();

            //유효하지않은 번호 입력받았을때 쓰이는 함수
            protected Action<int> OnInputInvalidActionNumber = delegate { };

            private int actionNumber = 0;

            private Dictionary<int, ActionBase> actionDic = new Dictionary<int, ActionBase>();

            public ActionBase(int number)
            {
                actionNumber = number;
            }

            public virtual void OnEnter()
            {
                Console.Clear();

                actionDic = CreateNextActionDic();
                OnInputInvalidActionNumber = PrintErrorMessage;

                DisPlay();
                InputNextAction();
            }

            public virtual void OnExit() { }

            protected abstract void DisPlay();

            private void PrintInfo()
            {
                Console.WriteLine($"\n{actionNumber}. {UtilityManager.GetDescription(Type)}");
            }

            // 활동 표시
            protected void DisplayNextActions()
            {
                foreach (var action in actionDic.Values)
                {
                    action.PrintInfo();
                }
            }

            // 활동 선택
            protected void InputNextAction()
            {
                while (true)
                {
                    DisplayNextActions();
                    Console.Write("\n원하시는 행동을 입력해주세요.\n>>");

                    if (int.TryParse(Console.ReadLine(), out int actionNumber))
                    {
                        if (actionDic.ContainsKey(actionNumber))
                        {
                            Instance.CurrentAction = actionDic[actionNumber];
                            break;
                        }
                        OnInputInvalidActionNumber?.Invoke(actionNumber);
                    }
                    else
                    {
                        PrintErrorMessage(0);
                    }
                    Thread.Sleep(800);
                }
            }

            private void PrintErrorMessage(int number)
            {
                UtilityManager.PrintErrorMessage();
            }
        }

        public partial class ActionBase
        {
            public static ActionBase None { get; } = new Action_None();
            private class Action_None
                : ActionBase
            {
                public Action_None() : base(99) { }
                public override ActionType Type => ActionType.None;
                protected override Dictionary<int, ActionBase> CreateNextActionDic()
                {
                    throw new NotImplementedException();
                }
                protected override void DisPlay()
                {
                    throw new NotImplementedException();
                }
            }
        }
    }


}
