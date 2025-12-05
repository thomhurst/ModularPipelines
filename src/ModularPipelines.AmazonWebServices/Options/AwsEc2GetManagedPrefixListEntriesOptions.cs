using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "get-managed-prefix-list-entries")]
public record AwsEc2GetManagedPrefixListEntriesOptions(
[property: CliOption("--prefix-list-id")] string PrefixListId
) : AwsOptions
{
    [CliOption("--target-version")]
    public long? TargetVersion { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}