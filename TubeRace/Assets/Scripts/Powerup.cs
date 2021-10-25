using UnityEngine;

namespace Assets.Scripts.Race
{
    public abstract class Powerup : MonoBehaviour
    {
        [SerializeField]
        private RaceTrack m_Track;
        [SerializeField]
        private float m_Distance;

        [SerializeField]
        private float m_RollAngle;

        public void Update()
        {
            updateBikes();
        }

        public void OnValidate()
        {
            setPowerupPosition();
        }

        public abstract void OnPickedByBike(Bike bike);

        private void updateBikes()
        {
            foreach (var bikeObject in GameObject.FindGameObjectsWithTag(Bike.TAG))
            {
                Bike bike = bikeObject.GetComponent<Bike>();
                float prev = bike.PreviousDistance;
                float curr = bike.Distance;
                if (prev < m_Distance && curr > m_Distance)
                {
                    // limit angles.

                    // bike picks powerup
                    OnPickedByBike(bike);
                }
            }
        }

        private void setPowerupPosition()
        {
            Vector3 obstaclePos = m_Track.GetPosition(m_Distance);
            Vector3 obstacleDir = m_Track.GetDirection(m_Distance);

            Quaternion q = Quaternion.AngleAxis(m_RollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * (0));


            transform.position = obstaclePos - trackOffset;
            transform.rotation = Quaternion.LookRotation(obstacleDir, trackOffset);
        }
    }
}
