using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Options;

public class LoggingOptionsTests
{
    [Test]
    public async Task PerCallOptionsUseLoggingPropertyName()
    {
        using (Assert.Multiple())
        {
            await Assert.That(typeof(CommandExecutionOptions).GetProperty("Logging")).IsNotNull();
            await Assert.That(typeof(CommandExecutionOptions).GetProperty("LogSettings")).IsNull();
            await Assert.That(typeof(HttpOptions).GetProperty("Logging")).IsNotNull();
            await Assert.That(typeof(HttpOptions).GetProperty("LogSettings")).IsNull();
            await Assert.That(typeof(HttpOptions).GetProperty("LoggingType")).IsNull();
        }
    }

    [Test]
    public async Task RemovedDuplicateAndUnusedOptionsStayRemoved()
    {
        using (Assert.Multiple())
        {
            await Assert.That(typeof(PipelineCommandOptions).GetProperty("Execution")).IsNull();
            await Assert.That(typeof(CommandLoggingOptions).GetProperty("ShowTimestamps")).IsNotNull();
            await Assert.That(typeof(CommandLoggingOptions).GetProperty("IncludeTimestamps")).IsNull();
            await Assert.That(typeof(HttpOptions).Assembly.GetType("ModularPipelines.Http.HttpLoggingType")).IsNull();
        }
    }
}
