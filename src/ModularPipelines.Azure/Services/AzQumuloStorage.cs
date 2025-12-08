using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("qumulo")]
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