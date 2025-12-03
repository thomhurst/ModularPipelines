using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "update-endpoint")]
public record AwsComprehendUpdateEndpointOptions(
[property: CliOption("--endpoint-arn")] string EndpointArn
) : AwsOptions
{
    [CliOption("--desired-model-arn")]
    public string? DesiredModelArn { get; set; }

    [CliOption("--desired-inference-units")]
    public int? DesiredInferenceUnits { get; set; }

    [CliOption("--desired-data-access-role-arn")]
    public string? DesiredDataAccessRoleArn { get; set; }

    [CliOption("--flywheel-arn")]
    public string? FlywheelArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}