using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "wait", "service-instance-deployed")]
public record AwsProtonWaitServiceInstanceDeployedOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--service-name")] string ServiceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}