using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    GameObject persistentObject;

    private void OnTriggerEnter(Collider other)
    {
        // Destroy persistent object
        persistentObject = GameObject.FindGameObjectWithTag("PersistentGameObject");
        if (persistentObject)
            Destroy(persistentObject);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
