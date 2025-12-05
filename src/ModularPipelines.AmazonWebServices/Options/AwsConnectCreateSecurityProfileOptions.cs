using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-security-profile")]
public record AwsConnectCreateSecurityProfileOptions(
[property: CliOption("--security-profile-name")] string SecurityProfileName,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--permissions")]
    public string[]? Permissions { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--allowed-access-control-tags")]
    public IEnumerable<KeyValue>? AllowedAccessControlTags { get; set; }

    [CliOption("--tag-restricted-resources")]
    public string[]? TagRestrictedResources { get; set; }

    [CliOption("--applications")]
    public string[]? Applications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}