using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Build.Modules;

[DependsOn<RunUnitTestsModule>]
public class CodacyCodeCoverageUploader : Module<CommandResult>
{
    private readonly IOptions<CodacySettings> _options;

    public CodacyCodeCoverageUploader(IOptions<CodacySettings> options)
    {
        _options = options;
    }
    
    protected override async Task<ModuleResult<CommandResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return await context.Bash.Command(
            new BashCommandOptions("<(curl -Ls https://coverage.codacy.com/get.sh)")
            {
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["CODACY_PROJECT_TOKEN"] = _options.Value.ApiKey
                }
            },
            cancellationToken);
    }
}