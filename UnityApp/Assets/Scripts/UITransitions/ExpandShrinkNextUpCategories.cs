using UnityEngine;
using UnityEngine.UI;

public class ExpandShrinkNextUpCategories : MonoBehaviour
{
    public bool expanded = false;
    public int shrinkedHeight = 50;
    public int expandedHeight = 191;

    void OnEnable()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, expanded ? expandedHeight : shrinkedHeight);
        transform.GetChild(0).gameObject.SetActive(expanded);
        transform.GetChild(1).gameObject.SetActive(!expanded);
    }

    public void Expand()
    {
        expanded = true;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, expanded ? expandedHeight : shrinkedHeight);
        transform.GetChild(0).gameObject.SetActive(expanded);
        transform.GetChild(1).gameObject.SetActive(!expanded);
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.transform.parent.GetComponent<RectTransform>());

    }

    public void Shrink()
    {
        expanded = false;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, expanded ? expandedHeight : shrinkedHeight);
        transform.GetChild(0).gameObject.SetActive(expanded);
        transform.GetChild(1).gameObject.SetActive(!expanded);
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.transform.parent.GetComponent<RectTransform>());
    }
}