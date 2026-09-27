using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Engine;

public class DependencyPrinterTests
{
    private sealed class TestModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [Test]
    public void PrintDependencyChains_RendersLocalGroupCommandsAsMarkup()
    {
        const string startCommand = "[bold cyan]start[/]";
        const string endCommand = "[bold cyan]end[/]";
        var dependencyModel = new ModuleDependencyModel(new TestModule());
        var chainProvider = new Mock<IDependencyChainProvider>();
        chainProvider.SetupGet(x => x.ModuleDependencyModels).Returns([dependencyModel]);

        var consoleWriter = new Mock<IConsoleWriter>();
        var formatter = new Mock<IBuildSystemFormatter>();
        formatter.SetupGet(x => x.UsesRawCommands).Returns(false);
        formatter.Setup(x => x.GetStartBlockCommand("Module Dependencies")).Returns(startCommand);
        formatter.Setup(x => x.GetEndBlockCommand("Module Dependencies")).Returns(endCommand);

        var formatterProvider = new Mock<IBuildSystemFormatterProvider>();
        formatterProvider.Setup(x => x.GetFormatter()).Returns(formatter.Object);

        var tree = new Tree("Module Dependencies");
        var treeFormatter = new Mock<IDependencyTreeFormatter>();
        treeFormatter.Setup(x => x.FormatTree(It.IsAny<IEnumerable<ModuleDependencyModel>>())).Returns(tree);

        var printer = new DependencyPrinter(
            chainProvider.Object,
            consoleWriter.Object,
            Mock.Of<IBuildSystemCommandWriter>(),
            formatterProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            treeFormatter.Object);

        printer.PrintDependencyChains();

        consoleWriter.Verify(x => x.WriteMarkupLine(startCommand), Times.Once);
        consoleWriter.Verify(x => x.WriteMarkupLine(endCommand), Times.Once);
        consoleWriter.Verify(x => x.Write(tree), Times.Once);
        consoleWriter.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void PrintDependencyChains_Skips_Distributed_Worker()
    {
        var chainProvider = new Mock<IDependencyChainProvider>();
        chainProvider.SetupGet(x => x.ModuleDependencyModels).Returns([new ModuleDependencyModel(new TestModule())]);
        var consoleWriter = new Mock<IConsoleWriter>();
        var formatterProvider = new Mock<IBuildSystemFormatterProvider>();
        formatterProvider.Setup(x => x.GetFormatter()).Returns(Mock.Of<IBuildSystemFormatter>());
        var location = new Mock<IExecutionLocationContext>();
        location.SetupGet(x => x.IsWorker).Returns(true);

        var printer = new DependencyPrinter(
            chainProvider.Object,
            consoleWriter.Object,
            Mock.Of<IBuildSystemCommandWriter>(),
            formatterProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            Mock.Of<IDependencyTreeFormatter>(),
            location.Object);

        printer.PrintDependencyChains();

        consoleWriter.VerifyNoOtherCalls();
    }
}
