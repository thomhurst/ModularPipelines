using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quantum", "workspace", "clear")]
public record AzQuantumWorkspaceClearOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-account")] int StorageAccount,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--auto-accept")]
    public bool? AutoAccept { get; set; }

    [CommandSwitch("--provider-sku-list")]
    public string? ProviderSkuList { get; set; }

    [BooleanCommandSwitch("--skip-autoadd")]
    public bool? SkipAutoadd { get; set; }

    [BooleanCommandSwitch("--skip-role-assignment")]
    public bool? SkipRoleAssignment { get; set; }
}