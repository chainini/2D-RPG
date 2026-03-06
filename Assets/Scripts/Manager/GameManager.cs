using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;

    private Transform player;

    [SerializeField]private CheckPoint[] checkPoints;
    [SerializeField] private string closeCheckpointID;


    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);


        checkPoints = FindObjectsOfType<CheckPoint>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player.transform;
    }

    /// <summary>
    /// can not load skill £¿ why£¿
    /// </summary>
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        SaveManager.instance.LoadGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelay(_data));
    }

    private void LoadCheckPoint(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.ID == pair.Key && pair.Value == true)
                {

                    checkPoint.ActivateCheckPoint();
                }
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if(lostCurrencyAmount >0 )
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab,new Vector3(lostCurrencyX,lostCurrencyY),Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    } 

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        LoadCheckPoint(_data);
        LoadCloseCheckPoint(_data);
        LoadLostCurrency(_data);
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        if(FindCloseCheckPoint() != null)
            _data.closeCheckpointID = FindCloseCheckPoint().ID;

        _data.checkpoints.Clear();

        foreach(CheckPoint checkPoint in checkPoints)
        {
            _data.checkpoints.Add(checkPoint.ID, checkPoint.activationStatus);
        }
    }
    private void LoadCloseCheckPoint(GameData _data)
    {
        if (_data.closeCheckpointID == null)
            return;

        closeCheckpointID = _data.closeCheckpointID;

        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (closeCheckpointID == checkPoint.ID)
                player.position = checkPoint.transform.position;
        }
    }

    private CheckPoint FindCloseCheckPoint()
    {
        float closeDistance = Mathf.Infinity;
        CheckPoint closeCheckpoint = null;

        foreach(CheckPoint checkPoint in checkPoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkPoint.transform.position);
            
            if(distanceToCheckpoint < closeDistance && checkPoint.activationStatus == true)
            {
                closeDistance = distanceToCheckpoint;
                closeCheckpoint = checkPoint;
            }
        }

        return closeCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
