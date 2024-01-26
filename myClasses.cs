using System.ComponentModel;
using System.Runtime.InteropServices;
using escobacli;
namespace myClasses;

public class Game
{
    private readonly Player p1;
    private readonly Player p2;
    Table table = new Table();
    // a list for card selection
    List<Card> selectedCards = new List<Card>();

    // //////////////////////////////////////////////////////////
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

        DealTable(deck, table);
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
    public void DealTable(Deck deck, Table table)
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
        Console.WriteLine(p1.getName() + "'s turn:");
        Program.PrintSeparator();
        p1.printHand();
        Program.PrintSeparator();

        do
        {
            ShowGameMenu();
            int option = getOption();

            switch (option)
            {
                case 1:
                    // select cards
                    SelectCards();
                    break;
                case 2:
                    // show cards on hand
                    p1.printHand();
                    break;
                case 3:
                    // show cards on table
                    table.printTable();
                    break;
                default:
                    break;
            }

        } while (true);

    }

    private void SelectCards()
    {
        p1.printHand();
        table.printTable();
        Console.Write($"Select cards: ");
        int sum = 0;

        List<Card> previousCards = new(p1.getHand());
        do
        {
            
            Program.PrintSeparator();

            int cardid = -1;

            Console.WriteLine("1. From hand");
            Console.WriteLine("2. From table");
            Console.WriteLine("3. Back");
            int option = getOption();

            Console.WriteLine("Total: " + sum);

            switch (option)
            {
                case 1:
                    do
                    {
                        Program.PrintSeparator();
                        p1.printHand();
                        Console.WriteLine("Total: " + sum);
                        Console.Write($"Select card 1 to {p1.getHand().Count} (0 to go back): ");

                        cardid = Program.ReadDigit();
                        //goback?
                        if (cardid == 0)
                        {
                            break;
                        }

                        // else play card and add it to selected cards
                        else
                        {
                            Card card = p1.PlayCard(cardid);
                            selectedCards.Add(card);
                            sum += Convert.ToInt32(card.getValue());

                            Console.WriteLine($"Selected card: {card.getName()}");
                            Console.WriteLine($"Total: {sum}");
                        }

                        // check if we went past the limit, and if we did, clear everything and start again
                        if(isPast15(ref sum, ref previousCards))
                            break;

                    } while (true);

                    break;
                case 2:
                    Program.PrintSeparator();
                    table.printTable();
                    Console.WriteLine("Total: " + sum);

                    Console.Write($"Select card 1 to {table.getCards().Count} (0 to go back): ");
                    cardid = Program.ReadDigit();

                    if (cardid == 0)
                    {
                        break;
                    }
                    break;
                case 3:
                    p1.setHand(previousCards);
                    selectedCards.Clear();
                    return;
                default:
                    break;
            }
        } while (true);
    }

    private bool isPast15(ref int sum, ref List<Card> previousCards)
    {
        if (sum > 15)
        {
            Program.Print("You went past 15, try again");
            clearAll(ref sum, ref previousCards);
            return true;
        }
        return false;
    }

    private void clearAll(ref int sum, ref List<Card> previousCards)
    {
        selectedCards.Clear();
        p1.setHand(previousCards);
        sum = 0;
    }

    public void ShowGameMenu()
    {
        Console.WriteLine("Choose an action:");
        Console.WriteLine("1. Select cards");
        Console.WriteLine("2. Show cards on hand");
        Console.WriteLine("3. Show cards on the table");
    }

    public int getOption()
    {
        int option = -1;
        do
        {
            try
            {
                option = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Program.Print("Please enter a valid option");
            }
        } while (option < 1 || option > 3);

        return option;
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
        string[] values = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

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

    // Function that takes a number to indicate which card to play, then, extracts it from the hand of the player and returns it
    // If you want to get the card back to the player, save the card extracted and then use setHand();
    public Card PlayCard(int card)
    {
        Card thisCard = this.Hand.ElementAt(card - 1);
        this.Hand.RemoveAt(card - 1);

        return thisCard;
    }

    // Self describing
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

    public void setCards(List<Card> cards)
    {
        this.Cards = cards;
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

    public void printTable()
    {
        Program.PrintSeparator();
        foreach (var card in this.Cards)
        {
            Program.Print($"{card.getName()}");
        }
    }
}

