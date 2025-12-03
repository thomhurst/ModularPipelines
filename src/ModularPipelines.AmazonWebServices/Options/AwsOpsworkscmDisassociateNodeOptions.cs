using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworkscm", "disassociate-node")]
public record AwsOpsworkscmDisassociateNodeOptions(
[property: CliOption("--server-name")] string ServerName,
[property: CliOption("--node-name")] string NodeName
) : AwsOptions
{
    [CliOption("--engine-attributes")]
    public string[]? EngineAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}