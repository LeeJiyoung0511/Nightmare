using System.Text;

namespace Nightmare
{
    public enum QuestType
    {
        ChapterClear = 0,
        Equip = 1,
        UseItem = 2,
        KillBoss = 3,
        AllClear = 4,
    }

    public class Quest
    {
        public long Id { get; set; }
        public long QuestGroupId { get; set; }
        public string? Title { get; set; } // 퀘스트 제목
        public string? Desc { get; set; } // 퀘스트 내용
        public Condition Condition { get; set; } // 퀘스트 클리어 조건
        public Reward[] Rewards { get; set; } //퀘스트 보상

        //퀘스트 진행중인지
        public bool IsProgress
        {
            get => isProgress;
            set
            {
                isProgress = value;
                if (isProgress)
                {
                    WaitForCondition();
                }
            }
        }

        private bool isProgress = false;

        public string GetQuestTitle()
        {
            string IsProgressText = IsProgress ? "[진행중]" : "";
            return $"{Id}.{Title}{IsProgressText}";
        }

        public void DisplayQuestInfo()
        {
            Console.WriteLine($"{Title}");
            Console.WriteLine($"\n{Desc}\n");

            Console.Write($"{Condition.GetConditionText()}\n");

            Console.WriteLine($"\n-보상-");

            foreach (var reward in Rewards)
            {
                Console.WriteLine($"- {reward.GetRewardInfo()}");
            }
        }

        public async void WaitForCondition()
        {
            Task waitTask = WaitFonClearCondition();
            await waitTask;
            Console.WriteLine("\n퀘스트 완료!");
            Condition.IsConditionClear = true;
        }

        private async Task WaitFonClearCondition()
        {
            while (!Condition.GetClearConditoin())
            {
                await Task.Delay(1000);
            }
        }

        public void ReceiveReward()
        {
            foreach (var reward in Rewards)
            {
                reward.ReceiveReward();
            }
        }
    }


    public class Condition
    {
        public QuestType QuestType { get; set; }

        public long ConditionValue { get; set; }

        public bool IsConditionClear { get; set; }

        public string GetConditionText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("-");

            switch (QuestType)
            {
                case QuestType.ChapterClear:
                    sb.Append($"{ConditionValue}챕터 클리어");
                    break;
                case QuestType.Equip:
                    sb.Append($"장비 착용해보기");
                    break;
                case QuestType.UseItem:
                    sb.Append($"포션 사용해보기");
                    break;
                case QuestType.KillBoss:
                    sb.Append($"{DataManager.Instance.BossDatas[ConditionValue].Name} 처치");
                    break;
            }

            if (IsConditionClear)
            {
                sb.Append(" 완료!");
            }

            return sb.ToString();
        }

        public bool GetClearConditoin()
        {
            switch (QuestType)
            {
                case QuestType.ChapterClear:
                    // TODO:튜토리얼 스테이지 클리어
                    return GameManager.Instance.TutorialOk;
                case QuestType.KillBoss:
                    //보스 처치
                    return !DataManager.Instance.BossDatas[ConditionValue].IsLive;
                case QuestType.Equip:
                    //첫 아이템 장착
                    return DataManager.Instance.EquippedItems.Count > 0;
                case QuestType.UseItem:
                    //첫 포션 아이템 사용 
                    return GameManager.Instance.IsFirstUsePotion;
                default:
                    return false;
            }
        }

    }
}
