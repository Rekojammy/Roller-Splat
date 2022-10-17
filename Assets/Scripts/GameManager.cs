using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public ParticleSystem completedLevelParticle;
    public TextMeshProUGUI gameOverText;

    public float waitTime = 3f;

    public static bool isFinished;

    private GroundPieceController[] allGroundPieces;

    // Start is called before the first frame update
    void Start()
    {

        SetUpNewLevel();
    }


    private void SetUpNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPieceController>();
    }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetUpNewLevel();
    }

    public void CheckComplete()
    {
        isFinished = true;

        for (int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if (isFinished)
        {
            // Next Level

            completedLevelParticle.Play();
            gameOverText.gameObject.SetActive(true);

            StartCoroutine(EndLevel());
        }
    }


    IEnumerator EndLevel()
    {
        {
            yield return new WaitForSeconds(waitTime);

            NextLevel();
        }
    }
    

    private void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
}
