using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("vm")]
public class AzVmBootDiagnostics
{
    public AzVmBootDiagnostics(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Disable(AzVmBootDiagnosticsDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmBootDiagnosticsDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(AzVmBootDiagnosticsEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmBootDiagnosticsEnableOptions(), token);
    }

    public async Task<CommandResult> GetBootLog(AzVmBootDiagnosticsGetBootLogOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmBootDiagnosticsGetBootLogOptions(), token);
    }

    public async Task<CommandResult> GetBootLogUris(AzVmBootDiagnosticsGetBootLogUrisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmBootDiagnosticsGetBootLogUrisOptions(), token);
    }
}