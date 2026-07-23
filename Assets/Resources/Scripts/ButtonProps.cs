using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonProps : MonoBehaviour
{
    public enum ButtonAction
    {
        LoadScene,
        Exit,
        None
    };

    [Header("Action Settings")]
    [SerializeField] private ButtonAction action;
    [SerializeField] private string sceneName;

    private Color baseColor;
    private Image Image;
    private Color highlight = new(0.8549019607843137f, 0.8588235294117647f, 0.8666666666666667f, 0.5f);

    private void Start()
    {   
        Image = transform.GetComponent<Image>();
        baseColor = Image.color;
        Debug.Log(highlight);
    }

    private void OnMouseEnter()
    {
        Image.color = highlight;
    }

    private void OnMouseExit()
    {
        Image.color = baseColor;
    }

    private void OnMouseDown()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        switch(action)
        {
            case ButtonAction.LoadScene:
                if (!string.IsNullOrEmpty(sceneName))
                    SceneManager.LoadScene(sceneName);
                else
                    Debug.LogWarning($"{gameObject.name}: scene isn't set");
                break;

            case ButtonAction.Exit:
                Application.Quit();
                Debug.Log("Quit");
                break;

            case ButtonAction.None:
                break;
        }
    }

}
