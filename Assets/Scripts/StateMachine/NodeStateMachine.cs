using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using System;

[SerializeField]
public enum state_names {Main,  Start, CheckInst, ShuffleInst,
    PlayerTurn, CheckPossible, Shuffle, TimeOut, Result };

public class NodeStateMachine : MonoBehaviour
{
    public Node currentState;
    public GameObject g_currentState;
    public Animator anim;
    public List<GameObject> nodeDir = new List<GameObject>();
    void Start()
    {
        if (gameObject.GetComponent<Animator>() != null)
        {
            anim = gameObject.GetComponent<Animator>();
        }
        if (currentState != null)
        {
            nodeDir = currentState.transit;
            foreach (GameObject g in currentState.transit)
            {
                nodeDir.Add(g);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(anim !=null)
            anim.Play(currentState.anim);

        if (g_currentState.gameObject.GetComponent<Node>() != currentState)
        {
            currentState = g_currentState.gameObject.GetComponent<Node>();
            nodeDir.Clear();
            foreach (GameObject g in currentState.transit)
            {
                nodeDir.Add(g);
            }
        }

        if(currentState.auxScript!="" && this.gameObject.GetComponent(Type.GetType(currentState.auxScript)) == null)
        {
            this.gameObject.AddComponent(Type.GetType(currentState.auxScript));
        }
    }
}
