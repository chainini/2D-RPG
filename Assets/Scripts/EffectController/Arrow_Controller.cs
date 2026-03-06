using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVolcity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove;
    [SerializeField] private bool filped;

    private CharacterStats myStats;

    private void Start()
    {
        canMove = true;
    }

    private void Update()
    {
        if (canMove)
            rb.velocity = new Vector2(xVolcity, rb.velocity.y);
    }

    public void SetupArrow(float _speed,CharacterStats _stats)
    {
        xVolcity = _speed;
        myStats = _stats;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            //collision.GetComponent<CharacterStats>()?.TackDamage(damage);

            myStats.DoDamage(collision.GetComponent<CharacterStats>());

            StuckInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            StuckInto(collision);
    }

    private void StuckInto(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>()?.Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, Random.Range(5, 7));
    }

    public void FilpArrow()
    {
        if (filped)
            return;

        xVolcity *= -1;
        filped = true;

        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }
}
