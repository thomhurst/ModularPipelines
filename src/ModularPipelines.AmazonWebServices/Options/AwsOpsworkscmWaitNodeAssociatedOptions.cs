using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworkscm", "wait", "node-associated")]
public record AwsOpsworkscmWaitNodeAssociatedOptions(
[property: CliOption("--node-association-status-token")] string NodeAssociationStatusToken,
[property: CliOption("--server-name")] string ServerName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}