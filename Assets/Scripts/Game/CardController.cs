using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : SingletonMono<CardController>
{
    [SerializeField] public Image cardImage;
    [SerializeField] public Image backCardImage;
    [SerializeField] private AudioClip flipSound;
    public int id;
    public bool isHide;
    // Start is called before the first frame update
    void Start()
    {
        isHide = backCardImage.IsActive();
    }

    public void ShowCard()
    {
        if(!backCardImage.IsActive()) return;

        AudioManager.Instance.PlayOneShot(flipSound);
        backCardImage.gameObject.SetActive(false);
        GameController.instance.listCardShowed.Add(this.gameObject);
        GameController.instance.CompareCard();
    }

    public IEnumerator HideCard()
    {
        yield return new WaitForSeconds(1);
        backCardImage.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
