using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour {

    public Rigidbody myRigidbody;
    public float forceMin;
    public float forceMax;

    float lifetime = 4f;
    float fadetime = 2f;

	private void Start ()
    {
        float forceMagnitude = Random.Range(forceMin, forceMax);

        myRigidbody.AddForce(transform.right * forceMagnitude);
        myRigidbody.AddTorque(Random.insideUnitSphere * forceMagnitude);

        StartCoroutine(Fade());
	}
	
    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifetime);

        float percent = 0;
        float fadeSpeed = 1f / fadetime;

        Material shellMaterial = GetComponent<Renderer>().material;
        Color originalColor = shellMaterial.color;

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            shellMaterial.color = Color.Lerp(originalColor, Color.clear, percent);
            yield return null;
        }

        Destroy(gameObject);
    }
}
