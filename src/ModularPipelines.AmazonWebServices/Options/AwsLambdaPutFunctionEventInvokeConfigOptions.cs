using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "put-function-event-invoke-config")]
public record AwsLambdaPutFunctionEventInvokeConfigOptions(
[property: CliOption("--function-name")] string FunctionName
) : AwsOptions
{
    [CliOption("--qualifier")]
    public string? Qualifier { get; set; }

    [CliOption("--maximum-retry-attempts")]
    public int? MaximumRetryAttempts { get; set; }

    [CliOption("--maximum-event-age-in-seconds")]
    public int? MaximumEventAgeInSeconds { get; set; }

    [CliOption("--destination-config")]
    public string? DestinationConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}