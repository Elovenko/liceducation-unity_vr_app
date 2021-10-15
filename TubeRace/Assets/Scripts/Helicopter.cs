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
        //1 не нужно комментировать абсолютно каждую строку
        //документация кода - это хорошо, но, если название поля говорит само за себя (а к этому хорошо бы стремиться) то и пояснения ему ни к чему
        //помимо поясительной нагрузки summary банально жрет много места, что может усложнить восприятие кода программисту
        //например, если в классе "параметры вертолета" есть поле "масса", то очевидно, что масса является массой вертолета


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

        //2 хороший момент, что оба винта являются реализацией одной сущности


        /// <summary>
        /// Параметры заднего винта (расположенного на хвосте вертикально).
        /// </summary>
        public RotorParameters rearRotor;

        /// <summary>
        /// Параметры основного винта (расположенного горизонтально).
        /// </summary>
        public RotorParameters mainRotor;

        // 3 лучше модель вешать в дочерние обхъекты на сцене и в префабе хранить сам вертолет в целом

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
        // 4 в сущности винта есть проблемы, в частности они касаются того, что в ней идет указание ненужных параметров, тк они являются просто частью визуализации


        // 4.1 например это касается кол-ва лопастей. 3 или 1000 лопастей в пропеллере - почти всегда лишь визуализация, о которой скриптам знать и не нужно
        // исключениями могут быть какие-нибудь реалистичные симуляторы, в которых от кол-ва лопастей зависит скорость взлета, стабилизация полета и прочее

        /// <summary>
        /// Количество лопастей. Может изменяться от 4 до 8.
        /// </summary>
        [Range(4, 8)]
        public float numberOfBlades;

        /// <summary>
        /// Размер винта.
        /// </summary>
        [Range(1, 10)]
        public int size; //4.2, все размеры моделей растягиваются на сцене в компоненте transform, указывать его отдельным полем нет смысла

        /// <summary>
        /// Определяет положение винта, может быть горизонтальным или вертикальным.
        /// </summary>
        public bool isHorizontal; // 4.3 положение винта горизонтальное или вертикальное - опять же можно просто прямо на сцене повернуть винт, как угодно

        #region 5

        // 5 здесь я бы лучше убрал isForwardRotationDirection и дал бы возможность винту принимать отрицательную скорость
        // т.е maxRotationSpeed Range(-100, 100)

        /// <summary>
        /// Определяет направление вращения винта.
        /// </summary>
        public bool isForwardRotationDirection;

        /// <summary>
        /// Максимальная скорость вращения винта.
        /// </summary>
        [Range(0.0f, 100.0f)]
        public float maxRotationSpeed;

        #endregion
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
                
        //вот здесь особенно 1.2 вот здесь вот особенно не стоило писать пояснения, метод Start дефолтный и общеизвестный
        //это как объяснять человеку, что на старте происходит старт, дело излишнее

        /// <summary>
        /// The Start.
        /// </summary>
        internal void Start()
        {
            m_VisualController.SetupHelicopterView(m_HelicopterParameters);
        }

        //1.3 аналогично с 1.2

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
