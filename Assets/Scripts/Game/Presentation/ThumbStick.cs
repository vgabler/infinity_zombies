using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Game
{
    public class ThumbStick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        //Sensibilidade do controle
        [Range(100, 0), SerializeField] float sensibility = 20;

        //Quantia de movimento gráfico do Knob
        [SerializeField] float thumbMaxMovementMagnitude = 60;
        [SerializeField] float magnitudeMultiplier = 2;

        //Base do joystick
        [SerializeField] RectTransform stickBase;
        //Corpo do joystick, parte que se movimenta
        [SerializeField] RectTransform stickThumb;

        //Ancoramento do joystick na tela
        Vector2 anchorPosition { get { return stickBase.position; } }
        //Posição inicial do gráfico do joystick
        Vector2 startingPosition;

        public UnityEvent<Vector2> OnInputChanged;

        private void Start()
        {
            //Garantindo que o pivot point da base esteja no meio, sem comprometer o layout da cena
            var pivot = new Vector2(0.5f, 0.5f);
            var size = stickBase.rect.size;
            var deltaPivot = stickBase.pivot - pivot;
            var deltaPosition = new Vector2(deltaPivot.x * size.x, deltaPivot.y * size.y);
            stickBase.pivot = pivot;
            stickBase.anchoredPosition -= deltaPosition;

            //Salvando a posição inicial
            startingPosition = stickBase.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Calcula a direção do movimento do iput
            var diff = eventData.position - anchorPosition;
            var direction = diff.normalized;
            var magnitude = diff.magnitude * magnitudeMultiplier;

            //Atualiza posição do gráfico do Knob
            stickThumb.anchoredPosition = direction * Mathf.Clamp(magnitude, 0, thumbMaxMovementMagnitude);

            if (magnitude < sensibility)
                return;

            //Chamando os métodos para movimentar o jogador
            OnInputChanged.Invoke(direction);
        }

        /// <summary>
        /// Ancora o thumbstick onde começou a apertar e inicia
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            //Ancorando o joystick
            stickBase.position = eventData.position;
            stickThumb.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Reseta o thumbstick
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            stickThumb.anchoredPosition = Vector2.zero;

            //Voltando o joystick para posição inicial
            stickBase.anchoredPosition = startingPosition;
            OnInputChanged.Invoke(Vector2.zero);
        }
    }
}