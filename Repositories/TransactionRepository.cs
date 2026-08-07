using MusicStorePos.Models;

namespace MusicStorePos.Repositories;

public static class TransactionRepository
{
    public static List<Transaction> Transactions { get; set; } = new();
}
