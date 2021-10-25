using UnityEngine;

namespace Assets.Scripts.Race
{
    public class PowerupCoolant : Powerup
    {
        public override void OnPickedByBike(Bike bike)
        {
            bike.CoolAfterburner();
            Debug.Log($"{nameof(PowerupCoolant)} pickup by {bike.name}.");
        }
    }
}
