using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

