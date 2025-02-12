using System.Text;

namespace Nightmare
{
    public partial class GameManager
    {
        public class Skill
        {

            public string SkillName { get; set; }
            public float SkillDamage { get; set; }
            public float SkillTarget { get; set; }
            public float SkillMp { get; set; }
            public int SkillCoolTime { get; set; }
            public int CurrentCoolTime { get; set; }
            public int SkillInTime { get; set; }
            public int WhosSkill { get; set; }

            public Skill()
            {

            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("스킬명: ").Append(SkillName).Append("\n").Append($"소요마나: {SkillMp}\n").Append($"쿨타임: {SkillCoolTime}턴");
                return sb.ToString();
            }

            public Skill(string n, float damage, int t, int m, int cooltime, int skillInTime, int whos)
            {
                SkillName = n;
                SkillDamage = damage;
                SkillTarget = t;
                SkillMp = m;
                SkillCoolTime = cooltime;
                CurrentCoolTime = cooltime;
                SkillInTime = skillInTime;
                WhosSkill = whos;
            }

            public virtual void SkillUse(Player player, List<Monster> monster, ref int Death)
            {

            }


            public void SkillSet(Player player)
            {
                switch ((int)player.Job)
                {
                    case 1:
                        Skill dwarfSkill1 = new AttackSkill("광부의 곡괭이질", (player.Stat.BaseAtk + player.Stat.EquipAtk) * 2, 1, 10, 3, 0, 1,1);
                        Skill dwarfSkill2 = new StatSkill("광물의 공명", 5, 1, 10, 3, "버프", "치명타율", 3, 1);
                        player.Playerskill.Add(dwarfSkill1);
                        player.Playerskill.Add(dwarfSkill2);
                        break;
                    case 2:
                        Skill newSisterSkill1 = new AttackSkill("서투른 빗자루질", (player.Stat.BaseAtk + player.Stat.EquipAtk) / 2, 2, 10, 3, 0, 1,1);
                        Skill newSisterSkill2 = new StatSkill("저주받은 입담", 0.5f, 1, 10, 3, "디버프", "공격력", 3, 1);
                        player.Playerskill.Add(newSisterSkill1);
                        player.Playerskill.Add(newSisterSkill2);
                        break;
                    case 3:
                        Skill saisonSkill1 = new AttackSkill("성의 톱니바퀴", (player.Stat.BaseAtk + player.Stat.EquipAtk) / 1.5f, 2, 10, 3, 0, 1,1);
                        Skill saisonSkill2 = new StatSkill("오늘의 일정", 5, 1, 10, 3, "버프", "회피율", 3, 1);
                        player.Playerskill.Add(saisonSkill1);
                        player.Playerskill.Add(saisonSkill2);
                        break;
                    case 4:
                        Skill OctopusWitch1 = new AttackSkill("여덟 문어 다리", (player.Stat.BaseAtk + player.Stat.EquipAtk) * 1.5f, 2, 10, 3, 0, 1,1);
                        Skill OctopusWitch2 = new StatSkill("윤슬", 30, 1, 10, 3, "버프", "체력", 0, 1);
                        Skill OctopusWitch3 = new StatSkill("심해의 냉기", 0.5f, 1, 10, 3, "디버프", "방어력", 3, 1);
                        player.Playerskill.Add(OctopusWitch1);
                        player.Playerskill.Add(OctopusWitch2);
                        player.Playerskill.Add(OctopusWitch3);
                        break;
                    case 5:
                        Skill WildAnimal1 = new AttackSkill("날카로운 발톱", (player.Stat.BaseAtk + player.Stat.EquipAtk) * 1.5f, 1, 10, 3, 0, 1,1);
                        Skill WildAnimal2 = new AttackSkill("야수의 포효", (player.Stat.BaseAtk + player.Stat.EquipAtk), 2, 10, 3, 0, 1,1);
                        player.Playerskill.Add(WildAnimal1);
                        player.Playerskill.Add(WildAnimal2);
                        break;
                }
            }
            public void BossSkillSet(Player player, Boss boss)
            {
                switch ((int)player.Job)
                {

                    //공격 -> 이름/데미지/타겟/소모마나/쿨타임/지속시간/누가하냐(보스2)/얼마나 자주 때리냐
                    //스텟 -> 이름/데미지/타겟/소모마나/쿨타임/어떤거/어디/지속 시간/보스냐 플레이어냐
                    case 1:
                        Skill Snowwhite1 = new AttackSkill("숲의 공주님", (float)((boss.MonsterAttack)*(20.0/100)), 1, 10, 3, 0, 2,2);
                        Skill Snowwhite2 = new StatSkill("새들의 합창", (boss.MonsterHealth)*(15/100), 0, 0, 3, "버프", "체력", 0, 2);
                        Skill Snowwhite3 = new StatSkill("자연의 원망", 5, 1, 0, 3, "디버프", "회피율", 3, 2);
                        boss.BossSkill.Add(Snowwhite1);
                        boss.BossSkill.Add(Snowwhite2);
                        boss.BossSkill.Add(Snowwhite3);
                        break;
                    case 2:
                        Skill Cinderella1 = new AttackSkill("재투성이", (float)((boss.MonsterAttack) * (30.0 / 100)), 1,0,3,0,2,1);
                        Skill Cinderella2 = new StatSkill("신분 상승", boss.MonsterAttack, 0, 0, 3, "버프", "체력", 3, 2);
                        Skill Cinderella3 = new StatSkill("대모의 응원", (float)((player.Stat.BaseDef+player.Stat.EquipDef) * (20.0/100)), 0, 0, 3, "디버프", "방어력", 3, 2);
                        boss.BossSkill.Add(Cinderella1);
                        boss.BossSkill.Add(Cinderella2);
                        boss.BossSkill.Add(Cinderella3);
                        break;
                    case 3:
                        Skill aurora1 = new AttackSkill("오로라", (float)((boss.MonsterAttack) * (50.0 / 100)), 1, 0, 3, 0, 2,1);
                        Skill aurora2 = new StatSkill("백 년 가약", 10, 0, 0, 3, "버프", "치명타율", 3, 2);
                        Skill aurora3 = new StatSkill("운명의 소용돌이", (float)((player.Stat.EquipAtk+player.Stat.BaseAtk)*(10.0/100)), 0, 0, 3, "디버프", "공격력", 3, 2);
                        boss.BossSkill.Add(aurora1);
                        boss.BossSkill.Add(aurora2);
                        boss.BossSkill.Add(aurora3);
                        break;
                    case 4:
                        Skill Arial1 = new AttackSkill("인간 갈망", (float)((boss.MonsterAttack) * (15.0 / 100)), 2, 10, 3, 0, 2,3);
                        Skill Arial2 = new StatSkill("현혹의 노래", boss.MonsterDefense, 1, 10, 3, "버프", "체력", 0, 2);
                        Skill Arial3 = new StatSkill("비극적인 결말", 10, 1, 10, 3, "디버프", "마력", 0, 2);
                        boss.BossSkill.Add(Arial1);
                        boss.BossSkill.Add(Arial2);
                        boss.BossSkill.Add(Arial3);
                        break;
                    case 5:
                        Skill Belle1 = new AttackSkill("괴물 혐오", (float)((boss.MonsterAttack) * (30.0 / 100)), 1, 10, 3, 0, 2,2);
                        Skill Belle2 = new StatSkill("마음의 식량", (float)((boss.MonsterHealth) * (20.0 / 100)), 0, 0,3,"버프", "체력", 0, 2);
                        Skill Belle3 = new StatSkill("사냥꾼의 정신", 7, 0, 0, 3, "디버프", "치명타율", 3, 2);
                        boss.BossSkill.Add(Belle1);
                        boss.BossSkill.Add(Belle2);
                        boss.BossSkill.Add(Belle3);
                        break;
                }
            }
        }
  
    }
}


