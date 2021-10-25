using System;
using UnityEngine;

namespace Assets.Scripts.Race
{
    [Serializable]
    public class BikeParameters
    {
        [Range(0.0f, 10.0f)]
        public float Mass;

        [Range(0.0f, 100.0f)]
        public float Thrust;

        public float AfterburnerThrust;
        public float AfterburnerMaxSpeed;

        public float AfterburnerCoolSpeed;
        public float AfterburnerHeatGeneration; // per second
        public float AfterburnerMaxHeat;

        [Range(0.0f, 100.0f)]
        public float Agility;
        public float MaxSpeed;
        public float MaxDriftSpeed;

        [Range(0.0f, 1.0f)]
        public float LinearDrag;

        [Range(0.0f, 1.0f)]
        public float RollDrag;

        [Range(0.0f, 1.0f)]
        public float CollisionBounceFactor;

        [Range(0.0f, 100.0f)]
        public float CollisionDamage;

        public GameObject EngineModel;
        public GameObject HullModel;
    }

    public class Bike : MonoBehaviour
    {
        public const string TAG = "Bike";

        [SerializeField]
        private BikeParameters m_BikeParametersInitial;

        [SerializeField]
        private BikeViewController m_VisualController;

        [SerializeField]
        private RaceTrack m_Track;

        public float Distance => m_Distance;
        public float Velocity => m_Velocity;
        public float RollAngle
        {
            get
            {
                float angle = m_RollAngle % 360.0f;
                return angle > 0 ? angle : (360 + angle);
            }
        }

        public float PreviousDistance => m_PreviousDistance;

        public int CurrentLap => (int)(m_Distance / m_Track.GetTrackLength()) + 1;

        public float NormolizedHeat => (m_BikeParametersInitial.AfterburnerMaxHeat > 0) ? m_AfterburnerHeat / m_BikeParametersInitial.AfterburnerMaxHeat : 0;

        public float Fuel => m_Fuel;

        /// <summary>
        /// Управление газом байка. Нормализованное. От -1 до +1.
        /// </summary>
        private float m_ForwardThrustAxis;

        /// <summary>
        /// Управление отклонение влево и вправо. Нормализованное. От -1 до +1.
        /// </summary>
        private float m_HorizontalThrustAxis;

        /// <summary>
        /// Вкл/Выкл доп ускорителя.
        /// </summary>
        private bool m_AfterburnerIsEnabled;

        private float m_Distance;
        private float m_Velocity;
        private float m_RollAngle;
        private float m_DriftAngleSpeed;
        private float m_AfterburnerHeat;
        private float m_PreviousDistance;
        // 0 -100

        private float m_Fuel;

        /// <summary>
        /// Установка значение педали газа.
        /// </summary>
        public void SetForwardThrustAxis(float val) => m_ForwardThrustAxis = val;

        public void SetHorizontalThrustAxis(float val) => m_HorizontalThrustAxis = val;

        public void SetAfterburnerEnable(bool val) => m_AfterburnerIsEnabled = val;

        public void CoolAfterburner() => m_AfterburnerHeat = 0;

        public void AddFuel(float amount)
        {
            m_Fuel += amount;

            m_Fuel = Mathf.Clamp(m_Fuel, 0.0f, 100.0f);
        }

        public void Slowdown(float speedAmount)
        {
            m_Velocity -= speedAmount;
            if (m_Velocity < 0)
            {
                m_Velocity = 0;
            }
        }

        public void Update()
        {
            updateAfterBurnerHeat();
            updateBikePhysics();
        }

        private void updateAfterBurnerHeat()
        {
            float dt = Time.deltaTime;

            // calc heat dissipation
            m_AfterburnerHeat -= m_BikeParametersInitial.AfterburnerCoolSpeed * dt;
            if (m_AfterburnerHeat < 0)
            {
                m_AfterburnerHeat = 0;
            }

            // Check max heat?
        }

        private bool consumerFuelForAfterburner(float amount)
        {
            if (m_Fuel <= amount)
            {
                return false;
            }

            m_Fuel -= amount;

            return true;
        }

        private void updateBikePhysics()
        {
            // S=vt; F=ma; V=V0+at
            float dt = Time.deltaTime;


            float force = m_ForwardThrustAxis * m_BikeParametersInitial.Thrust;
            float forceThrustMax = m_BikeParametersInitial.Thrust;
            float maxSpeed = m_BikeParametersInitial.MaxSpeed;

            if (m_AfterburnerIsEnabled && consumerFuelForAfterburner(1.0f * dt))
            {
                m_AfterburnerHeat += m_BikeParametersInitial.AfterburnerHeatGeneration * dt;

                force += m_BikeParametersInitial.AfterburnerThrust;
                forceThrustMax += m_BikeParametersInitial.AfterburnerThrust;
                maxSpeed += m_BikeParametersInitial.AfterburnerMaxSpeed;
            }

            // Drag.
            force += -m_Velocity * (forceThrustMax / maxSpeed);

            float dv = dt * force;

            m_Velocity += dv;

            float ds = m_Velocity * dt;

            // collision on move forward
            if (Physics.Raycast(transform.position, transform.forward, ds))
            {
                m_Velocity = -m_Velocity * m_BikeParametersInitial.CollisionBounceFactor;
                ds = m_Velocity * dt;
                m_AfterburnerHeat += m_BikeParametersInitial.CollisionDamage;
            }

            m_PreviousDistance = m_Distance;

            m_Distance += ds;

            if (m_Distance < 0)
            {
                m_Distance = 0;
            }

            float deltaAngleVelocity = m_HorizontalThrustAxis * m_BikeParametersInitial.Agility * dt;

            m_DriftAngleSpeed += deltaAngleVelocity;

            m_DriftAngleSpeed = Mathf.Clamp(m_DriftAngleSpeed, -m_BikeParametersInitial.MaxDriftSpeed, m_BikeParametersInitial.MaxDriftSpeed);

            float dr = m_DriftAngleSpeed * dt;

            // collision on move to rigth
            if (Physics.Raycast(transform.position, transform.right, dr))
            {
                m_DriftAngleSpeed = -m_DriftAngleSpeed * m_BikeParametersInitial.CollisionBounceFactor;
                dr = m_DriftAngleSpeed * dt;
                m_AfterburnerHeat += m_BikeParametersInitial.CollisionDamage;
            }

            m_RollAngle += dr;

            m_DriftAngleSpeed += -m_DriftAngleSpeed * m_BikeParametersInitial.RollDrag * dt;

            Vector3 bikePos = m_Track.GetPosition(m_Distance);
            Vector3 bikeDir = m_Track.GetDirection(m_Distance);

            Quaternion q = Quaternion.AngleAxis(m_RollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * m_Track.Radius);

            transform.position = bikePos - trackOffset;
            transform.rotation = Quaternion.LookRotation(bikeDir, trackOffset);
        }

        private void moveBike()
        {
            float currentForwardVelocity = m_ForwardThrustAxis * m_BikeParametersInitial.MaxSpeed;
            Vector3 forwardModeDelta = transform.forward * currentForwardVelocity * Time.deltaTime;
            transform.position += forwardModeDelta;
        }
    }
}
