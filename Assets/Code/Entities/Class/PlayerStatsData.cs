using UnityEngine;

namespace Entities.Class{
    [System.Serializable]
    public class PlayerStatsData : IPlayerStats
    {
        public float maxHealth;
        public float currentHealth;
        public float maxEnergy;
        public float currentEnergy;
        public float maxOtherValue;
        public float currentOtherValue;

        public float MaxHealth => maxHealth;
        public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
        public float MaxEnergy => maxEnergy;
        public float CurrentEnergy { get => currentEnergy; set => currentEnergy = value; }
        public float MaxOtherValue => maxOtherValue;
        public float CurrentOtherValue { get => currentOtherValue; set => currentOtherValue = value; }

        // Ajusta el valor de salud
        public void Life(float amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        }

        // Ajusta el valor de energía
        public void Energy(float amount)
        {
            CurrentEnergy = Mathf.Clamp(CurrentEnergy + amount, 0, MaxEnergy);
        }

        // Ajusta el valor de la tercera barra
        public void OtherValue(float amount)
        {
            CurrentOtherValue = Mathf.Clamp(CurrentOtherValue + amount, 0, MaxOtherValue);
        }
    }
}