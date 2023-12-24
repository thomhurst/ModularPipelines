using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "update-file-system-association")]
public record AwsStoragegatewayUpdateFileSystemAssociationOptions(
[property: CommandSwitch("--file-system-association-arn")] string FileSystemAssociationArn
) : AwsOptions
{
    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--audit-destination-arn")]
    public string? AuditDestinationArn { get; set; }

    [CommandSwitch("--cache-attributes")]
    public string? CacheAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}