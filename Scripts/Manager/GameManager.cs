using System.Data;

namespace Nightmare
{
    public partial class GameManager
    {
        public static GameManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GameManager();
                }
                return _Instance;
            }
        }
        public bool TutorialOk = false;

        private static GameManager? _Instance = null;

        private string name;

        public List<(Monster monster, int remainingTurns, String doco, int howmany)> DebuffedMonsters = new List<(Monster, int, String, int)>();
        public List<(Boss Boss, int remainingTurns, String doco, int howmany)> buffboss = new List<(Boss, int, String, int)>();
        public List<(Player player, int remainingTurns, String doco, int howmany)> Buffedplayer = new List<(Player, int, String, int)>();
        public List<(Player player, int remainingTurns, String doco, int howmany)> deBuffedplayer = new List<(Player, int, String, int)>();
        private Dictionary<int, int> map = new Dictionary<int, int>();

        public bool IsFirstUsePotion { get; set; } = false;

        public int GameClearCount = 0;

        public void GameClear()
        {
            GameClearCount++;
            MoveNextAction(ActionType.GameClear);
        }

        public void GameReStart()
        {
            GameClearCount++;
            MoveNextAction(ActionType.GameClear);
        }

        public void GameStart()
        {
            SoundManager.PlayBGM("Intro");

            Player = new Player();
            Player.Level = new Level();
            Player.Stat = new Stat();
            Player.Gold = new Gold();
            Player.Avd = new Avd();
            Player.Crt = new Crt();

            startGame();
        }

        private void startGame() // 시작화면
        {
            Console.Clear();
            //string startMessage = "\"늦었다, 늦었어!\"" +
            //    "\n하얀 토끼가 시계를 든 채 어딘가로 달려 갑니다." +
            //    "\n말을 하는 토끼라니요 ? 어떻게 이런 일이 있을 수 있죠 ?" +
            //    "\n당신은 호기심에 말하는 토끼를 쫓아갑니다." +
            //    "\n이런, 발 밑을 잘못 디뎌 끝도 없는 굴로 떨어집니다." +
            //    "\n떨어지고," +
            //    "\n떨어져서," +
            //    "\n눈을 뜨면 그곳은..." +
            //    "\n악몽입니다." +
            //    "\n" +
            //    "\nOnce Upon a Nightmare";

            //foreach (char c in startMessage)
            //{
            //    Console.Write(c);
            //    //Thread.Sleep(30); 
            //}
            Console.WriteLine("\"늦었다, 늦었어!\"");
            Thread.Sleep(500);
            Console.WriteLine("하얀 토끼가 시계를 든 채 어딘가로 달려 갑니다.");
            Thread.Sleep(500);
            Console.WriteLine("말을 하는 토끼라니요 ? 어떻게 이런 일이 있을 수 있죠 ?");
            Thread.Sleep(500);
            Console.WriteLine("당신은 호기심에 말하는 토끼를 쫓아갑니다.");
            Thread.Sleep(500);
            Console.WriteLine("이런, 발 밑을 잘못 디뎌 끝도 없는 굴로 떨어집니다.");
            Thread.Sleep(500);
            Console.WriteLine("떨어지고,");
            Thread.Sleep(500);
            Console.WriteLine("떨어져서,");
            Thread.Sleep(500);
            Console.WriteLine("눈을 뜨면 그곳은...");
            Thread.Sleep(500);
            Console.WriteLine("악몽입니다.");
            Thread.Sleep(500);
            Console.WriteLine();            
            Console.WriteLine("Once Upon a Nightmare");
            Thread.Sleep(2000);
            Console.Clear() ;
            var Posterlines = ASCIIManager.Getlines("Poster");

            ASCIIManager.DisplayAlignASCIIArt(Posterlines, Align.Center, VerticalAlign.Top);
            Thread.Sleep(4000);
            //Console.WriteLine("다음 이야기를 들으려면 아무 키나 입력하세요.");
            //Console.ReadKey();

            GameLoad();
            SetName();
        }
        public void GameSave(object sender, EventArgs e)
        {
            DataManager.Instance.SaveGameData();
        }

        public void GameLoad()
        {
            DataManager.Initialize();
            DataManager.Instance.LoadGameData();
        }

        public void GameDataReset()
        {
            TutorialOk = false;
            IsFirstUsePotion = false;
            DataManager.Instance.DataReset();
        }


        // 이름 설정
        private void SetName()
        {
            Console.Clear();
            string nameMessage = "깜빡, 깜빡." +
                "\n시계 토끼가 당신을 내려다 보고 있습니다" +
                "\n" +
                "\n시계 토끼: 안녕? 악몽에서 나가고 싶니?" +
                "\n시계 토끼: 우선 네 이름을 알려 줘." +
                "\n" +
                "\n일그러진 동화 속을 유영할 당신의 이름은?\n";
            foreach (char c in nameMessage)
            {
                Console.Write(c);
                //Thread.Sleep(30);
            }

            name = Console.ReadLine();
            Console.Clear();
            string saveMessage = $"안녕하세요, {name}." +
                "\n설정하신 이름으로 주인공이 되시겠습니까?" +
                "\n" +
                "\n1. 저장\n2. 취소";

            foreach (char c in saveMessage)
            {
                Console.Write(c);
                //Thread.Sleep(30);
            }

            Console.WriteLine();
            Console.WriteLine();

            UtilityManager.InputNumberInRange(1, 2, SuccessInputNumberInRange, SetName, "어느 페이지로 넘어가시겠습니까?");
        }

        private void SuccessInputNumberInRange(int number)
        {
            if (number == 1)
            {
                SaveName(number, name);
            }
            else
            {
                SetName();

            }
        }

        private void SetJob() // 직업설정
        {
            Console.Clear();
            string jobChoiceMessage = "어떤 동화를 들어보시겠습니까?" +
                "\n" +
                "\n1. 백설공주와 일곱째 난쟁이" +
                "\n2. 신데렐라의 새 언니" +
                "\n3. 모두가 잠든 성의 하인" +
                "\n4. 깊은 바다 속 문어 마녀" +
                "\n5. 힘을 잃은 야수" +
                "\n";
            foreach (char c in jobChoiceMessage)
            {
                Console.Write(c);
                //Thread.Sleep(30);
            }

            UtilityManager.InputNumberInRange(1, 5, JobInputNumberInRange, SetJob, "듣고싶은 동화를 선택해주세요.");
        }

        private void JobInputNumberInRange(int number)
        {
            if (number >= 1 && number <= 5)
            {
                Player.Job = JobChoice(number);
                Skill skill = new Skill();
                skill.SkillSet(Player);
                SoundManager.PlayBGM("Main");
                MoveNextAction(ActionType.Village);
            }
        }

        public Player Player { get; set; }

        private void SaveName(int num, string name)
        {
            switch (num)
            {
                case 1:
                    Player.Name = name;
                    SetJob();
                    break;
                case 2:
                    SetName();
                    break;
            }
        }

        public void TakeAction()
        {

            if (DebuffedMonsters != null)
            {
                // 디버프 지속 시간 감소 및 제거
                for (int i = DebuffedMonsters.Count - 1; i >= 0; i--)
                {
                    var (monster, remainingTurns, doco, howmany) = DebuffedMonsters[i];
                    remainingTurns--;

                    if (remainingTurns <= 0)
                    {

                        Console.WriteLine($"{monster.Name}의 디버프가 해제됨!");
                        if (doco.Equals("공격력"))
                        {
                            monster.MonsterAttack += howmany;
                        }
                        else if (doco.Equals("방어력"))
                        {
                            monster.MonsterDefense += howmany;
                        }
                        DebuffedMonsters.RemoveAt(i);
                    }
                    else
                    {
                        DebuffedMonsters[i] = (monster, remainingTurns, doco, howmany); // 값 업데이트
                    }
                }
            }
            if (buffboss != null)
            {
                // 디버프 지속 시간 감소 및 제거
                for (int i = buffboss.Count - 1; i >= 0; i--)
                {
                    var (Boss, remainingTurns, doco, howmany) = buffboss[i];
                    remainingTurns--;

                    if (remainingTurns <= 0)
                    {

                        Console.WriteLine($"{Boss.Name}의 버프가 해제됨!");
                        if (doco.Equals("공격력"))
                        {
                            Boss.MonsterAttack -= howmany;
                        }
                        else if (doco.Equals("방어력"))
                        {
                            Boss.MonsterDefense -= howmany;
                        }
                        else if (doco.Equals("회피력"))
                        {
                            Boss.MissRate -= howmany;
                        }
                        else if (doco.Equals("치명타율"))
                        {
                            Boss.Crtical -= howmany;
                        }
                        buffboss.RemoveAt(i);
                    }
                    else
                    {
                        buffboss[i] = (Boss, remainingTurns, doco, howmany); // 값 업데이트
                    }
                }
            }
            if (Buffedplayer != null)
            {
                for (int i = Buffedplayer.Count - 1; i >= 0; i--)
                {
                    var (player, remainingTurns, doco, howmany) = Buffedplayer[i];
                    remainingTurns--;

                    if (remainingTurns <= 0)
                    {

                        Console.WriteLine($"{player.Name}의 버프가 해제됨!");
                        if (doco.Equals("공격력"))
                        {
                            Player.Stat.BaseAtk -= howmany;
                        }
                        else if (doco.Equals("방어력"))
                        {
                            Player.Stat.BaseDef -= howmany;
                        }
                        else if (doco.Equals("회피율"))
                        {
                            player.Avd.PlayerAvd -= howmany;
                        }
                        else if (doco.Equals("치명타율"))
                        {
                            player.Crt.PlayerCrt -= howmany;
                        }
                        Buffedplayer.RemoveAt(i);
                    }
                    else
                    {
                        Buffedplayer[i] = (Player, remainingTurns, doco, howmany); // 값 업데이트
                    }
                }

            }
            if (deBuffedplayer != null)
            {
                for (int i = Buffedplayer.Count - 1; i >= 0; i--)
                {
                    var (player, remainingTurns, doco, howmany) = Buffedplayer[i];
                    remainingTurns--;

                    if (remainingTurns <= 0)
                    {

                        Console.WriteLine($"{player.Name}의 디버프가 해제됨!");
                        if (doco.Equals("공격력"))
                        {
                            Player.Stat.BaseAtk += howmany;
                        }
                        else if (doco.Equals("방어력"))
                        {
                            Player.Stat.BaseDef += howmany;
                        }
                        else if (doco.Equals("회피율"))
                        {
                            player.Avd.PlayerAvd -= howmany;
                        }
                        else if (doco.Equals("치명타율"))
                        {
                            player.Crt.PlayerCrt -= howmany;
                        }
                 
                        Buffedplayer.RemoveAt(i);
                    }
                    else
                    {
                        Buffedplayer[i] = (Player, remainingTurns, doco, howmany); // 값 업데이트
                    }
                }

            }



        }
        private Job JobChoice(int num)
        {
            var jobStats = new Dictionary<int, (Job job, float BaseAtk, int BaseDef, int Hp, int Mp, float Avd, float Crt)> {
            { 1, (Job.Dwarf,10,5,100,30,0.1f,0.15f) },
            { 2, (Job.NewSister, 15, 5, 70, 30, 0.1f, 0.15f) },
            { 3, (Job.Saison, 12, 7, 100, 20, 0.1f, 0.15f) },
            { 4, (Job.OctopusWitch, 7, 4, 50, 50, 0.1f, 0.15f) },
            { 5, (Job.WildAnimal, 20, 10, 150, 10, 0.1f, 0.15f) }};


            if (jobStats.TryGetValue(num, out var stats))
            {
                Player.Stat.BaseAtk = stats.BaseAtk;
                Player.Stat.BaseDef = stats.BaseDef;
                Player.Stat.Hp = Player.Stat.MaxHp = stats.Hp;
                Player.Stat.Mp = Player.Stat.MaxMp = stats.Mp;
                Player.Avd.PlayerAvd = stats.Avd;
                Player.Crt.PlayerCrt = stats.Crt;
                Player.QuestGroupId = num;
                return stats.job;
            }

            return Job.None;
        }
    }
}
