using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware")]
public class AzVmwareAddon
{
    public AzVmwareAddon(
        AzVmwareAddonArc arc,
        AzVmwareAddonHcx hcx,
        AzVmwareAddonSrm srm,
        AzVmwareAddonVr vr,
        ICommand internalCommand
    )
    {
        Arc = arc;
        Hcx = hcx;
        Srm = srm;
        Vr = vr;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzVmwareAddonArc Arc { get; }

    public AzVmwareAddonHcx Hcx { get; }

    public AzVmwareAddonSrm Srm { get; }

    public AzVmwareAddonVr Vr { get; }

    public async Task<CommandResult> List(AzVmwareAddonListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

