using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworkscm", "update-server-engine-attributes")]
public record AwsOpsworkscmUpdateServerEngineAttributesOptions(
[property: CliOption("--server-name")] string ServerName,
[property: CliOption("--attribute-name")] string AttributeName
) : AwsOptions
{
    [CliOption("--attribute-value")]
    public string? AttributeValue { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}