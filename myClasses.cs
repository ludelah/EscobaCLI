using System.Numerics;
using System.Reflection;
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

        Program.Print("Press enter to start...");
        Console.ReadLine();


        Program.Print($"ROUND {roundNumber} START...");
        // Play a round of the game
        Play();
    }


    // This is the main play method. It will be called every round of the game (in games that have rounds, like escoba. This is pretty pointless for games like crazy eight)
    public void Play()
    {
        Deck deck = new();
        deck.Shuffle();

        //deck.printDeck();
        //Program.PrintSeparator();


        Deal(deck);
        table.DealTable(deck);
        Program.PrintSeparator();


        PlayTurn();
    }

    private void PlayTurn()
    {
        do
        {
            table.printTable(false);
            p1.PrintHand();
            Card card = p1.SelectHandCard();
            //List<Card> selectedCards = new(p1.SelectTableCard());
            //selectedCards.Add(card);

            /*if (isValidSum(ref selectedCards()))
            {
                foreach (var collectedCard in selectedCards)
                {
                    p1.collectedCards.Add(collectedCard);
                }
            }
            */
            Console.WriteLine(card.getName());
        } while (true);
    }

    private bool isValidSum(ref List<Card> selection)
    {
        int total = 0;

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

    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      CONSTRUCTOR                         /
    //                                                          /
    // //////////////////////////////////////////////////////////
    public Player(string name)
    {
        this.Name = name;
        this.Hand = new List<Card>(3);
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

        Card card;
        while (true)
        {
            // get the index
            int cardindex = Program.ReadDigit() - 1;

            // return it if input is valid
            if (cardindex >= 0 && cardindex < this.getHand().Count)
            { return card = this.getHand().ElementAt(cardindex); }

        }

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
        for(int i = 0; i < this.Cards.Count; i++)
        {
            if (withIndex)
            {
                Console.Write($"{i+1}. ");
            }
            Console.WriteLine($"{Cards[i].getName()}");
        }
    }
}

