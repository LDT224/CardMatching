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
    public List<GameObject> listCardShowed = new List<GameObject>();
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
                Debug.Log("RIGHT");
            }
            else
            {
                Debug.Log("WRONG");
                StartCoroutine(listCardShowed[0].GetComponent<CardController>().HideCard());
                StartCoroutine(listCardShowed[1].GetComponent<CardController>().HideCard());
            }
            listCardShowed.RemoveAt(1);
            listCardShowed.RemoveAt(0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
