using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEndBehaviour : MonoBehaviour
{
    [Tooltip("How much time to wait before destroing" + "the tile after reaching the end")]
    public float destroyTime = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBehaviour>())
        {
            var gm = GameObject.FindObjectOfType<GameManager>();
            gm.SpawnNextTile();

            Destroy(transform.parent.gameObject, destroyTime);
        }
    }
}
