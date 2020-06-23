using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Node : MonoBehaviour
{
    public string nodeName;
    public string anim;
    public string auxScript;
    public List<GameObject> transit = new List<GameObject>();


    public GameObject nextState(string name)
    {
        GameObject n = null;
        bool nAssigned = false;
        foreach(GameObject no in transit)
        {
            if (no.gameObject.GetComponent<Node>().nodeName == name)
            {
                n = no;
                nAssigned = true;
            }
        }
        if (!nAssigned)
        {
            Debug.LogWarning("Invalid State");
        }
            
        return n;
    }
}
