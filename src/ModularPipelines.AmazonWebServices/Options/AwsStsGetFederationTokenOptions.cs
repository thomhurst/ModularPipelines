using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sts", "get-federation-token")]
public record AwsStsGetFederationTokenOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--policy-arns")]
    public string[]? PolicyArns { get; set; }

    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}