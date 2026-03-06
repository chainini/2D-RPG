using TMPro;
using UnityEngine;

public class PopUpTextFX : MonoBehaviour
{
    private TextMeshPro myText;

    [SerializeField] private float speed;
    [SerializeField] private float disapperSpeed;
    [SerializeField] private float colorDisapperSpeed;

    [SerializeField] private float lifetime;

    private float textTimer;

    private void Start()
    {
        myText = GetComponent<TextMeshPro>();
        textTimer = lifetime;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), speed * Time.deltaTime);
        textTimer -= Time.deltaTime;

        if (textTimer <= 0)
        {
            float alpha = myText.color.a - colorDisapperSpeed * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.b, myText.color.g, alpha);

            if (myText.color.a < 50)
                speed = disapperSpeed;

            if (myText.color.a <= 0)
                Destroy(gameObject);
        }
    }
}
