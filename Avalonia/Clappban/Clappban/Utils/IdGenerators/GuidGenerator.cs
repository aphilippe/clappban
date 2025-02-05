using System;

namespace Clappban.Utils.IdGenerators;

public class GuidGenerator : IIdGenerator
{
    public string Generate()
    {
        var guid = Guid.NewGuid();
        return guid.ToString();
    }
}