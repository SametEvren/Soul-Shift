using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float moveSpeed = 5f;
    public float runMultiplier = 2f;
    public float walkAnimationThreshold = 0.1f;
    public float runAnimationThreshold = 0.5f;
    public float threshold;
    
    [SerializeField] private CharacterAnimatorController characterAnimatorController;
    private Vector3 _movement;
    private Rigidbody _playerRigidbody;
    private Animator _animator;
    private Quaternion _targetRotation;

    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        _movement = new Vector3(horizontal, 0f, vertical).normalized;

        if (_movement.magnitude < walkAnimationThreshold)
        {
            characterAnimatorController.PlayIdle();
        }
        else
        {
            if (threshold < runAnimationThreshold)
                characterAnimatorController.PlayWalk();
            else
                characterAnimatorController.PlayRun();
        }
    }

    void FixedUpdate()
    {
        threshold = Mathf.Clamp(threshold, 0, 1);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            threshold += Time.fixedDeltaTime;
            if (threshold > runAnimationThreshold)
            {
                _playerRigidbody.MovePosition(_playerRigidbody.position +
                                              _movement * moveSpeed * runMultiplier * Time.fixedDeltaTime);
            }
        }
        else
        {
            threshold -= Time.fixedDeltaTime;
            _playerRigidbody.MovePosition(_playerRigidbody.position + _movement * moveSpeed * Time.fixedDeltaTime);
        }
        
        if (_movement != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(_movement);
            _playerRigidbody.rotation = Quaternion.Slerp(_playerRigidbody.rotation, _targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}