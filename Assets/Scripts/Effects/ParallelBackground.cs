using UnityEngine;

public class ParallelBackground : MonoBehaviour
{
    private GameObject cam;
    // Start is called before the first frame update

    [SerializeField] private float parallelEffect;

    private float xPosition;
    private float length;
    void Start()
    {
        cam = GameObject.Find("Main Camera");

        xPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallelEffect);
        float distanceToMove = cam.transform.position.x * parallelEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > xPosition + length)
            xPosition += length;
        if (distanceMoved < xPosition - length)
            xPosition -= length;
    }
}
