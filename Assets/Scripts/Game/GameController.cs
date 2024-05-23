using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : SingletonMono<GameController>
{
    [SerializeField] private List<Sprite> cardSprite;
    [SerializeField] private GameObject listCards;
    [SerializeField] private GameObject modePanel;
    [SerializeField] private GameObject inGamePanel;

    private int mode;
    private List<GameObject> cardInGame = new List<GameObject>();
    private List<int> tempCardSprite = new List<int>();
    private List<int> tempCardInGame = new List<int>();
    public List<GameObject> listCardShowed = new List<GameObject>();

    public int score;
    [SerializeField] private Text scoreTxt;
    private int combo;
    // Start is called before the first frame update

    void Start()
    {
        score = 0;
        combo = 1;
        scoreTxt.text = "Score: " + score;
    }

    public void StartGame(int _mode)
    {
        mode = _mode;
        
        modePanel.SetActive(false);
        inGamePanel.SetActive(true);
        
        ActiveCard();
        RandomCard();
    }

    public void ActiveCard()
    {

        for (int i = 0; i < mode*4; i++)
        {
            listCards.transform.GetChild(i).gameObject.SetActive(true);
            cardInGame.Add(listCards.transform.GetChild(i).gameObject);
        }
    }

    public void RandomCard()
    {
        tempCardInGame.Clear();
        tempCardSprite.Clear();

        for(int i = 0;i < mode * 2; i++)
        {
            int ranSprite = Random.Range(0,cardSprite.Count);
            if (tempCardSprite.Contains(ranSprite))
            {
                i--;
                continue;
            }
            tempCardSprite.Add(ranSprite);

            for(int j = 0; j < 2; j++)
            {
                int ranCard = Random.Range(0,cardInGame.Count);
                if (tempCardInGame.Contains(ranCard))
                {
                    j--;
                    continue;
                }
                tempCardInGame.Add(ranCard);
                cardInGame[ranCard].GetComponent<CardController>().id = i;
                cardInGame[ranCard].GetComponent<CardController>().cardImage.sprite = cardSprite[ranSprite];
            }
        }
    }

    public void CompareCard()
    {
        if(listCardShowed.Count >= 2)
        {
            if (listCardShowed[0].GetComponent<CardController>().id == listCardShowed[1].GetComponent<CardController>().id)
            {
                IncreaseScore();
                combo++;
            }
            else
            {
                combo = 1;
                StartCoroutine(listCardShowed[0].GetComponent<CardController>().HideCard());
                StartCoroutine(listCardShowed[1].GetComponent<CardController>().HideCard());
            }
            listCardShowed.RemoveAt(1);
            listCardShowed.RemoveAt(0);
        }
    }

    public void IncreaseScore()
    {
        score += 100 * combo;
        scoreTxt.text = "Score: " + score.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
