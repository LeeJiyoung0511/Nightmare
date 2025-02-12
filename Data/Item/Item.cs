using System.ComponentModel;

namespace Nightmare
{
    public enum ItemType
    {
        None = 0,
        [Description("무기")]
        Weapon = 1,
        [Description("방어구")]
        Armor = 2,
        [Description("액세서리")]
        Accessory = 3,
        [Description("하트조각")]
        HeartPiece = 4,
        [Description("체력 회복 포션")]
        HPPotion = 5,
        [Description("마나 회복 포션")]
        MPPotion = 6,
        [Description("스페셜 아이템")]
        Special = 7,
    }
    public class Item
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ItemType Type { get; set; }
        public int PotionCount { get; set; }
        public int PotionMaxCount { get; set; }

        //Hp, Mp, Atk, Def, Avd, Crt를 하나의 변수로 묶어 아이템타입이 ~면 ~증가. 
        public float Value { get; set; }
        public string? Desc { get; set; }
        public int Cost { get; set; }
        public bool IsPurchase { get; set; } = false;
        public bool IsEquip { get; set; } = false;

        public bool IsSold { get; set; } = false;

        public Action OnEquipEvent = delegate { };

        //무기인지 방어구인지에 따라 공격력이나 방어력을 출력
        public string GetTypeString()
        {
            string str = "";
            switch (Type)
            {
                case ItemType.Weapon:
                    str = $"공격력 +{Value}";
                    break;
                case ItemType.Armor:
                    str = $"방어력 +{Value}";
                    break;
                case ItemType.Accessory:
                    str = $"치명타율 +{Value} | 회피율 +{Value}";
                    break;
                case ItemType.HeartPiece:
                    str = "";
                    break;
                case ItemType.HPPotion:
                    str = $"HP +{Value}";
                    break;
                case ItemType.MPPotion:
                    str = $"MP +{Value}";
                    break;
                case ItemType.Special:
                    str = $"HP +{Value} | MP +{Value}";
                    break;


            }
            return str;


        }
        //구매 여부에 따른 출력
        public string GetPriceString()
        {
            string str = IsPurchase ? "구매 완료" : $"{Cost}";
            return str;
        }
        //상점아이템 정보 출력 양식
        public string ShowShopItem()
        {
            string str = $"{Name} | {GetTypeString()} | {Desc} | {GetPriceString()}";
            return str;
        }
        //아이템판매 시 정보 출력 양식
        public string ShowSellItem()
        {
            string str = $"{Name} | {GetTypeString()} | {Desc} ";
            str += IsSold ? " | 판매완료" : $" | {Cost * 0.85f} G";
            return str;
        }
        public string SelectItem()
        {
            string str = IsEquip ? "[E]" : "";
            str += $"{Name} | {GetTypeString()} | {Desc}";
            return str;
        }

        public virtual String ToShow()
        {
            String s = $"{Name}|{GetTypeString()}|{Desc}|{Type}";
            return s;
        }

        public void BossKill(List<Monster> monsters, ref int Death)
        {
            monsters[0].MonsterHealth -= 999;
            Death++;

        }

        public virtual void UseItem(Item item)
        {
            // override를 통해 구현 예정
        }

        // 장착할 수 없는 특정 아이템 ID 목록
        private readonly HashSet<long> NonEquippableItemIds = new()
        {
              13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25  // 장착 불가능한 목록. 13-17(하트조각) 18(쿠키) 19-23(스페셜 드랍아이템) 24(음료) 25(사랑의 정수)
        };

        public void Equip(int number)
        {

            // 선택한 아이템이 존재하는지 확인
            if (number < 0 || number >= DataManager.Instance.HaveItems.Count)
            {
                Console.WriteLine("잘못된 입력입니다.");
                return;
            }

            Item selectItem = DataManager.Instance.HaveItems[number];

            if (selectItem == null)
            {
                Console.WriteLine("잘못된 입력입니다.");
                return;
            }
            //소모성 아이템 및 하트조각은 장착 불가
            if (NonEquippableItemIds.Contains(selectItem.Id))
            {
                Console.WriteLine($"{selectItem.Name}은(는) 장착할 수 없습니다.");
                return;
            }
            // 선택한 아이템이 이미 장착된 상태라면 해제

            if (selectItem.IsEquip)
            {
                UnEquip(selectItem);
                return;
            }

            // 같은 타입의 장비가 이미 장착된 경우, 기존 아이템을 해제
            Item equippedItem = DataManager.Instance.EquippedItems.FirstOrDefault(item => item.Type == selectItem.Type);

            if (equippedItem != null)
            {
                UnEquip(equippedItem);
            }

            // 새 아이템 장착
            selectItem.IsEquip = true;
            DataManager.Instance.EquippedItems.Add(selectItem);

            switch (selectItem.Type)
            {
                case ItemType.Weapon:
                    GameManager.Instance.Player.Stat.AddEquipAtk(selectItem.Value);
                    break;
                case ItemType.Armor:
                    GameManager.Instance.Player.Stat.AddEquipDef(selectItem.Value);
                    break;
                case ItemType.Accessory:
                    GameManager.Instance.Player.Avd.EquipAvd += selectItem.Value;
                    GameManager.Instance.Player.Crt.EquipCrt += selectItem.Value;
                    break;
            }
            OnEquipEvent();
        }

        public void UnEquip(Item item)
        {
            if (item == null || !item.IsEquip)
            {
                return;
            }

            if (NonEquippableItemIds.Contains(item.Id))
            {
                Console.WriteLine($"{item.Name}은(는) 해제할 수 없습니다.");
                return;
            }

            item.IsEquip = false;
            DataManager.Instance.EquippedItems.Remove(item);

            switch (item.Type)
            {
                case ItemType.Weapon:
                    GameManager.Instance.Player.Stat.SubtEquipAtk(item.Value);
                    break;
                case ItemType.Armor:
                    GameManager.Instance.Player.Stat.SubtEquipDef(item.Value);
                    break;
                case ItemType.Accessory:
                    GameManager.Instance.Player.Avd.EquipAvd -= item.Value;
                    GameManager.Instance.Player.Crt.EquipCrt -= item.Value;
                    break;
            }

            OnEquipEvent();
        }
    }
}
