using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "create-endpoint")]
public record AwsComprehendCreateEndpointOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--desired-inference-units")] int DesiredInferenceUnits
) : AwsOptions
{
    [CommandSwitch("--model-arn")]
    public string? ModelArn { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CommandSwitch("--flywheel-arn")]
    public string? FlywheelArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}