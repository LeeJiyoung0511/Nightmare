namespace Nightmare
{
    public partial class GameManager
    {
        public class Action_Village : ActionBase
        {
            public Action_Village(int number) : base(number) { }

            public override ActionType Type => ActionType.Village;

            protected override Dictionary<int, ActionBase> CreateNextActionDic()
            {
                return new Dictionary<int, ActionBase>()
                {
                    { 1,  new Action_State(1) },
                    { 2,  new Action_Inventory(2) },
                    { 3,  new Action_Shop(3) },
                    { 4,  new Action_Dungeon(4) },
                    { 5,  new Action_QuestList(5) },
                    { 6,  new Action_GameClear(6) }, //디버깅
                    { 7,  new Action_GameOver(7) }, //디버깅

                };
            }

            protected override void DisPlay()
            {
                string titleText = Instance.GameClearCount > 0 ? "악몽의 주인이 그림자를 드리우는 중..." : "동화의 흐름이 엉키는중....";
                Console.WriteLine(titleText);
            }
        }

    }

}
