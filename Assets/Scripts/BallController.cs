using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 15;

    public AudioClip clickSound;
    public AudioClip swipeSound;
    public AudioClip completedLevelSound;
    public AudioSource ballAudio;

    public ParticleSystem hitParticle;

    private bool isTravelling;
    private Vector3 travellingDirection;
    private Vector3 nextCollisionPosition;

    public int minSwipeRecognition = 500;
    private Vector2 swipePosCurrentFrame;
    private Vector2 swipePosLastFrame;
    private Vector2 currentSwipe;

    private Color solveColor;

    // Start is called before the first frame update
    void Start()
    {
        ballAudio = GetComponent<AudioSource>();
        solveColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isTravelling)
        {
            rb.velocity = speed * travellingDirection;
            ballAudio.PlayOneShot(swipeSound, 0.4f);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
        int i = 0;
        while(i < hitColliders.Length)
        {
            GroundPieceController ground = hitColliders[i].transform.GetComponent<GroundPieceController>();
            if (ground && !ground.isColored)
            {
                ground.ChangeColor(solveColor);
            }
            i++;
        }

        if (nextCollisionPosition != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition) < 1)
            {
                isTravelling = false;
                travellingDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        if (isTravelling)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            ballAudio.PlayOneShot(clickSound, 0.5f);
        }

        if (Input.GetMouseButton(0))
        {

            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (swipePosCurrentFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                if (currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }

                currentSwipe.Normalize();

                // Up/Down
                if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5)
                {
                    // Go Up/Down
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }
                 
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5)
                {
                    // Go Left/Right
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }
            }

            swipePosLastFrame = swipePosCurrentFrame;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ballAudio.PlayOneShot(clickSound, 0.5f);

            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }

        if (GameManager.isFinished)
        {
            Debug.Log("Finished");

            ballAudio.PlayOneShot(completedLevelSound, 0.6f);
        }

    }

    private void SetDestination(Vector3 direction)
    {
        travellingDirection = direction;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;
        }

        isTravelling = true;

    }
}
