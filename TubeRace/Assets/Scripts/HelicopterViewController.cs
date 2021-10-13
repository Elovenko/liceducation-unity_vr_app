using UnityEngine;

namespace Assets.Scripts.Helicopter
{
    /// <summary>
    /// Defines the <see cref="HelicopterViewController" />.
    /// </summary>
    public class HelicopterViewController : MonoBehaviour
    {
       public void SetupHelicopterView(HelicopterParameters parameters)
        {
            Debug.Log($"Parameters: {parameters}.");
        }
    }
}
