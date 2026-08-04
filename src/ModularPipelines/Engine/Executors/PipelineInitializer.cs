using System.Collections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Constants;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Spectre.Console;
using Spectre.Console.Rendering;

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
    ISecretObfuscator secretObfuscator,
    IOptions<SecretMaskingOptions> secretMaskingOptions) : IPipelineInitializer
{
    // Two outer borders, one separator, and one space of padding on each side of both columns.
    private const int TableDecorationWidth = 7;

    private static readonly string[] SensitiveEnvironmentVariableNameParts =
    [
        "TOKEN",
        "SECRET",
        "PASSWORD",
        "PASSWD",
        "PASSPHRASE",
        "KEY",
        "PWD",
        "CREDENTIAL",
        "AUTH",
        "CONNECTION_STRING",
        "CONNECTIONSTRING",
        "CONNSTR",
    ];

    private static readonly string[] SensitiveEnvironmentVariableDelimitedNameParts =
    [
        "PASS",
        "PIN",
    ];

    private static readonly string[] SensitiveEnvironmentVariableNames =
    [
        "AZURE_DEVOPS_EXT_PAT",
        "AzureWebJobsStorage",
        "ALL_PROXY",
        "CLOUDAMQP_URL",
        "DATABASE_URL",
        "HTTP_PROXY",
        "HTTPS_PROXY",
        "IDENTITY_HEADER",
        "MONGODB_URI",
        "PIP_EXTRA_INDEX_URL",
        "PIP_INDEX_URL",
        "REDIS_URL",
        "SLACK_WEBHOOK_URL",
        "VCAP_SERVICES",
        "VSS_NUGET_EXTERNAL_FEED_ENDPOINTS",
    ];

    private static readonly string[] NonSensitiveEnvironmentVariableNames =
    [
        "APPVEYOR_REPO_COMMIT_AUTHOR",
        "APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL",
        "AWS_CONTAINER_AUTHORIZATION_TOKEN_FILE",
        "AWS_SHARED_CREDENTIALS_FILE",
        "AWS_WEB_IDENTITY_TOKEN_FILE",
        "AZURE_FEDERATED_TOKEN_FILE",
        "AZURE_STORAGE_AUTH_MODE",
        "BUILD_SOURCEVERSIONAUTHOR",
        "CI_COMMIT_AUTHOR",
        "CLOUDSDK_AUTH_CREDENTIAL_FILE_OVERRIDE",
        "GIT_AUTHOR_DATE",
        "GIT_AUTHOR_EMAIL",
        "GIT_AUTHOR_NAME",
        "GOOGLE_APPLICATION_CREDENTIALS",
        "NPM_CONFIG_AUTH_TYPE",
        "NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED",
        "OLDPWD",
        "PWD",
        "REGISTRY_AUTH_FILE",
        "SSH_AUTH_SOCK",
        "TOKENIZERS_PARALLELISM",
        "XAUTHORITY",
        "YARN_NPM_ALWAYS_AUTH",
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
    private readonly IOptions<SecretMaskingOptions> _secretMaskingOptions = secretMaskingOptions;
    private OrganizedModules? _organizedModules;

    public IReadOnlyList<IModule> RegisteredModules => _moduleRetriever.RegisteredModules;

    public async Task<OrganizedModules> Initialize(CancellationToken cancellationToken = default)
    {
        return _organizedModules ??= await InitializeInternal(cancellationToken).ConfigureAwait(false);
    }

    internal static Table CreateEnvironmentVariablesTable(
        IDictionary variables,
        Func<string, string> obfuscate,
        string maskValue = LoggingConstants.SecretMask,
        int? consoleWidth = null)
    {
        var effectiveMaskValue = string.IsNullOrWhiteSpace(maskValue)
            ? LoggingConstants.SecretMask
            : maskValue;
        var entries = variables
            .Cast<DictionaryEntry>()
            .OrderBy(entry => entry.Key?.ToString(), StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var maximumNameWidth = Math.Max(
            GetMaximumCellWidth("Name"),
            entries
                .Select(entry => GetMaximumCellWidth(entry.Key?.ToString() ?? string.Empty))
                .DefaultIfEmpty()
                .Max());
        int? configuredMaximumValueWidth = consoleWidth is { } width
            ? Math.Max(0, width - maximumNameWidth - TableDecorationWidth)
            : null;
        var table = new Table
        {
            Border = TableBorder.Rounded,
            Expand = true,
            Title = new TableTitle("[bold]Environment variables[/]"),
        };

        table.AddColumn(new TableColumn("[bold]Name[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Value[/]").LeftAligned());

        foreach (var environmentVariable in entries)
        {
            var name = environmentVariable.Key?.ToString() ?? string.Empty;
            var value = environmentVariable.Value?.ToString() ?? string.Empty;
            var obfuscatedValue = obfuscate(value);

            table.AddRow(
                new Text(name),
                new SafeEnvironmentValueRenderable(
                    value,
                    obfuscatedValue,
                    effectiveMaskValue,
                    IsSensitiveEnvironmentVariable(name, value),
                    configuredMaximumValueWidth));
        }

        return table;
    }

    private static bool RequiresUnsafeRendering(
        string value,
        string obfuscatedValue,
        string maskValue,
        int maximumValueWidth)
    {
        if (!value.Equals(obfuscatedValue, StringComparison.Ordinal))
        {
            return !obfuscatedValue.Equals(maskValue, StringComparison.Ordinal);
        }

        return GetMaximumCellWidth(value) > maximumValueWidth
               || value.Any(char.IsControl);
    }

    private static int GetMaximumCellWidth(string value)
    {
        return value.Sum(character => char.IsAscii(character) ? 1 : 2);
    }

    private sealed class SafeEnvironmentValueRenderable(
        string value,
        string obfuscatedValue,
        string maskValue,
        bool isSensitiveName,
        int? configuredMaximumWidth) : IRenderable
    {
        public Measurement Measure(RenderOptions options, int maxWidth) =>
            CreateRenderable(maxWidth).Measure(options, maxWidth);

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth) =>
            CreateRenderable(maxWidth).Render(options, maxWidth);

        private IRenderable CreateRenderable(int maxWidth)
        {
            var effectiveMaximumWidth = configuredMaximumWidth is { } configured
                ? Math.Min(configured, maxWidth)
                : maxWidth;
            var displayValue = isSensitiveName
                               || RequiresUnsafeRendering(
                                   value,
                                   obfuscatedValue,
                                   maskValue,
                                   effectiveMaximumWidth)
                ? maskValue
                : MakeSingleLine(obfuscatedValue);
            return new Text(displayValue).Ellipsis();
        }
    }

    private static bool IsSensitiveEnvironmentVariable(string name, string value)
    {
        if (IsGitConfigKeyName(name))
        {
            return ContainsUriUserInfo(value);
        }

        if (name.StartsWith("GIT_CONFIG_VALUE_", StringComparison.OrdinalIgnoreCase)
            || (name.StartsWith("OTEL_EXPORTER_OTLP_", StringComparison.OrdinalIgnoreCase)
                && name.EndsWith("_HEADERS", StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return !NonSensitiveEnvironmentVariableNames.Contains(name, StringComparer.Ordinal)
               && (SensitiveEnvironmentVariableNames.Contains(name, StringComparer.OrdinalIgnoreCase)
                   || SensitiveEnvironmentVariableNameParts.Any(
                       part => name.Contains(part, StringComparison.OrdinalIgnoreCase))
                   || SensitiveEnvironmentVariableDelimitedNameParts.Any(
                       part => ContainsDelimitedNamePart(name, part)));
    }

    private static bool IsGitConfigKeyName(string name)
    {
        const string prefix = "GIT_CONFIG_KEY_";
        return name.StartsWith(prefix, StringComparison.Ordinal)
               && name.Length > prefix.Length
               && name[prefix.Length..].All(char.IsAsciiDigit);
    }

    private static bool ContainsUriUserInfo(string value)
    {
        var schemeSeparator = value.IndexOf("://", StringComparison.Ordinal);
        if (schemeSeparator < 0)
        {
            return false;
        }

        var authorityStart = schemeSeparator + 3;
        var authorityEnd = value.IndexOfAny(['/', '?', '#'], authorityStart);
        if (authorityEnd < 0)
        {
            authorityEnd = value.Length;
        }

        var userInfoSeparator = value.IndexOf('@', authorityStart, authorityEnd - authorityStart);
        return userInfoSeparator > authorityStart;
    }

    private static bool ContainsDelimitedNamePart(string name, string part)
    {
        var searchStart = 0;
        while (searchStart <= name.Length - part.Length)
        {
            var index = name.IndexOf(part, searchStart, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                return false;
            }

            var end = index + part.Length;
            if ((index == 0 || !char.IsLetterOrDigit(name[index - 1]))
                && (end == name.Length || !char.IsLetterOrDigit(name[end])))
            {
                return true;
            }

            searchStart = index + 1;
        }

        return false;
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
            value => _secretObfuscator.Obfuscate(value, null),
            _secretMaskingOptions.Value.MaskValue));
    }
}
