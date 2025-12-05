using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "start-read-set-import-job")]
public record AwsOmicsStartReadSetImportJobOptions(
[property: CliOption("--sequence-store-id")] string SequenceStoreId,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--sources")] string[] Sources
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}