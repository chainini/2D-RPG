using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string ID;
    public bool activationStatus;

    private void Awake()
    {
        anim = GetComponent<Animator>();

    }

    private void Start()
    {
    }
    [ContextMenu("Generate checkpoint ID")]
    private void GenerateID()
    {
        ID = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckPoint();
        }
    }

    public void ActivateCheckPoint()
    {
        if (activationStatus == false)
            AudioManager.instance.PlaySFX(5, transform);
        activationStatus = true;
        anim.SetBool("active", true);
    }
}
