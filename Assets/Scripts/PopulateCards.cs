using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class PopulateCards : MonoBehaviour
{
    // Start is called before the first frame update
    private Button[] cards;
    private List<Button> flippedCards;
    private List<(int, int)> cardPairs;
    private bool gameOver = false;
    // public float delayTime = 5.0f;
    // private WaitForSeconds delay;

    void Awake()
    {
        Sprite[] images = Resources.LoadAll<Sprite>("Images");
        Sprite blank = Resources.Load<Sprite>("blank");
        cards = this.GetComponentsInChildren<Button>(true);
        var cardNumbers = Enumerable.Range(0, cards.Length).ToList();
        cardPairs = new List<(int, int)>();
        flippedCards = new List<Button>();
        for (int i = 0; i < images.Length; i++)
        {
            int randomIndex_1 = Random.Range(0, cardNumbers.Count);
            int card1 = cardNumbers[randomIndex_1];
            cardNumbers.Remove(card1);
            int randomIndex_2 = Random.Range(0, cardNumbers.Count);
            int card2 = cardNumbers[randomIndex_2];
            cardNumbers.Remove(card2);
            cards[card1].GetComponentsInChildren<Image>(true)[1].sprite = images[i];
            cards[card2].GetComponentsInChildren<Image>(true)[1].sprite = images[i];
            cardPairs.Add(new (cards[card1].GetHashCode(), cards[card2].GetHashCode()));
        }
        // delay = new WaitForSeconds(delayTime);
    }

    void Start()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Button card = cards[i];
            card.onClick.AddListener(() => StartCoroutine(FlipCard(card.gameObject.GetComponent<Button>())));
        }
    }

    private void Update() {
        if (gameOver) {
            SceneManager.LoadScene("QuitMenu");
        }
    }

    IEnumerator FlipCard(Button clickedCard)
    {
        if (flippedCards.Count < 2)
        {
            Image blankFace = clickedCard.GetComponentsInChildren<Image>(true)[2];
            if (blankFace.gameObject.activeSelf)
            {
                blankFace.gameObject.SetActive(false);
                flippedCards.Add(clickedCard);
                Debug.Log("Card hash: " + clickedCard.GetHashCode());
                Debug.Log("Flipped cards: " + flippedCards.Count);
            }
            if (flippedCards.Count == 2)
            {
                if (isMatchingPair(flippedCards[0].GetHashCode(), flippedCards[1].GetHashCode()))
                {
                    Debug.Log("Match!");
                    flippedCards[0].onClick.RemoveAllListeners();
                    flippedCards[1].onClick.RemoveAllListeners();
                    flippedCards.Clear();
                }
                else
                {
                    Debug.Log("No match!");
                }
            }
        }
        else if (flippedCards.Count == 2)
        {
            hideAllCards();
            Image blankFace = clickedCard.GetComponentsInChildren<Image>(true)[2];
            if (blankFace.gameObject.activeSelf)
            {
                blankFace.gameObject.SetActive(false);
                flippedCards.Add(clickedCard);
            }
        }
        yield return new WaitForSecondsRealtime(5.0f);
        if (flippedCards.Count == 2)
        {
            hideAllCards();
        }
    }
    
    private void hideAllCards()
    {
        foreach (Button card in flippedCards)
        {
            Image blankFace = card.GetComponentsInChildren<Image>(true)[2];
            if (!blankFace.gameObject.activeSelf)
            {
                blankFace.gameObject.SetActive(true);
            }
        }
        flippedCards.Clear();
    }

    private bool isMatchingPair(int num1, int num2)
    {
        foreach ((int, int) pair in cardPairs)
        {
            if ((pair.Item1 == num1 && pair.Item2 == num2) || (pair.Item1 == num2 && pair.Item2 == num1))
            {
                return true;
            }
        }
        return false;
    }
}
