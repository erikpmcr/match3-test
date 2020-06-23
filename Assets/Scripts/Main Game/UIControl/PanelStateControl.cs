using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelStateControl : MonoBehaviour
{
    public NodeStateMachine NSM;
    public state_names workingState;

    // Start is called before the first frame update
    void Start()
    {
        if (NSM == null)
        {
            NSM = GameObject.Find("SelectControl").GetComponent<NodeStateMachine>();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(NSM.currentState!=null && workingState.ToString() != NSM.currentState.nodeName)
        {
            QuitPanel(this.gameObject.transform.GetChild(0).gameObject);
        }
        else
        {
            LoadPanel(this.gameObject.transform.GetChild(0).gameObject);
        }
    }

    public void LoadPanel(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void QuitPanel(GameObject obj)
    {
        obj.SetActive(false);
    }
}
