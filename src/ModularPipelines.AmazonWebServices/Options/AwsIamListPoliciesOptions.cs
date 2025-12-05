using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "list-policies")]
public record AwsIamListPoliciesOptions : AwsOptions
{
    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--path-prefix")]
    public string? PathPrefix { get; set; }

    [CliOption("--policy-usage-filter")]
    public string? PolicyUsageFilter { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}