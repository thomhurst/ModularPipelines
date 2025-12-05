using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identitystore", "update-group")]
public record AwsIdentitystoreUpdateGroupOptions(
[property: CliOption("--identity-store-id")] string IdentityStoreId,
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--operations")] string[] Operations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}