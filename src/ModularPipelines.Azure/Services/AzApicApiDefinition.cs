using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("apic", "api")]
public class AzApicApiDefinition
{
    public AzApicApiDefinition(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzApicApiDefinitionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzApicApiDefinitionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicApiDefinitionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ExportSpecification(AzApicApiDefinitionExportSpecificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Head(AzApicApiDefinitionHeadOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicApiDefinitionHeadOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ImportSpecification(AzApicApiDefinitionImportSpecificationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicApiDefinitionImportSpecificationOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzApicApiDefinitionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzApicApiDefinitionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicApiDefinitionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzApicApiDefinitionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicApiDefinitionUpdateOptions(), cancellationToken: token);
    }
}