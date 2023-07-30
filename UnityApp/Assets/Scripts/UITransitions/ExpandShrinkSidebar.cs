using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class ExpandShrinkSidebar : MonoBehaviour
{
    public GameObject shrinkedBackground;
    public GameObject expandedBackground;

    public GameObject mainScreenGameObject;

    Sprite shrinkedBackgroundSprite;
    Sprite expandedBackgroundSprite;

    int expandedWidth = 327;
    int shrinkedWidth = 85;

    private void Start()
    {
        shrinkedBackgroundSprite = shrinkedBackground.GetComponent<Image>().sprite;
        expandedBackgroundSprite = expandedBackground.GetComponent<Image>().sprite;
    }

    public void ExpandSidebar()
    {
        shrinkedBackground.GetComponent<Image>().sprite = expandedBackgroundSprite;

        LeanTween.value(shrinkedBackground, shrinkedWidth, expandedWidth, 0.2f).setOnUpdate(
        (float val) =>
        {
            shrinkedBackground.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(val, 835);
        }
        ).setOnComplete(() =>
        {
            shrinkedBackground.transform.parent.gameObject.SetActive(false);
            expandedBackground.transform.parent.gameObject.SetActive(true);

            shrinkedBackground.GetComponent<Image>().sprite = shrinkedBackgroundSprite;
            shrinkedBackground.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(shrinkedWidth, 835);
        });
    }

    public void ShrinkSidebar()
    {
        expandedBackground.GetComponent<Image>().sprite = shrinkedBackgroundSprite;

        LeanTween.value(expandedBackground, expandedWidth, shrinkedWidth, 0.2f).setOnUpdate(
        (float val) =>
        {
            expandedBackground.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(val, 835);
        }
        ).setOnComplete(() =>
        {
            expandedBackground.transform.parent.gameObject.SetActive(false);
            shrinkedBackground.transform.parent.gameObject.SetActive(true);

            expandedBackground.GetComponent<Image>().sprite = expandedBackgroundSprite;
            expandedBackground.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(expandedWidth, 835);
        });
    }

    //public void WaitAndGoMainScreen()
    //{
    //    StartCoroutine(WaitAndGoMainScreenEnumerator());
    //}

    //IEnumerator WaitAndGoMainScreenEnumerator()
    //{
    //    yield return new WaitForSeconds(5);
    //    expandedBackground.transform.parent.gameObject.SetActive(false);
    //    mainScreenGameObject.SetActive(true);
    //}
}