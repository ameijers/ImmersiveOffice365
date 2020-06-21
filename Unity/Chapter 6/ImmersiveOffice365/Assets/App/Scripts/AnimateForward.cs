using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateForward : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            StartCoroutine("MoveForward");
        }
    }

    //protected void MoveForward()
    //{
    //    Vector3 initialPos = gameObject.transform.position;

    //    for(float posZ = 0; posZ <= 1f; posZ += 0.01f)
    //    {
    //        gameObject.transform.position = initialPos + new Vector3(0, 0, posZ);
    //    }
    //}

    protected IEnumerator MoveForward()
    {
        Vector3 initialPos = gameObject.transform.position;

        for (float posZ = 0; posZ <= 1f; posZ += 0.01f)
        {
            gameObject.transform.position = initialPos + new Vector3(0, 0, posZ);

            yield return new WaitForSecondsRealtime(.1f);
        }
    }
}
