using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
    //[SerializeField] Animator myAnimator;
    [SerializeField] float thrustSpeed = 5f;
    [SerializeField] float rotateSpeed = 5f;

    private enum GameState { Playing, Paused };
    private GameState currentGameState;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    bool canMove;

    private void Start()
    {
        canMove = true;
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "WorldConfine":
                StartCoroutine(BounceOff(collision.gameObject));
                canMove = true;
                myRigidbody.freezeRotation = false;
                break;
            default:
                break;
        }
    }

    private IEnumerator BounceOff(GameObject confine)
    {
        canMove = false;

        //myRigidbody.velocity = Vector2.down;

        ////if (confine.transform.position.y > 0)
        ////{
        ////    myRigidbody.velocity = Vector2.up;
        ////}
        ////else if (confine.transform.position.y < 0)
        ////{
        ////    myRigidbody.velocity = Vector2.down;
        ////}

        //if (confine.transform.position.x > 0)
        //{
        //    myRigidbody.velocity = Vector2.left;
        //}
        //else if (confine.transform.position.x < 0)
        //{
        //    myRigidbody.velocity = Vector2.right;
        //}

        myRigidbody.freezeRotation = true;

        yield return new WaitForSecondsRealtime(0.5f);
    }

    void OnMove(InputValue inputValue)
    {
        if (currentGameState == GameState.Paused)
        {
            return;
        }
        moveInput = inputValue.Get<Vector2>();
    }

    private void PlayerInput()
    {
        if (!canMove || currentGameState == GameState.Paused) { return; }

        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (moveInput.y > 0)
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void ProcessRotation()
    {
        if (moveInput.x < 0)
        {
            RotateLeft();
        }
        else if (moveInput.x > 0)
        {
            RotateRight();
        }
        else
        {
            StopRotation();
        }
    }

    private void StartThrusting()
    {
        //if (!thrustParticles.isPlaying)
        //{
        //    thrustParticles.Play();
        //}
        myRigidbody.AddRelativeForce(Vector2.up * thrustSpeed * Time.deltaTime);
    }

    private void StopThrusting()
    {
        //thrustParticles.Stop();
    }

    public void DisableMovement()
    {
        this.enabled = false;
    }

    private void RotateRight()
    {
        //if (!leftThrustParticles.isPlaying)
        //{
        //    leftThrustParticles.Play();
        //}
        //rightThrustParticles.Stop();
        CalculateRotation(rotateSpeed);
    }

    private void RotateLeft()
    {
        //if (!rightThrustParticles.isPlaying)
        //{
        //    rightThrustParticles.Play();
        //}
        //leftThrustParticles.Stop();
        CalculateRotation(-rotateSpeed);
    }

    private void CalculateRotation(float rotateSpeed)
    {
        myRigidbody.freezeRotation = true;
        transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
        myRigidbody.freezeRotation = false;
    }

    private void StopRotation()
    {
        //leftThrustParticles.Stop();
        //rightThrustParticles.Stop();
    }

    public void PauseGame()
    {
        currentGameState = GameState.Paused;
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        currentGameState = GameState.Playing;
        Time.timeScale = 1f;
    }
}
