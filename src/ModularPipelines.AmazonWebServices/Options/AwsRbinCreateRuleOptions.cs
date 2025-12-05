using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rbin", "create-rule")]
public record AwsRbinCreateRuleOptions(
[property: CliOption("--retention-period")] string RetentionPeriod,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--resource-tags")]
    public string[]? ResourceTags { get; set; }

    [CliOption("--lock-configuration")]
    public string? LockConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}