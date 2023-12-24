using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "describe-app-version-resources-resolution-status")]
public record AwsResiliencehubDescribeAppVersionResourcesResolutionStatusOptions(
[property: CommandSwitch("--app-arn")] string AppArn,
[property: CommandSwitch("--app-version")] string AppVersion
) : AwsOptions
{
    [CommandSwitch("--resolution-id")]
    public string? ResolutionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}