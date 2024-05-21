using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using NReco.Logging.File;
using TUnit.Assertions.Extensions;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests;

public class ModuleLoggerTests
{
    private static readonly string RandomString = Guid.NewGuid().ToString();
    private class Module1 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogToConsole(RandomString);

            return await NothingAsync();
        }
    }

    [Test]
    public async Task Can_Write_To_Console_Successfully()
    {
        var consoleStringBuilder = new StringBuilder();
        var file = File.GetNewTemporaryFilePath();

        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddLogging(builder => { builder.AddFile(file); });
                collection.AddSingleton<IConsoleWriter>(new StringBuilderConsoleWriter(consoleStringBuilder));
            })
            .AddModule<Module1>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        await host.DisposeAsync();

        var stringOutput = consoleStringBuilder.ToString();
        await Assert.That(stringOutput).Does.Contain(RandomString);
        await Assert.That(await file.ReadAsync()).Does.Not.Contain(RandomString);
    }
}