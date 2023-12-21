using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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