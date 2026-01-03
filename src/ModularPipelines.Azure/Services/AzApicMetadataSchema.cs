using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("apic")]
public class AzApicMetadataSchema
{
    public AzApicMetadataSchema(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzApicMetadataSchemaCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzApicMetadataSchemaDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicMetadataSchemaDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ExportMetadataSchema(AzApicMetadataSchemaExportMetadataSchemaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Head(AzApicMetadataSchemaHeadOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicMetadataSchemaHeadOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzApicMetadataSchemaListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzApicMetadataSchemaShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicMetadataSchemaShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzApicMetadataSchemaUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzApicMetadataSchemaUpdateOptions(), cancellationToken: token);
    }
}