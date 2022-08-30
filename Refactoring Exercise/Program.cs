using Refactoring_Exercise.Models;
using System.Globalization;

Dictionary<string, PlayData> plays = new()
{
    ["hamlet"] =
    new()
    {
        Name = "Hamlet",
        Type = "tragedy"
    },
    ["as-like"] =
    new()
    {
        Name = "As You Like It",
        Type = "comedy"
    },
    ["othello"] =
    new()
    {
        Name = "Othello",
        Type = "tragedy"
    }
};

List<Invoice> invoices = new()
{
    new()
    {
        Customer = "BigCo",
        Performances = new()
        {
            new(){
                PlayID = "hamlet",
                Audience = 55
            },
            new()
            {
                PlayID = "as-like",
                Audience = 35
            },
            new()
            {
                PlayID = "othello",
                Audience = 40
            }
        }
    }
};

Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
DrawResult(Statement(invoices[0], plays));

string Statement(Invoice invoice, Dictionary<string, PlayData> plays)
{
    int totalAmount = 0;
    int VolumeCredits = 0;
    string result = $"Statement for {invoice.Customer}\n";

    foreach (Performance performance in invoice.Performances)
    {
        PlayData play = plays[performance.PlayID];
        int thisAmount = 0;

        switch (play.Type)
        {
            case "tragedy":
                thisAmount = 40000;
                if (performance.Audience > 30)
                {
                    thisAmount += 1000 * (performance.Audience - 30);
                }
                break;
            case "comedy":
                thisAmount = 30000;
                if (performance.Audience > 20)
                {
                    thisAmount += 10000 + 500 * (performance.Audience - 20);
                }
                thisAmount += 300 * performance.Audience;
                break;
            default:
                throw new ArgumentException($"Unknown type: {play.Type}");
        }

        // add volume credits
        VolumeCredits += Math.Max(performance.Audience - 30, 0);
        // add extra credit for every ten comedy attendees
        if (play.Type == "comedy")
        {
            VolumeCredits += performance.Audience / 5;
        }

        // print line for this order
        result += $"{play.Name}: {thisAmount / 100:C} ({performance.Audience} seats)\n";
        totalAmount += thisAmount;
    }
    result += $"Amount owed is {totalAmount / 100:C}\n";
    result += $"You earned {VolumeCredits} credits\n";
    return result;
}

void DrawResult(string result)
{
    Console.WriteLine(result);
}
