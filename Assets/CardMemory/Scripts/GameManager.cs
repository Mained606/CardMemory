using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private int level = 1;
    private int score = 0;
    private int highScore = 0;

    private float timer;
    [SerializeField] private float maxTimer = 60f;
    
    [SerializeField] private GameObject firstCard;
    [SerializeField] private GameObject secondCard;
    
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private bool gameOver = false;
    public bool isProcessingMatch = false; // 정답 확인 중인지 여부
    
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

    private void Start()
    {
        InitializeTimer();
        selectCard();
        UpdateUI();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        UpdateUI();
        
        if (timer <= 0)
        {
            //게임 오버
            if (!gameOver)
            {
                gameOver = true;
                Debug.Log("Game Over");
                if (highScore < score)
                {
                    highScore = score;
                    
                    // Ui 보여줌
                    
                }
            }
        }
    }

    private void selectCard()
    {
        switch (level)
        {
            case 1:
                CardManager.instance.SelectedCardData(2);
                break;
            case 2:
                CardManager.instance.SelectedCardData(3);
                break;
            case 3:
                CardManager.instance.SelectedCardData(9);
                break;
            case 4:
                CardManager.instance.SelectedCardData(16);
                break;
            default:
                CardManager.instance.SelectedCardData(level * 4);
                break;
        }
    }

    public void SelectedCard(GameObject obj)
    {
        // 첫 번째 카드가 null이면
        if (firstCard == null)
        {
            firstCard = obj;
        }
        // 첫 번째 카드가 null이 아니면
        else
        {
            // 첫 번째 카드와 두 번째 카드가 같은 오브젝트인지 확인 후 다르면 대입
            if (firstCard != obj)
            {
                secondCard = obj;
            }
            // 같은 카드를 선택했으면 리턴
            else
            {
                return;
            }
        }
        
        // 첫 번째 카드와 두 번째 카드가 null이 아니라면
        if (firstCard != null && secondCard != null)
        {
            isProcessingMatch = true; // 정답 확인 시작
            
            // 첫 번째 카드와 두 번째 카드 데이터 참조
            Card firstCardData = firstCard.GetComponent<Card>();
            Card secondCardData = secondCard.GetComponent<Card>();
            
            // 첫 번째 카드와 두 번째 카드의 카드 이름이 같다면
            if (firstCardData.cardData.cardName == secondCardData.cardData.cardName)
            {
                // 레벨 만큼 스코어 증가
                score += level;
                
                // 정답시 카드 삭제
                CardManager.instance.RemoveCardFromList(firstCard);
                CardManager.instance.RemoveCardFromList(secondCard);
                
                Destroy(firstCard, 0.5f);
                Destroy(secondCard, 0.5f);
                
                firstCard = null;
                secondCard = null;
                
                UpdateUI();
                
                StartCoroutine(WaitAndCheckAllCardsMatched(0.6f));
            }
            else
            {
                Debug.Log("오답");
                StartCoroutine(FlipBackAfterDelay(firstCardData, secondCardData));
            }
        }
    }
    private IEnumerator FlipBackAfterDelay(Card first, Card second)
    {
        yield return new WaitForSeconds(1f);
        first.ForceFlipCard();
        second.ForceFlipCard();
        
        firstCard = null;
        secondCard = null;

        isProcessingMatch = false; // 다시 클릭 가능
    }
    
    private IEnumerator WaitAndCheckAllCardsMatched(float delay)
    {
        yield return new WaitForSeconds(delay);
        isProcessingMatch = false; // 다시 클릭 가능

        if (CardManager.instance.cards.Count == 0)
        {
            StartCoroutine(DelayedLevelUp());
        }
    }
    
    private IEnumerator DelayedLevelUp()
    {
        Debug.Log("모든 카드를 맞췄습니다! 다음 레벨로 이동합니다...");
        yield return new WaitForSeconds(1.5f); // 1.5초 딜레이 추가
        LevelUp();
    }
    
    public void LevelUp()
    {
        level++;
        InitializeTimer(); // 타이머 초기화
        selectCard();
        UpdateUI();
    }
    
    private void InitializeTimer()
    {
        maxTimer = 60f + (level * 10f); // 레벨마다 타이머 증가
        timer = maxTimer;
    }
    
    private void UpdateUI()
    {
        // 시간 및 점수 표시
        timerText.text = $"Time: {Mathf.Max(0, (int)timer)} sec";
        scoreText.text = $"Score: {score}";
    }
}
