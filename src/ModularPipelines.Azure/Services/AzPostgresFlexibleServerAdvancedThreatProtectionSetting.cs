using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "flexible-server")]
public class AzPostgresFlexibleServerAdvancedThreatProtectionSetting
{
    public AzPostgresFlexibleServerAdvancedThreatProtectionSetting(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzPostgresFlexibleServerAdvancedThreatProtectionSettingShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresFlexibleServerAdvancedThreatProtectionSettingShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzPostgresFlexibleServerAdvancedThreatProtectionSettingUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}