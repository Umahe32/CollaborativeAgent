using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public bool isEvolutivo = false;
    public AlgoritmoEvolutivo Evolutivo;

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    //PlayerShooting playerShooting;
    bool isDead;
    bool damaged;
    public bool EsEntrenador;

    void Awake ()
    {

            anim = GetComponent<Animator>();
            playerAudio = GetComponent<AudioSource>();

        playerMovement = GetComponent <PlayerMovement> ();
        //playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
        if (isEvolutivo)
        Evolutivo = GameObject.FindGameObjectWithTag("Evolutivo").GetComponent<AlgoritmoEvolutivo>();
    }


    void Update ()
    {
        if (!EsEntrenador) { 
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
           damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        }
        damaged = false;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;
        if (amount < 0)
        {
            Debug.Log("esta es mi salud actual:" + currentHealth);
        }
        if (!isDead)
        {
            currentHealth -= amount;
        }
        if (amount < 0)
        {
            Debug.Log("esta es mi salud despues de curar:" + currentHealth);
        }

        if (!EsEntrenador)
        {
            healthSlider.value = currentHealth;
            playerAudio.Play();
        }

        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        //playerShooting.DisableEffects ();

        //anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        if (!EsEntrenador)
        {
            playerAudio.Play();
        }
        playerMovement.enabled = false;
        //playerShooting.enabled = false;
        //Evolutivo.Invoke("CambiarCromosoma", 0f);
        //if (isEvolutivo)
           // Evolutivo.CambiarCromosoma();
    }


  /*  public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }*/
}
