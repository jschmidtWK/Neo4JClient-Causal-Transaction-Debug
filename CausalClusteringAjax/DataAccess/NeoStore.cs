using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Library.Neo4J;
using Neo4jClient;
using Neo4jClient.Cypher;
using Neo4jClient.Transactions;

namespace Library.Data
{
    /// <summary>
    /// This class handles the I/O to the Neo4J Database.
    /// </summary>
    internal static class NeoStore
    {
        static int sleepBeforeRetry = 500;
        static int numberOfRetry = 5;

        static BoltGraphClient cachedGraphClient;

        public static ITransaction BeginTransaction(TransactionScopeOption scopeOption = TransactionScopeOption.Join)
        {
            return GetGraphClient().BeginTransaction(scopeOption);
        }

        static BoltGraphClient GetGraphClient()
        {
                if (cachedGraphClient == null)
                {
                    cachedGraphClient = CoreGetGraphClient();
                }
                return cachedGraphClient;
        }

        static BoltGraphClient CoreGetGraphClient()
        {
            Uri uri = new Uri("");

            BoltGraphClient client = new BoltGraphClient(uri, "", "");
            int counter = 0;
            while (!client.IsConnected)
            {
                try
                {
                    client.Connect();
                }
                catch
                {
                    if (counter++ == 10)
                    {
                        break;
                    }
                }
            }
            return client;
        }

        /* TEMPLATE SECTION */

        internal static bool Save<T>(T entry, bool replaceEntry = false) where T : IDataObject
        {
            if (entry.Id == Guid.Empty)
            {
                entry.Id = Guid.NewGuid();
                return Create<T>(entry);
            }
            return Update<T>(entry, replaceEntry);
        }

        internal static bool Delete<T>(T entry) where T : IDataObject
        {
            BoltGraphClient graphClient = GetGraphClient();
            {
                var entryId = entry.Id;
                var inputString = $"(record:{entry.GetType().Name})";
                var query = graphClient.Cypher
                    .Match(inputString)
                    .Where((T record) => record.Id == entryId)
                    .Delete("record");

                int failed = 0;
                while (failed < numberOfRetry)
                {
                    try
                    {
                        query.ExecuteWithoutResults();
                        return true;
                    }
                    catch (Exception e)
                    {
                        failed++;
                        if (failed == numberOfRetry)
                        {
                            throw;
                        }
                        Thread.Sleep(sleepBeforeRetry);
                    }
                }
            }
            return false;
        }

        static bool Update<T>(T entry, bool replaceEntry = false) where T : IDataObject
        {
            BoltGraphClient graphClient = GetGraphClient();
            {
                var entryId = entry.Id;

                String updateMarker = replaceEntry ? "" : "+";

                var inputString = $"(record:{entry.GetType().Name})";

                var query = graphClient.Cypher
                    .Match(inputString)
                    .Where((T record) => record.Id == entryId)
                    .Set($"record {updateMarker}= {{updatedRecord}}")
                    .SetLabels("record", entry.Labels())
                    .WithParam("updatedRecord", entry);

                int failed = 0;
                while (failed < numberOfRetry)
                {
                    try
                    {
                        query.ExecuteWithoutResults();
                        return true;
                    }
                    catch (Exception e)
                    {
                        failed++;
                        if (failed == numberOfRetry)
                        {
                            throw;
                        }
                        Thread.Sleep(sleepBeforeRetry);
                    }
                }
            }
            return false;
        }

        internal static bool DetachDelete<T>(T entry) where T : IDataObject
        {
            BoltGraphClient graphClient = GetGraphClient();
            {
                var entryId = entry.Id;
                var inputString = string.Format("({0}:{1})", "record", entry.GetType().Name);
                var query = graphClient.Cypher
                    .Match(inputString)
                    .Where((T record) => record.Id == entryId)
                    .DetachDelete("record");
                int failed = 0;
                while (failed < numberOfRetry)
                {
                    try
                    {
                        query.ExecuteWithoutResults();
                        return true;
                    }
                    catch (Exception e)
                    {
                        failed++;
                        if (failed == numberOfRetry)
                        {
                            throw;
                        }
                        Thread.Sleep(sleepBeforeRetry);
                    }
                }
            }
            return false;
        }

        static bool Create<T>(T record) where T : IDataObject
        {
            BoltGraphClient graphClient = GetGraphClient();
            {
                var variableName = record.GetType().Name.ToLower();
                var inputString = $"({variableName}:{record.GetType().Name}";
                inputString += " { newRecord })";
                var query = graphClient.Cypher
                    .Create(inputString)
                    .SetLabels(variableName, record.Labels())
                    .WithParam("newRecord", record);
                int failed = 0;
                while (failed < numberOfRetry)
                {
                    try
                    {
                        query.ExecuteWithoutResults();
                        return true;
                    }
                    catch (Exception e)
                    {
                        failed++;
                        if (failed == numberOfRetry)
                        {
                            throw;
                        }
                        Thread.Sleep(sleepBeforeRetry);
                    }
                }
            }
            return false;
        }

        internal static T Get<T>(Guid identifier) where T : class, IDataObject
        {
            BoltGraphClient graphClient = GetGraphClient();
            {
                var inputString = string.Format("({0}:{1})", "record", typeof(T).Name);
                var query = graphClient.Cypher
                        .Match(inputString)
                        .Where((T record) => record.Id == identifier)
                        .Return(record => record.As<T>());
                int failed = 0;
                while (failed < numberOfRetry)
                {
                    try
                    {
                        return query.Results.FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        failed++;
                        if (failed == numberOfRetry)
                        {
                            throw;
                        }
                        Thread.Sleep(sleepBeforeRetry);
                    }
                }
            }
            return null;
        }

        internal static IList<T> GetAll<T>() where T : class, IDataObject
        {
            BoltGraphClient graphClient = GetGraphClient();
            {
                var query = graphClient.Cypher
                        .Match($"(record:{typeof(T).Name})")
                        .Return(record => record.As<T>());
                int failed = 0;
                while (failed < numberOfRetry)
                {
                    try
                    {
                        return query.Results.ToList();
                    }
                    catch (Exception e)
                    {
                        failed++;
                        if (failed == numberOfRetry)
                        {
                            throw;
                        }
                        Thread.Sleep(sleepBeforeRetry);
                    }
                }
            }
            return new List<T>();
        }

        internal static void AcquireWriteLock<T>(T entry) where T : IDataObject
        {
            if (GetGraphClient().InTransaction)
            {
                BoltGraphClient graphClient = GetGraphClient();
                {
                    Guid entryId = entry.Id;
                    graphClient.Cypher
                    .Match($"(node:{nameof(T)})")
                    .Where((T node) => node.Id == entryId)
                    .Set("node._WriteLock = 1")
                    .ExecuteWithoutResults();

                    graphClient.Cypher
                    .Match($"(node:{nameof(T)})")
                    .Where((T node) => node.Id == entryId)
                    .Remove("node._WriteLock")
                    .ExecuteWithoutResults();
                }
            }
        }

        public static ICypherFluentQuery SetLabels(this ICypherFluentQuery query, string variableName, HashSet<string> labels)
        {
            if (labels != null)
            {
                if (labels.Count > 0)
                {
                    string labelsString = " ";
                    foreach (string label in labels)
                    {
                        labelsString += $":{label}";
                    }
                    return query.Set(variableName + labelsString);
                }
            }
            return query;
        }
    }
}