using TMPro;
using UnityEngine;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;
    private SpriteRenderer sr;
    private KeyCode myKeyCode;

    private Transform enemy;
    private BlackHole_Skill_Controller blackHole;

    /// <summary>
    /// ³õÊ¼»¯ºÚ¶´ÈÈ¼ü
    /// </summary>
    /// <param name="_keyCode">ÄÄ¸ö¼ü</param>
    /// <param name="myEnemy">µÐÈË</param>
    /// <param name="myBlackHole">ºÚ¶´</param>
    public void SetupHotKey(KeyCode _keyCode, Transform myEnemy, BlackHole_Skill_Controller myBlackHole)
    {
        myKeyCode = _keyCode;
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();
        textMeshProUGUI.text = myKeyCode.ToString();
        enemy = myEnemy;
        blackHole = myBlackHole;
    }

    private void Update()
    {
        if (Input.GetKeyUp(myKeyCode))
        {
            blackHole.AddEnemyToList(enemy);
            textMeshProUGUI.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
