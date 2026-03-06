using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("After image fx")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCoolDown;
    private float afterImageCoolDonwTimer;

    [SerializeField] private ParticleSystem dustFX;

    protected override void Update()
    {
        afterImageCoolDonwTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 在Dash的时候在身后创造残影
    /// </summary>
    public void CreateAfterImage()
    {
        if (afterImageCoolDonwTimer <= 0)
        {
            afterImageCoolDonwTimer = afterImageCoolDown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);
        }

    }
    /// <summary>
    /// Dash粒子特效
    /// </summary>
    public void PlayDustFX()
    {
        if (dustFX != null)
            dustFX.Play();
    }
}
