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
    private readonly ModuleRetriever _moduleRetriever;
    private readonly IDependencyChainProvider _dependencyChainProvider;
    private readonly IConsolePrinter _consolePrinter;
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;
    private readonly IBuildSystemDetector _buildSystemDetector;
    private readonly IPipelineFileWriter _pipelineFileWriter;
    private readonly ILogger<PipelineInitializer> _logger;
    private OrganizedModules? _organizedModules;

    public PipelineInitializer(IConsolePrinter consolePrinter,
        ModuleRetriever moduleRetriever,
        IDependencyChainProvider dependencyChainProvider,
        IRequirementChecker requirementsChecker,
        IDependencyDetector dependencyDetector,
        IPipelineSetupExecutor pipelineSetupExecutor,
        IBuildSystemDetector buildSystemDetector,
        IPipelineFileWriter pipelineFileWriter,
        ILogger<PipelineInitializer> logger)
    {
        _consolePrinter = consolePrinter;
        _moduleRetriever = moduleRetriever;
        _dependencyChainProvider = dependencyChainProvider;
        _requirementsChecker = requirementsChecker;
        _dependencyDetector = dependencyDetector;
        _pipelineSetupExecutor = pipelineSetupExecutor;
        _buildSystemDetector = buildSystemDetector;
        _pipelineFileWriter = pipelineFileWriter;
        _logger = logger;
    }

    public async Task<OrganizedModules> Initialize(CancellationToken cancellationToken = default)
    {
        return _organizedModules ??= await InitializeInternal(cancellationToken).ConfigureAwait(false);
    }

    private async Task<OrganizedModules> InitializeInternal(CancellationToken cancellationToken)
    {
        _consolePrinter.PrintLogo();

        PrintEnvironmentVariables();

        _logger.LogInformation("Build System: {BuildSystem}",
            _buildSystemDetector.GetCurrentBuildSystem().ToString());

        await _pipelineFileWriter.WritePipelineFiles().ConfigureAwait(false);

        await _pipelineSetupExecutor.OnPipelineStartAsync().ConfigureAwait(false);

        await _requirementsChecker.CheckRequirementsAsync().ConfigureAwait(false);

        var organizedModules = await _moduleRetriever.GetOrganizedModules(cancellationToken).ConfigureAwait(false);
        _dependencyChainProvider.Initialize(organizedModules.AllModules);
        _dependencyDetector.Check();

        return organizedModules;
    }

    private void PrintEnvironmentVariables()
    {
        _logger.LogTrace(
            "Environment variables:\r\n{EnvironmentVariables}",
            FormatEnvironmentVariables(Environment.GetEnvironmentVariables()));
    }

    internal static string FormatEnvironmentVariables(IDictionary variables)
    {
        var environmentVariables = new StringBuilder();

        foreach (DictionaryEntry environmentVariable in variables)
        {
            environmentVariables.AppendLine($"{environmentVariable.Key}: {environmentVariable.Value}");
        }

        return environmentVariables.ToString();
    }
}
