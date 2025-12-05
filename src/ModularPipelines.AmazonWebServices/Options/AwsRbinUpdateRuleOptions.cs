using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rbin", "update-rule")]
public record AwsRbinUpdateRuleOptions(
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--resource-tags")]
    public string[]? ResourceTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}