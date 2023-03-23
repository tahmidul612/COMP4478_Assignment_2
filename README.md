## COMP4478_Assignment_2
# TAHMIDUL ISLAM
https://github.com/tahmidul612/COMP4478_Assignment_2

Unity Version 2021.3.21f1

---
### Sprite
I loaded all the sprites using the Resources method

### Scripts
My solution uses three scripts, a summary is given below and further explanation is given as in-line comments in the scripts.
- PopulateCards: This is the main script that controls all aspects of the main game related to the cards, including loading the card images, setting the images to random cards, flipping the cards on click, checking for match, game over logic, etc.
- StartGame: This is a button controller for the start button in the start menu
- QuitGame: The button controller for the Quit and Restart buttons in the quit menu

### UI
I used Unity Editor to create the Scenes for the start menu, main game and quit menu. The start menu contains a button to start the main game, and the quit menu contains buttons to quit the game or restart. The main game scene contains a parent Canvas, under that a Panel and under the panel, 16 Button elements. Each button element contains two children image gameobjects, called "Shown" and "Hidden", where the card image (bean, doraemon, etc.) and the blank face is set respectively.

Clicking any card hides the blank face to show the main image (controlled by the PopulateCards script) and hides it after an interval. If two cards are already showing, and a third card is clicked then the flipped cards are flipped to blank face again and the clicked card is shown. All flipped cards are returned to blank face after an interval.

### Game Logic
The `FlipCard` method in the `PopulateCards` scripts flips cards, saving the `Button` object of each flipped card in a list, and clearing when all cards are hidden.

When images are set to card pairs, the `HashCode` of each button pair is saved to a list of tuples named `cardPairs`. The script checks if two flipped cards are the same, using the `isMatchingPair` method. The `isMatchingPair` method checks if the given numbers form a pair (unordered) that exists in the `cardPairs` variable.

When two cards are matched, the `onClickListener` is removed from both button objects and the counter `numOfMatchingPair` is incremented by two. The overriden `Update` method checks if the `numOfMatchingPair` is equal to the total number of cards and loads the quit screen when true.