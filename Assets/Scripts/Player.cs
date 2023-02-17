using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private bool _thursting;

    private float _turnDirection;

    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;

    public Bullet bulletPrefab;

    private Bounds screenBounds;

    private float warpOffset = 0.3f;

    private void Awake()
    {
        // Player class is attached to the Player component (with a RigidBody2D element), this will grab that reference 
        _rigidbody = GetComponent<Rigidbody2D>();
        
        screenBounds = new Bounds();
        screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(Vector3.zero));
        screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)));  
    }

    // Update is called once per frame (variable - dependent on frame rate of game)
    private void Update()
    {
        if (!PauseMenu.isPaused)
        {
            _thursting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _turnDirection = 1.0f;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                _turnDirection = -1.0f;
            }
            else
            {
                _turnDirection = 0.0f;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Shoot();
            }   
        }
    }

    // FixedUpdate is called on a fixed time interval (for physics related code)
    private void FixedUpdate()
    {
        // Player boundry warping
        if (_rigidbody.position.x > screenBounds.max.x + warpOffset) {
            _rigidbody.position = new Vector2(screenBounds.min.x - warpOffset, _rigidbody.position.y);
        } else if (_rigidbody.position.x < screenBounds.min.x - warpOffset) {
            _rigidbody.position = new Vector2(screenBounds.max.x + warpOffset, _rigidbody.position.y);
        } else if (_rigidbody.position.y > screenBounds.max.y + warpOffset) {
            _rigidbody.position = new Vector2(_rigidbody.position.x, screenBounds.min.y - warpOffset);
        } else if (_rigidbody.position.y < screenBounds.min.y - warpOffset) {
            _rigidbody.position = new Vector2(_rigidbody.position.x, screenBounds.max.y + warpOffset);
        }

        if (_thursting)
        {
            _rigidbody.AddForce(this.transform.up * thrustSpeed);
        }

        if (_turnDirection != 0.0f)
        {
            _rigidbody.AddTorque(_turnDirection * turnSpeed);
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }

}
