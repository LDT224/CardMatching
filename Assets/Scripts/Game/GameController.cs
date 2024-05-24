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
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject winPanel;

    private int mode;
    private List<GameObject> listCardInGame = new List<GameObject>();
    private List<int> tempCardSprite = new List<int>();
    private List<int> tempCardInGame = new List<int>();
    public List<GameObject> listCardShowed = new List<GameObject>();

    public int score;
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Text endGameScoreText;
    private int combo;
    private int compareRight;

    [SerializeField] private AudioClip rightSound;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private AudioClip winSound;

    public SaveLoadManager saveLoadManager;
    // Start is called before the first frame update

    private void OnEnable()
    {
        menuPanel.SetActive(true);
        
        
    }
    void Start()
    {
        AudioManager.Instance.Reset();
    }

    public void NewGame()
    {
        menuPanel.SetActive(false);
        modePanel.SetActive(true);
    }

    public void StartGame(int _mode)
    {
        mode = _mode;
        
        modePanel.SetActive(false);
        inGamePanel.SetActive(true);
        
        ActiveCard();
        RandomCard();

        score = 0;
        combo = 1;
        compareRight = 0;
        scoreTxt.text = "Score: " + score;
    }

    public void ActiveCard()
    {
        for(int j = 0; j < listCards.transform.childCount; j++)
        {
            listCards.transform.GetChild(j).gameObject.SetActive(false);
            listCards.transform.GetChild(j).GetComponent<CardController>().backCardImage.gameObject.SetActive(true);
            listCards.transform.GetChild(j).GetComponent<CardController>().isHide = true;
        }
        listCardInGame.Clear();

        for (int i = 0; i < mode*4; i++)
        {
            listCards.transform.GetChild(i).gameObject.SetActive(true);
            listCardInGame.Add(listCards.transform.GetChild(i).gameObject);
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
                int ranCard = Random.Range(0, listCardInGame.Count);
                if (tempCardInGame.Contains(ranCard))
                {
                    j--;
                    continue;
                }
                tempCardInGame.Add(ranCard);
                listCardInGame[ranCard].GetComponent<CardController>().id = i;
                listCardInGame[ranCard].GetComponent<CardController>().cardImage.sprite = cardSprite[ranSprite];
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
                compareRight++;
                AudioManager.Instance.PlayOneShot(rightSound);
                listCardShowed[0].GetComponent<CardController>().isHide = false;
                listCardShowed[1].GetComponent<CardController>().isHide = false;
                if (compareRight == mode * 2)
                {
                    StartCoroutine(Win());
                }
            }
            else
            {
                combo = 1;
                AudioManager.Instance.PlayOneShot(wrongSound);
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

    public void BackToMenu()
    {
        inGamePanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(1);
        AudioManager.Instance.PlayOneShot(winSound);
        winPanel.SetActive(true);
        inGamePanel.SetActive(false);
        endGameScoreText.text = scoreTxt.text;

        DeleteGameSave();
    }

    public void Replay()
    {
        for (int i = 0; i < mode * 4; i++)
        {
            listCards.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
        }
        RandomCard();

        winPanel.SetActive(false);
        inGamePanel.SetActive(true);

        score = 0;
        combo = 1;
        compareRight = 0;
        scoreTxt.text = "Score: " + score;
    }

    public void SaveGame()
    {
        listCardShowed.Clear();
        saveLoadManager.SaveGame(score, combo, compareRight, mode, listCardInGame);
    }

    public void LoadGame()
    {
        var gameData = saveLoadManager.LoadGame();
        if (gameData == null) return;

        score = gameData.score;
        combo = gameData.combo;
        compareRight = gameData.compareRight;
        mode = gameData.mode;

        foreach (var cardData in gameData.cards)
        {
            Transform cardTransform = listCards.transform.GetChild(cardData.childIndex);
            Debug.Log(cardData.childIndex);
            GameObject card = cardTransform.gameObject;
            card.SetActive(cardData.isActive);
            var cardController = card.GetComponent<CardController>();
            cardController.id = cardData.id;
            cardController.cardImage.sprite = cardSprite.Find(sprite => sprite.name == cardData.spriteName);
            cardController.backCardImage.gameObject.SetActive(cardData.isHide);
        }
        Debug.Log("Game Loaded!");

        menuPanel.SetActive(false);
        inGamePanel.SetActive(true);
        scoreTxt.text = "Score: " + score;
    }

    public void DeleteGameSave()
    {
        saveLoadManager.DeleteSave();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
