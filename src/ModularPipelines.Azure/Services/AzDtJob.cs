using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt")]
public class AzDtJob
{
    public AzDtJob(
        AzDtJobDeletion deletion,
        AzDtJobImport import
    )
    {
        Deletion = deletion;
        Import = import;
    }

    public AzDtJobDeletion Deletion { get; }

    public AzDtJobImport Import { get; }
}