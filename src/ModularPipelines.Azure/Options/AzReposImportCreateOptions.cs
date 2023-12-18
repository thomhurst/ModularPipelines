using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "import", "create")]
public record AzReposImportCreateOptions(
[property: CommandSwitch("--git-source-url")] string GitSourceUrl
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--git-service-endpoint-id")]
    public string? GitServiceEndpointId { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [BooleanCommandSwitch("--requires-authorization")]
    public bool? RequiresAuthorization { get; set; }

    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }
}

