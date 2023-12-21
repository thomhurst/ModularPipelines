using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "midb")]
public class AzSqlMidbAdvancedThreatProtectionSetting
{
    public AzSqlMidbAdvancedThreatProtectionSetting(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzSqlMidbAdvancedThreatProtectionSettingShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMidbAdvancedThreatProtectionSettingShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlMidbAdvancedThreatProtectionSettingUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMidbAdvancedThreatProtectionSettingUpdateOptions(), token);
    }
}