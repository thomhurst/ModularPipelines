using Microsoft.Extensions.DependencyInjection;
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

    public ModuleOutputWriterFactory(
        IServiceProvider serviceProvider,
        IBuildSystemFormatterProvider formatterProvider,
        IConsoleWriter consoleWriter,
        ISecretObfuscator secretObfuscator)
    {
        _serviceProvider = serviceProvider;
        _formatterProvider = formatterProvider;
        _consoleWriter = consoleWriter;
        _secretObfuscator = secretObfuscator;
    }

    /// <inheritdoc />
    public ModuleOutputWriter Create(string moduleName)
    {
        // Get a fresh scoped buffer for this module
        var scope = _serviceProvider.CreateScope();
        var buffer = scope.ServiceProvider.GetRequiredService<ILogEventBuffer>();

        return new ModuleOutputWriter(
            buffer,
            _formatterProvider,
            _consoleWriter,
            _secretObfuscator,
            moduleName);
    }
}
