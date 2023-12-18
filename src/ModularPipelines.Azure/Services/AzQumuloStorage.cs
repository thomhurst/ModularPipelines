using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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