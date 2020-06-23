
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicker : MonoBehaviour
{
    [SerializeField]
    GameObject g_pick, g_drop;
    [SerializeField]
    Gem_Type pickType;
    [SerializeField]
    Gem_Type dropType;
    [SerializeField]
    bool canDrop = false;

    [SerializeField]
    Color32 shade;

    [SerializeField]
    GridLoader GL;

    [SerializeField]
    AudioClip s_sel;

    [SerializeField]
    AudioClip s_swap;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if(hit.collider != null && 
                GetComponent<NodeStateMachine>().currentState.nodeName == state_names.PlayerTurn.ToString())
            {
                if (hit.collider.gameObject.CompareTag("Gem"))
                {
                    if(g_pick == null)
                    {
                        g_pick = hit.collider.gameObject;
                        pickType = g_pick.GetComponent<Gem>().type;
                        g_pick.GetComponent<SpriteRenderer>().color = shade;
                        SFXManager.voice1.PlayAudio(s_sel);
                        
                    }
                    if (g_pick != null && g_drop == null && g_pick != hit.collider.gameObject)
                    {
                        g_drop = hit.collider.gameObject;
                        dropType = g_drop.GetComponent<Gem>().type;
                    }

                }
            }
            //Debug.Log("Mouse Clicked");
        }
        if(g_pick != null && g_drop != null)
        {
            Vector3 testvec = g_pick.transform.position - g_drop.transform.position;
            if (Mathf.Abs(testvec.magnitude) < 1.3f && GL.MatchCheckForPosition(g_drop,g_pick))
            {
                canDrop = true;
                g_pick.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            }
            else
            {
                g_pick.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                g_pick = null;
                g_drop = null;

                
            }
        }

        if (canDrop &&
                GetComponent<NodeStateMachine>().currentState.nodeName == state_names.PlayerTurn.ToString())
        {
            if(g_pick.GetComponent<Gem>().parent != g_drop.GetComponent<Gem>().parent)
            {
                NewGemGenerator temp1 = g_pick.GetComponent<Gem>().parent;
                NewGemGenerator temp2 = g_drop.GetComponent<Gem>().parent;
                temp1.column.Remove(g_pick);
                temp2.column.Remove(g_drop);
                temp2.column.Add(g_drop);
                temp1.column.Add(g_pick);
                


            }
            g_pick.GetComponent<Gem>().type = dropType;
            g_drop.GetComponent<Gem>().type = pickType;

            g_pick = null;
            g_drop = null;


            canDrop = false;
            AtemptStateChange(state_names.CheckPossible.ToString());
            SFXManager.voice1.PlayAudio(s_swap);
        }
    }

    public void AtemptStateChange(string state)
    {
        NodeStateMachine NSM = GameObject.Find("SelectControl").GetComponent<NodeStateMachine>();
        //string s_state = state.ToString();
        GameObject g_state = null;
        bool check;
        if (check = (g_state = NSM.currentState.nextState(state)) != null)
        {
            NSM.g_currentState = g_state;
        }

        Debug.Log(check ? "State changed to " + state : "State Unchanged");


    }
}
