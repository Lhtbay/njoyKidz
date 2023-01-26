using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject HaveLineRenderer;

    public bool HavePlug = false;

    public static GameManager Instance;

    [Header("Prefabs Settings")]
    [SerializeField] private GameObject _obstaclePrefab;  
    [SerializeField] private GameObject _greenBoxPrefab;   
    [SerializeField] private GameObject _lineRendererPrefabs;
    [SerializeField] private GameObject _lineRendererParent;
    [SerializeField] private GameObject _obstaclesParent;
    [SerializeField] private GameObject _greenBoxParent;
    [SerializeField] private GameObject _plugsAndOutletParent;
    [SerializeField] private GameObject _coinsParent;

    [Header("List Settings")]
    [SerializeField] private List<GameObject> _listAllObjects;
    [SerializeField] private List<GameObject> _listCoins;
    [SerializeField] private List<GameObject> _listGreenBox;
    [SerializeField] private List<GameObject> _listLineRenderer;
    [SerializeField] private List<GameObject> _listPlugAndOutlet;  
    private List<bool> _listActiveGreenBox;
    private List<bool> _listComplatedPlugAndOutlet;

    private GameObject _greenBoxObject;
    private GameObject _plugObject;

    private bool _checkTileIsFullBool,_greenBoxHere = false;
    private bool _greenBoxInstantiateCheck, _obstacleInstantiateCheck,_coinsInstantiateCheck,
        _plugAndOutletInstantiateCheck = false;
    
    private bool _isLevelCompleted,_isPlugCompleted,_isWrongPlug = false;

    private float _timer,_timer2,_timer3 = 0;

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
        _listAllObjects = new List<GameObject>();    
        _listGreenBox = new List<GameObject>();
        _listLineRenderer = new List<GameObject>();        
        _listActiveGreenBox = new List<bool>();
        _listComplatedPlugAndOutlet = new List<bool>();
     
        // Instantiate objects

        for (int i = 0; i < 10; i++)
        {
            GameObject newObstacle = Instantiate(_obstaclePrefab, new Vector3(Random.Range(-7, 7),Random.Range(-5, 5),0), Quaternion.identity);

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
            GameObject newGreenBox = Instantiate(_greenBoxPrefab, new Vector3(Random.Range(-7, 7), Random.Range(-5, 5), 0), Quaternion.identity);

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
            GameObject newCoins = Instantiate(_listCoins[i], new Vector3(Random.Range(-7, 7), Random.Range(-5, 5), 0), Quaternion.identity);
            _coinsInstantiateCheck = false;

            newCoins.transform.parent = _coinsParent.transform;

            while (!_coinsInstantiateCheck)
            {
                if (newCoins.transform.position == new Vector3(0, 0, 0))
                {
                    newCoins.transform.position = new Vector3(Random.Range(-7, 7), Random.Range(-5, 5), 0);
                }
                else
                {
                    foreach (var item in _listAllObjects)
                    {
                        if (item.transform.position == newCoins.transform.position)
                        {
                            newCoins.transform.position = new Vector3(Random.Range(-7, 7), Random.Range(-5, 5), 0);                           
                        }
                        else
                        {
                            _coinsInstantiateCheck = true;
                        }
                    }
                }
                
            }
        }

        for (int i = 0; i < 6; i++)
        {
            GameObject newPlugOutlet = Instantiate(_listPlugAndOutlet[i], new Vector3(Random.Range(-7, 7), Random.Range(-5, 5), 0), Quaternion.identity);

            newPlugOutlet.transform.parent = _plugsAndOutletParent.transform;

            _listAllObjects.Add(newPlugOutlet);

            while (!_plugAndOutletInstantiateCheck)
            {
                if (newPlugOutlet.transform.position == new Vector3 (0,0,0))
                {
                    newPlugOutlet.transform.position = new Vector3(Random.Range(-7, 7), Random.Range(-5, 5), 0);
                }
                else
                {
                    foreach (var item in _listAllObjects)
                    {
                        if (item.transform.position == newPlugOutlet.transform.position)
                        {
                            newPlugOutlet.transform.position = new Vector3(Random.Range(-7, 7), Random.Range(-5, 5), 0);        
                        }
                        else
                        {
                            _plugAndOutletInstantiateCheck = true;
                            
                        }
                    }
                }
            }
            
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject newLineRenderer = Instantiate(_lineRendererPrefabs,new Vector3(0,0,0),Quaternion.identity);

            newLineRenderer.transform.parent = _lineRendererParent.transform;
            newLineRenderer.SetActive(false);
            _listLineRenderer.Add(newLineRenderer);
            _listComplatedPlugAndOutlet.Add(false);
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
        if (_isLevelCompleted)
        {
            print("!!! Level Completed !!!");
            _isLevelCompleted = false;
        }
        if (_isPlugCompleted)
        {
            print("!!! Plugs Level Completed !!!");
            _isPlugCompleted = false;
        }
        if (_isWrongPlug)
        {
            _timer3 += Time.deltaTime;
            if (_timer3 >= 0.2f)
            {
                _isWrongPlug = false;
                _timer3 = 0;
            }
        }
    }

    #endregion

    #region Other Methods

    public bool CheckTileIsFull(Vector3 Position,Vector3 Position2)
    {
        //Position = new Vector3((int)Position.x, (int)Position.y, (int)Position.z);     

        foreach (var item in _listAllObjects)
        {
            if (item.tag == "Obstacle")
            {
                if (item.transform.position == Position)
                {
                    _checkTileIsFullBool = true;
                    break;
                }
            }

            if (item.tag == "GreenBox")
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
                if (item.tag == "Obstacle")
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

    public void CheckPlug(Vector3 Position)
    {
        //Position = new Vector3((int)Position.x, (int)Position.y, (int)Position.z);
        if (!HavePlug)
        {
            foreach (var item in _listAllObjects)
            {
                if (item.transform.position == Position && item.tag == "Plug")
                {
                    HavePlug = true;
                    _plugObject = item.transform.GetChild(0).gameObject;                
                }
            }
            if (HavePlug)
            {
                foreach (var item in _listLineRenderer)
                {
                    if (!item.activeInHierarchy)
                    {
                        item.GetComponent<LineRenderer>().SetColors(_plugObject.GetComponent<SpriteRenderer>().color, _plugObject.GetComponent<SpriteRenderer>().color);
                        item.GetComponent<LineRenderer>().SetPosition(0, _plugObject.transform.position);
                        HaveLineRenderer = item;
                        item.SetActive(true);
                        break;
                    }
                }
            }
            
        }
        
    }

    public void CheckOutlet(Vector3 Position)
    {
        //Position = new Vector3((int)Position.x,(int)Position.y,(int)Position.z);
        if (HavePlug)
        {
            foreach (var item in _listAllObjects)
            {
                if (item.transform.position == Position && item.tag == "Outlet")
                {                  
                    foreach (var item2 in _listLineRenderer)
                    {
                        if (item2.activeInHierarchy)
                        {
                            if (item2.GetComponent<LineRenderer>().startColor == item.transform.GetChild(0).GetComponent<SpriteRenderer>().color)
                            {
                                HaveLineRenderer = null;
                                HavePlug = false;
                                CheckPlugAndOutletCompleted();
                                _isWrongPlug = true;
                            }
                        }
                    }
                   
                }
                if (!_isWrongPlug)
                {
                    print("!!! Wrong Plug !!!");
                }
            }
        }
        
        
    }

    public void CheckFalledGreenBox()
    {
        for (int i = 0; i < _listGreenBox.Count; i++)
        {
            _listActiveGreenBox[i] = _listGreenBox[i].activeInHierarchy;
        }
        if (_listActiveGreenBox.Contains(true))
        {
            _isLevelCompleted = false;
        }
        else
        {
            _isLevelCompleted = true;
        }
    }

    private void CheckPlugAndOutletCompleted()
    {
        for (int i = 0; i < _listLineRenderer.Count; i++)
        {
            _listComplatedPlugAndOutlet[i] = _listLineRenderer[i].activeInHierarchy;
        }
        if (_listComplatedPlugAndOutlet.Contains(false))
        {
            _isPlugCompleted = false;
        }
        else 
        {
            _isPlugCompleted = true;
        }
        
    }

    #endregion

}
