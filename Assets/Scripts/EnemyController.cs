using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public int buildSceneIndex;
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public GameObject enemy;
    public PlayerController player;
    public float fadeWait;

    private void Start()
    {
        if (fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1f);
        }
                
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            StartCoroutine(Fadeco());
        }
    }

    public IEnumerator Fadeco()
    {
        if (fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }

        yield return new WaitForSeconds(fadeWait);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(buildSceneIndex,LoadSceneMode.Additive);
        player.isMoving = false;

        if (enemy != null)
        {
            enemy.SetActive(false);
        }

        while (!asyncOperation.isDone)
        {
            yield return null;
        }



    }


}


/**

*/

/**
public float detectionRange = 5f;
public LayerMask playerLayer;

private bool playerDetected = false;

public int sceneIndex;

// Update is called once per frame
public void HandleUpdate()
{
    if (!playerDetected)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange, playerLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("Enemy in sight");
                StartCombat();
                break;
            }
        }
    }
}

void StartCombat()
{
    SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
}
**/