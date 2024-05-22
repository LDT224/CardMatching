using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : SingletonMono<GameController>
{
    [SerializeField] private List<Sprite> cardSprite;
    [SerializeField] private GameObject listCards;
    [SerializeField] public int mode;
    private List<GameObject> cardInGame = new List<GameObject>();
    private List<int> tempCardSprite = new List<int>();
    private List<int> tempCardInGame = new List<int>();
    // Start is called before the first frame update
    private void OnEnable()
    {
        mode = 4;
        ActiveCard();
    }

    void Start()
    {
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
        for(int i = 0;i < mode * 2; i++)
        {
            int ranSprite = Random.Range(0,cardSprite.Count);
            if (tempCardSprite.Contains(ranSprite))
            {
                i--;
                continue;
            }
            tempCardSprite.Add(ranSprite);
            Debug.Log("SPRITE : " + ranSprite);

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
                Debug.Log("CARD : " + ranCard);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
