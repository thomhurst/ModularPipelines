using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "update-acl")]
public record AwsMemorydbUpdateAclOptions(
[property: CommandSwitch("--acl-name")] string AclName
) : AwsOptions
{
    [CommandSwitch("--user-names-to-add")]
    public string[]? UserNamesToAdd { get; set; }

    [CommandSwitch("--user-names-to-remove")]
    public string[]? UserNamesToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}