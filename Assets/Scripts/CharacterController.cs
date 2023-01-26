using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Vector3 _moveRight;
    [SerializeField] private Vector3 _moveFront;
    private Vector3 _nextPosition,_nextPosition2;
    private Vector3 _startPosition;
    private Vector3 _difference;
   
    private bool _canMove,_canDiagonalMove = false;
    private bool _canPressA,_canPressS,_canPressD,_canPressW = true;
    private bool _coinCompletedTextOneTime,_gameOverTextOneTime = false;

    private float _timer,_timer2 = 0;
    private float _moveTime = 0.5f;
    private float _percentToMove;

    private int _coinCount = 1;

    private Animator _thisAnimator;

    #endregion

    private void Start()
    {
        _thisAnimator = transform.GetComponent<Animator>();

        _startPosition = transform.position;
        _timer = 0;
    }

    private void Update()
    {
        ControllerStsythem();
        DiagonalCheckSysthem();
        MoveSysthem();
        LineRendererSysthem();
    }

    private void FixedUpdate()
    {
        if (!_gameOverTextOneTime)
        {
            CheckGameOver();
        }
        if (!_coinCompletedTextOneTime)
        {
            CheckCoins();
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin"+_coinCount.ToString())
        {
            collision.gameObject.SetActive(false);
            _coinCount++;
        }
    }

    #region Controls Methods

    private void ControllerStsythem()
    {
        if (!_canMove)
        {
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));

            _startPosition = transform.position;

            if (Input.GetKey(KeyCode.A))
            {
                if (_canPressA)
                {                   
                    _nextPosition -= _moveRight;
                    _nextPosition2 = _nextPosition - _moveRight;
                    _canPressA = false;
                }                
                _canDiagonalMove = true;                                             
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (_canPressS)
                {
                    _nextPosition -= _moveFront;
                    _nextPosition2 = _nextPosition - _moveFront;
                    _canPressS = false;
                }              
                _canDiagonalMove = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (_canPressD)
                {
                    _nextPosition += _moveRight;
                    _nextPosition2 = _nextPosition + _moveRight;
                    _canPressD = false;
                }               
                _canDiagonalMove = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                if (_canPressW)
                {
                    _nextPosition += _moveFront;
                    _nextPosition2 = _nextPosition + _moveFront;
                    _canPressW = false;
                }               
                _canDiagonalMove = true;
            }           
            if (_canDiagonalMove && _timer2 >= 0.1f)
            {
                if (!GameManager.Instance.CheckTileIsFull(_nextPosition, _nextPosition2))
                {
                    _canMove = true;
                    ControllerCheckSysthem();
                    print("Current Position : "+_nextPosition);                                
                }
                else
                {
                    print("!!! Cant Move Because Tile Is Full !!!");
                    
                    _canDiagonalMove = false;
                    _canMove = false;

                    _canPressA = true;
                    _canPressD = true;
                    _canPressS = true;
                    _canPressW = true;

                    _nextPosition = transform.position;
                    _timer2 = 0;                   
                }
            }         
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _startPosition = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
            GameManager.Instance.CheckPlug(_startPosition);
            GameManager.Instance.CheckOutlet(_startPosition);
        }
       
    }

    private void LineRendererSysthem()
    {
        if (GameManager.Instance.HavePlug)
        {          
            GameManager.Instance.HaveLineRenderer.GetComponent<LineRenderer>().SetPosition(GameManager.Instance.HaveLineRenderer.GetComponent<LineRenderer>().positionCount - 1, transform.position); 
        }     
    }

    private void ControllerCheckSysthem()
    {       
        _difference = _nextPosition - transform.position;
        
        _canPressA = true;
        _canPressD = true;
        _canPressS = true;
        _canPressW = true;

        _canDiagonalMove = false;
        
        _timer2 = 0;        
    }

    private void DiagonalCheckSysthem()
    {
        if (_canDiagonalMove)
        {
            _timer2 += Time.deltaTime;
        }
        
    }

    private void CheckGameOver()
    {
        if (transform.position.x < -7.5f || 
            transform.position.x > 7.5f  || 
            transform.position.y > 5.5f  || 
            transform.position.y < -5.5f)
        {
            _thisAnimator.SetBool("GameOver",true);
            print("!!! Game Over !!!");

            _gameOverTextOneTime = true;
        }
      
    }

    private void CheckCoins()
    {
        if (_coinCount > 5)
        {
            print("!!! Coins Level Is Completed !!!");
            _coinCompletedTextOneTime = true;
        }
    }

    #endregion
    #region Other Methods

    private void MoveSysthem()
    {
        if (_canMove)
        {          
            _timer += Time.deltaTime;
            _percentToMove = _timer / _moveTime;
            transform.position = _startPosition + _difference * _percentToMove;

            if (_timer >= 0.5f)
            {
                if (GameManager.Instance.HavePlug)
                {
                    GameManager.Instance.HaveLineRenderer.GetComponent<LineRenderer>().positionCount++;
                    GameManager.Instance.HaveLineRenderer.GetComponent<LineRenderer>().SetPosition(GameManager.Instance.HaveLineRenderer.GetComponent<LineRenderer>().positionCount-1, _nextPosition);     
                }
                _canMove = false;               
                _timer = 0;               
            }          
        }
    }

    #endregion

}
