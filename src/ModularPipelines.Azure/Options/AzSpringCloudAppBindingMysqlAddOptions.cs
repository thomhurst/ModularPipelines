using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "app", "binding", "mysql", "add")]
public record AzSpringCloudAppBindingMysqlAddOptions(
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--key")] string Key,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--username")] string Username
) : AzOptions
{
}

