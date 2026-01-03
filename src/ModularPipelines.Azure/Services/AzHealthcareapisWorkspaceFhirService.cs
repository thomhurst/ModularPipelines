using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("healthcareapis", "workspace")]
public class AzHealthcareapisWorkspaceFhirService
{
    public AzHealthcareapisWorkspaceFhirService(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzHealthcareapisWorkspaceFhirServiceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzHealthcareapisWorkspaceFhirServiceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceFhirServiceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzHealthcareapisWorkspaceFhirServiceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzHealthcareapisWorkspaceFhirServiceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceFhirServiceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzHealthcareapisWorkspaceFhirServiceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceFhirServiceUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzHealthcareapisWorkspaceFhirServiceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHealthcareapisWorkspaceFhirServiceWaitOptions(), cancellationToken: token);
    }
}