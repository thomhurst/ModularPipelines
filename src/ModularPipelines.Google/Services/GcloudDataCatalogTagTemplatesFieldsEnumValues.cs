using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates", "fields")]
public class GcloudDataCatalogTagTemplatesFieldsEnumValues
{
    public GcloudDataCatalogTagTemplatesFieldsEnumValues(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Rename(GcloudDataCatalogTagTemplatesFieldsEnumValuesRenameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}