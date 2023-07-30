using System.Linq;
using UnityEngine;

public class AllPagesController : MonoBehaviour
{
    public static AllPagesController Instance;
    bool signedIn = false;

    public GameObject[] tabs;

    public TabName currentTab;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(this);

        if (PlayerPrefs.HasKey("signedIn"))
        {
            if (PlayerPrefs.GetInt("signedIn") == 1)
            {
                currentTab = TabName.ExpandedMainScreen;

            }
            else currentTab = TabName.ExpandedLoggedOut;
        }
        else
        {
            currentTab = TabName.ExpandedLoggedOut;
        }

        tabs.ToList().ForEach(tab => { tab.SetActive(true); });
        tabs.ToList().ForEach(tab => { tab.SetActive(false); });
        tabs[(int)currentTab].SetActive(true);
    }

    public void MoveTab(TabName tabName)
    {
        tabs.ToList().ForEach(tab => { tab.SetActive(false); });
        tabs[(int)tabName].SetActive(true);
        currentTab = tabName;
    }

    public void MoveTab(int tabIndex)
    {
        tabs.ToList().ForEach(tab => { tab.SetActive(false); });
        tabs[tabIndex].SetActive(true);
        currentTab = (TabName)tabIndex;
    }

    public enum TabName
    {
        Shrinked,
        ExpandedLoggedOut,
        ExpandedMainScreen,
        ExpandedSliceIn,
        ExpandedSlicedIn,
        ExpandedAddEvent,
        ExpandedAddedEvent,
        ExpandedToDoList
    }
}