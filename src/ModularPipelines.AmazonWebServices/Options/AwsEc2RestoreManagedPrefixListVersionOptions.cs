using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "restore-managed-prefix-list-version")]
public record AwsEc2RestoreManagedPrefixListVersionOptions(
[property: CliOption("--prefix-list-id")] string PrefixListId,
[property: CliOption("--previous-version")] long PreviousVersion,
[property: CliOption("--current-version")] long CurrentVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}