using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFocusScript : MonoBehaviour
{

    public float halfXBounds = 70f;
    public float halfYBounds = 70f;
    public float halfZBounds = 15f;

    public Bounds focusBounds;

    public List<GameObject> players;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        //position = Vector3.Lerp(players[0].transform.position, players[1].transform.position, 0.8f * Time.deltaTime);
        position = Vector3.Lerp(new Vector3(players[0].transform.position.x, players[0].transform.position.y + 20f, players[0].transform.position.z),
            new Vector3(players[1].transform.position.x, players[1].transform.position.y + 20f, players[1].transform.position.z), 0.8f * Time.deltaTime);

        Bounds bounds = new Bounds();
        bounds.Encapsulate(new Vector3(position.x - halfXBounds,
            position.y - halfYBounds, position.z - halfZBounds));
        bounds.Encapsulate(new Vector3(position.x + halfXBounds,
           position.y + halfYBounds, position.z + halfZBounds));
        focusBounds = bounds;
    }
}

