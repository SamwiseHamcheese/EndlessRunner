using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleBehaviour : MonoBehaviour
{
    [Tooltip("How long to wait before restating the game")]
    public float waitTime = 2.0f;

    [Tooltip("Explosion effect to play when tapped")]
    public GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerBehaviour>())
        {
            Destroy(collision.gameObject);
            PlayerTouch();
            Invoke("ResetGame", waitTime);
        }
    }
    private void ResetGame()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
    private void PlayerTouch()
    {
        if(explosion != null)
        {
            var particles = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(particles, 1.0f);
            this.gameObject.SetActive(false);
        }
    }
}
