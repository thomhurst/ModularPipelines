using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworkscm", "associate-node")]
public record AwsOpsworkscmAssociateNodeOptions(
[property: CliOption("--server-name")] string ServerName,
[property: CliOption("--node-name")] string NodeName,
[property: CliOption("--engine-attributes")] string[] EngineAttributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}