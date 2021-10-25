using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Race.UI
{
    public class BikeHudViewController : MonoBehaviour
    {
        [SerializeField]
        private Text m_LabelSpped;
        [SerializeField]
        private Text m_LabelDistance;
        [SerializeField]
        private Text m_LabelRollAngle;
        [SerializeField]
        private Text m_LabelLapNumber;
        [SerializeField]
        private Text m_LabelHeat;
        [SerializeField]
        private Text m_LabelFuel;

        [SerializeField]
        private Bike m_Bike;

        public void Update()
        {
            m_LabelSpped.text = $"Speed: {Mathf.Abs(Mathf.RoundToInt(m_Bike.Velocity))} m/s";
            m_LabelDistance.text = $"Distance: {Mathf.RoundToInt(m_Bike.Distance)} m";
            m_LabelRollAngle.text = $"Angle: {Mathf.RoundToInt(m_Bike.RollAngle)} deg";

            m_LabelLapNumber.text = $"Lap: {m_Bike.CurrentLap}";
            m_LabelHeat.text = $"Heat: {Mathf.RoundToInt(m_Bike.NormolizedHeat * 100.0f)}";
            m_LabelFuel.text = $"Fuel: {Mathf.RoundToInt(m_Bike.Fuel)}";
        }
    }
}