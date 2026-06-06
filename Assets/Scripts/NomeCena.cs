using UnityEngine;
using TMPro;
using System.Collections;

// Mari, aqui eu fiz/copiei um sistema do nome da area aparecer, ai desculpa usar i.a, mas pedi uma ajuda pra adicionar uma animacao sem ser pelo animator.

public class AvisoRapido : MonoBehaviour
{
    public string nomeDaArea;
    public TextMeshProUGUI textoUI;
    public float tempoParado = 2f; // Tempo que ele parada na tela

    private RectTransform rectTransform;
    private Vector2 posicaoOriginal;
    private Vector2 posicaoForaDaTela;

     void Start()
    {
        if (textoUI == null) return;

        rectTransform = textoUI.GetComponent<RectTransform>();
        
        // Salva a posição certa do texto e calcula a posição lá no alto
        posicaoOriginal = rectTransform.anchoredPosition;
        posicaoForaDaTela = new Vector2(posicaoOriginal.x, posicaoOriginal.y + 300f);

        textoUI.text = nomeDaArea;
        StartCoroutine(AnimacaoFacil());
    }

    IEnumerator AnimacaoFacil()
    {
        // 1. Garante que o texto começa invisível e lá no alto
        SetTextoAlpha(0f);
        rectTransform.anchoredPosition = posicaoForaDaTela;
        textoUI.gameObject.SetActive(true);

        // 2. Desce e faz o Fade In ao mesmo tempo (Duração: 0.5 segundos)
        float tempo = 0f;
        while (tempo < 0.5f)
        {
            tempo += Time.deltaTime;
            float progresso = tempo / 0.5f;

            rectTransform.anchoredPosition = Vector2.Lerp(posicaoForaDaTela, posicaoOriginal, progresso);
            SetTextoAlpha(progresso); // Fade in vai de 0 a 1
            yield return null;
        }

        // 3. Fica parado na tela pelo tempo
        yield return new WaitForSeconds(tempoParado);

        // 4. Some com Fade Out Duração: 0.5 segundos
        tempo = 0f;
        while (tempo < 0.5f)
        {
            tempo += Time.deltaTime;
            float progresso = tempo / 0.5f;

            SetTextoAlpha(1f - progresso); 
            yield return null;
        }

        textoUI.gameObject.SetActive(false);
    }

    // Função auxiliar para mudar a transparência do texto de um jeito fácil
    private void SetTextoAlpha(float alpha)
    {
        Color cor = textoUI.color;
        cor.a = alpha;
        textoUI.color = cor;
    }
}