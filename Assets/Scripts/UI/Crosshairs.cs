using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshairs : MonoBehaviour {

    public LayerMask targetMask;
    public SpriteRenderer crosshairsDot;
    public Color dotHighlightColor;
    public float rotationSpeed = 40f;

    Color dotOriginalColor;

    private void Start()
    {
        Cursor.visible = false;
        dotOriginalColor = crosshairsDot.color;
    }

	private void Update () {
        transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
	}

    public void DetectTargets(Ray ray)
    {
        if (Physics.Raycast(ray, 100f, targetMask))
        {
            crosshairsDot.color = dotHighlightColor;
        }
        else
        {
            crosshairsDot.color = dotOriginalColor;
        }
    }
}
