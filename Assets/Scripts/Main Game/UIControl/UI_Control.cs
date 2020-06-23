using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Control : MonoBehaviour
{

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
        
    }

    public void AtemptStateChange(string state)
    {
        //string s_state = state.ToString();
        GameObject g_state = null;
        bool check;
        if (check = (g_state = NSM.currentState.nextState(state)) != null)
        {
            NSM.g_currentState = g_state;
        }

        Debug.Log(check ? "State changed to " + state: "State Unchanged");


    }

    public void LoadPanel(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void QuitPanel(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void Quit()
    {
        //gameManager.gameObject.GetComponent<AutoSave>().save();
        StartCoroutine(QuitCountdown(1));
        Debug.Log("Game Exited");
    }

    private IEnumerator QuitCountdown(float duration)
    {

        float totalTime = 0;
        while (totalTime <= duration)
        {
            totalTime += Time.deltaTime;
            yield return null;
        }

        Application.Quit();

    }
}
