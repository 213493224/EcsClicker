using Systems;
using UnityComponents;
using UnityEngine;

namespace UiAdapters
{
    public class BusinessCardButtonAdapter : MonoBehaviour
    {
        [SerializeField] private BusinessCardView _businessCardView;

        private UiClickEventSystem _uiSystem;

        public void Init(UiClickEventSystem uiSystem)
        {
            _uiSystem = uiSystem;
            _businessCardView.LevelUpButton.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _uiSystem.OnBusinessCardClicked(_businessCardView);
        }
    }
}

