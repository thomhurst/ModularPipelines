using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "set-definition", "create")]
public record AzPolicySetDefinitionCreateOptions(
[property: CommandSwitch("--definitions")] string Definitions,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--definition-groups")]
    public string? DefinitionGroups { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--management-group")]
    public string? ManagementGroup { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--params")]
    public string? Params { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}