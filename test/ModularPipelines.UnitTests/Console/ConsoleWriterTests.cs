using ModularPipelines.Logging;
using Moq;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.UnitTests.Console;

[TUnit.Core.NotInParallel(nameof(ConsoleWriterTests))]
public class ConsoleWriterTests
{
    [Test]
    public async Task LogToConsole_UsesAmbientModuleConsoleWriter()
    {
        var moduleLogger = new Mock<IModuleLogger>();
        var moduleConsoleWriter = moduleLogger.As<IConsoleWriter>();
        var consoleWriter = new ConsoleWriter();

        await using (new ModuleLoggerScope(moduleLogger.Object, typeof(ConsoleWriterTests)))
        {
            consoleWriter.LogToConsole("[green]module output[/]");
        }

        moduleConsoleWriter.Verify(
            writer => writer.LogToConsole("[green]module output[/]"),
            Times.Once);
    }

    [Test]
    public async Task Write_UsesAmbientModuleConsoleWriter()
    {
        var moduleLogger = new Mock<IModuleLogger>();
        var moduleConsoleWriter = moduleLogger.As<IConsoleWriter>();
        var consoleWriter = new ConsoleWriter();
        IRenderable table = new Table().AddColumn("Value").AddRow("module output");

        await using (new ModuleLoggerScope(moduleLogger.Object, typeof(ConsoleWriterTests)))
        {
            consoleWriter.Write(table);
        }

        moduleConsoleWriter.Verify(writer => writer.Write(table), Times.Once);
    }
}
