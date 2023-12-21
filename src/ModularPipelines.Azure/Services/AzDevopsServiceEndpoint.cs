using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops")]
public class AzDevopsServiceEndpoint
{
    public AzDevopsServiceEndpoint(
        AzDevopsServiceEndpointAzurerm azurerm,
        AzDevopsServiceEndpointGithub github,
        ICommand internalCommand
    )
    {
        Azurerm = azurerm;
        Github = github;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDevopsServiceEndpointAzurerm Azurerm { get; }

    public AzDevopsServiceEndpointGithub Github { get; }

    public async Task<CommandResult> Create(AzDevopsServiceEndpointCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDevopsServiceEndpointDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDevopsServiceEndpointListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevopsServiceEndpointListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDevopsServiceEndpointShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzDevopsServiceEndpointUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}