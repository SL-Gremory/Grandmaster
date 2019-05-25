using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUI : MonoBehaviour
{
    public GameObject actionPanel;








    public void ShowItems()
    {
        actionPanel.gameObject.SetActive(true);
    }

    public void HideItems()
    {
        actionPanel.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
