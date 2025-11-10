namespace Shannon;

static class UserInput
{
    public static int AskLimit(int available)
    {
        while (true)
        {
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int limit))
            {
                ConsoleText.ErrorNotNumber();
                continue;
            }

            if (limit < 2)
            {
                ConsoleText.ErrorTooSmall();
                continue;
            }

            if (limit > available)
            {
                ConsoleText.WarnTooLarge(available);
                return available;
            }

            return limit;
        }
    }
}