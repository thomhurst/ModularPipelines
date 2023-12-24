using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworkscm", "disassociate-node")]
public record AwsOpsworkscmDisassociateNodeOptions(
[property: CommandSwitch("--server-name")] string ServerName,
[property: CommandSwitch("--node-name")] string NodeName
) : AwsOptions
{
    [CommandSwitch("--engine-attributes")]
    public string[]? EngineAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}