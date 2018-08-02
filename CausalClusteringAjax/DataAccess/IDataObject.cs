using System;
using System.Collections.Generic;

namespace Library.Neo4J
{
    /// <summary>
    /// The interface of (almost) every object pushed to Neo4J
    /// </summary>
    /// <seealso cref="DataObjectImpl"/>>
    public interface IDataObject
    {
        Guid Id { get; set; }

        DateTime TimeCreated { get; }

        DateTime TimeLastModified { get; }

        bool Update(bool replaceEntry = false);

        bool Delete();

        HashSet<string> Labels();
    }
}
