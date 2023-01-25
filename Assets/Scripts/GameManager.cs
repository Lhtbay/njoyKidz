using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private GameObject _obstaclesParent;
    [SerializeField] private GameObject _greenBoxPrefab;
    [SerializeField] private GameObject _greenBoxParent;

    [SerializeField] private List<GameObject> _listAllObjects;
    [SerializeField] private List<GameObject> _listCoins;
    [SerializeField] private List<GameObject> _listGreenBox;
    [SerializeField] private List<bool> _listActiveGreenBox;

    private GameObject _greenBoxObject;

    private bool _checkTileIsFullBool,_greenBoxHere = false;
    private bool _greenBoxInstantiateCheck, _obstacleInstantiateCheck,_coinsInstantiateCheck = false;
    private bool _isGameOver = false;

    private float _timer,_timer2 = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartMethod();
    }

    private void Update()
    {
        UpdateMethod();
    }

    #region Main Methods

    private void StartMethod()
    {
             
        // Instantiate objects

        for (int i = 0; i < 10; i++)
        {
            GameObject newObstacle = Instantiate(_obstaclePrefab, new Vector3(Random.RandomRange(-7, 7),Random.RandomRange(-5, 5),0), Quaternion.identity);

            foreach (var item in _listAllObjects)
            {
                if (item.gameObject.transform.position == newObstacle.transform.position)
                {
                    Destroy(newObstacle);
                    _obstacleInstantiateCheck = true;
                    i--;
                }
            }
            if (!_obstacleInstantiateCheck)
            {              
                if (newObstacle.transform.position == new Vector3(0, 0, 0))
                {                   
                    Destroy(newObstacle);
                    i--;
                }
                else
                {
                    _listAllObjects.Add(newObstacle);
                    newObstacle.transform.parent = _obstaclesParent.transform;
                }
            }
            _obstacleInstantiateCheck = false;
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject newGreenBox = Instantiate(_greenBoxPrefab, new Vector3(Random.RandomRange(-7, 7), Random.RandomRange(-5, 5), 0), Quaternion.identity);

            foreach (var item in _listAllObjects)
            {
                if (item.transform.position == newGreenBox.transform.position)
                {
                    Destroy(newGreenBox);
                    _greenBoxInstantiateCheck = true;
                    i--;
                }
            }
            if (!_greenBoxInstantiateCheck)
            {
                if (newGreenBox.transform.position == new Vector3 (0,0,0))
                {
                    Destroy(newGreenBox);                   
                    i--;
                }
                else
                {
                    _listAllObjects.Add(newGreenBox);
                    _listGreenBox.Add(newGreenBox);
                    _listActiveGreenBox.Add(true);
                    newGreenBox.transform.parent = _greenBoxParent.transform;
                }
            }
            
            _greenBoxInstantiateCheck = false;
        }

        for (int i = 0; i < _listCoins.Count; i++)
        {                       
            GameObject newCoins = Instantiate(_listCoins[i], new Vector3(Random.RandomRange(-7, 7), Random.RandomRange(-5, 5), 0), Quaternion.identity);
            _coinsInstantiateCheck = false;
          
            while (!_coinsInstantiateCheck)
            {
                if (newCoins.transform.position == new Vector3(0, 0, 0))
                {
                    newCoins.transform.position = new Vector3(Random.RandomRange(-7, 7), Random.RandomRange(-5, 5), 0);
                }
                foreach (var item in _listAllObjects)
                {
                    if (item.transform.position == newCoins.transform.position)
                    {
                        newCoins.transform.position = new Vector3(Random.RandomRange(-7, 7), Random.RandomRange(-5, 5), 0);
                    }
                    else
                    {
                        _coinsInstantiateCheck = true;
                    }
                }
            }
        }    
    }

    private void UpdateMethod()
    {
        if (_checkTileIsFullBool)
        {
            _timer += Time.deltaTime;
            if (_timer >= 0.3f)
            {
                _checkTileIsFullBool = false;              
                _timer = 0;
            }

        }
        if (_greenBoxHere)
        {
            _timer2 += Time.deltaTime;
            if (_timer2>= 0.3f)
            {
                _greenBoxHere = false;
                _greenBoxObject = null;
                _timer2 = 0;
            }
            
        }
        if (_isGameOver)
        {
            print("!!! Game Over !!!");
            _isGameOver = false;
        }
    }

    #endregion

    #region Other Methods

    public bool CheckTileIsFull(Vector3 Position,Vector3 Position2)
    {
        Position = new Vector3((int)Position.x, (int)Position.y, (int)Position.z);     

        foreach (var item in _listAllObjects)
        {
            if (item.gameObject.tag == "Obstacle")
            {
                if (item.transform.position == Position)
                {
                    _checkTileIsFullBool = true;
                    break;
                }
            }

            if (item.gameObject.tag == "GreenBox")
            {
                if (item.transform.position == Position)
                {
                    _greenBoxHere = true;
                    _greenBoxObject = item;
                    break;
                }               
            }

        }
        if (_greenBoxHere)
        {
            foreach (var item in _listAllObjects)
            {
                if (item.gameObject.tag == "Obstacle")
                {
                    if (item.transform.position == Position2)
                    {
                        _checkTileIsFullBool = true;
                        _greenBoxHere = false;
                        break;
                    }
                }
            }
        }
        if (_greenBoxHere && !_checkTileIsFullBool)
        {            
            _greenBoxObject.GetComponent<GreenBox>().NextPosition = Position2;
            _greenBoxObject.GetComponent<GreenBox>().CanMove = true;           
        }
        return _checkTileIsFullBool;       
    }

    public void CheckFalledGreenBox()
    {
        for (int i = 0; i < _listGreenBox.Count; i++)
        {
            _listActiveGreenBox[i] = _listGreenBox[i].activeInHierarchy;
        }
        if (_listActiveGreenBox.Contains(true))
        {
            _isGameOver = false;
        }
        else
        {
            _isGameOver = true;
        }
    }

    #endregion

}
