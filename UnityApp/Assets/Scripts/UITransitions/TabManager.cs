using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    public List<GameObject> tabs;

    public int selectedTab;

    void OnEnable ()
    {
        MoveTab(selectedTab);
    }

    public void MoveTab(int index)
    {
        selectedTab = index;
        tabs.ForEach(tab => { tab.SetActive(false); });
        tabs[selectedTab].SetActive(true);
    }
}