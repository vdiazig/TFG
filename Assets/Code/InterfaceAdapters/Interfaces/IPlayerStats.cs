namespace InterfaceAdapters.Interfaces
{
    public interface IPlayerStats
    {
        void TakeDamage(float amount);
        void Heal(float amount);
        void UseEnergy(float amount);
        void RegainEnergy(float amount);
        void DecreaseOtherValue(float amount);
        void IncreaseOtherValue(float amount);
    }
}