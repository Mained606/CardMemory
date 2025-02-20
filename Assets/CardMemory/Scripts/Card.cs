using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image frontImage;
    public Image backImage;
    public CardData cardData;
    public bool flipped = false;
    
    // 마우스 클릭 시 카드 플립(토글)
    public void FlipCard()
    {
        if (flipped || GameManager.instance.isProcessingMatch) return; // 정답 확인 중일 때 클릭 방지
    
        flipped = true; // 한 번만 변경
    
        frontImage.enabled = flipped;
        backImage.enabled = !flipped;
    
        GameManager.instance.SelectedCard(this.gameObject);
    }

    
    // 카드 매니저에서 데이터를 전달받는 함수
    public void SetCardData(CardData cardData)
    {
        this.cardData = cardData;
        frontImage.sprite = cardData.frontSprite;
        backImage.sprite = cardData.backSprite;
    }
    
    public void ForceFlipCard()
    {
        flipped = false; // 강제로 false 설정
        frontImage.enabled = false;
        backImage.enabled = true;
    }
}