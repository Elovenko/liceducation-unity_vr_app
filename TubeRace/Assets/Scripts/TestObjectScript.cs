using UnityEngine;

namespace Assets.Scripts.Race
{
    public class TestObjectScript : MonoBehaviour
    {
        [Range(1.0f, 10.0f)]
        [SerializeField] private float m_TestSpeed;

        [SerializeField] private bool m_IsForwardDirection;

        [SerializeField] private RaceTrackLinear m_TrackObject;

        private float trackLength;

        private void Start()
        {
            trackLength = m_TrackObject.GetTrackLength();
        }

        void Update()
        {
            // Меняем направление движения после каждой 3-й итерации.
            //if (Mathf.Ceil(m_TrackObject.m_TestDistance) % (3 * trackLength) == 0)
            //{
            //    m_IsForwardDirection = !m_IsForwardDirection;
            //}

            //m_TrackObject.m_TestDistance += (m_IsForwardDirection ? 1 : -1) * m_TestSpeed;
            //m_TrackObject.OnValidate();
        }
    }
}
