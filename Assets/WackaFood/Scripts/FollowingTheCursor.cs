using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowingTheCursor : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    Vector3 myTrailPosition;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
   public void Update()
    {
        myTrailPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        myTrailPosition = new Vector3(myTrailPosition.x, myTrailPosition.y);
        gameObject.transform.position = myTrailPosition;
    }
}
