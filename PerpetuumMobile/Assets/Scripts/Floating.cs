using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    #region public

    public float amplitude;
    public float frequency;

    #endregion public

    #region private

    private float startingY;

    #endregion private
   
    // Start is called before the first frame update
    void Start()
    {
        startingY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, startingY + Mathf.Sin(Time.time * frequency) * amplitude, transform.position.z);
        transform.rotation = new Quaternion(0.0f, Mathf.Sin(Time.time * frequency) * amplitude, 0, 0);
    }
}
