using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rbin", "list-rules")]
public record AwsRbinListRulesOptions(
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--resource-tags")]
    public string[]? ResourceTags { get; set; }

    [CliOption("--lock-state")]
    public string? LockState { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}