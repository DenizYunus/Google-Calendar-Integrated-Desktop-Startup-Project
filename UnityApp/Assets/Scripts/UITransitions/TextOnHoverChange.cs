using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextOnHoverChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI textMeshPro;
    public Color defaultColor = Color.black;
    public Color hoverColor = Color.red;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textMeshPro.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMeshPro.color = defaultColor;
    }
}