using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLoader : MonoBehaviour
{
    [SerializeField]
    int gridX=6;
    [SerializeField]
    int gridY=6;
    [SerializeField]
    float offX = 0.625f;
    [SerializeField]
    float offY = 0.625f;
    [SerializeField]
    float startX = 3.125f;
    [SerializeField]
    float startY = 3.125f;
    [SerializeField]
    ObjectPooler objectPooler;
    public string objTag =  "Gems";
    public string genTag = "GemGenerator";

    public bool scan = false;

    public bool start = false;

    public bool shuffle = false;

    public bool checkPossible = false;

    public bool refresh = true;
    [SerializeField]
    float t_refresh = 2f;

    public List<GameObject> gems = new List<GameObject>();

    public List<Gem> gemScript = new List<Gem>();

    //public List<Vector3> vecs = new List<Vector3>();

    public List<GameObject> gemGenerator = new List<GameObject>();

    public bool playerTurn = false;
    public bool checkScan = false;
    public bool checkNext = false;

    public NodeStateMachine NSM;

    [SerializeField]
    AudioClip s_clear;

    // Start is called before the first frame update
    void Start()
    {

        if (NSM == null)
        {
            NSM = GameObject.Find("SelectControl").GetComponent<NodeStateMachine>();
        }

        objectPooler = ObjectPooler.Instance;
        for (int k = 0; k < gridX; k++)
        {
            GameObject g = objectPooler.SpawnFromPool(genTag, new Vector3(-startX + k * offX, 4.4f, 0), Quaternion.identity);
            if (!gemGenerator.Contains(g))
            {
                gemGenerator.Add(g);
            }

        }
        for (int k = 0; k < gridY; k++)
        {
            for(int l=0; l<gridX; l++)
            {
                
                GameObject g = objectPooler.SpawnFromPool(objTag, new Vector3(-startX + l * offX, startY - k * offY, 0), Quaternion.identity);
                g.gameObject.GetComponent<Gem>().type = (Gem_Type)Random.Range(0, Gem_Type.GetNames(typeof(Gem_Type)).Length);
                if (!gems.Contains(g))
                {
                    gems.Add(g);
                }
                g.gameObject.GetComponent<Gem>().parent = gemGenerator[l].gameObject.GetComponent<NewGemGenerator>();
                if (!gemGenerator[l].gameObject.GetComponent<NewGemGenerator>().column.Contains(g))
                {
                    gemGenerator[l].gameObject.GetComponent<NewGemGenerator>().column.Add(g);
                }

                //vecs.Add(g.transform.position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (start)
        {
            GemSort();
            while (MatchScan())
            {
                MatchShuffle();
            }
            
        }

        if(gems.Count >= 36 && !playerTurn)
        {
            //MatchScan();
            GemSort();
            
                
            
            if (checkPossible = MatchScanPossible())
            {

            }
            else
            {
                shuffle = true;
            }

            playerTurn = true;
        }

        if(gems.Count >=36 && scan && RollSettle() && refresh)
        {
            GemSort();
            scan = MatchScan();
            if (scan )
            {
                GemSort();
                MatchScanClear();
            }
            refresh = false;
            StartCoroutine(Refresher(t_refresh));
        }
        */
        if (gems.Count > 0)
        {
            foreach (GameObject g in gems)
            {
                if (!g.activeSelf)
                {
                    gems.Remove(g);
                }
            }
        }


        if(NSM.currentState != null)
        {
            switch ((state_names)(System.Enum.Parse(typeof(state_names),NSM.currentState.nodeName)))
            {
                case state_names.Start:
                    GemSort();
                    AtemptStateChange(state_names.CheckInst.ToString());
                    break;

                case state_names.CheckInst:
                    GemSort();
                    //AtemptStateChange(state_names.CheckInst.ToString());
                    if (MatchScanPossible())
                    {
                        AtemptStateChange(state_names.ShuffleInst.ToString());
                    }
                    else
                    {
                        AtemptStateChange(state_names.PlayerTurn.ToString());
                    }
                    break;

                case state_names.ShuffleInst:
                    
                    GemSort();
                    MatchShuffle();
                    AtemptStateChange(state_names.PlayerTurn.ToString());
                   
                    break;


                case state_names.PlayerTurn:
                    GemSort();
                    if (gems.Count >= 36)
                    {
                        if (MatchScanPossible())
                        {
                            if (MatchScan())
                            {

                                AtemptStateChange(state_names.CheckPossible.ToString());
                            }
                        }
                        else
                        {
                            if (MatchScan())
                            {

                                AtemptStateChange(state_names.CheckPossible.ToString());
                            }
                            else
                            {
                                AtemptStateChange(state_names.Shuffle.ToString());
                            }
                        }
                    }
                    break;

                case state_names.CheckPossible:
                    GemSort();

                    if (gems.Count >= 36)
                    {
                        if (MatchScanPossible())
                        {
                            shuffle = false;
                        }
                        else
                        {
                            shuffle = true;
                        }

                        if (shuffle)
                        {
                            AtemptStateChange(state_names.Shuffle.ToString());
                        }
                        else
                        {
                            scan = MatchScan();
                            if (gems.Count >= 36 && scan && RollSettle() && refresh)
                            {
                                GemSort();
                                MatchScanClear();
                                
                            }
                            else
                            {
                                if (gems.Count >= 36 && !scan && RollSettle() && refresh)
                                    AtemptStateChange(state_names.PlayerTurn.ToString());
                            }

                            refresh = false;
                            StartCoroutine(Refresher(t_refresh));


                        }
                    }
                    break;

                case state_names.Shuffle:
                    GemSort();
                    ShuffleTypes();
                    MatchShuffle();
                    AtemptStateChange(state_names.PlayerTurn.ToString());
                    
                    break;

                    /*
                case state_names.CheckInst:
                    GemSort();
                    AtemptStateChange(state_names.CheckInst.ToString());
                    break;
                    */


            }
        }

    }

    bool RollSettle()
    {
        bool result= true;
        foreach(GameObject g in gemGenerator)
        {

            if (!g.GetComponent<NewGemGenerator>().rayTouch)
            {
                result = false;
            }
        }
        return result;
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

    private IEnumerator Refresher(float duration)
    {

        float totalTime = 0;
        while (totalTime <= duration)
        {
            totalTime += Time.deltaTime;
            yield return null;
        }
        refresh = true;
    }


    private IEnumerator CheckClear()
    {

        while (scan = MatchScanClear() || gems.Count < 36)
        {
            GemSort();
            yield return null;
        }
    }

    private void GemSort()
    {
        Vector3 tempVec1;

        Vector3 tempVec2;

        float tempf = 0;

        for (int k = 0; k < gems.Count-1; k++)
        {
            GameObject g_current;
            g_current = gems[k];
            int temp = 0;
            
            for (int l = k + 1; l < gems.Count; l++)
            {
                tempVec1 = g_current.transform.localPosition;

                tempVec2 = gems[l].transform.localPosition;

                tempf = Mathf.Abs(g_current.transform.localPosition.y - gems[l].transform.localPosition.y);
                if (g_current.transform.localPosition.y < gems[l].transform.localPosition.y && tempf >.4f)
                {
                    g_current = gems[l];
                    temp = l;

                }
                else if (tempf < .4f)
                {
                    if(g_current.transform.localPosition.x > gems[l].transform.localPosition.x)
                    { 
                    g_current = gems[l];
                    temp = l;
                    }
                }
            }

            if(g_current!= gems[k])
            {
                gems.Remove(g_current);

                //vecs.RemoveAt(temp);
                
                gems.Insert(k, g_current);

                //vecs.Insert(k, g_current.transform.position);
                
            }
        }

    }
    private bool MatchScan()
    {
        bool clear = false;
        int temp = 0;
        foreach (GameObject g in gems)
        {
            gemScript.Add(gems[temp].GetComponent<Gem>());
            temp++;
        }

        for (int k = 0; k < gemScript.Count; k++)
        {
            if (k - 1 >= 0 && k - 2 >= 0 && (k) % 6 != 0
                && k % gridX != 0 && k % gridX != 1)
            {
                if (gemScript[k].type == gemScript[k - 1].type && gemScript[k].type == gemScript[k - 2].type)
                {
                    clear = true;
                }
            }
            if (k - 1 >= 0 && k + 1 < gemScript.Count
                && k % gridX != gridX - 1 && k % gridX != 0)
            {
                if (gemScript[k].type == gemScript[k - 1].type && gemScript[k].type == gemScript[k + 1].type)
                {
                    clear = true;
                }
            }
            if (k + 1 < gemScript.Count && k + 2 < gemScript.Count
                && k % gridX != gridX - 1 && k % gridX != gridX - 2)
            {
                if (gemScript[k].type == gemScript[k + 1].type && gemScript[k].type == gemScript[k + 2].type)
                {
                    clear = true;
                }
            }
            
            if (k - 1 * gridX >= 0 && k - 2 * gridX >= 0)
            {
                if (gemScript[k].type == gemScript[k - 1 * gridX].type && gemScript[k].type == gemScript[k - 2 * gridX].type)
                {
                    clear = true;
                }
            }
            if (k - 1 * gridX >= 0 && k + 1 * gridX < gemScript.Count)
            {
                if (gemScript[k].type == gemScript[k - 1 * gridX].type && gemScript[k].type == gemScript[k + 1 * gridX].type)
                {
                    clear = true;
                }
            }
            if (k + 1 * gridX < gemScript.Count && k + 2 * gridX < gemScript.Count)
            {
                if (gemScript[k].type == gemScript[k + 1 * gridX].type && gemScript[k].type == gemScript[k + 2 * gridX].type)
                {
                    clear = true;
                }
            }

        }
        gemScript.Clear();

        return clear;
    }

    private bool MatchScanClear()
    {
        bool clear = false;
        int temp = 0;
        foreach(GameObject g in gems)
        {
            gemScript.Add(gems[temp].GetComponent<Gem>());
            temp++;
        }

        for (int k = 0; k < gemScript.Count; k++)
        {
            if(k - 1 >= 0 && k - 2 >= 0 && (k) % 6 != 0 
                && k % gridX != 0 && k % gridX != 1)
            {
                if (gemScript[k].type == gemScript[k - 1].type && gemScript[k].type == gemScript[k - 2].type)
                {
                    gemScript[k - 1].GetComponent<Gem>().despawnTag = true;
                    gemScript[k - 2].GetComponent<Gem>().despawnTag = true;
                    clear = true;
                }
            }
            if (k - 1 >= 0 && k + 1 < gemScript.Count
                && k % gridX != gridX - 1 && k % gridX != 0)
            {
                if (gemScript[k].type == gemScript[k - 1].type && gemScript[k].type == gemScript[k + 1].type)
                {
                    gemScript[k - 1].GetComponent<Gem>().despawnTag = true;
                    gemScript[k + 1].GetComponent<Gem>().despawnTag = true;
                    clear = true;
                }
            }
            if (k + 1 < gemScript.Count && k + 2 < gemScript.Count 
                && k % gridX != gridX - 1 && k % gridX != gridX - 2)
            {
                if (gemScript[k].type == gemScript[k + 1].type && gemScript[k].type == gemScript[k + 2].type)
                {
                    gemScript[k + 1].GetComponent<Gem>().despawnTag = true;
                    gemScript[k + 2].GetComponent<Gem>().despawnTag = true;
                    clear = true;
                }
            }

            if (k - 1 * gridX >= 0 && k - 2 * gridX >= 0)
            {
                if (gemScript[k].type == gemScript[k - 1 * gridX].type && gemScript[k].type == gemScript[k - 2 * gridX].type)
                {
                    gemScript[k - 1 * gridX].GetComponent<Gem>().despawnTag = true;
                    gemScript[k - 2 * gridX].GetComponent<Gem>().despawnTag = true;
                    clear = true;
                }
            }
            if (k - 1 * gridX >= 0 && k + 1 * gridX < gemScript.Count)
            {
                if (gemScript[k].type == gemScript[k - 1 * gridX].type && gemScript[k].type == gemScript[k + 1 * gridX].type)
                {
                    gemScript[k - 1 * gridX].GetComponent<Gem>().despawnTag = true;
                    gemScript[k + 1 * gridX].GetComponent<Gem>().despawnTag = true;
                    clear = true;
                }
            }
            if (k + 1 * gridX < gemScript.Count && k + 2 * gridX < gemScript.Count)
            {
                if (gemScript[k].type == gemScript[k + 1 * gridX].type && gemScript[k].type == gemScript[k + 2 * gridX].type)
                {
                    gemScript[k + 1 * gridX].GetComponent<Gem>().despawnTag = true;
                    gemScript[k + 2 * gridX].GetComponent<Gem>().despawnTag = true;
                    clear = true;
                }
            }

        }
        gemScript.Clear();
        if (clear)
        {
            SFXManager.voice2.PlayAudio(s_clear);
        }
        return clear;
    }

    private bool MatchScanPossible()
    {
        bool possible = false;
        int temp = 0;
        foreach (GameObject g in gems)
        {
            gemScript.Add(gems[temp].GetComponent<Gem>());
            temp++;
        }
        List<int> tempLI = new List<int>();
        for (int k = 0; k < gemScript.Count; k++)
        {
            if (k - 1 >= 0 && k - 2 >= 0 && (k) % 6 != 0
                && k % gridX != 0 && k % gridX != 1)
            {
                if (gemScript[k].type == gemScript[k - 1].type || gemScript[k].type == gemScript[k - 2].type)
                {
                    
                    if (gemScript[k].type == gemScript[k - 1].type)
                    {
                        tempLI = checkPossibleMovment(k - 2);
                        tempLI.Remove(k - 1);
                        foreach(int i in tempLI)
                        {
                            if (i >= 0 && i < gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                        
                    }
                    else
                    {
                        tempLI = checkPossibleMovment(k - 1);
                        tempLI.Remove(k - 2);
                        tempLI.Remove(k);
                        foreach (int i in tempLI)
                        {
                            if (i >= 0 && i < gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                    }
                }
            }
            if (k - 1 >= 0 && k + 1 < gemScript.Count
                && k % gridX != gridX - 1 && k % gridX != 0)
            {
                if (gemScript[k].type == gemScript[k - 1].type || gemScript[k].type == gemScript[k + 1].type)
                {
                    
                    if (gemScript[k].type == gemScript[k - 1].type)
                    {
                        tempLI = checkPossibleMovment(k + 1);
                        tempLI.Remove(k);
                        foreach (int i in tempLI)
                        {
                            if (i >= 0 && i < gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                    }
                    else
                    {
                        tempLI = checkPossibleMovment(k - 1);
                        tempLI.Remove(k);
                        foreach (int i in tempLI)
                        {
                            if (i >= 0 && i< gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                    }
                }
            }
            if (k + 1 < gemScript.Count && k + 2 < gemScript.Count
                && k % gridX != gridX - 1 && k % gridX != gridX - 2)
            {
                if (gemScript[k].type == gemScript[k + 1].type || gemScript[k].type == gemScript[k + 2].type)
                {
                    
                    if (gemScript[k].type == gemScript[k + 1].type)
                    {
                        tempLI = checkPossibleMovment(k + 2);
                        tempLI.Remove(k + 1);
                        foreach (int i in tempLI)
                        {
                            if (i >= 0 && i < gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                    }
                    else
                    {
                        tempLI = checkPossibleMovment(k + 1);
                        tempLI.Remove(k + 2);
                        tempLI.Remove(k);
                        foreach (int i in tempLI)
                        {
                            if (i >= 0 && i < gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                    }
                }
            }

            if (k - 1 * gridX >= 0 && k - 2 * gridX >= 0)
            {
                if (gemScript[k].type == gemScript[k - 1 * gridX].type || gemScript[k].type == gemScript[k - 2 * gridX].type)
                {
                   
                    if (gemScript[k].type == gemScript[k - 1 * gridX].type)
                    {
                        tempLI = checkPossibleMovment(k - 2 * gridX);
                        tempLI.Remove(k - 1 * gridX);
                        foreach (int i in tempLI)
                        {
                            if (i >= 0 && i < gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                    }
                    else
                    {
                        tempLI = checkPossibleMovment(k - 1 * gridX);
                        tempLI.Remove(k - 2 * gridX);
                        tempLI.Remove(k);
                        foreach (int i in tempLI)
                        {
                            if (i >= 0 && i < gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                    }
                }
            }
            if (k - 1 * gridX >= 0 && k + 1 * gridX < gemScript.Count)
            {
                if (gemScript[k].type == gemScript[k - 1 * gridX].type || gemScript[k].type == gemScript[k + 1 * gridX].type)
                {
                    
                    if (gemScript[k].type == gemScript[k - 1 * gridX].type)
                    {
                        tempLI = checkPossibleMovment(k + 1 * gridX);
                        tempLI.Remove(k);
                        foreach (int i in tempLI)
                        {
                            if (i >= 0 && i < gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                    }
                    else
                    {
                        tempLI = checkPossibleMovment(k - 1 * gridX);
                        tempLI.Remove(k);
                        foreach (int i in tempLI)
                        {
                            if (i >= 0 && i < gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                    }
                }
            }
            if (k + 1 * gridX < gemScript.Count && k + 2 * gridX < gemScript.Count)
            {
                if (gemScript[k].type == gemScript[k + 1 * gridX].type || gemScript[k].type == gemScript[k + 2 * gridX].type)
                {
                    
                    if (gemScript[k].type == gemScript[k + 1 * gridX].type)
                    {
                        tempLI = checkPossibleMovment(k + 2 * gridX);
                        tempLI.Remove(k + 1 * gridX);
                        foreach (int i in tempLI)
                        {
                            if (i >= 0 && i < gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                    }
                    else
                    {
                        tempLI = checkPossibleMovment(k + 1 * gridX);
                        tempLI.Remove(k + 2 * gridX);
                        tempLI.Remove(k);
                        foreach (int i in tempLI)
                        {
                            if (i >= 0 && i < gemScript.Count && gemScript[i].type == gemScript[k].type)
                            {
                                possible = true;
                            }
                        }
                    }
                }
            }
            if (possible)
            {
                k = gemScript.Count;
            }
        }
        gemScript.Clear();

        /*
        if (possible)
        {
            MatchShuffle();
        }
        */

        return possible;
    }

    public bool MatchCheckForPosition(GameObject GD, GameObject GP)
    {
        int indexD = 0;
        int indexP = 0;
        bool clear = false;
        int temp = 0;
        foreach (GameObject g in gems)
        {
            if (!gemScript.Contains(g.GetComponent<Gem>()))
            {
                gemScript.Add(gems[temp].GetComponent<Gem>());
                temp++;
            }
        }
        foreach (GameObject g in gems)
        {
            if(g.gameObject == GD)
            {
                break;
            }
            else
            {
                indexD++;
            }
        }
        foreach (GameObject g in gems)
        {
            if (g.gameObject == GP)
            {
                break;
            }
            else
            {
                indexP++;
            }
        }



        if (indexD - 1 >= 0 && indexD - 2 >= 0 && (indexD) % 6 != 0
                && indexD % gridX != 0 && indexD % gridX != 1 && indexP+1!=indexD)
            {
            if (gemScript[indexP].type == gemScript[indexD - 1].type && gemScript[indexP].type == gemScript[indexD - 2].type)
            {
                clear = true;
            }
        }
        if (indexD - 1 >= 0 && indexD + 1 < gemScript.Count
            && indexD % gridX != gridX - 1 && indexD % gridX != 0 && indexP + 1 != indexD && indexP - 1 != indexD)
        {
            if (gemScript[indexP].type == gemScript[indexD - 1].type && gemScript[indexP].type == gemScript[indexD + 1].type)
            {
                clear = true;
            }
        }
        if (indexD + 1 < gemScript.Count && indexD + 2 < gemScript.Count
            && indexD % gridX != gridX - 1 && indexD % gridX != gridX - 2 && indexP - 1 != indexD)
        {
            if (gemScript[indexP].type == gemScript[indexD + 1].type && gemScript[indexP].type == gemScript[indexD + 2].type)
            {
                clear = true;
            }
        }

        if (indexD - 1 * gridX >= 0 && indexD - 2 * gridX >= 0 && indexP + 1 * gridX != indexD)
        {
            if (gemScript[indexP].type == gemScript[indexD - 1 * gridX].type && gemScript[indexP].type == gemScript[indexD - 2 * gridX].type)
            {
                clear = true;
            }
        }
        if (indexD - 1 * gridX >= 0 && indexD + 1 * gridX < gemScript.Count && indexP - 1 * gridX != indexD && indexP + 1 * gridX != indexD)
        {
            if (gemScript[indexP].type == gemScript[indexD - 1 * gridX].type && gemScript[indexP].type == gemScript[indexD + 1 * gridX].type)
            {
                clear = true;
            }
        }
        if (indexD + 1 * gridX < gemScript.Count && indexD + 2 * gridX < gemScript.Count && indexP - 1 * gridX != indexD)
        {
            if (gemScript[indexP].type == gemScript[indexD + 1 * gridX].type && gemScript[indexP].type == gemScript[indexD + 2 * gridX].type)
            {
                clear = true;
            }
        }


        return clear;

    }

    private void MatchShuffle()
    {
        int temp = 0;
        foreach (GameObject g in gems)
        {
            if (!gemScript.Contains(g.GetComponent<Gem>()))
            {
                gemScript.Add(gems[temp].GetComponent<Gem>());
                temp++;
            }
        }

        for (int k = 0; k < gemScript.Count; k++)
        {
            if (k - 1 >= 0 && k - 2 >= 0 && (k) % 6 != 0
                && k % gridX != 0 && k % gridX != 1)
            {
                if (gemScript[k].type == gemScript[k - 1].type && gemScript[k].type == gemScript[k - 2].type)
                {
                    //checkGemOnGrid
                    List<int> tempLI = checkPossibleMovment(k);
                    tempLI.Remove(k - 1);
                    int op = tempLI[Random.Range(0, tempLI.Count - 1)];
                    //Gem_Type gem = gemScript[op].type;
                    gemScript[op].type = gemScript[k].type;
                    gemScript[k].type = (Gem_Type)Random.Range(0, Gem_Type.GetNames(typeof(Gem_Type)).Length);
                }
            }
            if (k - 1 >= 0 && k + 1 < gemScript.Count
                && k % gridX != gridX - 1 && k % gridX != 0)
            {
                if (gemScript[k].type == gemScript[k - 1].type && gemScript[k].type == gemScript[k + 1].type)
                {
                    List<int> tempLI = checkPossibleMovment(k);
                    tempLI.Remove(k - 1);
                    tempLI.Remove(k + 1);
                    int op = tempLI[Random.Range(0, tempLI.Count - 1)];
                    //Gem_Type gem = gemScript[op].type;
                    gemScript[op].type = gemScript[k].type;
                    gemScript[k].type = (Gem_Type)Random.Range(0, Gem_Type.GetNames(typeof(Gem_Type)).Length);
                }
            }
            if (k + 1 < gemScript.Count && k + 2 < gemScript.Count
                && k % gridX != gridX - 1 && k % gridX != gridX - 2)
            {
                if (gemScript[k].type == gemScript[k + 1].type && gemScript[k].type == gemScript[k + 2].type)
                {
                    List<int> tempLI = checkPossibleMovment(k);
                    tempLI.Remove(k + 1);
                    int op = tempLI[Random.Range(0, tempLI.Count - 1)];
                    //Gem_Type gem = gemScript[op].type;
                    gemScript[op].type = gemScript[k].type;
                    gemScript[k].type = (Gem_Type)Random.Range(0, Gem_Type.GetNames(typeof(Gem_Type)).Length);
                }
            }

            if (k - 1 * gridX >= 0 && k - 2 * gridX >= 0)
            {
                if (gemScript[k].type == gemScript[k - 1 * gridX].type && gemScript[k].type == gemScript[k - 2 * gridX].type)
                {
                    List<int> tempLI = checkPossibleMovment(k);
                    tempLI.Remove(k - gridX);
                    int op = tempLI[Random.Range(0, tempLI.Count - 1)];
                    //Gem_Type gem = gemScript[op].type;
                    gemScript[op].type = gemScript[k].type;
                    gemScript[k].type = (Gem_Type)Random.Range(0, Gem_Type.GetNames(typeof(Gem_Type)).Length);
                }
            }
            if (k - 1 * gridX >= 0 && k + 1 * gridX < gemScript.Count)
            {
                if (gemScript[k].type == gemScript[k - 1 * gridX].type && gemScript[k].type == gemScript[k + 1 * gridX].type)
                {
                    List<int> tempLI = checkPossibleMovment(k);
                    tempLI.Remove(k - gridX);
                    tempLI.Remove(k + gridX);
                    int op = tempLI[Random.Range(0, tempLI.Count - 1)];
                    //Gem_Type gem = gemScript[op].type;
                    gemScript[op].type = gemScript[k].type;
                    gemScript[k].type = (Gem_Type)Random.Range(0, Gem_Type.GetNames(typeof(Gem_Type)).Length);
                }
            }
            if (k + 1 * gridX < gemScript.Count && k + 2 * gridX < gemScript.Count)
            {
                if (gemScript[k].type == gemScript[k + 1 * gridX].type && gemScript[k].type == gemScript[k + 2 * gridX].type)
                {
                    List<int> tempLI = checkPossibleMovment(k);
                    tempLI.Remove(k + gridX);
                    int op = tempLI[Random.Range(0, tempLI.Count - 1)];
                    //Gem_Type gem = gemScript[op].type;
                    gemScript[op].type = gemScript[k].type;
                    gemScript[k].type = (Gem_Type)Random.Range(0, Gem_Type.GetNames(typeof(Gem_Type)).Length);
                }
            }

        }
        gemScript.Clear();


    }

    List<int> checkGemOnGrid(int current, int testPos)
    {
        List<int> result = new List<int>();
        if (current > testPos)
        {
            if(current - testPos == 1 && current % gridX != 0)
            {
                result.Add(testPos);
            }
            if (current - testPos == 2 && current % gridX != 0 && current % gridX != 1)
            {
                result.Add(testPos);
            }
            if(current - testPos == gridX && current >= gridX)
            {
                result.Add(testPos);
            }
            if (current - testPos == gridX * 2 && current >= gridX * 2)
            {
                result.Add(testPos);
            }
        }
        else
        {
            if (current - testPos == -1 && current % gridX != gridX - 1)
            {
                result.Add(testPos);
            }
            if (current - testPos == -2 && current % gridX != gridX - 1 && current % gridX != gridX - 2)
            {
                result.Add(testPos);
            }
            if (current - testPos == -gridX && current < gemScript.Count - gridX)
            {
                result.Add(testPos);
            }
            if (current - testPos == -gridX * 2 && current < gemScript.Count - gridX * 2)
            {
                result.Add(testPos);
            }
        }

        return result;
    }

    List<int> checkPossibleMovment(int current)
    {
        List<int> result = new List<int>();
        if (current % gridX != 0)
        {
            result.Add(current - 1);
        }
        if (current >= gridX)
        {
            result.Add(current-gridX);
        }

        if (current % gridX != gridX - 1)
        {
            result.Add(current + 1);
        }
        if (current % gridX <= gridX-1)
        {
            result.Add(current+gridX);
        }
        
        return result;
    }

    void ShuffleTypes()
    {
        int temp = 0;
        foreach (GameObject g in gems)
        {
            gemScript.Add(gems[temp].GetComponent<Gem>());
            temp++;
        }

        foreach (Gem g in gemScript)
        {
            g.type = (Gem_Type)Random.Range(0, Gem_Type.GetNames(typeof(Gem_Type)).Length);
        }
    }
}
