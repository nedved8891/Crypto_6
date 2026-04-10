using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Views.General;

namespace Views.Menu
{
    public class OnboardingPanel : PanelView
    {
        [SerializeField] private List<HintView> _hintViews;
        [SerializeField] private float _delayBetweenHints;
        [SerializeField] private SpriteRenderer _continentSpriteRenderer;
        [SerializeField] private Material _selectedMaterial;

        private int _hintIndex;
        private float _delayTimer;
        private bool _canContinue;
        private Material _material;

        private void Awake()
        {
            _hintIndex = 0;
            _delayTimer = 0;
            _canContinue = true;
            _material = _continentSpriteRenderer.material;
        }

        private void Start()
        {
            StartHint();
        }

        public void Update()
        {
            if (_canContinue && _hintViews[_hintIndex].IsTypingCompleted)
            {
                _delayTimer += Time.deltaTime;

                if (_delayTimer >= _delayBetweenHints)
                {
                    _hintViews[_hintIndex].gameObject.SetActive(false);

                    _hintIndex++;
                    _delayTimer = 0f;

                    if (_hintIndex == _hintViews.Count)
                    {
                        _canContinue = false;

                        _continentSpriteRenderer.material = _material;
                        _continentSpriteRenderer.sortingLayerName = "Default";
                        
                        base.PressBtnAction.Invoke(0);
                        
                        return;
                    }

                    StartCoroutine(DelayStartHint());
                }
            }
        }

        private void StartHint()
        {
            switch (_hintIndex)
            {
                case 0:
                    _continentSpriteRenderer.sortingLayerName = "Overlay";
                    break;
                case 1:
                    _continentSpriteRenderer.material = _selectedMaterial;
                    break;
            }
            
            _hintViews[_hintIndex].gameObject.SetActive(true);
            _hintViews[_hintIndex].SetHintText();
        }

        private IEnumerator DelayStartHint()
        {
            yield return new WaitForSeconds(1);
            
            StartHint();
        }
    }
}