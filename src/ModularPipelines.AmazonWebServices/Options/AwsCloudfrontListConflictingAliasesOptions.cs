using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "list-conflicting-aliases")]
public record AwsCloudfrontListConflictingAliasesOptions(
[property: CommandSwitch("--distribution-id")] string DistributionId,
[property: CommandSwitch("--alias")] string Alias
) : AwsOptions
{
    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}