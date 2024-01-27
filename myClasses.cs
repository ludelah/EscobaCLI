using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using escobacli;
namespace myClasses;

/*
    This serves as a template for any or most card games that I might want to make in the future, so I'm publishing as a separate branch from
    the card game this was originally from (Escoba CLI). I'm also going to try to make it as generic as possible so that it can be used for any card game.
    
    If you need to change the number of cards that the players can hold, change the value of PLAYER_MAX_CARDS in the Player class.
    If you need to change the number of cards that the table can hold, change the value of TABLE_MAX_CARDS in the Table class.
    If you need to change the number of cards in the deck, add the cards in the variable string list.
    
    My plan is to make a method that lets you change the number of cards in the deck and the type of deck so i can host multiple games from the same menu, like
    making it check for a number and in case 0 is entered, it will use the default deck, but if 1 is entered, it will use a deck with 2 jokers, and if 2 is entered,
    it will use a different regional deck, etc. Another idea was to make it so that the cards on the table can overlap, as in the cards that get putDown() on the table
    get pushed one on top of the other, but that would require a different approach to the table class, so I'll leave it for later. Maybe with an ordered list.

    Another unprioritized idea is to change the language of the game. It is weird to have spanish cards with english text. 
*/
enum cardValues
{
    As = 1,
    Dos,
    Tres,
    Cuatro,
    Cinco,
    Seis,
    Siete,
    Sota,
    Caballo,
    Rey,
}

public class Game
{
    // i want to make it so that you can pick how many players to play with, escoba can be played with multiple players
    private readonly Player p1;
    private readonly Player p2;
    Table table = new Table();
    readonly int WIN_SCORE = 31;


