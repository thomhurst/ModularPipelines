using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworkscm", "wait", "node-associated")]
public record AwsOpsworkscmWaitNodeAssociatedOptions(
[property: CommandSwitch("--node-association-status-token")] string NodeAssociationStatusToken,
[property: CommandSwitch("--server-name")] string ServerName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}