using UnityEngine;
using UnityEngine.UI;

namespace UnityComponents
{
    public class UiRoot : MonoBehaviour
    {
        public Transform businessCardContainer;
        public GameObject hud;
        
        public ScrollRect ScrollRect;

        public void ResetScrollToTop()
        {
            if (ScrollRect != null)
            {
                ScrollRect.verticalNormalizedPosition = 1f;
            }
        }
    }
}
