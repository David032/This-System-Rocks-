using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    [SerializeField] private Camera activeCamera;

    [SerializeField] private GameObject dummyObject;

    private bool dummyActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!dummyActive)
            {
                var mousePos = Input.mousePosition;
                mousePos.z = 1000;

                var adjustedPos = activeCamera.ScreenToWorldPoint(mousePos);
                
                print(Input.mousePosition.ToString());
                GameObject newDummy = Instantiate(dummyObject);
                newDummy.transform.position = adjustedPos;
                dummyActive = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
            dummyActive = false;

    }
}
