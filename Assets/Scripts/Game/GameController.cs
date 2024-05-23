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
                if(compareRight == mode * 2)
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
        for (int i = 0; i < mode * 4; i++)
        {
            listCards.transform.GetChild(i).gameObject.SetActive(false);
            StartCoroutine(listCards.transform.GetChild(i).GetComponent<CardController>().HideCard());
            listCardInGame.Remove(listCards.transform.GetChild(i).gameObject);
        }
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
