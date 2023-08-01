using DatabaseContext.AddTicketToDatabase.CreatingTickets;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseContext.AddTicketToDatabase;

internal class AddTicket
{
    private const string PROCEDURE_NAME = "sp_InsertTickets";
    private const string CONNECTION_STRING =
        @"Server=localhost\SQLEXPRESS;Database=LotteryTickets;Trusted_Connection=True;";

    internal static void Add()
    {
        List<TicketEntity> tickets = CreatingTicket.CreateTicket(1);

        using SqlConnection connection = new SqlConnection(CONNECTION_STRING);
        using SqlCommand command = new SqlCommand(PROCEDURE_NAME, connection);
        connection.Open();

        command.CommandType = CommandType.StoredProcedure;

        for (int index = 0; index < tickets.Count; index++)
        {
            command.Parameters.AddWithValue("@Id", tickets[index].Id);
            command.Parameters.AddWithValue("@Status", tickets[index].Status);
            command.Parameters.AddWithValue("@WinningAmount", tickets[index].WinningAmount);

            command.ExecuteNonQuery();
        }
        
        connection.Close();
    }

    internal static void AddRange(int countAddTickets)
    {
        for (int index = 0; index < countAddTickets; index++)
            Add();
    }
}