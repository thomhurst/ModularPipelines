using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "job", "create")]
public class AzBatchJobCreateAzureBatchCliExtensions
{
    public AzBatchJobCreateAzureBatchCliExtensions(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzBatchJobCreateAzureBatchCliExtensionsExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBatchJobCreateAzureBatchCliExtensionsExtensionOptions(), token);
    }
}