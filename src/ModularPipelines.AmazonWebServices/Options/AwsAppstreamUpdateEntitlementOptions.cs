using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "update-entitlement")]
public record AwsAppstreamUpdateEntitlementOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--stack-name")] string StackName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--app-visibility")]
    public string? AppVisibility { get; set; }

    [CliOption("--attributes")]
    public string[]? Attributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}