using UnityEngine;

namespace Assets.Scripts.Race
{
    public class PowerupSlowdown : Powerup
    {
        [Range(0.0f, 100.0f)]
        [SerializeField]
        private float m_SpeedAmount;

        public override void OnPickedByBike(Bike bike)
        {
            bike.Slowdown(m_SpeedAmount);
            Debug.Log($"{nameof(PowerupSlowdown)} pickup by {bike.name}.");
        }
    }
}