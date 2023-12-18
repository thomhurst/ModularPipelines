using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "integration-runtime", "self-hosted", "create")]
public record AzDatafactoryIntegrationRuntimeSelfHostedCreateOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--integration-runtime-name")] string IntegrationRuntimeName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--linked-info")]
    public string? LinkedInfo { get; set; }
}