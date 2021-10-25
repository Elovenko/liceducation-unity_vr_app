using UnityEngine;

namespace Assets.Scripts.Race
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField]
        private RaceTrack m_Track;

        [SerializeField]
        private float m_RollAngle;

        [SerializeField]
        private float m_Distance;

        [Range(0.0f, 20.0f)]
        [SerializeField]
        private float m_RadiusModifier = 1;

        [Range(-200.0f, 200.0f)]
        [SerializeField]
        private float m_RotationSpeed = 50.0f;

        public void Update()
        {
            setObstaclePosition(Time.deltaTime * m_RotationSpeed);
        }
        public void OnValidate()
        {
            setObstaclePosition(0);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 centerLinePos = m_Track.GetPosition(m_Distance);
            Gizmos.DrawSphere(centerLinePos, m_Track.Radius);
        }

        private void setObstaclePosition(float deltaRoll)
        {
            m_RollAngle += deltaRoll;
            Vector3 obstaclePos = m_Track.GetPosition(m_Distance);
            Vector3 obstacleDir = m_Track.GetDirection(m_Distance);

            Quaternion q = Quaternion.AngleAxis(m_RollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * (m_RadiusModifier * m_Track.Radius));


            transform.position = obstaclePos - trackOffset;
            transform.rotation = Quaternion.LookRotation(obstacleDir, trackOffset);
        }
    }
}
