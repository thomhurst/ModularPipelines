using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apic", "api", "definition", "import-specification")]
public record AzApicApiDefinitionImportSpecificationOptions(
[property: CommandSwitch("--api")] string Api,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [CommandSwitch("--definition")]
    public string? Definition { get; set; }

    [CommandSwitch("--file-name")]
    public string? FileName { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--specification")]
    public string? Specification { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}

