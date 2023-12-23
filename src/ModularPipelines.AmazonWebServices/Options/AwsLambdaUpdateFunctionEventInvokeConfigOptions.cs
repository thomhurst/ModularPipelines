using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "update-function-event-invoke-config")]
public record AwsLambdaUpdateFunctionEventInvokeConfigOptions(
[property: CommandSwitch("--function-name")] string FunctionName
) : AwsOptions
{
    [CommandSwitch("--qualifier")]
    public string? Qualifier { get; set; }

    [CommandSwitch("--maximum-retry-attempts")]
    public int? MaximumRetryAttempts { get; set; }

    [CommandSwitch("--maximum-event-age-in-seconds")]
    public int? MaximumEventAgeInSeconds { get; set; }

    [CommandSwitch("--destination-config")]
    public string? DestinationConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}