using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-managed-prefix-list")]
public record AwsEc2ModifyManagedPrefixListOptions(
[property: CliOption("--prefix-list-id")] string PrefixListId
) : AwsOptions
{
    [CliOption("--current-version")]
    public long? CurrentVersion { get; set; }

    [CliOption("--prefix-list-name")]
    public string? PrefixListName { get; set; }

    [CliOption("--add-entries")]
    public string[]? AddEntries { get; set; }

    [CliOption("--remove-entries")]
    public string[]? RemoveEntries { get; set; }

    [CliOption("--max-entries")]
    public int? MaxEntries { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}