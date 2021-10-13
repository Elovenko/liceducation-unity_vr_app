namespace Assets.Scripts.Race
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="Car" />.
    /// </summary>
    public class Car : MonoBehaviour
    {
        /// <summary>
        /// Defines the m_Mass.
        /// </summary>
        [SerializeField]
        [Range(0.0f, 100.0f)]
        private float m_Mass;

        /// <summary>
        /// Defines the m_Model.
        /// </summary>
        [SerializeField]
        private string m_Model;

        /// <summary>
        /// Defines the m_EnginePower.
        /// </summary>
        [SerializeField]
        private float m_EnginePower;

        /// <summary>
        /// Defines the m_NumSteeringWheels.
        /// </summary>
        [SerializeField]
        private int m_NumSteeringWheels;

        /// <summary>
        /// Defines the m_Color.
        /// </summary>
        [HideInInspector]
        [SerializeField]
        private Color m_Color;

        /// <summary>
        /// Defines the m_Postion.
        /// </summary>
        [SerializeField]
        private Vector3 m_Postion;

        /// <summary>
        /// Defines the m_Rotation.
        /// </summary>
        [SerializeField]
        private Quaternion m_Rotation;

        /// <summary>
        /// Defines the m_WheelA.
        /// </summary>
        [SerializeField]
        private Transform m_WheelA;

        /// <summary>
        /// Defines the m_WheelB.
        /// </summary>
        [SerializeField]
        private Transform m_WheelB;

        /// <summary>
        /// The Start.
        /// </summary>
        internal void Start()
        {
        }
    }
}
