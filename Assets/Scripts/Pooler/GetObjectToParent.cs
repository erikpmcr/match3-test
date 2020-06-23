using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObjectToParent : MonoBehaviour
{
    public List<GameObject> Objects;
    public List<Transform> positions;
    // Start is called before the first frame update
    void Start()
    {
        if (Objects.Count == positions.Count)
        {
            int i = 0;
            foreach (GameObject Obj in Objects)
            {
                Obj.transform.parent = this.gameObject.transform;
                Obj.transform.position = positions[i].position;
                i++;
            }
        }
        else
        {
            Debug.LogWarning("ObjectNumber and Positons are different");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        foreach (GameObject Obj in Objects)
        {
            Obj.transform.parent = this.gameObject.transform.parent;
        }
        
    }
}
