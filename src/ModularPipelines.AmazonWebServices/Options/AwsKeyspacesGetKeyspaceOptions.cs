using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyspaces", "get-keyspace")]
public record AwsKeyspacesGetKeyspaceOptions(
[property: CommandSwitch("--keyspace-name")] string KeyspaceName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}