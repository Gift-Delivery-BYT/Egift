namespace Egift_main;

public class Review_Sys
{
    private int raiting { get; set; }
    private string comment { get; set; }

    public Review_Sys(int raiting, string comment)
    {
        this.raiting = raiting;
        this.comment = comment;
    }

    public void LeaveReview(string comment = "", int raiting = 0)
    {
        bool isCorrectRaiting = false;

        while (!isCorrectRaiting)
        {
            try
            {
                if (raiting <= 1 || raiting >= 10)
                    throw new ArgumentOutOfRangeException(nameof(raiting), "Rating must be between 1 and 10.");

                this.raiting = raiting;
                this.comment = string.IsNullOrEmpty(comment) ? this.comment : comment;
                isCorrectRaiting = true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Please, enter a valid rating between 1 and 10: ");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int newRating))
                    raiting = newRating;
                else
                    Console.WriteLine("Please, enter a numeric value between 1 and 10: ");
            }
        }
    }
    
    public void DisplayReview()
    {
        Console.WriteLine($"Rating: {raiting}");
        Console.WriteLine($"Comment: {comment}");
    }
}