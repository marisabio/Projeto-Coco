using UnityEngine;
using TMPro;
using System.Collections;

public class AvisoFaseDaDireita : MonoBehaviour
{
    [Header("Configurações da UI")]
    public GameObject painelCompleto;    // Arraste o Painel_Area aqui
    public TextMeshProUGUI textoUI;      // Arraste o texto aqui
    
    [Header("Configurações do Texto")]
    public string nomeDaArea;
    public float tempoParado = 2f;       // Tempo que ele fica parado no centro

    private RectTransform rectTransformPainel;
    private CanvasGroup canvasGroupPainel;
    private Vector2 posicaoOriginal;
    private Vector2 posicaoForaDaTela;

    private void Start()
    {
        if (painelCompleto == null || textoUI == null)
        {
            Debug.LogWarning("Esqueceu de arrastar o Painel ou o Texto no Inspector!");
            return;
        }

        rectTransformPainel = painelCompleto.GetComponent<RectTransform>();
        
        // Garante o componente CanvasGroup para controlar a transparência
        canvasGroupPainel = painelCompleto.GetComponent<CanvasGroup>();
        if (canvasGroupPainel == null)
        {
            canvasGroupPainel = painelCompleto.AddComponent<CanvasGroup>();
        }

        // Salva a posição original (onde você montou na tela)
        posicaoOriginal = rectTransformPainel.anchoredPosition;

        // CALCULA A POSIÇÃO NA DIREITA: Somamos no eixo X para ele começar fora da tela pelo lado direito
        posicaoForaDaTela = new Vector2(posicaoOriginal.x + 600f, posicaoOriginal.y);

        // Define o texto
        textoUI.text = nomeDaArea;

        // Limpa e inicia a animação
        StopAllCoroutines();
        StartCoroutine(AnimacaoDireitaParaEsquerda());
    }

    IEnumerator AnimacaoDireitaParaEsquerda()
    {
        // 1. Preparação: Começa invisível e totalmente na direita
        canvasGroupPainel.alpha = 0f;
        rectTransformPainel.anchoredPosition = posicaoForaDaTela;
        painelCompleto.SetActive(true);

        // 2. ENTRADA: Vem da direita para o centro + Fade In (Duração: 0.5s)
        float tempo = 0f;
        while (tempo < 0.5f)
        {
            tempo += Time.deltaTime;
            float progresso = tempo / 0.5f;

            // Transição suave de movimento e transparência
            rectTransformPainel.anchoredPosition = Vector2.Lerp(posicaoForaDaTela, posicaoOriginal, progresso);
            canvasGroupPainel.alpha = progresso; 
            yield return null;
        }

        // Garante que terminou exatamente na posição certa
        rectTransformPainel.anchoredPosition = posicaoOriginal;
        canvasGroupPainel.alpha = 1f;

        // 3. ESPERA: Fica parado na tela
        yield return new WaitForSeconds(tempoParado);

        // 4. SAÍDA: Faz o Fade Out no lugar (Duração: 0.5s)
        tempo = 0f;
        while (tempo < 0.5f)
        {
            tempo += Time.deltaTime;
            float progresso = tempo / 0.5f;

            canvasGroupPainel.alpha = 1f - progresso; 
            yield return null;
        }

        // 5. Desativa o painel por completo para sumir da tela
        canvasGroupPainel.alpha = 0f;
        painelCompleto.SetActive(false);
    }
}