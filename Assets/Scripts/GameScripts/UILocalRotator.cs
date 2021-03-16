using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILocalRotator : MonoBehaviour
{
    private Quaternion relatativeRotation; // the rotation of our parent;

    // Start is called before the first frame update
    void Start()
    {
        relatativeRotation = transform.parent.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = relatativeRotation;
    }
}
