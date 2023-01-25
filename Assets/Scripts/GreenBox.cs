using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBox : MonoBehaviour
{
    public bool CanMove = false;
    public Vector3 NextPosition;

    private bool _oneTime = true;
    private bool _boxFall = false;

    private float _timer,_timer2 = 0;
    private float _moveTime = 0.5f;
    private float _percentToMove = 0;

    private Vector3 _startPosition, _difference;

    private Animator _thisAnimator;

    void Start()
    {
        CanMove = false;
        _thisAnimator = this.GetComponent<Animator>();
        transform.position = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        MoveSysthem();
        CheckAmIOnTile();
    }

    private void OnDisable()
    {
        GameManager.Instance.CheckFalledGreenBox();
    }

    #region Other Methods

    private void CheckAmIOnTile()
    {
        if (transform.position.x < -7.5f ||
            transform.position.x > 7.5f ||
            transform.position.y > 5.5f ||
            transform.position.y < -5.5f)
        {
            _thisAnimator.SetBool("GameOver", true);
            _boxFall = true;
        }
        if (_boxFall)
        {
            _timer2 += Time.deltaTime;
            if (_timer2 >= 0.6f)
            {
                _timer2 = 0;               
                this.gameObject.SetActive(false);                               
            }
        }
    }

    private void MoveSysthem()
    {
        if (CanMove)
        {
            if (_oneTime)
            {
                _difference = NextPosition - transform.position;
                _oneTime = false;
            }

            _timer += Time.deltaTime;
            _percentToMove = _timer / _moveTime;
            transform.position = _startPosition + _difference * _percentToMove;

            if (_timer >= 0.5f)
            {                
                CanMove = false;
                _oneTime = true;
                _timer = 0;
            }
        }
        else
        {
            transform.position = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
            _startPosition = transform.position;
            NextPosition = _startPosition;
        }
    }

    #endregion

}
