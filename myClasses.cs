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
    public void Start()
    {

            Program.Print("P1: " + p1.Name);
            Program.Print("P2: " + p2.Name);

            // TODO: add a menu to start a new game, exit or learn how to play
            Program.Print("Press enter to start");
            Console.ReadLine();

            Program.Print("Creating deck of cards...");
            var deck = new Deck();

            Program.Print("Shuffling deck...");
            deck.Shuffle();

            Program.Print("Done!");

        
    }
}

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
            Program.Print("Drew a card: ");
            Program.Print($"{card.getValue()} de {card.getSuit()}");
        }
    }
}

public class Card
{
    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      VARIABLES                           /
    //                                                          /
    // //////////////////////////////////////////////////////////
    private string Suit;
    private string Value;

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

    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      CONSTRUCTOR                         /
    //                                                          /
    // //////////////////////////////////////////////////////////
    public Card(string suit, string value)
    {
        this.Suit = suit;
        this.Value = value;
    }
}

public class Player
{
    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      VARIABLES                           /
    //                                                          /
    // //////////////////////////////////////////////////////////
    public readonly string Name;
    private List<Card> Hand;

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

    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      CONSTRUCTOR                         /
    //                                                          /
    // //////////////////////////////////////////////////////////
    public Player(string name)
    {
        this.Name = name;
        this.Hand = [];
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



