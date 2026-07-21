using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonProps : MonoBehaviour
{
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
        Debug.Log("uso");
        Image.color = highlight;
        Debug.Log("izaso");
    }
    private void OnMouseExit()
    {
        Image.color = baseColor;
    }


}
