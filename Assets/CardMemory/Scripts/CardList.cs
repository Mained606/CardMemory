using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardListData", menuName = "Card Memory/Card List")]
public class CardList : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();
}

[System.Serializable]
public class CardData
{
    public string cardName;     // 카드 이름
    public string suitedName;   // 문양 이름
    public int cardNumber;      // 카드 번호
    public Sprite frontSprite;  // 앞면 이미지
    public Sprite backSprite;   // 뒷면 이미지
}