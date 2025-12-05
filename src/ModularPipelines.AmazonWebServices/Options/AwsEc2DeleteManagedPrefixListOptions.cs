using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-managed-prefix-list")]
public record AwsEc2DeleteManagedPrefixListOptions(
[property: CliOption("--prefix-list-id")] string PrefixListId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}