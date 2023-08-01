using System.Text;

namespace DatabaseContext.AddTicketToDatabase.CreatingTickets.CreatingTicketParameters;

internal static class SetTicketID
{
    private static readonly StringBuilder _builderId = new StringBuilder();
    public static readonly List<long> _allId = new List<long>();
    private const int COUNT_OF_NUMBERS_IN_ID = 8;
    
    internal static long CreateID()
    {
        for (int count = 0; count < COUNT_OF_NUMBERS_IN_ID; count++)
            _builderId.Append(new Random().Next(1, 9));

        long id = Convert.ToInt64(_builderId.ToString());

        _allId.Add(id);
        _builderId.Clear();

        return _allId.Contains(id) ? id : CreateID();
    }
}