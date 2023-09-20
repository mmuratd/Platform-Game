using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levele : MonoBehaviour
{
    [SerializeField] float delayTime = 2f; 
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
        }
       
    }
 

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(delayTime);
        int currentSceeneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceeneIndex = currentSceeneIndex+1 ;

        if (nextSceeneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceeneIndex = 0;
        }
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceeneIndex);
    }


}
