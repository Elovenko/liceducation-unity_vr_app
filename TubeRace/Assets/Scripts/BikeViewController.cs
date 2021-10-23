using UnityEngine;

namespace Assets.Scripts.Race
{
    public class BikeViewController : MonoBehaviour
    {
        public void SetupBikeView(BikeParameters parameters)
        {
            Debug.Log($"Bike parameters: {JsonUtility.ToJson(parameters, true)}.");
        }
    }
}
