using escobacli;
namespace myClasses;

public class Game
{
    private readonly Player p1;
    private readonly Player p2;



    public Game(Player p1, Player p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    // NOTE for myself: this is the entry point of the game
    // it sets it up with a deck, shuffles it and then starts the game. Afterall, what more do you need to start a card game?
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

    public void Play(Table table)
    {
        Deck deck = new();
        deck.Shuffle();

        deck.printDeck();

        Program.PrintSeparator();

        // for every card that the players can hold
        for (int i = 0; i < p1.getMaxCards(); i++)
        {
            Card card = deck.DealCard();
            p1.DrawCard(card);

            Program.Print($"You got card: {card.getName()}");
            card = deck.DealCard();
            p2.DrawCard(card);


            // comment next line for release
            Program.Print($"P2 got card: {card.getName()}");
            Console.WriteLine();
        }

        Program.PrintSeparator();

        // for every card that the table can fit
        for(int i = 0; i < table.getTableMaxCards(); i++)
        {
            Card card = deck.DealCard();

            table.putDownCard(card);
            Program.Print($"Placed {card.getValue()} de {card.getSuit()} on the table");
        }

        Program.PrintSeparator();


    }

    public void Deal()
    {

    }

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
        // create a deck of cards
        string[] suits = { "Oros", "Copas", "Espadas", "Bastos" };
        string[] values = { "As", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Sota", "Caballo", "Rey" };

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

