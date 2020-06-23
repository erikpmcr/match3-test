using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gem_Type {Milk, Apple, Orange, Bread, Brocolli, Coconut, Crystal};
public class Gem : MonoBehaviour
{
    public Gem_Type type;
    public int point = 10;
    ObjectPooler objectPooler;
    GridLoader gl;
    public bool despawnTag = false;
    public bool despawnConfirm = false;
    [SerializeField]
    public NewGemGenerator parent;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        gl = GameObject.Find("GridControl").GetComponent<GridLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<SpriteSelect>().SpriteNumber = (int)type;
        if (despawnTag)
        {
            Despawn();
        }
    }

    public void Despawn()
    {
        if (NeighborConfirm()) {
            if (MatchConfirm() || despawnConfirm)
            {
                parent.column.Remove(this.gameObject);
                gl.GetComponent<Score>().score += point;
                this.gameObject.transform.position = objectPooler.gameObject.transform.position;
                this.gameObject.SetActive(false);
                gl.gems.Remove(this.gameObject);
                this.parent = null;
                despawnTag = false;
                despawnConfirm = false;
            }
            else
            {
                despawnTag = false;
                despawnConfirm = false;
            }
        }
    }

    public bool NeighborConfirm()
    {
        bool result = false;

        RaycastHit2D[] hit = new RaycastHit2D[8];
        hit[0] = Physics2D.Raycast(new Vector2(this.transform.position.x - 1.3f * 2, this.transform.position.y),
            new Vector2(this.transform.position.x - 1.4f * 2, this.transform.position.y));
        hit[1] = Physics2D.Raycast(new Vector2(this.transform.position.x - 1.3f * 1, this.transform.position.y),
            new Vector2(this.transform.position.x - 1.4f * 1, this.transform.position.y));
        hit[2] = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y - 1.3f * 2),
            new Vector2(this.transform.position.x , this.transform.position.y - 1.4f * 2));
        hit[3] = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y - 1.3f * 1),
            new Vector2(this.transform.position.x, this.transform.position.y - 1.4f * 1));
        hit[4] = Physics2D.Raycast(new Vector2(this.transform.position.x + 1.3f * 2, this.transform.position.y),
            new Vector2(this.transform.position.x + 1.4f * 2, this.transform.position.y));
        hit[5] = Physics2D.Raycast(new Vector2(this.transform.position.x + 1.3f * 1, this.transform.position.y),
            new Vector2(this.transform.position.x + 1.4f * 1, this.transform.position.y));
        hit[6] = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y + 1.3f * 2),
            new Vector2(this.transform.position.x, this.transform.position.y + 1.4f * 2));
        hit[7] = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y + 1.3f * 1),
            new Vector2(this.transform.position.x, this.transform.position.y + 1.4f * 1));

        Debug.DrawLine(new Vector2(this.transform.position.x - 1.3f * 2, this.transform.position.y),
            new Vector2(this.transform.position.x - 1.4f * 2, this.transform.position.y)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x - 1.3f * 1, this.transform.position.y),
            new Vector2(this.transform.position.x - 1.4f * 1, this.transform.position.y)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y - 1.3f * 2),
            new Vector2(this.transform.position.x, this.transform.position.y - 1.4f * 2)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y - 1.3f * 1),
            new Vector2(this.transform.position.x, this.transform.position.y - 1.4f * 1)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x + 1.3f * 2, this.transform.position.y),
            new Vector2(this.transform.position.x + 1.4f * 2, this.transform.position.y)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x + 1.3f * 1, this.transform.position.y),
            new Vector2(this.transform.position.x + 1.4f * 1, this.transform.position.y)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y + 1.3f * 2),
            new Vector2(this.transform.position.x, this.transform.position.y + 1.4f * 2)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y + 1.3f * 1),
            new Vector2(this.transform.position.x, this.transform.position.y + 1.4f * 1)
            , Color.red);


        int connect = 0;
        foreach (RaycastHit2D h in hit)
        {
            if (h.collider != null && h.collider.gameObject.GetComponent<Gem>()!=null)
            {
                connect++;
            }
        }

        if (connect >= 4)
        {
            result = true;
        }

        return result;
    }
    public bool MatchConfirm()
    {
        bool result = false;

        RaycastHit2D[] hit = new RaycastHit2D[8];
        hit[0] = Physics2D.Raycast(new Vector2(this.transform.position.x - 1.3f * 2, this.transform.position.y),
            new Vector2(this.transform.position.x - 1.4f * 2, this.transform.position.y));
        hit[1] = Physics2D.Raycast(new Vector2(this.transform.position.x - 1.3f * 1, this.transform.position.y),
            new Vector2(this.transform.position.x - 1.4f * 1, this.transform.position.y));
        hit[2] = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y - 1.3f * 2),
            new Vector2(this.transform.position.x, this.transform.position.y - 1.4f * 2));
        hit[3] = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y - 1.3f * 1),
            new Vector2(this.transform.position.x, this.transform.position.y - 1.4f * 1));
        hit[4] = Physics2D.Raycast(new Vector2(this.transform.position.x + 1.3f * 2, this.transform.position.y),
            new Vector2(this.transform.position.x + 1.4f * 2, this.transform.position.y));
        hit[5] = Physics2D.Raycast(new Vector2(this.transform.position.x + 1.3f * 1, this.transform.position.y),
            new Vector2(this.transform.position.x + 1.4f * 1, this.transform.position.y));
        hit[6] = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y + 1.3f * 2),
            new Vector2(this.transform.position.x, this.transform.position.y + 1.4f * 2));
        hit[7] = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y + 1.3f * 1),
            new Vector2(this.transform.position.x, this.transform.position.y + 1.4f * 1));

        Debug.DrawLine(new Vector2(this.transform.position.x - 1.3f * 2, this.transform.position.y),
            new Vector2(this.transform.position.x - 1.4f * 2, this.transform.position.y)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x - 1.3f * 1, this.transform.position.y),
            new Vector2(this.transform.position.x - 1.4f * 1, this.transform.position.y)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y - 1.3f * 2),
            new Vector2(this.transform.position.x, this.transform.position.y - 1.4f * 2)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y - 1.3f * 1),
            new Vector2(this.transform.position.x, this.transform.position.y - 1.4f * 1)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x + 1.3f * 2, this.transform.position.y),
            new Vector2(this.transform.position.x + 1.4f * 2, this.transform.position.y)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x + 1.3f * 1, this.transform.position.y),
            new Vector2(this.transform.position.x + 1.4f * 1, this.transform.position.y)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y + 1.3f * 2),
            new Vector2(this.transform.position.x, this.transform.position.y + 1.4f * 2)
            , Color.red);
        Debug.DrawLine(new Vector2(this.transform.position.x, this.transform.position.y + 1.3f * 1),
            new Vector2(this.transform.position.x, this.transform.position.y + 1.4f * 1)
            , Color.red);

        if (hit[0].collider != null && hit[1].collider != null && 
            type == hit[0].collider.gameObject.GetComponent<Gem>().type && type == hit[1].collider.gameObject.GetComponent<Gem>().type)
        {
            hit[0].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            hit[1].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            result = true;
        }
        if (hit[2].collider != null && hit[3].collider != null && 
            type == hit[2].collider.gameObject.GetComponent<Gem>().type && type == hit[3].collider.gameObject.GetComponent<Gem>().type)
        {
            hit[2].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            hit[3].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            result = true;
        }
        if (hit[4].collider != null && hit[5].collider != null &&
            type == hit[4].collider.gameObject.GetComponent<Gem>().type && type == hit[5].collider.gameObject.GetComponent<Gem>().type)
        {
            hit[4].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            hit[5].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            result = true;
        }
        if (hit[6].collider != null && hit[7].collider != null &&
            type == hit[6].collider.gameObject.GetComponent<Gem>().type && type == hit[7].collider.gameObject.GetComponent<Gem>().type)
        {
            hit[6].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            hit[7].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            result = true;
        }
        if (hit[1].collider != null && hit[5].collider != null &&
            type == hit[1].collider.gameObject.GetComponent<Gem>().type && type == hit[5].collider.gameObject.GetComponent<Gem>().type)
        {
            hit[1].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            hit[5].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            result = true;
        }
        if (hit[3].collider != null && hit[7].collider != null &&
            type == hit[3].collider.gameObject.GetComponent<Gem>().type && type == hit[7].collider.gameObject.GetComponent<Gem>().type)
        {
            hit[3].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            hit[7].collider.gameObject.GetComponent<Gem>().despawnConfirm = true;
            result = true;
        }
        return result;
    }
}
