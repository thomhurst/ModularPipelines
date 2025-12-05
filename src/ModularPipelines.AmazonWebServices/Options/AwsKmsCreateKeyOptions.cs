using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "create-key")]
public record AwsKmsCreateKeyOptions : AwsOptions
{
    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--key-usage")]
    public string? KeyUsage { get; set; }

    [CliOption("--customer-master-key-spec")]
    public string? CustomerMasterKeySpec { get; set; }

    [CliOption("--key-spec")]
    public string? KeySpec { get; set; }

    [CliOption("--origin")]
    public string? Origin { get; set; }

    [CliOption("--custom-key-store-id")]
    public string? CustomKeyStoreId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--xks-key-id")]
    public string? XksKeyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}