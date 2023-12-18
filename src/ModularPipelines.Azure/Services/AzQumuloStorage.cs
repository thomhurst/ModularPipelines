using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qumulo")]
public class AzQumuloStorage
{
    public AzQumuloStorage(
        AzQumuloStorageFileSystem fileSystem
    )
    {
        FileSystem = fileSystem;
    }

    public AzQumuloStorageFileSystem FileSystem { get; }
}

