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

public class Game
{
    // i want to make it so that you can pick how many players to play with, escoba can be played with multiple players
    private readonly Player p1;
    private readonly Player p2;



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

        Table table = new Table();

        Program.Print($"ROUND {roundNumber} START...");
        // Play a round of the game
        Play(table);
    }


    // This is the main play method. It will be called every round of the game (in games that have rounds, like escoba. This is pretty pointless for games like crazy eight)
    public void Play(Table table)
    {
        Deck deck = new();
        deck.Shuffle();

        deck.printDeck();

        Program.PrintSeparator();

        Deal(deck);

        Program.PrintSeparator();

        deck.printDeck();

        // Uncomment this to put cards on the table
        //
        // for every card that the table can fit
        // for(int i = 0; i < table.getTableMaxCards(); i++)
        // {
        //     Card card = deck.DealCard();
        //     table.putDownCard(card);
        //     Program.Print($"Placed {card.getValue()} de {card.getSuit()} on the table");
        // }
        //Program.PrintSeparator();


    }

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
            Program.Print($"P2 got card: {card.getName()}");
            Console.WriteLine();
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
        string[] values = { "As", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Sota", "Caballo", "Rey" };

        //uncomment for a deck of american cards
        //string [] suits = {"Hearts", "Diamonds", "Spades", "Clubs"};	
        //string [] values = {"Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack","Queen", "King"};

        foreach (var suit in suits)
        {
            foreach (var value in values)
            {
                cards.Add(new Card(suit, value));
            }
        }
    }

    public void Shuffle()
    {
        Random random = new();
        cards = cards.OrderBy(x => random.Next()).ToList();
    }

    public void printDeck()
    {
        foreach (var card in cards)
        {
            Console.Write("Drew a card: ");
            Program.Print($"{card.getValue()} de {card.getSuit()}");
        }
        Program.Print($"The deck has {cards.Count} cards left");
    }

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
    private string Value;

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

    public String getValue()
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
    public Card(string suit, string value)
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
    public void DrawCard(Card card)
    {
        this.Hand.Add(card);
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

    public int getTableMaxCards()
    {
        return this.TABLE_MAX_CARDS;
    }

    //methods
    public void putDownCard(Card card)
    {
        this.Cards.Add(card);
    }
}

