using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloodWater : MonoBehaviour
{
    public float waterRiseSpeed = 1.0f; // Adjust the speed of water rising
    private bool isFlooding = false;
    private float maxHeight = -34f;

    private void Update()
    {
        if (isFlooding)
        {
            MoveUp();
        }
    }

    public void StartFlooding()
    {
        isFlooding = true;
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * waterRiseSpeed * Time.deltaTime);

        if (transform.position.y > maxHeight)
        {
            isFlooding = false;
        }
    }
}
