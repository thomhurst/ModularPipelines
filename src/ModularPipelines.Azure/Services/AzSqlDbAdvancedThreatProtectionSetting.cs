using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "db")]
public class AzSqlDbAdvancedThreatProtectionSetting
{
    public AzSqlDbAdvancedThreatProtectionSetting(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzSqlDbAdvancedThreatProtectionSettingShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbAdvancedThreatProtectionSettingShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSqlDbAdvancedThreatProtectionSettingUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDbAdvancedThreatProtectionSettingUpdateOptions(), token);
    }
}