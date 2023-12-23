using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "describe-app-version-app-component")]
public record AwsResiliencehubDescribeAppVersionAppComponentOptions(
[property: CommandSwitch("--app-arn")] string AppArn,
[property: CommandSwitch("--app-version")] string AppVersion,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}