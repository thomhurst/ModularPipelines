using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-namespace")]
public record AwsQuicksightCreateNamespaceOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--identity-store")] string IdentityStore
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}