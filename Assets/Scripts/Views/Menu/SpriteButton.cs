using System;
using Types;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Views.Menu
{
    public class SpriteButton : MonoBehaviour
    {
        public event Action<SpriteButton> OnButtonClicked;

        [SerializeField] 
        private ContinentType _continentType;
        [SerializeField]
        private Vector3 pressedScale = new Vector3(0.9f, 0.9f, 1f);
        [SerializeField] 
        private Material _pressedMaterial;
        

        private SpriteRenderer spriteRenderer;
        private Vector3 originalScale;
        private bool isPressed = false;

        private Material _material;

        public ContinentType Type => _continentType;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer)
                _material = spriteRenderer.material;

            originalScale = transform.localScale;
        }

        private void Update()
        {
#if UNITY_EDITOR
            HandleMouseInput();
#else
        HandleTouchInput();
#endif
        }

        public void SetState(ContinentStateType type)
        {
            switch (type)
            {
                case ContinentStateType.Default:
                    ResetButtonState();
                    break;
                case ContinentStateType.Selected:
                    SetSelectedState();
                    break;
            }
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (hit.collider != null && hit.collider.gameObject == gameObject)
                        {
                            OnButtonPressed();
                        }
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        break;
                }
            }
        }
        
        private void HandleMouseInput()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePosition);

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetMouseButtonDown(0))
            {
                if (hit != null && hit.gameObject == gameObject)
                {
                    isPressed = true;
                    OnButtonPressed();
                }
            }
        }

        private void OnButtonPressed()
        {
            isPressed = true;
            
            OnButtonClicked?.Invoke(this);
        }

        private void SetSelectedState()
        {
            transform.localScale = pressedScale;

            if (spriteRenderer)
                spriteRenderer.material = _pressedMaterial;
        }

        private void ResetButtonState()
        {
            isPressed = false;

            transform.localScale = originalScale;

            if (spriteRenderer)
                spriteRenderer.material = _material;
        }
    }
}