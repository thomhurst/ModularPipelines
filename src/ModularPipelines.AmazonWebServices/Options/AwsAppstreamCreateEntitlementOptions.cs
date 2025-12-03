using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-entitlement")]
public record AwsAppstreamCreateEntitlementOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--stack-name")] string StackName,
[property: CliOption("--app-visibility")] string AppVisibility,
[property: CliOption("--attributes")] string[] Attributes
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}