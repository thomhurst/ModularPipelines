using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "create-acl")]
public record AwsMemorydbCreateAclOptions(
[property: CliOption("--acl-name")] string AclName
) : AwsOptions
{
    [CliOption("--user-names")]
    public string[]? UserNames { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}