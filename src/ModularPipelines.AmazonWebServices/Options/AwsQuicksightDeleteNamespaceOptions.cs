using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "delete-namespace")]
public record AwsQuicksightDeleteNamespaceOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--namespace")] string Namespace
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}