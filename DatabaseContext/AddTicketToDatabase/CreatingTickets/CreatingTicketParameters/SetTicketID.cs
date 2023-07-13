﻿using System.Collections.Immutable;
using System.Text;

namespace DatabaseContext.AddTicketToDatabase.CreatingTickets.CreatingTicketParameters;

internal static class SetTicketID
{
    private static readonly StringBuilder _builderId = new StringBuilder();
    private static readonly ImmutableHashSet<long> _checkRepeatId = ImmutableHashSet<long>.Empty;
    private const int COUNT_OF_NUMBERS_IN_ID = 8;

    internal static long CreateTicketID()
    {
        for (int count = 0; count < COUNT_OF_NUMBERS_IN_ID; count++)
            _builderId.Append(new Random().Next(1, 9));

        long id = Convert.ToInt64(_builderId.ToString());

        _checkRepeatId.Add(id);
        _builderId.Clear();

        return id;
    }
}