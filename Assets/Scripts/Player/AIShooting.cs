using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIShooting : MonoBehaviour
{


    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;


    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    CompanionEvolutivo companionEvolutivo;
    Gimnasio gym;

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }
    void Start()
    {
        companionEvolutivo = GameObject.FindGameObjectWithTag("Companion").GetComponent<CompanionEvolutivo>();
        /*if (GameObject.FindGameObjectWithTag("AmbienteEvolutivo").GetComponent<Gimnasio>() != null)
        {
            gym = GameObject.FindGameObjectWithTag("AmbienteEvolutivo").GetComponent<Gimnasio>();
        }
        else {
            gym = null;
        }*/
        gym = null;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot()
    {
        timer = 0f;

       // gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                if (gym == null)
                {
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                }
                else if(transform.parent.parent.parent==enemyHealth.transform.parent){
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                    if (companionEvolutivo != null)
                    {
                        if (enemyHealth.Murio())
                        {
                            companionEvolutivo.aplicarPuntos((int)CompanionEvolutivo.Categoria.ELIMINACIONES);
                        }
                        else
                        {
                            companionEvolutivo.aplicarPuntos((int)CompanionEvolutivo.Categoria.PELIGROS);
                        }
                    }
                }
                
            }
            else {
                companionEvolutivo.aplicarCastigo((int)CompanionEvolutivo.Categoria.PELIGROS);
            }
            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }


    //*********** comienzan las modificaciones****************/
    public void dispararIA()
    {
        if (timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }
    }
}
