using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform target;
    public float speedhorizontal = .2f;

    public float speedvertical;

    private Vector3 lastPos;

    public bool verticalUp;
    // Use this for initialization
    // Update is called once per frame
    void LateUpdate()
    {

        target = GameManager.Singleton.mainCamera.transform;
        Vector3 pos = transform.position;
        pos.x += (target.transform.position.x - lastPos.x) * speedhorizontal;
        if(verticalUp)
        pos.y += (target.transform.position.x - lastPos.x) * speedvertical;
        else
        pos.y -= (target.transform.position.x - lastPos.x) *  speedvertical;
        transform.position = pos;

        lastPos = target.transform.position;
    }
}

