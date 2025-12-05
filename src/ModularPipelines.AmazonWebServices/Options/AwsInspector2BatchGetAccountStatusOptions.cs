using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "batch-get-account-status")]
public record AwsInspector2BatchGetAccountStatusOptions : AwsOptions
{
    [CliOption("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}