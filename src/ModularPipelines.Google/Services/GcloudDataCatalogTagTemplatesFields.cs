using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates")]
public class GcloudDataCatalogTagTemplatesFields
{
    public GcloudDataCatalogTagTemplatesFields(
        GcloudDataCatalogTagTemplatesFieldsEnumValues enumValues,
        ICommand internalCommand
    )
    {
        EnumValues = enumValues;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDataCatalogTagTemplatesFieldsEnumValues EnumValues { get; }

    public async Task<CommandResult> Create(GcloudDataCatalogTagTemplatesFieldsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudDataCatalogTagTemplatesFieldsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Rename(GcloudDataCatalogTagTemplatesFieldsRenameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudDataCatalogTagTemplatesFieldsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}