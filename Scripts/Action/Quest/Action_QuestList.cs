namespace Nightmare
{
    public partial class GameManager
    {
        public class Action_QuestList : ActionBase
        {
            public Action_QuestList(int number) : base(number) { }

            public override ActionType Type => ActionType.QuestList;

            private List<Quest> quests = new();

            private Dictionary<int, Quest> questDic = new();

            private Quest selectedQuest;

            protected override Dictionary<int, ActionBase> CreateNextActionDic()
            {
                return new Dictionary<int, ActionBase>()
                {
                    { 0,  new Action_Return(0) },
                };
            }

            public override void OnEnter()
            {
                quests = DataManager.Instance.GetPlayerQuestGroup();
                DisplayQuestList();
            }

            private void DisplayQuestList()
            {
                Console.Clear();
                DisPlay();
                UtilityManager.InputNumberInRange(0, quests.Count, SelectQuest, null, "");
            }

            protected override void DisPlay()
            {
                ASCIIManager.DisplayAlignASCIIArt("Glasses", Align.Center, VerticalAlign.Top);

                var questTexts = new List<string>();
                questTexts.Add("[퀘스트]");

                for (int i = 0; i < quests.Count; i++)
                {
                    questDic.Add(i + 1, quests[i]);
                    quests[i].Id = i + 1;
                    questTexts.Add($"{questDic[i + 1].GetQuestTitle()}");
                }
                
                questTexts.Add("0. 나가기");
                questTexts.Add("원하시는 퀘스트를 입력해주세요");

                ASCIIManager.AlignText(questTexts.ToArray(), Align.Center, VerticalAlign.Bottom,3);
            }

            private void SelectQuest(int num)
            {
                if (num == 0)
                {
                    Instance.MoveNextAction(ActionType.Village);
                }
                DisplayQuestInfo(questDic[num]);
            }

            private void DisplayQuestInfo(Quest quest)
            {
                Console.Clear();
                selectedQuest = quest;
                selectedQuest.DisplayQuestInfo();
                DisplayNextAction();
                UtilityManager.InputNumberInRange(1, 2, SelectAction, null, "원하는 행동을 입력해주세요");
            }

            private void DisplayNextAction()
            {
                if (!selectedQuest.IsProgress)
                {
                    Console.WriteLine("\n1. 수락");
                    Console.Write("2. 거절\n");
                }
                else
                {
                    if (selectedQuest.Condition.IsConditionClear)
                    {
                        Console.WriteLine("\n1. 보상받기");
                        Console.Write("2. 돌아가기\n");
                    }
                    else
                    {
                        Console.Write("\n1. 돌아가기\n");
                    }
                }
            }

            private void SelectAction(int input)
            {
                //1번을 입력받았으면
                if (input == 1)
                {
                    // 퀘스트가 진행중이면
                    if (selectedQuest.IsProgress)
                    {
                        if (selectedQuest.Condition.IsConditionClear)
                        {
                            //보상받기
                            selectedQuest.ReceiveReward();
                            //완료한 퀘스트는 표시되지않게 리스트에서 삭제
                            quests.Remove(selectedQuest);
                            selectedQuest.IsProgress = false;
                        }
                    }
                    //퀘스트가 진행중이지않으면
                    else
                    {
                        // 퀘스트 진행하기
                        selectedQuest.IsProgress = true;
                    }
                }

                questDic.Clear();
                DisplayQuestList();
            }
        }
    }

}