    public Game(Player p1, Player p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    // this is the entry point of the game
    // it sets it up with a deck, shuffles it and then starts the game.
    public void Start()
    {
        int roundNumber = 1;

        Program.Print("P1: " + p1.Name);
        Program.Print("P2: " + p2.Name);
        Deck deck = new();

        Program.Print("Press enter to start...");
        Console.ReadLine();
        deck.Shuffle();


        Program.Print($"ROUND {roundNumber} START...");
        Program.PrintSeparator();
        // Play a round of the game
        do
        {
            Play(ref deck);
        } while (p1.getScore() >= 31 || p2.getScore() >= 31);
    }


    // This is the main play method. It will be called every round of the game (in games that have rounds, like escoba. This is pretty pointless for games like crazy eight)
    public void Play(ref Deck deck)
    {

        //deck.printDeck();
        //Program.PrintSeparator();


        Deal(deck);
        table.DealTable(deck);
        Program.PrintSeparator();

        int turncount = 0;
        do
        {
            Player currentTurn = p1;
            PlayTurn(currentTurn);
            currentTurn = p2;
            PlayTurn(currentTurn);

            turncount++;
        } while (turncount != 3);
        // TODO add scores from played round and collect table cards after round ends

    }

    private void PlayTurn(Player currentplayer)
    {
        do
        {
            table.printTable(false);
            currentplayer.PrintHand();

            Card handcard = currentplayer.SelectHandCard();
            Console.WriteLine($"Selected card: {handcard.getName()} \nTotal: {handcard.getValue()}");
            Program.PrintSeparator();

            List<Card> selectedCards = new(currentplayer.SelectTableCard(table, handcard.getValue()));

            if (isValidSum(ref selectedCards, ref handcard))
            {
                selectedCards.Add(handcard); currentplayer.RemoveCard(handcard);

                foreach (Card collectedCard in selectedCards)
                {
                    currentplayer.collectedCards.Add(collectedCard);
                }
                break;
            }
        } while (true);

        /*
        BUGS FOUND SO FAR:

          · After you select all the cards on the table and you didn't make it past 15, the loop should
            break and it should return back to the SelectHandCard() loop. Instead, it lets you select more
            cards despite not being cards to select.

          · So then, you can select the same card table multiple times

          · If theres no cards on the table, there has to be a mechanic that lets you put down a card on it:
                *select hand card
                *remove from players hand
                *add to table card list
        */

        Console.WriteLine("END TURN");
        Program.PrintSeparator();
        Console.WriteLine($"Next player's turn. {currentplayer.getName()} don't look at his cards");
        Console.ReadLine();
    }

    private bool isValidSum(ref List<Card> selection, ref Card handcard)
    {
        int total = handcard.getValue();

        foreach (Card card in selection)
        {
            total += card.getValue();
        }
        if (total != 15)
        {
            return false;
        }

        return true;
    }

    ///<summary>
    /// It saves a <i>Card</i> in it's <i>Card</i> buffer from a  <br/>
    /// given deck using the <b>Deck.DealCard()</b> method. <br/>
    /// Then, it calls the <b>Player.DrawCard(Card)</b> method,<br/>
    /// which takes a given <i>Card</i> and adds it to the player's<br/>
    /// hand until all players hands are full
    ///</summary>
    public void Deal(Deck deck)
    {
        // The players are declared right inside the Game class so their cards and names are always available.

        // for every card that the players can hold
        for (int i = 0; i < p1.getMaxCards(); i++)
        {
            Card card = deck.DealCard();
            p1.DrawCard(card);

            Program.Print($"You got card: {card.getName()}");
            card = deck.DealCard();
            p2.DrawCard(card);


            // This line is for debugging purposes only
            // Program.Print($"P2 got card: {card.getName()}");
            // Console.WriteLine();
        }

        // i was worried that when cards where dealt here, the deck at Play() wouldnt have the cards that were dealt, but it does, so i dont need to worry about it
    }

    // Signature for the future Tutorial() method that'll teach the player how to play the game
    // public void Tutorial() {}
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////

public class Deck
{

    List<Card> cards = [];

    // constructor
    public Deck()
    {
        // create a deck of spanish cards
        string[] suits = { "Oros", "Copas", "Espadas", "Bastos" };

        //uncomment for a deck of american cards
        //string [] suits = {"Hearts", "Diamonds", "Spades", "Clubs"};	
        //string [] values = {"Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack","Queen", "King"};

        foreach (var suit in suits)
        {
            for (int i = 1; i <= (int)cardValues.Rey; i++)
            {
                cards.Add(new Card(suit, i));
            }
        }
    }

    /// <summary>
    /// Shuffles it's instantiated List of <i>Card</i>
    /// </summary>
    public void Shuffle()
    {
        Random random = new();
        cards = cards.OrderBy(x => random.Next()).ToList();
    }

    /// <summary>
    /// Prints each card from it's list of <i>Card</i> to the console along with how many cards it has. Used for debugging purposes
    /// </summary>
    public void printDeck()
    {
        foreach (var card in cards)
        {
            Console.Write("Drew a card: ");
            Program.Print($"{card.getValue()} de {card.getSuit()}");
        }
        Program.Print($"The deck has {cards.Count} cards left");
    }
    /// <summary>
    /// Returns a <i>Card</i> to wherever the function is called and removes it from it's instatiated list of cards.
    /// </summary>
    /// <returns></returns>
    public Card DealCard()
    {
        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////


public class Card
{
    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      VARIABLES                           /
    //                                                          /
    // //////////////////////////////////////////////////////////
    private string Suit;
    private int Value;

    private string cardName;

    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      GET N SET                           /
    //                                                          /
    // //////////////////////////////////////////////////////////
    public String getSuit()
    {
        return this.Suit;
    }

    public int getValue()
    {
        return this.Value;
    }

    public string getName()
    {
        return this.cardName;
    }

    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      CONSTRUCTOR                         /
    //                                                          /
    // //////////////////////////////////////////////////////////
    public Card(string suit, int value)
    {
        this.Suit = suit;
        this.Value = value;
        this.cardName = $"{value} de {suit}";
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////



public class Player
{
    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      VARIABLES                           /
    //                                                          /
    // //////////////////////////////////////////////////////////
    public readonly string Name;
    private List<Card> Hand;
    public List<Card> collectedCards;
    readonly int PLAYER_MAX_CARDS = 3;
    int score = 0;

    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      GET N SET                           /
    //                                                          /
    // //////////////////////////////////////////////////////////

    public List<Card> getHand()
    {
        return this.Hand;
    }

    // wanted to determine the number of cards but realized the list already has it lol
    public void setHand(List<Card> hand)
    {
        this.Hand = hand;
    }

    public string getName()
    {
        return this.Name;
    }

    public int getMaxCards()
    {
        return this.PLAYER_MAX_CARDS;
    }

    public int getScore()
    {
        return this.score;
    }

    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      CONSTRUCTOR                         /
    //                                                          /
    // //////////////////////////////////////////////////////////
    public Player(string name)
    {
        this.Name = name;
        this.Hand = new List<Card>(3);
        this.collectedCards = [];
    }

    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      METHODS                             /
    //                                                          /
    // //////////////////////////////////////////////////////////

    public void PrintHand()
    {
        int i = 1;
        Console.WriteLine("Your hand:");
        foreach (Card card in this.Hand)
        {
            Console.WriteLine($"{i}. {card.getName()}");
            i++;
        }
    }


    /// <summary>
    /// Function that takes a <i>Card</i> as <br/>
    /// argument and adds it to its hand.
    /// </summary>
    /// <param name="card"></param>
    public void DrawCard(Card card)
    {
        this.Hand.Add(card);
    }

    public Card SelectHandCard()
    {
        Console.WriteLine($"Select card (1 to {this.getHand().Count}):");

        while (true)
        {
            // get the index
            int cardindex = Program.ReadDigit() - 1;

            // return it if input is valid
            if (isValidIndex(cardindex, this.getHand()))
            { return this.Hand.ElementAt(cardindex); }

            else Console.WriteLine("Invalid card index. Try again.");
        }
    }


    private bool isValidIndex(int cardindex, List<Card> cards)
    {
        return cardindex >= 0 && cardindex < cards.Count;
    }

    /// <summary>
    /// Shows a given table and asks the player to </br>
    /// input a number. Then, it traces that number </br>
    /// to its respective card in the table and adds it </br>
    /// to a list of selected cards so far. It loops </br>
    /// back until the sum of the cards selected </br>
    /// plus the card selected from the hand (sum) </br>
    /// equal or pass 15. If they equal 15, the </br>
    /// cards are removed from the given table and </br>
    /// the sellected cards are returned. If it passes 15 </br>
    /// it clears the selectedcards list and returns it empty.
    /// </summary>
    public List<Card> SelectTableCard(Table table, int sum)
    {
        Program.PrintSeparator();
        List<Card> selectedCards = [];
        Card cardbuffer;
        int total = sum;

        Console.WriteLine("Select the cards from the table to sum:");
        table.printTable();
        do
        {
            // clean the selectedcards array and break && return if it passed the quota
            if (total > 15)
            {
                Console.WriteLine("You passed 15. Try again.");
                selectedCards.Clear();
                Program.PrintSeparator();
                return selectedCards;
            }

            // which cards do you want to select from the table?
            int cardindex = Program.ReadDigit() - 1;

            if (!isValidIndex(cardindex, table.getCards()))
            {
                Console.WriteLine ("Invalid card index. Try again.");
                continue;
            }
            cardbuffer = table.getCards().ElementAt(cardindex);
            selectedCards.Add(cardbuffer);

            total += cardbuffer.getValue();
            Console.WriteLine($"Total: {total}");

        } while (total != 15);


        // if everything went correctly, clean the table from the picked up cards
        foreach (var card in selectedCards)
        {
            table.RemoveCard(card);
        }

        return selectedCards;
    }

    public void RemoveCard(Card handcard)
    {
        this.Hand.Remove(handcard);
    }

}

////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
public class Table
{
    //variables
    readonly int TABLE_MAX_CARDS = 4;

    private List<Card> Cards;

    //constructor
    public Table()
    {
        this.Cards = new List<Card>(4);
    }

    //get set
    public List<Card> getCards()
    {
        return this.Cards;
    }

    public int GetTableMaxCards()
    {
        return this.TABLE_MAX_CARDS;
    }

    //methods

    public void DealTable(Deck deck)
    {
        for (int i = 0; i < this.Cards.Capacity; i++)
        {
            this.Cards.Add(deck.DealCard());
        }
    }

    public void printTable(bool withIndex = true)
    {
        Console.WriteLine("The table has:");
        for (int i = 0; i < this.Cards.Count; i++)
        {
            if (withIndex)
            {
                Console.Write($"{i + 1}. ");
            }
            Console.WriteLine($"{Cards[i].getName()}");
        }
    }

    public void RemoveCard(Card card)
    {
        this.Cards.Remove(card);
    }
}

