using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : SingletonMono<CardController>
{
    [SerializeField] public Image cardImage;
    [SerializeField] private Image backCardImage;
    public int id;
    public bool showed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowCard()
    {
        backCardImage.gameObject.SetActive(false);

        if(!showed)
            StartCoroutine(HideCard());
    }

    IEnumerator HideCard()
    {
        yield return new WaitForSeconds(2);
        backCardImage.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
