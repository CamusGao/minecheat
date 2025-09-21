namespace Minecheat;

public class Log(string message, DateTime? timestamp = null)
{
    public string Message { get; private set; } = message;

    public DateTime Timestamp { get; private set; } = timestamp ?? DateTime.Now;
}
