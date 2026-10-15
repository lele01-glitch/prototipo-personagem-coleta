using UnityEngine;

using TMPro; // Necessário para usar TextMeshPro. Caso use o Text antigo da Unity, altere para 'using UnityEngine.UI;'

[RequireComponent(typeof(CharacterController))]

public class PlayerMov : MonoBehaviour

{

    [Header("Movimentação")]

    public float moveSpeed = 5f;

    public float velocidadegiro = 720f;

    [Header("Pulo e Gravidade")]

    public float jumpHeight = 2f;

    public float gravity = -20f;

    [Header("Interface e Pontuação")]

    public TextMeshProUGUI countText; // Arraste o seu objeto de texto aqui na Unity

    public GameObject winTextObject;  // Arraste o objeto "Você Venceu!" aqui na Unity

    public int pontosParaVencer = 12;

    private CharacterController controller;

    private float VelocidadeY;

    private int count;

    void Start()

    {

        controller = GetComponent<CharacterController>();

        // Inicializa a pontuação e a interface

        count = 0;

        SetCountText();

        if (winTextObject != null)

        {

            winTextObject.SetActive(false); // Esconde a mensagem de vitória no início

        }

    }

    void Update()

    {

        // Entrada dos eixos

        float h = Input.GetAxis("Horizontal");

        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0f, v);

        // Normaliza o vetor para não andar mais rápido na diagonal

        if (move.magnitude > 1f)

        {

            move.Normalize();

        }

        // Rotação suave do personagem na direção do movimento

        if (move != Vector3.zero)

        {

            Quaternion targetRotation = Quaternion.LookRotation(move);

            transform.rotation = Quaternion.RotateTowards(

                transform.rotation,

                targetRotation,

                velocidadegiro * Time.deltaTime

            );

        }

        // Lógica de chão e pulo

        if (controller.isGrounded)

        {

            if (VelocidadeY < 0f)

            {

                VelocidadeY = -2f; // Mantém o personagem colado ao chão

            }

            if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))

            {

                VelocidadeY = Mathf.Sqrt(jumpHeight * -2f * gravity);

            }

        }

        // Aplica a gravidade continuamente

        VelocidadeY += gravity * Time.deltaTime;

        // Combina movimento horizontal com o movimento vertical (pulo/gravidade)

        Vector3 movimentoFinal = move * moveSpeed;

        movimentoFinal.y = VelocidadeY;

        // Move o personagem

        controller.Move(movimentoFinal * Time.deltaTime);

    }

    // Chamado quando o jogador entra em um colisor marcado como Trigger

    private void OnTriggerEnter(Collider other)

    {

        if (other.gameObject.CompareTag("PickUp"))

        {

            other.gameObject.SetActive(false);  // Oculta o item coletado

            count = count + 1;                  // Adiciona ponto

            SetCountText();

        }

    }

    // Atualiza o texto na tela e verifica a condição de vitória

    void SetCountText()

    {

        if (countText != null)

        {

            countText.text = "Pontos: " + count.ToString();

        }

        if (count >= pontosParaVencer)

        {

            if (winTextObject != null)

            {

                winTextObject.SetActive(true); // Exibe a tela de vitória

            }

        }

    }

}
