using UnityEngine;
using TMPro;
using System.Collections;

public class NomeCena : MonoBehaviour
{
    public string nomeDaArea;
    public TextMeshProUGUI textoUI;
    public float tempoParado = 2f; 

    private RectTransform rectTransform;
    private Vector2 posicaoOriginal;
    private Vector2 posicaoForaDaTela;

     void Start()
    {
        if (textoUI == null) return;

        rectTransform = textoUI.GetComponent<RectTransform>();
        
        posicaoOriginal = rectTransform.anchoredPosition;
        posicaoForaDaTela = new Vector2(posicaoOriginal.x, posicaoOriginal.y + 300f);

        textoUI.text = nomeDaArea;
        StartCoroutine(Animacao());
    }

    IEnumerator Animacao()
    {
        SetTextoAlpha(0f);
        rectTransform.anchoredPosition = posicaoForaDaTela;
        textoUI.gameObject.SetActive(true);

        float tempo = 0f;
        while (tempo < 0.5f)
        {
            tempo += Time.deltaTime;
            float progresso = tempo / 0.5f;

            rectTransform.anchoredPosition = Vector2.Lerp(posicaoForaDaTela, posicaoOriginal, progresso);
            SetTextoAlpha(progresso); 
            yield return null;
        }

        yield return new WaitForSeconds(tempoParado);

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

    private void SetTextoAlpha(float alpha)
    {
        Color cor = textoUI.color;
        cor.a = alpha;
        textoUI.color = cor;
    }
}