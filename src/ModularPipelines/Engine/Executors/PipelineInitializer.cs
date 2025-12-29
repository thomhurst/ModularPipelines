using System.Collections;
using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

internal class PipelineInitializer : IPipelineInitializer
{
    private readonly IDependencyDetector _dependencyDetector;
    private readonly IRequirementChecker _requirementsChecker;
    private readonly IModuleRetriever _moduleRetriever;
    private readonly IConsolePrinter _consolePrinter;
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IBuildSystemDetector _buildSystemDetector;
    private readonly IPipelineFileWriter _pipelineFileWriter;
    private readonly ILogger<PipelineInitializer> _logger;
    private OrganizedModules? _organizedModules;

    public PipelineInitializer(IConsolePrinter consolePrinter,
        IModuleRetriever moduleRetriever,
        IRequirementChecker requirementsChecker,
        IDependencyDetector dependencyDetector,
        IPipelineSetupExecutor pipelineSetupExecutor,
        IBuildSystemDetector buildSystemDetector,
        IPipelineFileWriter pipelineFileWriter,
        ILogger<PipelineInitializer> logger)
    {
        _consolePrinter = consolePrinter;
        _moduleRetriever = moduleRetriever;
        _requirementsChecker = requirementsChecker;
        _dependencyDetector = dependencyDetector;
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _buildSystemDetector = buildSystemDetector;
        _pipelineFileWriter = pipelineFileWriter;
        _logger = logger;
    }

    public async Task<OrganizedModules> Initialize()
    {
        return _organizedModules ??= await InitializeInternal().ConfigureAwait(false);
    }

    private async Task<OrganizedModules> InitializeInternal()
    {
        _consolePrinter.PrintLogo();

        PrintEnvironmentVariables();

        _logger.LogInformation("{Header} {BuildSystem}",
            MarkupFormatter.FormatHeader("Build System"),
            MarkupFormatter.FormatModuleName(_buildSystemDetector.GetCurrentBuildSystem().ToString()));

        await _pipelineFileWriter.WritePipelineFiles().ConfigureAwait(false);

        _dependencyDetector.Check();

        await _pipelineSetupExecutor.OnStartAsync().ConfigureAwait(false);

        await _requirementsChecker.CheckRequirementsAsync().ConfigureAwait(false);

        return await _moduleRetriever.GetOrganizedModules().ConfigureAwait(false);
    }

    private void PrintEnvironmentVariables()
    {
        var environmentVariables = new StringBuilder();

        foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
        {
            environmentVariables.AppendLine($"{environmentVariable.Key}: {environmentVariable.Value}");
            environmentVariables.AppendLine();
        }

        _logger.LogTrace("Environment variables:\r\n{EnvironmentVariables}", environmentVariables);
    }
}