using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-file-system-association")]
public record AwsStoragegatewayUpdateFileSystemAssociationOptions(
[property: CliOption("--file-system-association-arn")] string FileSystemAssociationArn
) : AwsOptions
{
    [CliOption("--user-name")]
    public string? UserName { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--audit-destination-arn")]
    public string? AuditDestinationArn { get; set; }

    [CliOption("--cache-attributes")]
    public string? CacheAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}