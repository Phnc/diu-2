using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Usado para o movimento do personagem: andar para direita, esquerda, pular etc
    /// </summary>
    Rigidbody2D body;

    /// <summary>
    /// Usado para movimento do personagem: mudar a posição que ele olha para direita ou esquerda etc
    /// </summary>
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    SpriteRenderer renderer;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    /// <summary>
    /// Usado para acessar as condicoes e fazer transicao de estados
    /// </summary>
    Animator animator;

    /// <summary>
    /// Inicia como true pois o personagem comeca olhando para a direita
    /// </summary>
    bool facingRight = true;

    /// <summary>
    /// Determina se o jogador pode mover-se
    /// </summary>
    bool canMove = true;


    // Pulo

    /// <summary>
    /// O personagem começa fora do chao, por isso comeca como false
    /// </summary>
    bool grounded = false;

    /// <summary>
    /// Raio do ground checker que foi criado no personagem
    /// </summary>
    float groundCheckRadius = 0.2f;

    /// <summary>
    /// Usada para identificar a layer que queremos validar se foi tocada
    /// </summary>
    public LayerMask groundLayer;

    /// <summary>
    /// Localizacao do ground checker
    /// </summary>
    public Transform groundCheck;

    /// <summary>
    /// Funciona como a "velocidade de movimento" do pulo
    /// </summary>
    public float jumpPower;


    //Movimento

    /// <summary>
    /// Velocidade maxima de movimento do jogador
    /// </summary>
    public float maxSpeed;

    // Funciona quase como um construtor
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();

        Run();
    }

    private void Jump()
    {
        if (canMove && grounded && Input.GetAxis("Vertical") > 0)
        {
            // muda a propriedade para falso para fazer as transicoes
            animator.SetBool("IsGrounded", false);
            // zera o y, para garantir que o tamanho do pulo sempre eh o mesmo
            body.velocity = new Vector2(body.velocity.x, 0f);
            // Aplica um impulso no nosso personagem
            body.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            // sinaliza que o personagem nao esta no chao
            grounded = false;
        }

        // Verifica se o groundchecker esta sobreposto sobre a layer ground
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("IsGrounded", grounded);
    }

    private void Run()
    {
        // Será algum valor entre -1 e 1, que diz respeito a esquerda ou direita, respectivamente. Baseado no input do usuario
        float move = Input.GetAxis("Horizontal");

        if (canMove)
        {

            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }

            // mudar o valor da velocity de rigid body fara com que o personagem se movimente
            body.velocity = new Vector2(move * maxSpeed, body.velocity.y);

            // Faz a transicao de idle para running e vice-versa
            animator.SetFloat("MoveSpeed", Math.Abs(move));

        }
        else
        {
            body.velocity = new Vector2(0f, body.velocity.y);
            animator.SetFloat("MoveSpeed", 0f);
        }

    }

    void Flip()
    {
        // Muda a direcao para a qual o personagem esta olhando
        renderer.flipX = !renderer.flipX;
        facingRight = !facingRight;
    }

    public void ToggleCanMove()
    {
        canMove = !canMove;
    }
}
