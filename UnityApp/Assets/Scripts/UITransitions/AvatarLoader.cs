using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AvatarLoader : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().sprite = Sprite.Create(StoreAndCommunication.Instance.profilePicture, new Rect(0, 0, StoreAndCommunication.Instance.profilePicture.width, StoreAndCommunication.Instance.profilePicture.height), new Vector2(0.5f, 0.5f));
    }
}