using UnityEngine;

namespace Assets.Scripts.Race
{
    /// <summary>
    /// Базовый класс, который определяет трубу для гонок.
    /// </summary>
    public abstract class RaceTrack : MonoBehaviour
    {
        /// <summary>
        /// Радиус трубы.
        /// </summary>
        [SerializeField]
        [Header("Base track properties")]
        private float m_Radius;
        public float Radius => m_Radius;

        /// <summary>
        /// Возвращает длину трека.
        /// </summary>
        public abstract float GetTrackLength();

        /// <summary>
        /// Возвращает позицию в 3д кривой центр-линии трубы.
        /// </summary>
        /// <param name="distance">дистанция от начала трубы до её GetTrackLength</param>
        public abstract Vector3 GetPosition(float distance);

        /// <summary>
        /// Возвращает направление в 3д кривой центр-линии трубы. 
        /// Касательная к кривой в точке.
        /// </summary>
        /// <param name="distance"></param>
        public abstract Vector3 GetDirection(float distance);
    }
}
