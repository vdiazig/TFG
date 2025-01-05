public interface IPlayerStats
{
    float MaxHealth { get; }
    float CurrentHealth { get; set; }
    float MaxEnergy { get; }
    float CurrentEnergy { get; set; }
    float MaxOtherValue { get; }
    float CurrentOtherValue { get; set; }

    void Life(float amount);
    void Energy(float amount);
    void OtherValue(float amount);
}
