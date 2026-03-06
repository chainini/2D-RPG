using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    private CharacterStats targetStats;
    public float shockStrikeSpeed;
    private float shockStrikeDamage;

    private Animator anim;
    private bool triggered;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(float _shockStrikeDamage, CharacterStats _targetStats)
    {
        shockStrikeDamage = _shockStrikeDamage;
        targetStats = _targetStats;
    }

    private void Update()
    {
        if (!targetStats)
            return;

        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, shockStrikeSpeed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.position = new Vector3(anim.transform.position.x, anim.transform.position.y + .5f);
            anim.transform.rotation = Quaternion.identity;

            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);


            Invoke("DamageAndSelfDestory", .2f);
            anim.SetTrigger("Hit");
            triggered = true;
        }
    }

    private void DamageAndSelfDestory()
    {
        targetStats.AlppyShock(true);
        targetStats.TackDamage(shockStrikeDamage);
        Destroy(gameObject, .4f);
    }

}
