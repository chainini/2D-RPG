using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Player player;

    [Header("Pop up text")]
    [SerializeField] protected GameObject popUpTextPrefab;

    [Header("Screen shake FX")]
    protected CinemachineImpulseSource screenShake;
    [SerializeField] protected float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;

    

    [Header("FX info")]
    [SerializeField] protected Material hitMat;
    [SerializeField] protected float hitDuration;
    protected Material originMat;
    protected Color currentColor;

    [Header("Aliment colors")]
    [SerializeField] protected Color[] igniteColor;
    [SerializeField] protected Color[] chillColor;
    [SerializeField] protected Color[] shockColor;

    [Header("Ailment particales")]
    [SerializeField] public ParticleSystem igniteFx;
    [SerializeField] public ParticleSystem chillFx;
    [SerializeField] public ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] protected GameObject hitFXPrefab;
    [SerializeField] protected GameObject criticalFXPrefab;

    public GameObject myHealthBar;


    protected void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = PlayerManager.instance.player;
        screenShake = GetComponent<CinemachineImpulseSource>();
        originMat = sr.material;
        currentColor = sr.color;

    }

    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// Entity 弹出的信息
    /// </summary>
    /// <param name="_text">要弹出的信息</param>
    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(3, 5);

        Vector3 posOffset = new Vector3(randomX, randomY,0);

        GameObject newText = Instantiate(popUpTextPrefab, transform.position+posOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

    /// <summary>
    /// 屏幕晃动
    /// </summary>
    /// <param name="_shakePower">晃动向量</param>
    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }


    /// <summary>
    /// 使Entity消失
    /// </summary>
    /// <param name="_transprent">是否要消失</param>
    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
        {
            myHealthBar.SetActive(false);
            sr.color = Color.clear;
        }
        else
        {
            myHealthBar.SetActive(true);
            sr.color = Color.white;
        }
    }

    /// <summary>
    /// 调用了EntityFX里的协程FlashFX
    /// </summary>
    public void StartFlasHFX()=>StartCoroutine(FlashFX());

    /// <summary>
    /// Entity闪烁效果
    /// </summary>
    /// <returns></returns>
    public IEnumerator FlashFX()
    {   
        sr.material = hitMat;
        sr.color = Color.white;
        yield return new WaitForSeconds(hitDuration);
        sr.color = currentColor;
        sr.material = originMat;
    }

    protected void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    /// <summary>
    /// 停止所有颜色特效 和 粒子特效
    /// </summary>
    protected void CancelColorChange()
    {
        //停止所有Invoke
        CancelInvoke();
        sr.color = Color.white;

        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }

    /// <summary>
    /// 火粒子
    /// </summary>
    /// <param name="_second">持续时间</param>
    public void IgniteFXFor(float _second)
    {
        igniteFx.Play();

        InvokeRepeating("IgniteFX", 0, .3f);
        Invoke("CancelColorChange", _second);
    }
    /// <summary>
    /// 冰粒子
    /// </summary>
    /// <param name="_second">持续时间</param>
    public void ChillFXFor(float _second)
    {
        chillFx.Play();

        InvokeRepeating("ChillFX", 0, .3f);
        Invoke("CancelColorChange", _second);
    }
    /// <summary>
    /// 雷粒子
    /// </summary>
    /// <param name="_second">持续时间</param>
    public void ShockFXFor(float _second)
    {
        shockFx.Play();

        InvokeRepeating("ShockFX", 0, .3f);
        Invoke("CancelColorChange", _second);
    }
    /// <summary>
    /// 着火
    /// </summary>
    protected void IgniteFX()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }
    /// <summary>
    /// 冰冻
    /// </summary>
    protected void ChillFX()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }
    /// <summary>
    /// 麻痹
    /// </summary>
    protected void ShockFX()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    /// <summary>
    /// 打击的粒子特效
    /// </summary>
    /// <param name="_target">目标</param>
    /// <param name="_critical">是否暴击</param>
    public void CreateHitFX(Transform _target, bool _critical)
    {
        float zRotation = Random.Range(-90, 90);
        float xRotation = Random.Range(-.5f, .5f);
        float yRotation = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFXPrefab;

        if (_critical)
        {
            hitFXPrefab = criticalFXPrefab;

            float _yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if (transform.parent.GetComponent<Entity>().facingDir == -1)
                _yRotation = 180;

            hitFxRotation = new Vector3(0, _yRotation, zRotation);
        }

        GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xRotation, yRotation, 0), Quaternion.identity);
        newHitFx.transform.Rotate(hitFxRotation);
        Destroy(newHitFx, .5f);

    }


}
