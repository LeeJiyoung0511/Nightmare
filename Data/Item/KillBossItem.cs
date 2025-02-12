namespace Nightmare
{
    public class KillBossItem : Potion
    {
        public long KillBossId { get; set; }
        public Item Data => DataManager.Instance.ItemDatas[KillBossId];

        //보상이 스페셜 드랍아이템일때 이함수 사용
        //public void PickUpItem(Item item)
        //{
        //    DataManager.Instance.HaveItems.Add(item);
        //    DataManager.Instance.ConsumableItems.Add(item);
        //}

        //스페셜 드랍아이템 사용 구현
        public override void UsePotion()
        {
            //Console.WriteLine("사용 조건 구현 시 변경부탁드립니다. ");
            //if(사용조건) {}
            //DataManager.Instance.HaveItems.Remove(this);
            //DataManager.Instance.ConsumableItems.Remove(this);
        }

        public override void UseItem(Item item)
        {
            if (item.Type == ItemType.HPPotion || item.Type == ItemType.MPPotion || item.Type == ItemType.Special)
            {

            }
            else if (item.Type == ItemType.Accessory)
            {
           
            }
        }


        //public override string ToShow()
        //{
        //    string str = $"{base.ToShow} | (효과가 들어갈듯)";
        //    return str;
        //}
    }
}
