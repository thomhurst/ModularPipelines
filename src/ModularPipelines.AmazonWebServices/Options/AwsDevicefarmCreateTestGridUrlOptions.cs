using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "create-test-grid-url")]
public record AwsDevicefarmCreateTestGridUrlOptions(
[property: CommandSwitch("--project-arn")] string ProjectArn,
[property: CommandSwitch("--expires-in-seconds")] int ExpiresInSeconds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}