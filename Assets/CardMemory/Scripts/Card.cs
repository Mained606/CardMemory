using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image frontImage;
    public Image backImage;
    public CardData cardData;
    public bool flipped = false;
    public bool isMatched = false; // 매치 여부 플래그
    
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
    
    public void SetMatched()
    {
        isMatched = true;
        // 카드의 시각적 요소를 비활성화하지만, GameObject는 그대로 두어 Grid Layout의 자리가 유지되도록 함.
        frontImage.enabled = false;
        backImage.enabled = false;
        
        // RaycastTarget도 비활성화하여 상호작용 차단 가능
        var button = GetComponent<Button>();
        if (button != null) button.interactable = false;
    }
    
    public void ForceFlipCard()
    {
        flipped = false;
        frontImage.enabled = false;
        backImage.enabled = true;
    }
}