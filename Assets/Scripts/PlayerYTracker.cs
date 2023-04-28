using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerYTracker : MonoBehaviour
{
    Rigidbody rbody;
    public PlayerMovementFixed pm;
    public float lastY;
    public float curY;
    LinkedList<float> yvels = new LinkedList<float>();
    // Start is called before the first frame update
    void Start()
    {
        rbody = transform.root.GetComponent<Rigidbody>();
        yvels.AddLast(transform.position.y);
        yvels.AddLast(transform.position.y);
        yvels.AddLast(transform.position.y);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        curY = transform.position.y;
        if (curY != yvels.Last.Value)
        {
            pm.onPlatform = false;
        }
        else if (curY == yvels.Last.Value && yvels.Last.Value == getPrev())
        {
            pm.onPlatform = true;
        }
        else if (curY == yvels.Last.Value)
        {
            pm.onPlatform = true;
        }
        else
        {
            pm.onPlatform = false;
        }

        yvels.AddLast(curY);
        if (yvels.Count > 10)
        {
            yvels.RemoveFirst();
        }
    }

    float getPrev()
    {
       if (yvels.Last.Previous != null)
        {
            return yvels.Last.Previous.Value;
        }
        return 0;
    }
}
