using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private HealthController healthController;
    private Animator animator;
    private GameObject gameOverCanvas;
    private GameObject playerHUDCanvas;

    private void Awake()
    {
        healthController = GetComponent<HealthController>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameOverCanvas = GameObject.FindGameObjectWithTag("GameOver");
        gameOverCanvas.SetActive(false);
        
        playerHUDCanvas = GameObject.FindGameObjectWithTag("PlayerHUD");

        if (healthController)
        {
            healthController.OnHealthChange += HealthChange;
            if (playerHUDCanvas)
                    healthController.healthUI = playerHUDCanvas.transform.GetChild(0).GetComponent<HealthBarUI>();
        }
    }

    private void HealthChange(object sender, HealthController.OnHealthChangeEventArgs e)
    {
        if (e.currentHealth <= 0)
        {
            healthController.OnHealthChange -= HealthChange;

            animator.SetBool("IsDead", true);
            DisablePlayerInput();

            StartCoroutine(GameOverWait());
        }
    }

    private void DisablePlayerInput()
    {
        PlayerMovement playerMovement;
        if(gameObject.TryGetComponent<PlayerMovement>(out playerMovement))
            playerMovement.enabled = false;
        
        WeaponBehaviour weaponBehaviour;
        if (gameObject.TryGetComponent<WeaponBehaviour>(out weaponBehaviour))
            weaponBehaviour.enabled = false;
    }

    IEnumerator GameOverWait()
    {
        yield return new WaitForSeconds(3f);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.None; 
        if (gameOverCanvas)
            gameOverCanvas.SetActive(true);
    }
}
