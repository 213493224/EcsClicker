using UnityEngine;
using UnityEngine.UI;

namespace UnityComponents
{
    public class UiRoot : MonoBehaviour
    {
        [SerializeField] private Transform _businessCardContainer;
        [SerializeField] private GameObject _hud;
        [SerializeField] private ScrollRect _scrollRect;

        public Transform BusinessCardContainer => _businessCardContainer;
        public GameObject Hud => _hud;

        public void ResetScrollToTop()
        {
            if (_scrollRect != null)
            {
                _scrollRect.verticalNormalizedPosition = 1f;
            }
        }
    }
}
