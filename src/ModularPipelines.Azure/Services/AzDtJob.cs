using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt")]
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