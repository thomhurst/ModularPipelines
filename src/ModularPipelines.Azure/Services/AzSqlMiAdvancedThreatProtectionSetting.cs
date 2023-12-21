using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi")]
public class AzSqlMiAdvancedThreatProtectionSetting
{
    public AzSqlMiAdvancedThreatProtectionSetting(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzSqlMiAdvancedThreatProtectionSettingShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiAdvancedThreatProtectionSettingShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlMiAdvancedThreatProtectionSettingUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiAdvancedThreatProtectionSettingUpdateOptions(), token);
    }
}