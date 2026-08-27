using UnityEngine;
using UnityEngine.UI;

namespace Resources.Scripts
{
    public class SettingsCard : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        private void Awake()
        {
            var menu = transform.GetComponentInParent<SettingsMenu>();
            transform.GetComponent<Button>().onClick.AddListener(
                () => menu.ShowPanel(panel));
        }
    }
}