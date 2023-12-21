using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server")]
public class AzSqlServerAdvancedThreatProtectionSetting
{
    public AzSqlServerAdvancedThreatProtectionSetting(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzSqlServerAdvancedThreatProtectionSettingShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerAdvancedThreatProtectionSettingShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlServerAdvancedThreatProtectionSettingUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerAdvancedThreatProtectionSettingUpdateOptions(), token);
    }
}