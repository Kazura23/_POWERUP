using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FollowPlayers : MonoBehaviour
{

    public Transform Player1, Player2;
    public float GroundLevel;
    public float MinSize;
    public float MaxSize;
    public float Margin;
    public float CameraRatio;
    public float Follow;

    private Vector3 _followPos;
    private Camera _cam;
    private bool _isFreezed = false;
    public bool _player1Follow = true, _player2Follow = true;
    //[HideInInspector]
    public bool doubleRoast;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _cam = GetComponent<Camera>();
        _followPos = transform.position;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_isFreezed)
        {
            return;
        }
        if (_player1Follow && _player2Follow)
        {
            float xDistance = Mathf.Abs((Player1.position - Player2.position).x) + Margin * 2;


            float rotationZoom = (Mathf.Abs(transform.parent.eulerAngles.z) % 90f);
            if (rotationZoom > 45f)
                rotationZoom = Mathf.Abs(rotationZoom - 90f);

            float newSize = xDistance * CameraRatio + rotationZoom * .5f;
            _cam.orthographicSize = Mathf.Clamp(newSize / 2, MinSize, MaxSize);
            _followPos.y = GroundLevel + _cam.orthographicSize + Player1.position.y / 5 + Player2.position.y / 5;
            _followPos.x = (Player1.position.x + Player2.position.x) / 2f;
            //transform.position = _followPos;
            transform.DOMove(_followPos, Follow);

            //			transform.position = Vector3.Lerp (transform.position, _followPos, Time.fixedDeltaTime * 5f);
            //			transform.DOMove (_followPos, Follow);
        }
        else if (_player1Follow)
        {
            if (doubleRoast)
            {
                _followPos.y = Player1.position.y - Player1.position.y / 5;
                _followPos.x = Player1.position.x;
            }
            else
            {
                _followPos.y = GroundLevel + _cam.orthographicSize + Player1.position.y - Player1.position.y/1.5f;
                _followPos.x = Player1.position.x;
            }
        }
        else if (_player2Follow)
        {
            if (doubleRoast) { 
                _followPos.y = Player1.position.y - Player1.position.y / 5;
                _followPos.x = Player2.position.x;
            }
            else
            {
                _followPos.y = GroundLevel + _cam.orthographicSize + Player2.position.y - Player2.position.y/1.5f;
                _followPos.x = Player2.position.x;
            }
        }

       
        transform.position = Vector3.Lerp(transform.position, _followPos, Time.fixedDeltaTime * 5f);
    }

    /*public void ToggleFrezee ()
	{
		_isFreezed = !_isFreezed;
	}*/

    public void StopFollow(int playerNumber)
    {
        if (playerNumber == 1)
            _player1Follow = false;
        else
            _player2Follow = false;

    }

    public void ResetFollow(int playerNumber)
    {
        if (playerNumber == 1)
            _player1Follow = true;
        else
            _player2Follow = true;
    }
    
}
