using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "delete-access")]
public record AwsTransferDeleteAccessOptions(
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--external-id")] string ExternalId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}