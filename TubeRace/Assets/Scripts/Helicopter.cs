using System;
using UnityEngine;

namespace Assets.Scripts.Helicopter
{
    /// <summary>
    /// Параметры вертолёта.
    /// </summary>
    [Serializable]
    public class HelicopterParameters
    {
        /// <summary>
        /// Масса вертолёта.
        /// </summary>
        [Range(1.0f, 200.0f)]
        public float mass;

        /// <summary>
        /// Максимальная скорость передвижения.
        /// </summary>
        [Range(0.0f, 500.0f)]
        public float maxSpeed;

        /// <summary>
        /// Определяет использует ли вертолёт колоча или полозья.
        /// </summary>
        public bool useWheels;

        /// <summary>
        /// Параметры заднего винта (расположенного на хвосте вертикально).
        /// </summary>
        public RotorParameters rearRotor;

        /// <summary>
        /// Параметры основного винта (расположенного горизонтально).
        /// </summary>
        public RotorParameters mainRotor;

        /// <summary>
        /// Префаб с моделью корпуса.
        /// </summary>
        public GameObject hullModel;

        public override string ToString()
        {
            return $"Mass: {mass}, MaxSpeed: {maxSpeed}, useWheels: {(useWheels ? "yes" : "no")}";
        }
    }

    /// <summary>
    /// Параметры винта.
    /// </summary>
    [Serializable]
    public class RotorParameters
    {
        /// <summary>
        /// Количество лопастей. Может изменяться от 4 до 8.
        /// </summary>
        [Range(4, 8)]
        public float numberOfBlades;

        /// <summary>
        /// Максимальная скорость вращения винта.
        /// </summary>
        [Range(0.0f, 100.0f)]
        public float maxRotationSpeed;

        /// <summary>
        /// Размер винта.
        /// </summary>
        [Range(1, 10)]
        public int size;

        /// <summary>
        /// Определяет направление вращения винта.
        /// </summary>
        public bool isForwardRotationDirection;

        /// <summary>
        /// Определяет положение винта, может быть горизонтальным или вертикальным.
        /// </summary>
        public bool isHorizontal;
    }

    /// <summary>
    /// Controller. Entity.
    /// </summary>
    public class Helicopter : MonoBehaviour
    {
        /// <summary>
        /// Data model.
        /// </summary>
        [SerializeField]
        private HelicopterParameters m_HelicopterParameters;

        /// <summary>
        /// View.
        /// </summary>
        [SerializeField]
        private HelicopterViewController m_VisualController;

        private bool isEngineStarted = false;
        private float currentSpeed = 0;
        private Vector3 currentPosition = Vector3.zero;

        /// <summary>
        /// Запускаем или останавливаем двигатель.
        /// </summary>
        /// <param name="isEngineStarted">Текущее состояние двигателя.</param>
        private void startStopEngine(bool isEngineStarted)
        {
            Debug.Log($"Engine is {(isEngineStarted? "started" : "stopped")}.");
            isEngineStarted = !isEngineStarted;
            currentSpeed = isEngineStarted ? 10 : 0;
            Debug.Log($"Current speed: {currentSpeed}.");
            Debug.Log($"Current position: {currentPosition}.");
        }

        /// <summary>
        /// Изменяем текущую скорость вертолёта на переданное значение. 
        /// </summary>
        /// <param name="speedDelta">Значение на сколько должна изменится скорость.
        /// Если значени > 0, то вертолёт ускоряется, а если < 0, то вертолёт замедляется.</param>
        private void changeSpeed(float speedDelta)
        {
            if (currentSpeed > 0 || currentSpeed < m_HelicopterParameters.maxSpeed)
            {
                currentSpeed += speedDelta;
            }
            Debug.Log($"Current speed: {currentSpeed}.");
        }

        /// <summary>
        /// Изменяем текущее положение вертолёта на переданное смещение.
        /// </summary>
        /// <param name="offset">Смещение вертолёта влево или вправо.</param>
        private void moveRightOrLeft(Vector3 offset)
        {
            currentPosition += offset;
            Debug.Log($"Current position: {currentPosition}.");
        }

        #region Unity events

        /// <summary>
        /// The Start.
        /// </summary>
        internal void Start()
        {
            m_VisualController.SetupHelicopterView(m_HelicopterParameters);
        }

        /// <summary>
        /// The Update.
        /// </summary>
        internal void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startStopEngine(isEngineStarted);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                changeSpeed(10);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                changeSpeed(-10);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                moveRightOrLeft(Vector3.right);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveRightOrLeft(Vector3.left);
            }
        }

        #endregion
    }
}
