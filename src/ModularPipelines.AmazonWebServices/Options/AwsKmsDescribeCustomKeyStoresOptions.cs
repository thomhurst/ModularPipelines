using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "describe-custom-key-stores")]
public record AwsKmsDescribeCustomKeyStoresOptions : AwsOptions
{
    [CliOption("--custom-key-store-id")]
    public string? CustomKeyStoreId { get; set; }

    [CliOption("--custom-key-store-name")]
    public string? CustomKeyStoreName { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}