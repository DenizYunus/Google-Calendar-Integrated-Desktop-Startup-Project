using UnityEngine;

public class LoadingCircleFiller : MonoBehaviour
{
    GameObject fillerGameObject;
    public float progress;

    public bool animating = false;
    public bool expanding = false;

    void OnEnable()
    {
        fillerGameObject = transform.GetChild(0).gameObject;
    }

    public void StartAnimation()
    {
        animating = true;
    }

    private void Update()
    {
        if (animating)
        {
            if (expanding)
            {
                if (fillerGameObject.transform.localScale.x <= 0.9)
                    fillerGameObject.transform.localScale = Vector2.Lerp(fillerGameObject.transform.localScale, Vector2.one , Time.deltaTime);
                else
                    expanding = false;
            }
            else
            {
                if (fillerGameObject.transform.localScale.x >= 0.1)
                    fillerGameObject.transform.localScale = Vector2.Lerp(fillerGameObject.transform.localScale, Vector2.zero, Time.deltaTime);
                else
                    expanding = true;
            }
        }
    }

    public void StopAnimation()
    {
        animating = false;
        fillerGameObject.transform.localScale = new Vector2(0, 0);
    }
}
