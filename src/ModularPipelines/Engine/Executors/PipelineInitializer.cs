using System.Collections;
using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using Spectre.Console;

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
    private readonly IConsoleWriter _consoleWriter;
    private readonly ISecretObfuscator _secretObfuscator;
    private OrganizedModules? _organizedModules;

    public PipelineInitializer(IConsolePrinter consolePrinter,
        ModuleRetriever moduleRetriever,
        IDependencyChainProvider dependencyChainProvider,
        IRequirementChecker requirementsChecker,
        IDependencyDetector dependencyDetector,
        IPipelineSetupExecutor pipelineSetupExecutor,
        IBuildSystemDetector buildSystemDetector,
        IPipelineFileWriter pipelineFileWriter,
        ILogger<PipelineInitializer> logger,
        IConsoleWriter consoleWriter,
        ISecretObfuscator secretObfuscator)
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
        _consoleWriter = consoleWriter;
        _secretObfuscator = secretObfuscator;
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
        if (!_logger.IsEnabled(LogLevel.Trace))
        {
            return;
        }

        _consoleWriter.Write(CreateEnvironmentVariablesTable(
            Environment.GetEnvironmentVariables(),
            value => _secretObfuscator.Obfuscate(value, null)));
    }

    internal static Table CreateEnvironmentVariablesTable(
        IDictionary variables,
        Func<string, string> obfuscate)
    {
        var table = new Table
        {
            Border = TableBorder.Rounded,
            Expand = true,
            Title = new TableTitle("[bold]Environment variables[/]"),
        };

        table.AddColumn(new TableColumn("[bold]Name[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Value[/]").LeftAligned());

        foreach (var environmentVariable in variables
                     .Cast<DictionaryEntry>()
                     .OrderBy(entry => entry.Key?.ToString(), StringComparer.OrdinalIgnoreCase))
        {
            table.AddRow(
                Markup.Escape(environmentVariable.Key?.ToString() ?? string.Empty),
                Markup.Escape(obfuscate(environmentVariable.Value?.ToString() ?? string.Empty)));
        }

        return table;
    }
}
