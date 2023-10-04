using System;

public sealed class DateTimeProvider
{
    private static readonly DateTimeProvider instance = new DateTimeProvider();

    private DateTimeProvider() { }

    public static DateTimeProvider Instance
    {
        get { return instance; }
    }

    public DateTime CurrentDate { get; private set; }
    public TimeSpan CurrentTime { get; private set; }

    public void Update()
    {
        // Fetch and update the current date and time
        CurrentDate = DateTime.Now.Date;
        CurrentTime = DateTime.Now.TimeOfDay;
    }
}
