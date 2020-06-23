using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnWarning : MonoBehaviour
{
    [SerializeField]
    public NodeStateMachine NSM;
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

        if (NSM.currentState.nodeName == state_names.PlayerTurn.ToString())
        {
            this.gameObject.GetComponent<Text>().text = "Your Turn!";
        }
        else
        {
            this.gameObject.GetComponent<Text>().text = "Wait";
        }
        
    }
}
