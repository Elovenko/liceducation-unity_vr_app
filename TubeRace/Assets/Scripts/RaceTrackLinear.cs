using UnityEngine;

namespace Assets.Scripts.Race
{
    public class RaceTrackLinear : RaceTrack
    {
        [Header("Linear track properties")]
        [SerializeField]
        private Transform m_Start;

        [SerializeField]
        private Transform m_End;

        public override Vector3 GetDirection(float distance)
        {
            distance = Mathf.Clamp(distance, 0, GetTrackLength());
            return (m_End.position - m_Start.position).normalized;
        }

        public override Vector3 GetPosition(float distance)
        {
            distance = Mathf.Repeat(distance, GetTrackLength());

            Vector3 direction = m_End.position - m_Start.position;
            direction = direction.normalized;

            return m_Start.position + direction * distance;
        }

        public override float GetTrackLength()
        {
            Vector3 direction = m_End.position - m_Start.position;
            return direction.magnitude;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(m_Start.position, m_End.position);
        }
    }
}