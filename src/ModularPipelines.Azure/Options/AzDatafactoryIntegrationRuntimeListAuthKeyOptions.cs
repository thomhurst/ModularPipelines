using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "integration-runtime", "list-auth-key")]
public record AzDatafactoryIntegrationRuntimeListAuthKeyOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--integration-runtime-name")] string IntegrationRuntimeName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--key-name")]
    public string? KeyName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}