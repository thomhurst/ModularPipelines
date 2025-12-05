using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "create-endpoint")]
public record AwsComprehendCreateEndpointOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--desired-inference-units")] int DesiredInferenceUnits
) : AwsOptions
{
    [CliOption("--model-arn")]
    public string? ModelArn { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CliOption("--flywheel-arn")]
    public string? FlywheelArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}