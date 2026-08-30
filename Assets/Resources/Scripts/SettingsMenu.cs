using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject defaultPanel;
        [SerializeField] private GameObject[] panels;

        private void Awake()
        {
            if (defaultPanel == null)
            {
                Debug.LogWarning("No default panel assigned!");
                return;
            }

            ShowPanel(defaultPanel);
        }

        public void ShowPanel(GameObject targetPanel)
        {
            foreach (var panel in panels)
            {
                panel.SetActive(panel == targetPanel);
            }
        }
    }
}