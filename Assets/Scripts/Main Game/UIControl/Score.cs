using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int score=0;
    public Text t_score;
    public string s_score;
    // Start is called before the first frame update
    void Start()
    {
        s_score = t_score.text;
    }

    // Update is called once per frame
    void Update()
    {
        t_score.text = s_score + score;
    }
}
