using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("apic")]
public class AzApicApi
{
    public AzApicApi(
        AzApicApiDefinition definition,
        AzApicApiDeployment deployment,
        AzApicApiVersion version,
        ICommand internalCommand
    )
    {
        Definition = definition;
        Deployment = deployment;
        Version = version;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzApicApiDefinition Definition { get; }

    public AzApicApiDeployment Deployment { get; }

    public AzApicApiVersion Version { get; }

    public async Task<CommandResult> Create(AzApicApiCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzApicApiDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicApiDeleteOptions(), token);
    }

    public async Task<CommandResult> Head(AzApicApiHeadOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicApiHeadOptions(), token);
    }

    public async Task<CommandResult> List(AzApicApiListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Register(AzApicApiRegisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzApicApiShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicApiShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzApicApiUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicApiUpdateOptions(), token);
    }
}