using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    
    [SerializeField] private CardList cardList;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardContainer;
    
    public List<GameObject> cards = new List<GameObject>();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public void SelectedCardData(int number)
    {
        ClearCards(); // 기존 카드 삭제
        
        if (cardList == null || cardList.cards.Count == 0)
        {
            Debug.LogError("카드 리스트가 비어 있습니다!");
            return;
        }

        List<CardData> selectedCards = new List<CardData>();
        List<CardData> allCards = new List<CardData>(cardList.cards);
        
        // 1. 카드 리스트에서 number 개수만큼 랜덤으로 뽑기
        for (int i = 0; i < number; i++)
        {
            if (allCards.Count == 0) break; // 카드가 부족할 경우 방어 코드
            int randomIndex = Random.Range(0, allCards.Count);
            selectedCards.Add(allCards[randomIndex]);
            allCards.RemoveAt(randomIndex);
        }

        // 2. 선택된 카드들의 복제본을 만들어서 리스트에 추가
        List<CardData> finalCardList = new List<CardData>();
        foreach (var card in selectedCards)
        {
            finalCardList.Add(new CardData { cardName = card.cardName, frontSprite = card.frontSprite, backSprite = card.backSprite });
            finalCardList.Add(new CardData { cardName = card.cardName, frontSprite = card.frontSprite, backSprite = card.backSprite });
        }
        
        // 3. 리스트 셔플 (카드 순서를 랜덤화)
        Shuffle(finalCardList);

        // 4. 카드 프리팹 생성 및 데이터 적용
        foreach (var cardData in finalCardList)
        {
            GameObject card = Instantiate(cardPrefab, cardContainer);
            card.GetComponent<Card>().SetCardData(cardData);
            cards.Add(card);
        }
    }
    
    public void ClearCards()
    {
        foreach (var card in cards)
        {
            Destroy(card);
        }
        cards.Clear();
    }

    // Fisher-Yates 셔플 알고리즘을 사용한 리스트 랜덤 섞기
    private void Shuffle(List<CardData> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
