using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearch", "update-scaling-parameters")]
public record AwsCloudsearchUpdateScalingParametersOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--scaling-parameters")] string ScalingParameters
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}