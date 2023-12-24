using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("health", "describe-event-details")]
public record AwsHealthDescribeEventDetailsOptions(
[property: CommandSwitch("--event-arns")] string[] EventArns
) : AwsOptions
{
    [CommandSwitch("--locale")]
    public string? Locale { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}