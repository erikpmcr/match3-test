using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGemGenerator : MonoBehaviour
{
    [SerializeField]
    GridLoader gl;
    public bool canCreate = false;
    public float startTimer = 1f;
    ObjectPooler objectPooler;
    public string objTag = "Gems";
    public bool cache = false;
    public bool ready = false;
    public bool rayTouch = false;
    [SerializeField]
    float rayOffset = -1f;
    [SerializeField]
    float rayLenght = -9f;

    public List<GameObject> column = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown(2));
        objectPooler = ObjectPooler.Instance;
        gl = GameObject.Find("GridControl").GetComponent<GridLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y + rayOffset),
            new Vector2(this.transform.position.x, this.transform.position.y + rayOffset) + new Vector2(0, rayLenght));
        /*Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y + rayOffset),
            new Vector2(this.transform.position.x, this.transform.position.y + rayOffset) + new Vector2(0, rayLenght)
            , Color.red);*/
        if (hit)
        {
            rayTouch = true;
            ready = true;
        }
        else
        {
            rayTouch = false;
        }
        if (column.Count < 6)
        {
            if (!rayTouch)
            {

                if (canCreate && ready)
                {
                    GameObject g = objectPooler.SpawnFromPool(objTag, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
                    g.gameObject.GetComponent<Gem>().type = (Gem_Type)Random.Range(0, Gem_Type.GetNames(typeof(Gem_Type)).Length);
                    if (!gl.gems.Contains(g))
                    {
                        gl.gems.Add(g);
                    }
                    if (!column.Contains(g))
                    {
                        column.Add(g);
                    }
                    canCreate = false;
                    StartCoroutine(Countdown(startTimer));
                    cache = false;
                    ready = false;
                }
                else if (ready)
                {
                    cache = true;
                }

            }

            if (cache && canCreate && ready)
            {
                GameObject g = objectPooler.SpawnFromPool(objTag, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
                g.gameObject.GetComponent<Gem>().type = (Gem_Type)Random.Range(0, Gem_Type.GetNames(typeof(Gem_Type)).Length);
                if (!gl.gems.Contains(g))
                {
                    gl.gems.Add(g);
                }

                if (!column.Contains(g))
                {
                    column.Add(g);
                }
                canCreate = false;
                StartCoroutine(Countdown(startTimer));
                cache = false;
                ready = false;
            }
        }
        foreach(GameObject g in column)
        {
            g.gameObject.GetComponent<Gem>().parent = this;
            float testvec = this.gameObject.transform.position.x - g.transform.position.x;
            if (Mathf.Abs(testvec)>1f)
            {
                column.Remove(g);
            }
        }
    }

    private IEnumerator Countdown(float duration)
    {

        float totalTime = 0;
        while (totalTime <= duration)
        {
            totalTime += Time.deltaTime;
            yield return null;
        }
        canCreate = true;
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Gem>() != null)
        {
            ready = true;
        }

    }
    */
    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Gem>() != null && canCreate && ready)
        {
            GameObject g = objectPooler.SpawnFromPool(objTag, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0), Quaternion.identity);
            g.gameObject.GetComponent<Gem>().type = (Gem_Type)Random.Range(0, Gem_Type.GetNames(typeof(Gem_Type)).Length);
            gl.gems.Add(g);
            canCreate = false;
            StartCoroutine(Countdown(startTimer));
            cache = false;
            ready = false;
        }
        else if (collision.GetComponent<Gem>() != null && ready)
        {
            cache = true;
        }
    }
    */
}
