using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;

namespace ModularPipelines.Logging;

/// <summary>
/// Factory for creating scoped <see cref="ModuleOutputWriter"/> instances.
/// </summary>
internal class ModuleOutputWriterFactory : IModuleOutputWriterFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IBuildSystemFormatterProvider _formatterProvider;
    private readonly IConsoleWriter _consoleWriter;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly ILoggerFactory _loggerFactory;

    public ModuleOutputWriterFactory(
        IServiceProvider serviceProvider,
        IBuildSystemFormatterProvider formatterProvider,
        IConsoleWriter consoleWriter,
        ISecretObfuscator secretObfuscator,
        ILoggerFactory loggerFactory)
    {
        _serviceProvider = serviceProvider;
        _formatterProvider = formatterProvider;
        _consoleWriter = consoleWriter;
        _secretObfuscator = secretObfuscator;
        _loggerFactory = loggerFactory;
    }

    /// <inheritdoc />
    public ModuleOutputWriter Create(string moduleName, ILogger? logger = null)
    {
        // Get a fresh scope for this module
        var scope = _serviceProvider.CreateScope();

        // Use provided logger or create one from factory
        var effectiveLogger = logger ?? _loggerFactory.CreateLogger(moduleName);

        return new ModuleOutputWriter(
            scope,
            _formatterProvider,
            _consoleWriter,
            _secretObfuscator,
            effectiveLogger,
            moduleName);
    }
}
