using System.Collections;
using Microsoft.Extensions.Logging;
using ModularPipelines.Constants;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using Spectre.Console;

namespace ModularPipelines.Engine.Executors;

internal class PipelineInitializer(
    IConsolePrinter consolePrinter,
    ModuleRetriever moduleRetriever,
    IDependencyChainProvider dependencyChainProvider,
    IRequirementChecker requirementsChecker,
    IDependencyDetector dependencyDetector,
    IPipelineSetupExecutor pipelineSetupExecutor,
    IBuildSystemDetector buildSystemDetector,
    IPipelineFileWriter pipelineFileWriter,
    ILogger<PipelineInitializer> logger,
    IConsoleWriter consoleWriter,
    ISecretObfuscator secretObfuscator) : IPipelineInitializer
{
    private static readonly string[] SensitiveEnvironmentVariableNameParts =
    [
        "TOKEN",
        "SECRET",
        "PASSWORD",
        "KEY",
        "PWD",
        "CREDENTIAL",
        "AUTH",
        "CONNECTION_STRING",
        "CONNECTIONSTRING",
        "CONNSTR",
    ];

    private static readonly string[] SensitiveEnvironmentVariableNames =
    [
        "AzureWebJobsStorage",
        "REDIS_URL",
    ];

    private static readonly Action<ILogger, BuildSystem, string, Exception?> LogDetectedBuildSystem =
        LoggerMessage.Define<BuildSystem, string>(
            LogLevel.Information,
            new EventId(1, nameof(LogDetectedBuildSystem)),
            "Build System: {BuildSystem} (detected from {EnvironmentVariable})");

    private static readonly Action<ILogger, BuildSystem, Exception?> LogBuildSystem =
        LoggerMessage.Define<BuildSystem>(
            LogLevel.Information,
            new EventId(2, nameof(LogBuildSystem)),
            "Build System: {BuildSystem}");

    private readonly IDependencyDetector _dependencyDetector = dependencyDetector;
    private readonly IRequirementChecker _requirementsChecker = requirementsChecker;
    private readonly ModuleRetriever _moduleRetriever = moduleRetriever;
    private readonly IDependencyChainProvider _dependencyChainProvider = dependencyChainProvider;
    private readonly IConsolePrinter _consolePrinter = consolePrinter;
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor = pipelineSetupExecutor;
    private readonly IBuildSystemDetector _buildSystemDetector = buildSystemDetector;
    private readonly IPipelineFileWriter _pipelineFileWriter = pipelineFileWriter;
    private readonly ILogger<PipelineInitializer> _logger = logger;
    private readonly IConsoleWriter _consoleWriter = consoleWriter;
    private readonly ISecretObfuscator _secretObfuscator = secretObfuscator;
    private OrganizedModules? _organizedModules;

    public async Task<OrganizedModules> Initialize(CancellationToken cancellationToken = default)
    {
        return _organizedModules ??= await InitializeInternal(cancellationToken).ConfigureAwait(false);
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
            var name = environmentVariable.Key?.ToString() ?? string.Empty;
            var value = environmentVariable.Value?.ToString() ?? string.Empty;
            var displayValue = IsSensitiveEnvironmentVariableName(name)
                ? LoggingConstants.SecretMask
                : MakeSingleLine(obfuscate(value));

            table.AddRow(
                new Text(name),
                new Text(displayValue).Ellipsis());
        }

        return table;
    }

    private static bool IsSensitiveEnvironmentVariableName(string name)
    {
        return !name.Equals("PWD", StringComparison.OrdinalIgnoreCase)
               && !name.Equals("OLDPWD", StringComparison.OrdinalIgnoreCase)
               && !name.Equals("SSH_AUTH_SOCK", StringComparison.OrdinalIgnoreCase)
               && (SensitiveEnvironmentVariableNames.Contains(name, StringComparer.OrdinalIgnoreCase)
                   || SensitiveEnvironmentVariableNameParts.Any(
                       part => name.Contains(part, StringComparison.OrdinalIgnoreCase)));
    }

    private static string MakeSingleLine(string value)
    {
        return value
            .Replace("\r", "\\r", StringComparison.Ordinal)
            .Replace("\n", "\\n", StringComparison.Ordinal);
    }

    private async Task<OrganizedModules> InitializeInternal(CancellationToken cancellationToken)
    {
        _consolePrinter.PrintLogo();

        PrintEnvironmentVariables();

        var buildSystem = _buildSystemDetector.Current;
        if (_buildSystemDetector.MatchedEnvironmentVariable is { } variable)
        {
            LogDetectedBuildSystem(_logger, buildSystem, variable, null);
        }
        else
        {
            LogBuildSystem(_logger, buildSystem, null);
        }

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
}
