using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glacier", "list-jobs")]
public record AwsGlacierListJobsOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--vault-name")] string VaultName
) : AwsOptions
{
    [CliOption("--statuscode")]
    public string? Statuscode { get; set; }

    [CliOption("--completed")]
    public string? Completed { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}