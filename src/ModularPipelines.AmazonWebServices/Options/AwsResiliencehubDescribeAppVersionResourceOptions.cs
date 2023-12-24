using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "describe-app-version-resource")]
public record AwsResiliencehubDescribeAppVersionResourceOptions(
[property: CommandSwitch("--app-arn")] string AppArn,
[property: CommandSwitch("--app-version")] string AppVersion
) : AwsOptions
{
    [CommandSwitch("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CommandSwitch("--aws-region")]
    public string? AwsRegion { get; set; }

    [CommandSwitch("--logical-resource-id")]
    public string? LogicalResourceId { get; set; }

    [CommandSwitch("--physical-resource-id")]
    public string? PhysicalResourceId { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}