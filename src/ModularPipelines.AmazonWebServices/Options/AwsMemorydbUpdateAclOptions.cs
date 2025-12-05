using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "update-acl")]
public record AwsMemorydbUpdateAclOptions(
[property: CliOption("--acl-name")] string AclName
) : AwsOptions
{
    [CliOption("--user-names-to-add")]
    public string[]? UserNamesToAdd { get; set; }

    [CliOption("--user-names-to-remove")]
    public string[]? UserNamesToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}