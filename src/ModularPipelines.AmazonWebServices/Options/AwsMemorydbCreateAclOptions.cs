using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "create-acl")]
public record AwsMemorydbCreateAclOptions(
[property: CommandSwitch("--acl-name")] string AclName
) : AwsOptions
{
    [CommandSwitch("--user-names")]
    public string[]? UserNames { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}