using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;
    public bool EsEntranador = false;

    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;



        currentHealth -= amount;
        if(!EsEntranador) {
            enemyAudio.Play();
            hitParticles.transform.position = hitPoint;
            hitParticles.Play();
        }
        if (currentHealth <= 0)
        {
            Death ();
        }
        //CompanionEvolutivo e= resposable.GetComponent<"">
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;
        if (!EsEntranador)
        {
            anim.SetTrigger("Dead");

            enemyAudio.clip = deathClip;
            enemyAudio.Play();

        }
        else {
            StartSinking();
        }
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        //ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }


    //******************************+
    
    public bool Murio() {
        return isDead;
    }
}
