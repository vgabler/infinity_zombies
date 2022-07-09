using UnityEngine;

namespace Utils
{
    //TODO criar mais casos, plataformas
    public class PlatformActivator : MonoBehaviour
    {
        public GameObject onMobile;
        public GameObject onDesktop;

#if UNITY_ANDROID || UNITY_IOS
        bool isMobile = true;
#else
        bool isMobile = false;
#endif

        private void Start()
        {
            if (onMobile != null)
                onMobile.SetActive(isMobile);

            if (onDesktop != null)
                onDesktop.SetActive(!isMobile);
        }
    }
}