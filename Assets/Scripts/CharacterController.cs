using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Vector3 _moveRight;
    [SerializeField] private Vector3 _moveFront;
    private Vector3 _nextPosition;
    private Vector3 _startPosition;
    private Vector3 _difference;
   
    private bool _canMove,_canDiagonalMove = false;
    private bool _canPressA,_canPressS,_canPressD,_canPressW = true;

    private float _timer,_timer2 = 0;
    private float _moveTime = 0.5f;
    private float _percentToMove;

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
    }

    private void FixedUpdate()
    {
        CheckGameOver();
    }

    #region Controls Methods

    private void ControllerStsythem()
    {
        if (!_canMove)
        {
            _startPosition = transform.position;

            if (Input.GetKey(KeyCode.A))
            {
                if (_canPressA)
                {
                    _nextPosition -= _moveRight;
                    _canPressA = false;
                }                
                _canDiagonalMove = true;                                             
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (_canPressS)
                {
                    _nextPosition -= _moveFront;
                    _canPressS = false;
                }              
                _canDiagonalMove = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (_canPressD)
                {
                    _nextPosition += _moveRight;
                    _canPressD = false;
                }               
                _canDiagonalMove = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                if (_canPressW)
                {
                    _nextPosition += _moveFront;
                    _canPressW = false;
                }               
                _canDiagonalMove = true;
            }
            if (_canDiagonalMove && _timer2 >= 0.1f)
            {
                ControllerCheckSysthem();
            }
        }
       
    }

    private void ControllerCheckSysthem()
    {       
        _difference = _nextPosition - transform.position;

        _canMove = true;
        _canPressA = true;
        _canPressD = true;
        _canPressS = true;
        _canPressW = true;

        _canDiagonalMove = false;
        
        _timer2 = 0;
        _timer = 0;

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
                _canMove = false;
                _timer = 0;
            }

            
        }
    }


    #endregion

}