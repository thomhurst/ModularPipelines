using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "create-file-cache")]
public record AwsFsxCreateFileCacheOptions(
[property: CliOption("--file-cache-type")] string FileCacheType,
[property: CliOption("--file-cache-type-version")] string FileCacheTypeVersion,
[property: CliOption("--storage-capacity")] int StorageCapacity,
[property: CliOption("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--lustre-configuration")]
    public string? LustreConfiguration { get; set; }

    [CliOption("--data-repository-associations")]
    public string[]? DataRepositoryAssociations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}