using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    //public Image countdownImage;
    public Slider bar;
    public int level = 1;
    public int target = 100;
    public bool start;
    public Text t_target;
    public string s_target;
    public Text t_level;
    public string s_level;
    public float duration = 60f;
    public float totalTime = 0;
    [SerializeField]
    NodeStateMachine NSM;
    public bool countdown = false;
    [SerializeField]
    AudioClip music;
    [SerializeField]
    bool MusicOn = false;

    // Start is called before the first frame update
    void Start()
    {
        if (NSM == null)
        {
            NSM = GameObject.Find("SelectControl").GetComponent<NodeStateMachine>();
        }

        s_target = t_target.text;
        s_level = t_level.text;
        
    }

    // Update is called once per frame
    void Update()
    {
        t_target.text = s_target + target;
        t_level.text = s_level + level;
        if(GetComponent<Score>().score > target)
        {
            level++;
            target = target  + level * 100;
            totalTime = 0;
        }
        if (NSM.currentState.nodeName != null)
        {
            if (NSM.currentState.nodeName == state_names.Main.ToString() && !MusicOn)
            {
                MusicOn = true;
                MusicManager.inst.PlayAudio(music);
                MusicManager.inst.sounds.loop = true;
            }

            if (NSM.currentState.nodeName == state_names.Start.ToString() && !countdown)
            {
                level = 1;
                target = 100;
                start = true;
                StartCoroutine(Countdown());
            }

            if (NSM.currentState.nodeName == state_names.PlayerTurn.ToString() && !countdown && totalTime >= duration)
            {
                AtemptStateChange(state_names.TimeOut.ToString());
            }

            if (NSM.currentState.nodeName == state_names.CheckPossible.ToString() && !countdown && totalTime >= duration)
            {
                AtemptStateChange(state_names.TimeOut.ToString());
            }

            if (NSM.currentState.nodeName == state_names.TimeOut.ToString())
            {
                totalTime = 0;

                AtemptStateChange(state_names.Result.ToString());
            }
        }
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

        Debug.Log(check ? "State changed to " + state : "State Unchanged");
    }

    private IEnumerator Countdown()
    {
        
        countdown = true;


        while (totalTime <= duration)
        {
            totalTime += Time.deltaTime;
            bar.value = totalTime / duration;
            yield return null;
        }
        countdown = false;
    }
}
