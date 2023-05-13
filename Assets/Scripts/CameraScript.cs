using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public LevelFocusScript levelFocus;
    public List<GameObject> players;

    public float depthSpeed = 5f;
    public float angleSpeed = 7f;
    public float posnSpeed = 5f;

    public float depthMax = -20f;
    public float depthMin = -100f;

    public float angleMax = 11f;
    public float anglemin = 3f;

    private float cameraEulerX;
    private Vector3 cameraPosn;

    // public GameObject playerOne;
    // public GameObject playerTwo;
    public GameMatchManager _gmm;
    public float travelTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        //playerOne = _gmm._players[0];
        //playerTwo = _gmm._players[1];
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        
    }

    private void LateUpdate()
    {
        CalculateCamera();
        MoveCamera();

    }

    private void MoveCamera()
    {
        Vector3 position = gameObject.transform.position;
        if (position != cameraPosn)
        {
            Vector3 newPosn = Vector3.zero;
            newPosn.x = Mathf.MoveTowards(position.x, cameraPosn.x, posnSpeed * Time.deltaTime);
            newPosn.y = Mathf.MoveTowards(position.y, cameraPosn.y, posnSpeed * Time.deltaTime);
            newPosn.z = Mathf.MoveTowards(position.z, cameraPosn.z, depthSpeed * Time.deltaTime);
            transform.position = newPosn;
        }

        Vector3 localEulerAngles = transform.localEulerAngles;
        if (localEulerAngles.x != cameraEulerX)
        {
            Vector3 targetEulerAngles = new Vector3(cameraEulerX, localEulerAngles.y, localEulerAngles.z);
            transform.localEulerAngles = Vector3.MoveTowards(localEulerAngles, targetEulerAngles, angleSpeed * Time.deltaTime);
        }
    }

    private void CalculateCamera()
    {
        Vector3 avgCenter = Vector3.zero;
        Vector3 totalPos = Vector3.zero;
        Bounds playerBounds = new Bounds();
        for (int i = 0; i < players.Count; i++)
        {
            Vector3 playerPosn = players[i].transform.position;

            if (!levelFocus.focusBounds.Contains(playerPosn))
            {
                float playerX = Mathf.Clamp(playerPosn.x, levelFocus.focusBounds.min.x, levelFocus.focusBounds.max.x);
                float playerY = Mathf.Clamp(playerPosn.y, levelFocus.focusBounds.min.y, levelFocus.focusBounds.max.y);
                float playerZ = Mathf.Clamp(playerPosn.z, levelFocus.focusBounds.min.z, levelFocus.focusBounds.max.z);

                playerPosn = new Vector3(playerX, playerY, playerZ);
            }

            totalPos += playerPosn;
            playerBounds.Encapsulate(playerPosn);
        }

        avgCenter = (totalPos / players.Count);
        float extents = (playerBounds.extents.x + playerBounds.extents.y);
        float lerpPercent = Mathf.InverseLerp(0, (levelFocus.halfXBounds + levelFocus.halfYBounds) / 2, extents);

        float depth = Mathf.Lerp(depthMax, depthMin, lerpPercent);
        float angle = Mathf.Lerp(angleMax, anglemin, lerpPercent);

        cameraEulerX = angle;
        cameraPosn = new Vector3(avgCenter.x, avgCenter.y, depth);
    }
}

