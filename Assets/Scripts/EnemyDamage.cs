using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    /// <summary>
    /// Quantidade de dano ao personagem;
    /// </summary>
    public float damage;

    /// <summary>
    /// Frequencia com a qual o objeto causa dano
    /// </summary>
    public float damageRate;

    /// <summary>
    /// Proxima vez que o player pode tomar dano
    /// </summary>
    float nextDamage;

    /// <summary>
    /// A forca do knockback do objeto
    /// </summary>
    public float pushBackForce;

    void Start()
    {
        // Pode tomar dano assim que o objeto eh criado
        nextDamage = Time.time;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && nextDamage < Time.time)
        {
            // causa dano no personagem
            collision.gameObject.GetComponent<PlayerHealth>().AddDamage(damage);
            // seta a proxima vez que o player pode tomar dano
            nextDamage = Time.time + damageRate;
            PushBack(collision.transform);
        }
    }

    void PushBack(Transform pushed)
    {
        // Knockback do personagem sempre tera o mesmo valor, por isso o normalized
        var direction = new Vector2(0, (pushed.position.y - transform.position.y)).normalized;
        direction *= pushBackForce;

        var pushRigidBody = pushed.gameObject.GetComponent<Rigidbody2D>();

        // zera a velocidade em todas as direcoes
        pushRigidBody.velocity = Vector2.zero;

        pushRigidBody.AddForce(direction, ForceMode2D.Impulse);
    }

}
