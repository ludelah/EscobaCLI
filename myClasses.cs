using escobacli;
namespace myClasses;

public class Game
{
    private readonly Player p1;
    private readonly Player p2;
    Table table = new Table();



    public Game(Player p1, Player p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    // NOTE for myself: this is the entry point of the game
    // it sets it up with a deck, shuffles it and then starts the game. Afterall, what more do you need to start a card game?
    public void Start()
    {
        int round = 1;

        Program.Print("P1: " + p1.NAME);
        Program.Print("P2: " + p2.NAME);

        Program.Print("Press enter to start...");
        Console.ReadLine();


        Program.Print($"ROUND {round} START...");
        // Play a round of the game
        // while(noWinner)
        while (true)
        {
            Play();
        }
    }

    public void Play()
    {
        Deck deck = new();

        deck.Shuffle();
        deck.printDeck();
        Program.PrintSeparator();

        Deal(deck);
        Program.PrintSeparator();

        Deal(deck);
        Program.PrintSeparator();

        // loop to keep playing turns
        //while (!noCardsLeft)
        while (true)
        {
            Console.WriteLine("Your turn");
            PlayTurn();
            Console.WriteLine("P2's turn");
            PlayTurn();
        }
    }

    public void Deal(Deck deck)
    {
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
    }
    public void DealTable(Deck deck)
    {
        // for every card that the table can fit
        for (int i = 0; i < table.getTableMaxCards(); i++)
        {
            Card card = deck.DealCard();

            table.putDownCard(card);
            Program.Print($"Placed {card.getValue()} de {card.getSuit()} on the table");
        }
    }


    public void PlayTurn()
    {
        bool cardPlayed = false;
        // loop until the player plays a card
        while (!cardPlayed)
        {

            Program.Print("Choose cards to sum to 15");
            Console.WriteLine();

            List<int> selection = new();

            // loop until the player selects enough cards
            int sum = 0;
            do
            {
                if (sum > 15)
                {
                    Program.Print("You went over 15, select again.");
                    sum = 0;
                }

                Program.Print("Choose a card: ");
                p1.printHand();

                // get the card id
                int cardid;

                // try to get the card id while it's less than 1 or more than the number of cards in the hand
                do
                {
                    cardid = int.Parse(Program.ReadLineNonNull());
                } while (cardid < 1 || cardid > p1.getHand().Count);

                // remove the card from the hand and get it (-1 for zero base index)
                Card cardSelected = p1.PlayCard(cardid - 1);

                // add the card to the list of selected cards
                selection.Add(int.Parse(cardSelected.getValue()));

                // add the value of the card to the total
                sum = sum + int.Parse(p1.getHand().ElementAt(cardid - 1).getValue());


            } while (sum != 15);

        }
    }


    // public void Tutorial() {}
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//---------------------------------------------------------------------------------------------------------------
////////////////////////////////////////////   Deck    //////////////////////////////////////////////////////////
////////////////////////////////////////////   Class   //////////////////////////////////////////////////////////

public class Deck
{

    List<Card> cards = [];

    // constructor
    public Deck()
    {
        // create a deck of cards
        string[] suits = { "Oros", "Copas", "Espadas", "Bastos" };
        string[] values = { "1", "2", "3", "4", "5", "6", "7", "10", "11", "12" };

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

////////////////////////////////////////////   Card    //////////////////////////////////////////////////////////
////////////////////////////////////////////   Class   //////////////////////////////////////////////////////////


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

////////////////////////////////////////////   Player  //////////////////////////////////////////////////////////
////////////////////////////////////////////   Class   //////////////////////////////////////////////////////////



public class Player
{
    // //////////////////////////////////////////////////////////
    //                                                          /
    //                      VARIABLES                           /
    //                                                          /
    // //////////////////////////////////////////////////////////
    public readonly string NAME;
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
        return this.NAME;
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
        this.NAME = name;
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

    public Card PlayCard(int card)
    {
        Card thisCard = this.Hand.ElementAt(card - 1);
        this.Hand.RemoveAt(card - 1);

        return thisCard;
    }

    public void printHand()
    {
        Program.Print($"These are your cards:");
        int i = 1;
        foreach (var card in this.Hand)
        {
            Program.Print($"{i}. {card.getName()}");
            i++;
        }
    }
}

////////////////////////////////////////////   Table   //////////////////////////////////////////////////////////
////////////////////////////////////////////   Class   //////////////////////////////////////////////////////////

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

