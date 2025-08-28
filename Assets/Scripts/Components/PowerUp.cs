using Data;
using StaticData;

namespace Components
{
    public struct PowerUp
    {
        public PowerUpId Id;
        public BusinessTypeId BusinessTypeId;
        public bool Unlocked;
        public int Price;
        public float IncomeMultiplyerPercent;

        public int BusinessId;
    }
}