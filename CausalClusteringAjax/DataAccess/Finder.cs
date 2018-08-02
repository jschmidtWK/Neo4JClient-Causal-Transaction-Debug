using System;
using System.Collections.Generic;
using Library.Data;

namespace Library
{
    public static class Finder
    {
        public static Neo4jClient.Transactions.ITransaction BeginNeo4JTransaction(Neo4jClient.Transactions.TransactionScopeOption option = Neo4jClient.Transactions.TransactionScopeOption.Join) => NeoStore.BeginTransaction(option);
    }
}
