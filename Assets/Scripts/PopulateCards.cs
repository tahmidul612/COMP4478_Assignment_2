using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class PopulateCards : MonoBehaviour
{
    // List of all the cards in the scene
    private Button[] cards;
    // Variable to keep track of the cards that are flipped
    private List<Button> flippedCards;
    // List of all the pairs of matching cards
    private List<(int, int)> cardPairs;
    // Number of pairs that have been matched
    private int numOfMatchingPairs = 0;

    void Awake()
    {
        // Load all images from the Resources/Images folder
        Sprite[] images = Resources.LoadAll<Sprite>("Images");
        Sprite blank = Resources.Load<Sprite>("blank");
        // Get all the cards in the scene
        cards = this.GetComponentsInChildren<Button>(true);
        // Generate a list of numbers from 0 to the number of cards
        var cardNumbers = Enumerable.Range(0, cards.Length).ToList();

        cardPairs = new List<(int, int)>();
        flippedCards = new List<Button>();

        // For each image, randomly select two cards and assign the image to them
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
    }

    void Start()
    {
        // Add a listener to each card to flip it when clicked
        for (int i = 0; i < cards.Length; i++)
        {
            Button card = cards[i];
            card.onClick.AddListener(() => StartCoroutine(FlipCard(card.gameObject.GetComponent<Button>())));
        }
    }

    private void Update() {
        // If all the cards have been matched, load the quit menu
        if (numOfMatchingPairs == cards.Length) {
            SceneManager.LoadScene("QuitMenu");
        }
    }

    IEnumerator FlipCard(Button clickedCard)
    {
        // If there are less than 2 cards flipped, flip the clicked card
        if (flippedCards.Count < 2)
        {
            Image blankFace = clickedCard.GetComponentsInChildren<Image>(true)[2];
            if (blankFace.gameObject.activeSelf)
            {
                blankFace.gameObject.SetActive(false);
                flippedCards.Add(clickedCard);
            }
            // If there are 2 cards flipped, check if they are a matching pair
            if (flippedCards.Count == 2)
            {
                if (isMatchingPair(flippedCards[0].GetHashCode(), flippedCards[1].GetHashCode()))
                {
                    Debug.Log("Match!");
                    flippedCards[0].onClick.RemoveAllListeners();
                    flippedCards[1].onClick.RemoveAllListeners();
                    numOfMatchingPairs += 2;
                    flippedCards.Clear();
                }
                else
                {
                    Debug.Log("No match!");
                }
            }
        }
        // If there are 2 cards flipped, hide them and flip the clicked card
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
        // Wait 5 seconds before hiding the cards
        yield return new WaitForSecondsRealtime(5.0f);
        if (flippedCards.Count == 2)
        {
            hideAllCards();
        }
    }
    
    // Hide all the cards that are flipped
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

    // Check if the two cards are a matching pair
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
