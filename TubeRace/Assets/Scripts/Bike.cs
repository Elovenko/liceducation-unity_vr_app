using System;
using UnityEngine;

namespace Assets.Scripts.Race
{
    [Serializable]
    public class BikeParameters
    {
        [Range(0.0f, 10.0f)]
        public float mass;

        [Range(0.0f, 100.0f)]
        public float thrust;

        [Range(0.0f, 100.0f)]
        public float agility;
        public float maxSpeed;
        public float maxDriftSpeed;

        [Range(0.0f, 1.0f)]
        public float linearDrag;

        [Range(0.0f, 1.0f)]
        public float rollDrag;

        [Range(0.0f, 1.0f)]
        public float collisionBounceFactor;

        public bool afterburner;

        public GameObject engineModel;
        public GameObject hullModel;
    }

    public class Bike : MonoBehaviour
    {
        [SerializeField]
        private BikeParameters m_BikeParametersInitial;

        [SerializeField]
        private BikeViewController m_VisualController;

        [SerializeField]
        private RaceTrack m_Track;

        /// <summary>
        /// Управление газом байка. Нормализованное. От -1 до +1.
        /// </summary>
        private float m_ForwardThrustAxis;

        /// <summary>
        /// Управление отклонение влево и вправо. Нормализованное. От -1 до +1.
        /// </summary>
        private float m_HorizontalThrustAxis;

        private float m_Distance;
        private float m_Velocity;
        private float m_RollAngle;
        private float m_DriftAngleSpeed;

        /// <summary>
        /// Установка значение педали газа.
        /// </summary>
        public void SetForwardThrustAxis(float val) => m_ForwardThrustAxis = val;

        public void SetHorizontalThrustAxis(float val) => m_HorizontalThrustAxis = val;

        private void Update()
        {
            UpdateBikePhysics();
        }

        private void UpdateBikePhysics()
        {
            // S=vt; F=ma; V=V0+at
            float dt = Time.deltaTime;
            float dv = dt * m_ForwardThrustAxis * m_BikeParametersInitial.thrust;

            m_Velocity += dv;

            m_Velocity = Mathf.Clamp(m_Velocity, -m_BikeParametersInitial.maxSpeed, m_BikeParametersInitial.maxSpeed);

            float ds = m_Velocity * dt;

            // collision on move forward
            if (Physics.Raycast(transform.position, transform.forward, ds))
            {
                m_Velocity = -m_Velocity * m_BikeParametersInitial.collisionBounceFactor;
                ds = m_Velocity * dt;
            }

            m_Distance += ds;
            m_Velocity += -m_Velocity * m_BikeParametersInitial.linearDrag * dt;

            if (m_Distance < 0)
            {
                m_Distance = 0;
            }

            float deltaAngleVelocity = m_HorizontalThrustAxis * m_BikeParametersInitial.agility * dt;

            m_DriftAngleSpeed += deltaAngleVelocity;

            m_DriftAngleSpeed = Mathf.Clamp(m_DriftAngleSpeed, -m_BikeParametersInitial.maxDriftSpeed, m_BikeParametersInitial.maxDriftSpeed);

            float dr = m_DriftAngleSpeed * dt;

            // collision on move to rigth
            if (Physics.Raycast(transform.position, transform.right, dr))
            {
                m_DriftAngleSpeed = -m_DriftAngleSpeed * m_BikeParametersInitial.collisionBounceFactor;
                dr = m_DriftAngleSpeed * dt;
            }

            m_RollAngle += dr;

            m_DriftAngleSpeed += -m_DriftAngleSpeed * m_BikeParametersInitial.rollDrag * dt;

            Vector3 bikePos = m_Track.GetPosition(m_Distance);
            Vector3 bikeDir = m_Track.GetDirection(m_Distance);

            Quaternion q = Quaternion.AngleAxis(m_RollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * m_Track.Radius);

            transform.position = bikePos - trackOffset;
            transform.rotation = Quaternion.LookRotation(bikeDir, trackOffset);
        }

        private void MoveBike()
        {
            float currentForwardVelocity = m_ForwardThrustAxis * m_BikeParametersInitial.maxSpeed;
            Vector3 forwardModeDelta = transform.forward * currentForwardVelocity * Time.deltaTime;
            transform.position += forwardModeDelta;
        }
    }
}
