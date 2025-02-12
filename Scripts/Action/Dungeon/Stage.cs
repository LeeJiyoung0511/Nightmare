using System;

namespace Nightmare
{
    public partial class GameManager
    {
        internal class Stage
        {
            public int MoneyRange { get; set; }
            public int RandomRange { get; set; }
            public bool Clear { get; set; }
            public bool IsFinal { get; set; }

            public Stage(int randomRange, int MoneyRange)
            {
                RandomRange = randomRange;
                this.MoneyRange = MoneyRange;

            }
            public bool TutorialBattle(Player player)
            {
                List<Monster> monsters = new List<Monster>();
                Monster mon = new Monster();
                for (int i = 0; i < 3; i++)
                {
                    Monster monster = new Monster(1, 10, 3, 1, "연습용 표적", 0, 0);
                    monsters.Add(monster);
                }
                Random ran = new Random();
                int ii = 1;
                int DeathCount = 0;

                while (DeathCount < monsters.Count)
                {
                    //foreach로 넣기
                    foreach (Monster monster in monsters)
                    {
                        Console.WriteLine($"{ii}. {monster.MonsterDIe(ref DeathCount)}");
                        ii++;
                    }

                    if (DeathCount >= monsters.Count)
                    {
                        break;
                    }

                    //플레이어 상태 띄우기
                    Console.WriteLine($"Lv.{player.Level.PlayerLevel} {player.Name} ({player.Job})");
                    Console.WriteLine($"공격력: {player.Stat.BaseAtk + player.Stat.EquipAtk})");
                    Console.WriteLine($"방어력: {player.Stat.BaseDef + player.Stat.EquipDef})");
                    Console.WriteLine($"체력: {player.Stat.Hp}/{player.Stat.MaxHp} 마력: {player.Stat.Mp}/{player.Stat.MaxMp}");
                    Console.WriteLine($"치명타율: {Math.Round((player.Crt.PlayerCrt + player.Crt.EquipCrt) * 100),0} %");
                    Console.WriteLine($"회피율: {Math.Round((player.Avd.EquipAvd + player.Avd.PlayerAvd) * 100),0} %");
                    ii = 1;
                    Console.WriteLine("행동을 선택해주세요(스킬 사용 정지)");
                    Console.WriteLine("1. 공격");
                    int Select = int.Parse(Console.ReadLine());
                    if (Select == 1)
                    {
                        int AttackSelect;
                        //어느 적을 공격하는지
                        while (true)
                        {
                            Console.WriteLine("누굴 공격하니");
                            try
                            {
                                AttackSelect = int.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("숫자 제대로 입력해");
                                continue;
                            }
                            if (AttackSelect < 1 || AttackSelect > monsters.Count)
                            {
                                Console.WriteLine("잘못된 적 선택");

                                continue;
                            }
                            else if (!monsters[AttackSelect - 1].IsLive)
                            {
                                Console.WriteLine("이미 죽었습니다.");
                                continue;
                            }
                            else
                            {
                                mon.AttackedFromPlayer(monsters[AttackSelect - 1], player);
                                foreach (Skill skill in player.Playerskill)
                                {
                                    if (skill.CurrentCoolTime < skill.SkillCoolTime)
                                    {
                                        skill.CurrentCoolTime++;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                Console.WriteLine("튜토리얼 클리어!");
                return true;

            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public void Battle(Player player)
            {
                List<Monster> monsters = new List<Monster>();
                //랜덤 함수
                Random Ran = new Random();
                //함수를 사용하기 위한 개체참조
                Monster mon = new Monster();
                //몬스터를 저장할 변수
                int ii = 1;
                //리스트에 넣기
                for (int i = 0; i < 3; i++)
                {
                    monsters.Add(mon.Monstersummon(Ran.Next(1 + (3 * RandomRange), 4 + (3 * RandomRange)), MoneyRange));
                }

                BattlePhase(mon, monsters, player);

                //플레이어의 정보를 받아서 일정 확률로 장비 얻기
                //돈 추가
                Console.WriteLine("스테이지 클리어!");
                //일정 확률의 보상 얻기
                GetClearBoSang(monsters, player);
                //다시 돌아가기
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public void BossBattle(Player player) //player
            {
                Boss boss = new Boss();
                Skill skill = new Skill();

                skill.BossSkillSet(player, boss);
                boss = DataManager.Instance.BossDatas[(int)player.Job];
                boss.BossIntroduce((int)player.Job);

                List<Monster> monsters = new List<Monster>();

                monsters.Add(boss);

                BossBattlePhase(boss, monsters, player, boss);
                //플레이어의 정보를 받아서 일정 확률로 장비 얻기
                //돈 추가
                Console.WriteLine("스테이지 클리어!");
                //일정 확률의 보상 얻기
                GetClearBoSang(monsters, player);
                DataManager.Instance.HaveItems.Add(DataManager.Instance.ItemDatas[12 + (int)player.Job]);
                //다시 돌아가기
                Instance.GameClear();
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public void BossBattlePhase(Monster mon, List<Monster> monster, Player player, Boss boss)
            {
                SoundManager.PlayBGM("Boss");
                int turn = 0;
                int DeathCount = 0;
                while (boss.IsLive)
                {

                    Console.WriteLine($"{boss.MonsterDIe(ref DeathCount)}");

                    //플레이어 상태 띄우기
                    Playerinfo(player);
                    //플레이어 공격
                    PlayerAction(monster, player, mon, ref DeathCount);
                    turn++;
                    BossAttack(boss, monster, player, ref DeathCount, ref turn);
                    //몬스터 공격

                }

            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public void BattlePhase(Monster mon, List<Monster> monsters, Player player)
            {

                var bgmName = "";

                switch(MoneyRange)
                {
                    case 1:
                        bgmName = "Battle1";
                        break;
                    case 2:
                        bgmName = "Battle2";
                        break;
                    case 3:
                        bgmName = "Battle3";
                        break;
                }

                SoundManager.PlayBGM(bgmName);


                int ii = 1;
                int DeathCount = 0;
                while (DeathCount < monsters.Count)
                {
                    //foreach로 넣기
                    foreach (Monster monster in monsters)
                    {
                        Console.WriteLine($"{monster.MonsterDIe(ref DeathCount)}");
                        ii++;
                    }
                    ii = 1;
                    //플레이어 상태 띄우기
                    Playerinfo(player);
                    //플레이어 공격
                    PlayerAction(monsters, player, mon, ref DeathCount);
                    //몬스터 공격
                    MonsterAttack(monsters, player, mon, ref DeathCount);


                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public void BossAttack(Boss boo, List<Monster> monsters, Player player, ref int DeathCount, ref int turn)
            {
                Random ran = new Random();
                boo.MonsterDIe(ref DeathCount);
                if (boo.IsLive)
                {
                    if (turn % 3 != 0)
                    {
                        int Damage = boo.MonsterAttackToPlayer();
                        if (ran.Next(0, 101) < boo.Crtical)
                        {

                            if (((player.Avd.EquipAvd + player.Avd.PlayerAvd) * 100) < ran.Next(0, 101))
                            {
                                if (Damage > (player.Stat.BaseDef + player.Stat.EquipDef))
                                {
                                    player.Stat.Hp -= (2 * Damage) - (player.Stat.BaseDef + player.Stat.EquipDef);
                                    Console.WriteLine($"!!크리티컬 데미지 {Damage} -플레이어 방어력{player.Stat.BaseDef + player.Stat.EquipDef} =" +
                                        $"{2 * Damage - (player.Stat.BaseDef + player.Stat.EquipDef)} 피해를 입어 {player.Stat.Hp}가 되었습니다!!");
                                    turn++;
                                }
                                else
                                {
                                    player.Stat.Hp -= 1;
                                    Console.WriteLine("방어력이 높아서 1만 닳음");
                                    turn++;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"회피 성공");
                                turn++;
                            }
                            if (player.Stat.Hp <= 0)
                            {
                                Instance.MoveNextAction(ActionType.Village);
                            }
                        }
                        else
                        {
                            if (((player.Avd.EquipAvd + player.Avd.PlayerAvd) * 100) < ran.Next(0, 101))
                            {
                                if (Damage > (player.Stat.BaseDef + player.Stat.EquipDef))
                                {
                                    player.Stat.Hp -= (Damage) - (player.Stat.BaseDef + player.Stat.EquipDef);
                                    Console.WriteLine($" 데미지 {Damage} -플레이어 방어력{player.Stat.BaseDef + player.Stat.EquipDef} =" +
                                        $"{Damage - (player.Stat.BaseDef + player.Stat.EquipDef)} 피해를 입어 {player.Stat.Hp}가 되었습니다!!");
                                    turn++;
                                }
                                else
                                {
                                    player.Stat.Hp -= 1;
                                    Console.WriteLine("방어력이 높아서 1만 닳음");
                                    turn++;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"회피 성공");
                                turn++;
                            }
                            if (player.Stat.Hp <= 0)
                            {
                                Instance.MoveNextAction(ActionType.Village);
                            }
                        }
                    }
                    else
                    {
                        int whatSkill = ran.Next(0, boo.BossSkill.Count);
                        turn = 0;
                        if (boo.BossSkill[whatSkill].CurrentCoolTime < boo.BossSkill[whatSkill].SkillCoolTime)
                        {
                            if (whatSkill == 0)
                            {
                                boo.BossSkill[whatSkill + 1].SkillUse(player, monsters, ref DeathCount);

                            }
                            else
                            {
                                boo.BossSkill[whatSkill - 1].SkillUse(player, monsters, ref DeathCount);
                            }
                        }

                        foreach (Skill skill in boo.BossSkill)
                        {
                            if (skill.CurrentCoolTime < skill.SkillCoolTime)
                            {
                                skill.CurrentCoolTime++;
                            }
                        }
                        Instance.TakeAction();

                        boo.BossSkill[whatSkill].SkillUse(player, monsters, ref DeathCount);
                        boo.BossSkill[whatSkill].CurrentCoolTime = 0;
                        if (player.Stat.Hp <= 0)
                        {
                            Instance.MoveNextAction(ActionType.Village);
                        }

                    }
                }

            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public void Playerinfo(Player player)
            {
                Console.WriteLine($"Lv.{player.Level.PlayerLevel} {player.Name} ({player.Job})");
                Console.WriteLine("----------------------------------");
                Console.WriteLine($"공격력: {player.Stat.BaseAtk + player.Stat.EquipAtk})                      |");
                Console.WriteLine($"방어력: {player.Stat.BaseDef + player.Stat.EquipDef})                       |");
                Console.WriteLine($"체력: {player.Stat.Hp}/{player.Stat.MaxHp} 마력: {player.Stat.Mp}/{player.Stat.MaxMp}        |");
                Console.WriteLine($"치명타율: {(player.Crt.PlayerCrt + player.Crt.EquipCrt)}                     |");
                Console.WriteLine($"회피율: {player.Avd.EquipAvd + player.Avd.PlayerAvd})                      |");
                Console.WriteLine("----------------------------------");
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public int InputandReturn(int i)
            {
                int number;
                while (true)
                {
                    try
                    {
                        if (i == 1)
                        {
                            Console.WriteLine("행동을 골라주세요");
                            Console.WriteLine("1. 공격");
                            Console.WriteLine("2. 스킬");
                            Console.WriteLine("3. 아이템 사용");
                        }
                        else if (i == 2)
                        {
                            Console.WriteLine("0. 돌아가기");
                            Console.WriteLine("어느 적을 공격할거니");
                        }
                        else if (i == 3)
                        {
                            Console.WriteLine("0. 돌아가기");
                            Console.WriteLine("사용할 스킬을 골라주세요");
                        }
                        else
                        {
                            Console.WriteLine("사용할 아이템을 골라주세요");
                            Console.WriteLine("1. 앨리스의 쿠키");
                            Console.WriteLine("2. 엘리스의 음료");
                        }

                        number = int.Parse(Console.ReadLine());
                        return number;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("숫자를 입력하세요.");
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public void GetClearBoSang(List<Monster> mm, Player player)
            {
                Random random = new Random();
                int AllMoney = 0;
                foreach (Monster m in mm)
                {
                    player.Gold.PlayerGold += m.MonsterMoney;
                    AllMoney += m.MonsterMoney;
                }

                Console.WriteLine($"{AllMoney}Gold 획득! 총 골드 {player.Gold.PlayerGold} Gold");

                if (this.RandomRange > 0 && random.Next(0, 11) > 5)
                {
                    if (random.Next(0, 11) % 2 == 0)
                    {
                        
                        if(DataManager.Instance.ManaConsumableItems.Count >=3)
                        {
                            Console.WriteLine("가방이 꽉 찼습니다.");
                        }
                        else
                        {
                            Potion potion = new Potion()
                            {
                                Id = 18,
                                Type = (ItemType)5,
                                Name = "앨리스의 쿠키",
                                Value = 20,
                                Desc = "Eat Me! 라는 꼬리표가 달려있다.", //임의값
                            };
                            DataManager.Instance.ManaConsumableItems.Add(potion);
                            Console.WriteLine($"앨리스의 쿠키 획득! ");
                        }
                    
                    }
                    if (random.Next(0, 11) % 2 == 1)
                    {

                        if (DataManager.Instance.ManaConsumableItems.Count >= 3)
                        {
                            Console.WriteLine("가방이 꽉 찼습니다.");
                        }
                        else
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
                            Console.WriteLine($"앨리스의 음료 획득! ");
                        }
                    }
                    if (IsFinal && random.Next(0, 11) > -1)
                    {
                        
                        Item item = new Item();

                        item = DataManager.Instance.ItemDatas[18 + (int)player.Job];
                        Console.WriteLine($"{item.Name} 획득! 보스에게로 향할 수 있습니다. ");
                        DataManager.Instance.BossConsumableItems.Add(item);


                        //killBoss.PickUpItem(item);
                    }
                }

            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public override string ToString()
            {

                String s = $"적의 수: 3 평균 레벨{1 + (3 * RandomRange)} ~ {4 + (3 * RandomRange)} ";
                return s;
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public void PlayerAction(List<Monster> monsters, Player player, Monster mon, ref int DeathCount)
            {
                int Select = InputandReturn(1);
                if (Select == 1)
                {
                    //어느 적을 공격하는지
                    while (true)
                    {
                        int AttackSelect = InputandReturn(2);

                        if (AttackSelect < 1 || AttackSelect > monsters.Count)
                        {
                            UtilityManager.PrintErrorMessage();
                            continue;
                        }
                        else if (!monsters[AttackSelect - 1].IsLive)
                        {
                            Console.WriteLine("이미 죽었습니다.");
                            PlayerAction(monsters, player, mon, ref DeathCount);
                            return;
                        }
                        else if (AttackSelect == 0)
                        {
                            PlayerAction(monsters, player, mon, ref DeathCount);
                            return;
                        }
                        else
                        {
                            mon.AttackedFromPlayer(monsters[AttackSelect - 1], player);
                            foreach (Skill skill in player.Playerskill)
                            {
                                if (skill.CurrentCoolTime < skill.SkillCoolTime)
                                {
                                    skill.CurrentCoolTime++;
                                }
                            }
                            Instance.TakeAction();
                            break;
                        }
                    }
                }
                else if (Select == 2)
                {
                    int number = 1;
                    foreach (Skill skill in player.Playerskill)
                    {
                        Console.WriteLine($"{number}. {skill.ToString()}");
                        number++;
                    }

                    while (true)
                    {
                        int str = InputandReturn(3);
                        if (str > player.Playerskill.Count || str < 0)
                        {
                            UtilityManager.PrintErrorMessage();
                            continue;
                        }
                        else if (player.Playerskill[str - 1].SkillMp > player.Stat.Mp)
                        {
                            Console.WriteLine($"마나가  {player.Playerskill[str - 1].SkillMp - player.Stat.Mp}가 부족합니다");
                            PlayerAction(monsters, player, mon, ref DeathCount);
                            return;
                        }
                        else if (player.Playerskill[str - 1].CurrentCoolTime < player.Playerskill[str - 1].SkillCoolTime)
                        {
                            Console.WriteLine("쿨타임입니다.");
                            PlayerAction(monsters, player, mon, ref DeathCount);
                            return;
                        }
                        else if (str == 0)
                        {
                            PlayerAction(monsters, player, mon, ref DeathCount);
                            return;
                        }

                        foreach (Skill skill in player.Playerskill)
                        {
                            if (skill.CurrentCoolTime < skill.SkillCoolTime)
                            {
                                skill.CurrentCoolTime++;
                            }
                        }
                        Instance.TakeAction();
                        player.Playerskill[str - 1].SkillUse(player, monsters, ref DeathCount);
                        player.Playerskill[str - 1].CurrentCoolTime = 0;
                        break;
                    }
                }
                else if (Select == 3)
                {
                    while (true)
                    {
                        bool Isboss = false;
                        if (monsters[0] is Boss)
                        {
                            Isboss = true;
                        }

                        int hecount = DataManager.Instance.HealthConsumableItems.Count();
                        int manacount = DataManager.Instance.ManaConsumableItems.Count();
                        int lovecount = DataManager.Instance.LoveConsumableItems.Count();
                        int Bosscount = DataManager.Instance.BossConsumableItems.Count();

                        int number = 1;
                        Console.Write("0. 돌아가기\n");


                        if (hecount != 0)
                        {
                            foreach (Potion potion in DataManager.Instance.HealthConsumableItems)
                            {
                                Console.WriteLine($"{number}. {potion.ToShow()}");
                                number++;
                            }
                        }
                        if (manacount != 0)
                        {
                            foreach (Potion potion in DataManager.Instance.ManaConsumableItems)
                            {
                                Console.WriteLine($"{number}. {potion.ToShow()}");
                                number++;
                            }
                        }
                        if(lovecount != 0)
                        {
                            foreach (Potion potion in DataManager.Instance.LoveConsumableItems)
                            {
                                Console.WriteLine($"{number}. {potion.ToShow()}");
                                number++;
                            }
                        }
                        if(Bosscount != 0)
                        {
                            foreach (KillBossItem potion in DataManager.Instance.BossConsumableItems)
                            {
                                Console.WriteLine($"{number}. {potion.ToShow()}");
                                number++;
                            }
                        }




                        Console.Write("번호 입력: ");
                        int choice = int.Parse(Console.ReadLine());

                        if (choice == 0)
                        {
                            PlayerAction(monsters, player, mon, ref DeathCount);
                            return;
                        }
                        else if (choice > 0 && choice <= hecount+manacount+lovecount+Bosscount)
                        {
                            if (choice > 0 && choice <= hecount)
                            {
                                DataManager.Instance.HealthConsumableItems[choice - 1].UsePotion();
                                DataManager.Instance.HealthConsumableItems.RemoveAt(choice - 1);
                                break;

                            }
                            else if(choice>hecount &&  choice <= manacount+ hecount)
                            {
                                DataManager.Instance.ManaConsumableItems[choice - hecount - 1].UsePotion();
                                DataManager.Instance.ManaConsumableItems.RemoveAt(choice - hecount - 1);
                                break;
                            }
                            else if(choice > hecount+manacount && choice <= hecount+manacount+lovecount)
                            {
                                DataManager.Instance.LoveConsumableItems[choice - manacount - hecount - 1].UsePotion();
                                DataManager.Instance.LoveConsumableItems.RemoveAt(choice - manacount - hecount - 1);
                                break;
                            }
                            else
                            {
                                if (monsters[0].MonsterHealth < monsters[0].MonsterHealth * (15.0 / 100) && Isboss)
                                {
                                    DataManager.Instance.BossConsumableItems[0].BossKill(monsters,ref DeathCount);
                                }
                                else
                                {
                                    Console.WriteLine("사용할 조건이 충족되지 않았습니다.");
                                    PlayerAction(monsters, player, mon, ref DeathCount);
                                    return;
                                }
                              
                            }
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }




                    //while (true)
                    //{
                    //    bool Isboss = false;
                    //    if (monsters[0] is Boss)
                    //    {
                    //        Isboss = true;
                    //    }

                    //    int number = 1;
                    //    Console.Write("0. 돌아가기\n");
                    //    foreach (var portion in DataManager.Instance.PortionDatas)
                    //    {
                    //        Console.WriteLine($"{number}. {portion.ShowPotion()}");
                    //        number++;
                    //    }
                    //    if (DataManager.Instance.ConsumableItems.Any(d => d.Id == (int)Instance.Player.Job + 17) && DataManager.Instance.ConsumableItems != null)
                    //    {

                    //        Item selectedPotion = DataManager.Instance.ConsumableItems.FirstOrDefault(d => d.Id == (int)Instance.Player.Job + 17);
                    //        Console.WriteLine($"{number}. {selectedPotion.Name} {selectedPotion.Desc}");
                    //    }
                    //    Console.Write("번호 입력: ");
                    //    int choice = int.Parse(Console.ReadLine());

                    //    if (choice == 0)
                    //    {
                    //        PlayerAction(monsters, player, mon, ref DeathCount);
                    //        return;
                    //    }
                    //    var selectPortion = DataManager.Instance.PortionDatas[choice - 1];
                    //    if (choice <= DataManager.Instance.PortionDatas.Count && choice >= 1)
                    //    {
                    //        selectPortion.UsePotion();
                    //        break;
                    //    }
                    //    else if (choice == 4 && Isboss)
                    //    {
                    //        if (monsters[0].MonsterHealth < monsters[0].MonsterHealth * (15.0 / 100))
                    //        {
                    //            selectPortion.UsePotion();
                    //        }
                    //        else
                    //        {
                    //            Console.WriteLine("사용할 조건이 충족되지 않았습니다.");
                    //            PlayerAction(monsters, player, mon, ref DeathCount);
                    //            return;
                    //        }
                    //    }
                    //    Instance.TakeAction();
                    //    break;
                    //}

                }
                else
                {
                    UtilityManager.PrintErrorMessage();
                }

            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public void MonsterAttack(List<Monster> monsters, Player player, Monster mon, ref int DeathCount)
            {
                Random ran = new Random();
                foreach (Monster mons in monsters)
                {
                    mons.MonsterDIe(ref DeathCount);
                    if (mons.IsLive)
                    {

                        int Damage = mons.MonsterAttackToPlayer();
                        if (((player.Avd.EquipAvd + player.Avd.PlayerAvd) * 100) < ran.Next(0, 101))
                        {
                            if (Damage > (player.Stat.BaseDef + player.Stat.EquipDef))
                            {
                                player.Stat.Hp -= Damage - (player.Stat.BaseDef + player.Stat.EquipDef);
                                Console.WriteLine($"데미지 {Damage} -플레이어 방어력{player.Stat.BaseDef + player.Stat.EquipDef} =" +
                                    $"{Damage - (player.Stat.BaseDef + player.Stat.EquipDef)} 피해를 입어 {player.Stat.Hp}가 되었습니다");
                            }
                            else
                            {
                                player.Stat.Hp -= 1;
                                Console.WriteLine("방어력이 높아서 1만 닳음");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"회피 성공");
                        }
                        if (player.Stat.Hp <= 0)
                        {
                            Instance.MoveNextAction(ActionType.Village);
                        }
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }



    }
}
