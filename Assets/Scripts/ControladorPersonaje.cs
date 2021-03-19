using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorPersonaje : MonoBehaviour
{
    private Animator anim;
    private Arma arma;
    private Vector2 intro;
    private Vector2 centro;
    private bool puedoAtacar;
    private bool puedoRecoger;
    private GameObject objetivo;
    private GameObject armaArecoger;

    public float velocidad;
    public bool isDead = false;
    public int health = 100;
    public Scrollbar healthBar;
    public int danioCuerpoACuerpo = 1;

    void Start()
    {
        anim = GetComponent<Animator>();
        centro = new Vector2((int)Screen.width / 2, (int)Screen.height / 2);
        GameObject aux = BusquedaHijos.buscarHijoPorTag(gameObject, "Weapon");
        arma = aux != null ? aux.GetComponent<Arma>() : null;
        puedoAtacar = false;
        puedoRecoger = false;
        objetivo = null;
        armaArecoger = null;
    }

    void FixedUpdate()
    {
        if (!murio())
        {

            Rotar();

            float input_x = Input.GetAxisRaw("Horizontal");//Se captura movimiento horizontal

            float input_y = Input.GetAxisRaw("Vertical");//Se captura movimiento vertical

            bool isWalking = (Mathf.Abs(input_x) + Mathf.Abs(input_y)) > 0; //condicion directa

            anim.SetBool("isWalking", isWalking);//le paso al animator el booleano 

            if (isWalking)
            {//¿Esta Caminando?
                anim.SetFloat("x", input_x);//Le paso al animator el nuevo estado de la variable x  
                anim.SetFloat("y", input_y);
                input_x *= 3;
                input_y *= 3;
                Vector3 v = new Vector3(input_x, input_y, 0).normalized * Time.deltaTime;//se normaliza por el delta de tiempo para que el movimiento vaya acorde al tiempo de la escena
                v = v * velocidad;//Para poder encontrar personajes con diferentes velocidades
                transform.position += v;//Se altera la posicion del personaje
            }

            if (arma != null)
            {//si  tiene arma
                arma.reposicionar();
                if (arma.IsBobyToBody())
                {
                    if (Input.GetButtonDown("Fire1")) arma.ataque();
                }
                else
                {
                    if (Input.GetButton("Fire1"))
                    {
                        arma.ataque();
                        ComprobrarVisulizacion();
                    }
                }
                if (Input.GetButtonUp("Fire2"))//soltar el arma
                {
                    arma.gameObject.transform.SetParent(null);
                    arma = null;
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Invoke("ataque", 1f);
                    //Ataque cuerpo acuerpo 
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    //anim.SetBool("Attack", false);
                }
                if (Input.GetButtonUp("Fire2"))
                {//recoger arma
                    if (puedoRecoger)
                    {
                        armaArecoger.transform.SetParent(transform);
                        arma = BusquedaHijos.buscarHijoPorTag(gameObject, "Weapon").GetComponent<Arma>();
                        arma.reposicionar();
                        // RelativeJoint2D union = new RelativeJoint2D();


                    }
                }
            }
        }
    }

    void Rotar()
    {
        Vector2 entrada = Input.mousePosition;//Se captura donde esta el mouse  

        intro = new Vector2((entrada.x - centro.x), (entrada.y - centro.y));//Se traza un vector en la direccion del mouse

        float angulo = Vector2.Angle(Vector2.up, intro);//se calula el angulo dado de el vector arriba y la direccion del puntero 

        if (intro.x >= 0)
        {
            angulo = 180 + (180 - angulo);//debido a que el motor busca la forma mas corta de calcular el angulo se debe corregir cuando se pasa hacia la parte positiva de las X
        }

        Quaternion target = Quaternion.Euler(0, 0, angulo);//Define la rotacion a aplicar en el eje Z
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * velocidad);//Rota el personaje
    }

    public bool murio()
    {
        if (health <= 0)
        {
            isDead = true;
            anim.SetBool("IsDead", isDead);//el personaje murio, se comunica al animador para que se de la animacion de muerte
            if (arma != null)
            {
                arma.gameObject.transform.SetParent(null);
                arma = null;
            }

        }
        return isDead;
    }

    public bool haveWeapon()
    {
        return arma != null;
    }

    public void aplicarDanio(int danio)
    {
        health -= danio;
        healthBar.size = health / 100f;
    }



    public Arma getArma()
    {
        return arma;
    }

    void ataque()
    {
        //anim.SetBool("Attack",true);

        if (puedoAtacar)
        {
            objetivo.SendMessage("aplicarDanio", danioCuerpoACuerpo);
        }
    }

    void lanzarArma(GameObject armaALanzar)
    {
        Vector2 entrada = Input.mousePosition;//Se captura donde esta el mouse  
        intro = new Vector2((entrada.x - centro.x), (entrada.y - centro.y));//Se traza un vector en la direccion del mouse
        armaALanzar.transform.Translate(new Vector3(intro.x, intro.y, 0) * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            puedoAtacar = true;
            objetivo = other.gameObject;
        }
        if (other.gameObject.CompareTag("Weapon"))
        {
            if (other.transform.parent == null)
            {
                puedoRecoger = true;
                armaArecoger = other.gameObject;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            puedoAtacar = false;
            objetivo = null;
        }
        if (other.gameObject.CompareTag("Weapon"))
        {
            puedoRecoger = false;
            armaArecoger = null;
        }
    }


    void ComprobrarVisulizacion()
    {
        Vector2 entrada = Input.mousePosition;
        intro = new Vector2((entrada.x - centro.x), (entrada.y - centro.y));//Se traza un vector en la direccion del mouse
        RaycastHit2D hit = Physics2D.Raycast(transform.position, intro, 20f);
        if (hit.collider)
        {
            Debug.Log("Hit the collidable object " + hit.collider.name);
            // Debug.Log("cordenadas: x=" + hit.point.x + " y=" + hit.point.y);
            Debug.DrawRay(this.transform.position, intro, Color.red, 3f);
        }

    }


}
