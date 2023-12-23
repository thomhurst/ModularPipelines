using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworkscm", "associate-node")]
public record AwsOpsworkscmAssociateNodeOptions(
[property: CommandSwitch("--server-name")] string ServerName,
[property: CommandSwitch("--node-name")] string NodeName,
[property: CommandSwitch("--engine-attributes")] string[] EngineAttributes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}