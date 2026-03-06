using UnityEngine;
using Cinemachine;

public class BossFightTrigger : MonoBehaviour
{
    public GameObject boss_Bar;
    public Transform boss;
    public CinemachineConfiner confiner;
    public PolygonCollider2D bossFight;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Invoke("ActiveBoss_Bar", 1f);
        confiner.m_BoundingShape2D = bossFight;
        AudioManager.instance.PlayBGM(2);
    }

    private void ActiveBoss_Bar()
    {
        boss_Bar.SetActive(true);
    }
}
