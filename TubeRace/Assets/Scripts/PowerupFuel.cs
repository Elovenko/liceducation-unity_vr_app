using UnityEngine;

namespace Assets.Scripts.Race
{
    public class PowerupFuel : Powerup
    {
        [Range(0.0f, 100.0f)]
        [SerializeField]
        private float m_FuelAmount;

        public override void OnPickedByBike(Bike bike)
        {
            bike.AddFuel(m_FuelAmount);
            Debug.Log($"{nameof(PowerupFuel)} pickup by {bike.name}.");
        }
    }
}
