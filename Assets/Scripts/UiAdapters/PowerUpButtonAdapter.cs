using Systems;
using UnityComponents;
using UnityEngine;

namespace UiAdapters
{
    public class PowerUpButtonAdapter : MonoBehaviour
    {
        [SerializeField] private PowerUpView _powerUpView;

        private UiClickEventSystem _uiSystem;

        public void Init(UiClickEventSystem uiSystem)
        {
            _uiSystem = uiSystem;
            _powerUpView.PowerUpButton.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _uiSystem.OnPowerUpClicked(_powerUpView);
        }
    }
}