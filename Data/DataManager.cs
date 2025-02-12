using Newtonsoft.Json;
using static Nightmare.GameManager;

namespace Nightmare
{
    internal class DataManager
    {
        public static DataManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new DataManager();

                }
                return _Instance;
            }
        }

        private static DataManager? _Instance = null;

        private static bool isInitialized = false;

        private SaveGameData? saveGameData;

        public static void Initialize()
        {
            if (isInitialized) return;
            JsonDataLoad();
        }

        private static void JsonDataLoad()
        {
            string questfilePath = GetFilePath("QuestData", "Data\\Quest");
            string bossfilePath = GetFilePath("BossData", "Data");
            string itemfilePath = GetFilePath("ItemData", "Data\\Item");

            if (!File.Exists(questfilePath))
            {
                Console.WriteLine("파일이 없습니다.");
                return;
            }

            string questJson = File.ReadAllText(questfilePath);
            var questData = JsonConvert.DeserializeObject<List<Quest>>(questJson);
            Instance.QuestDatas = questData;

            if (!File.Exists(bossfilePath))
            {
                Console.WriteLine("파일이 없습니다.");
                return;
            }

            string bossJson = File.ReadAllText(bossfilePath);
            var bossData = JsonConvert.DeserializeObject<Dictionary<long, Boss>>(bossJson);
            Instance.BossDatas = bossData;

            if (!File.Exists(itemfilePath))
            {
                Console.WriteLine("파일이 없습니다.");
                return;
            }

            string itemJson = File.ReadAllText(itemfilePath);
            var itemData = JsonConvert.DeserializeObject<Dictionary<long, Item>>(itemJson);
            Instance.ItemDatas = itemData;

        }

        private static string GetFilePath(string fileName, string folderName)
        {
            var paths = AppDomain.CurrentDomain.BaseDirectory.Split('\\');
            var newPath = "";

            for (int i = 0; i < paths.Length - 4; i++)
            {
                newPath += paths[i] + "\\";
            }

            newPath += $"{folderName}\\{fileName}.json";

            return newPath;
        }

        public void SaveGameData()
        {
            saveGameData.HaveItems = Instance.HaveItems;
            saveGameData.GameClearCount = GameManager.Instance.GameClearCount;
            saveGameData.GoldAmount = GameManager.Instance.Player.Gold.PlayerGold;
            saveGameData.CanSelectPlayers = Instance.CanSelectPlayerDatas;

            string GameData = JsonConvert.SerializeObject(saveGameData);
            File.WriteAllText(GetFilePath("SaveData", "SaveData"), GameData);
        }

        public void LoadGameData()
        {
            if (!File.Exists(GetFilePath("SaveData", "SaveData")))
            {
                saveGameData = new SaveGameData();
                return;
            }
            else
            {
                string GameData = File.ReadAllText(GetFilePath("SaveData", "SaveData"));
                saveGameData = JsonConvert.DeserializeObject<SaveGameData>(GameData);

                GameManager.Instance.GameClearCount = saveGameData.GameClearCount;
                GameManager.Instance.Player.Gold.PlayerGold = (int)saveGameData.GoldAmount;
                Instance.HaveItems = saveGameData.HaveItems;
                Instance.CanSelectPlayerDatas = saveGameData.CanSelectPlayers;
            }
        }

        //Dict 데이터 사용시 for문 오류, 아이템 판매 시 출력되는 아이템목록 리스트 생성
        public List<Item> HaveItems = new List<Item>();

        //퀘스트 데이터 리스트
        public List<Quest> QuestDatas = new();

        //퀘스트 가져오기
        public List<Quest> GetPlayerQuestGroup()
        {
            long playerQuestGroupId = GameManager.Instance.Player.QuestGroupId;

            bool isDisplay(long questGroupId)
            {
                //TODO:보스 하나를 처치했을때 표시되는 퀘스트  
                return questGroupId == 0 || questGroupId == playerQuestGroupId;
            }

            return QuestDatas.Where(x => isDisplay(x.QuestGroupId)).ToList();
        }

        //아이템 데이터 리스트
        public Dictionary<long, Item> ItemDatas = new() { };

        //몬스터 데이터
        public Dictionary<long, Boss> BossDatas = new();

        //플레이어 데이터
        public Dictionary<long, Player> PlayerDatas = new()
        {
            { 1,new Player() },
            { 2,new Player() },
            { 3,new Player() },
            { 4,new Player() },
            { 5,new Player() }
        };

        //선택 가능한 직업
        public Dictionary<long, Player> CanSelectPlayerDatas = new();

        public void SetPlayerDatas()
        {
            PlayerDatas[1].Level = GameManager.Instance.Player.Level;
            PlayerDatas[1].Name = GameManager.Instance.Player.Name;
            PlayerDatas[1].Job = Job.Dwarf;
            PlayerDatas[1].Stat = new Stat(10, 5, 100, 100, 30, 30);
            PlayerDatas[1].Gold = GameManager.Instance.Player.Gold;
            PlayerDatas[1].Avd = GameManager.Instance.Player.Avd;
            PlayerDatas[1].Crt = GameManager.Instance.Player.Crt;
            PlayerDatas[1].CurrentExp = 0;
            PlayerDatas[1].QuestGroupId = 12345;

            PlayerDatas[2].Level = GameManager.Instance.Player.Level;
            PlayerDatas[2].Name = GameManager.Instance.Player.Name;
            PlayerDatas[2].Job = Job.NewSister;
            PlayerDatas[2].Stat = new Stat(15, 5, 70, 70, 30, 30);
            PlayerDatas[2].Gold = GameManager.Instance.Player.Gold;
            PlayerDatas[2].Avd = GameManager.Instance.Player.Avd;
            PlayerDatas[2].Crt = GameManager.Instance.Player.Crt;
            PlayerDatas[2].CurrentExp = 0;
            PlayerDatas[2].QuestGroupId = 12345;

            PlayerDatas[3].Level = GameManager.Instance.Player.Level;
            PlayerDatas[3].Name = GameManager.Instance.Player.Name;
            PlayerDatas[3].Job = Job.Saison;
            PlayerDatas[3].Stat = new Stat(12, 7, 100, 100, 20, 20);
            PlayerDatas[3].Gold = GameManager.Instance.Player.Gold;
            PlayerDatas[3].Avd = GameManager.Instance.Player.Avd;
            PlayerDatas[3].Crt = GameManager.Instance.Player.Crt;
            PlayerDatas[3].CurrentExp = 0;
            PlayerDatas[3].QuestGroupId = 12345;

            PlayerDatas[4].Level = GameManager.Instance.Player.Level;
            PlayerDatas[4].Name = GameManager.Instance.Player.Name;
            PlayerDatas[4].Job = Job.OctopusWitch;
            PlayerDatas[4].Stat = new Stat(7, 4, 50, 50, 50, 50);
            PlayerDatas[4].Gold = GameManager.Instance.Player.Gold;
            PlayerDatas[4].Avd = GameManager.Instance.Player.Avd;
            PlayerDatas[4].Crt = GameManager.Instance.Player.Crt;
            PlayerDatas[4].CurrentExp = 0;
            PlayerDatas[4].QuestGroupId = 12345;

            PlayerDatas[5].Level = GameManager.Instance.Player.Level;
            PlayerDatas[5].Name = GameManager.Instance.Player.Name;
            PlayerDatas[5].Job = Job.WildAnimal;
            PlayerDatas[5].Stat = new Stat(20, 10, 150, 150, 10, 10);
            PlayerDatas[5].Gold = GameManager.Instance.Player.Gold;
            PlayerDatas[5].Avd = GameManager.Instance.Player.Avd;
            PlayerDatas[5].Crt = GameManager.Instance.Player.Crt;
            PlayerDatas[5].CurrentExp = 0;
            PlayerDatas[5].QuestGroupId = 12345;
        }


        //소모성 아이템(전투 중 볼 수 있는 인벤토리) 리스트(포션 3종+스페셜 드랍아이템 5종)
        

        public List<Potion> HealthConsumableItems = new();
        public List<Potion> ManaConsumableItems = new();
        public List<Potion> LoveConsumableItems = new();
        public List<Item> BossConsumableItems = new();


        //보스 처치시 필요한 필수 아이템 데이터
        public Dictionary<long, KillBossItem> KillBossItemDatas = new();


        //기본 포션 3개씩 추가하는 함수
        public void InitializeConsumableItems()
        {

            for (int i = 0; i < 3; i++)
            {
                Potion potion = new Potion()
                {
                    Id = 18,
                    Type = (ItemType)5,
                    Name = "앨리스의 쿠키",
                    Value = 20,
                    Desc = "Eat Me! 라는 꼬리표가 달려있다.", //임의값
                };
                DataManager.Instance.HealthConsumableItems.Add(potion);

            }
            for (int i = 0; i < 3; i++)
            {
                Potion potion = new Potion()
                {
                    Id = 24,
                    Type = (ItemType)6,
                    Name = "앨리스의 음료",
                    Value = 10,
                    Desc = "Drink Me! 라는 꼬리표가 달려있다.", //임의값
                };
                DataManager.Instance.ManaConsumableItems.Add(potion);
            }
        }





        public List<Item> EquippedItems = new();

        public List<Potion> PortionDatas = new()
        {

            new Potion()
            {
                PotionId = 18,
                PotionCount = 0,
                PotionMaxCount = 3
            },
            new Potion()
            {
                PotionId = 24,
                PotionCount = 0,
                PotionMaxCount = 3
            },
            new Potion()
            {
                PotionId = 25,
                PotionCount = 0,
                PotionMaxCount = 4
            }
        };

        public void DataReset()
        {
            // 가지고 있는 아이템 중에 하트조각과 스페셜 아이템 이외에 아이템은 삭제
            HaveItems.RemoveAll(x => x.Type == ItemType.HeartPiece && x.Type == ItemType.Special);

            // 장착된 아이템 삭제
            EquippedItems.Clear();
        }
    }
}



