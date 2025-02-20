using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardList))]
public class CardListEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        CardList cardList = (CardList)target;
        
        if(GUILayout.Button("카드 생성하기"))
        {
            GenerateCard(cardList);
            EditorUtility.SetDirty(cardList);
        }
    }

    public void GenerateCard(CardList cardList)
    {
        cardList.cards.Clear();
        
        string[] suits = { "스페이드", "하트", "다이아몬드", "클러브" };
        string[] suitNames = {"Spades", "Hearts", "Diamonds", "Clubs"};

        for (int suitIndex = 0; suitIndex < suits.Length; suitIndex++)
        {
            for (int i = 1; i <= 13; i++)
            {
                CardData newCard = new CardData
                {
                    cardName = $"{suits[suitIndex]} {i}",
                    suitedName = suitNames[suitIndex],
                    cardNumber = i,
                    frontSprite = null, // 이후 자동화 작업 필요
                    backSprite = null   // 이후 자동화 작업 필요
                };
                cardList.cards.Add(newCard);
            }
        }
        Debug.Log("자동 카드 생성 완료!");
    }
}
