using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-managed-prefix-list")]
public record AwsEc2CreateManagedPrefixListOptions(
[property: CliOption("--prefix-list-name")] string PrefixListName,
[property: CliOption("--max-entries")] int MaxEntries,
[property: CliOption("--address-family")] string AddressFamily
) : AwsOptions
{
    [CliOption("--entries")]
    public string[]? Entries { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}