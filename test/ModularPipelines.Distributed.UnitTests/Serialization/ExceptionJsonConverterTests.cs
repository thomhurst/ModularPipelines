using System.Text.Json;
using ModularPipelines.Exceptions;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class ExceptionJsonConverterTests
{
    private static readonly JsonSerializerOptions Options = new() { Converters = { new ExceptionJsonConverter() } };

    [Test]
    public async Task Inner_Exception_Chain_Is_Preserved()
    {
        var original = new InvalidOperationException(
            "outer",
            new CustomWorkerException("middle", new ArgumentException("innermost")));

        var restored = RoundTrip(original);

        await Assert.That(restored).IsTypeOf<InvalidOperationException>();
        await Assert.That(restored.Message).IsEqualTo("outer");
        var middle = restored.InnerException as RemoteModuleException;
        await Assert.That(middle).IsNotNull();
        await Assert.That(middle!.OriginalExceptionType).IsEqualTo(typeof(CustomWorkerException).FullName);
        await Assert.That(middle.InnerException).IsTypeOf<ArgumentException>();
        await Assert.That(middle.InnerException!.Message).IsEqualTo("innermost");
    }

    [Test]
    public async Task Aggregate_Children_Are_Preserved_Without_Repeating_Messages()
    {
        var original = new AggregateException(
            "several failures",
            new InvalidOperationException("first"),
            new TimeoutException("second"));

        var restored = RoundTrip(original);

        var aggregate = restored as AggregateException;
        await Assert.That(aggregate).IsNotNull();
        await Assert.That(aggregate!.InnerExceptions.Select(inner => inner.GetType()))
            .IsEquivalentTo([typeof(InvalidOperationException), typeof(TimeoutException)]);
        await Assert.That(aggregate.Message).IsEqualTo(original.Message);
    }

    [Test]
    public async Task Parameter_Name_Constructors_Receive_The_Message()
    {
        var restored = RoundTrip(new ArgumentNullException("value", "The value was missing."));

        await Assert.That(restored).IsTypeOf<ArgumentNullException>();
        await Assert.That(restored.Message).StartsWith("The value was missing.");
    }

    [Test]
    public async Task ModularPipelines_Timeout_Keeps_Its_Type_And_Data()
    {
        var original = new ModuleTimeoutException(
            typeof(TimeoutModule),
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(7),
            wasCancellationTokenRespected: false);

        var restored = RoundTrip(original) as ModuleTimeoutException;

        await Assert.That(restored).IsNotNull();
        await Assert.That(restored!.ModuleType).IsEqualTo(typeof(TimeoutModule));
        await Assert.That(restored.ConfiguredTimeout).IsEqualTo(TimeSpan.FromSeconds(5));
        await Assert.That(restored.ElapsedTime).IsEqualTo(TimeSpan.FromSeconds(7));
        await Assert.That(restored.WasCancellationTokenRespected).IsFalse();
    }

    [Test]
    public async Task ModularPipelines_Module_Failure_Keeps_Its_Type_And_Inner_Exception()
    {
        var original = new ModuleFailedException(typeof(TimeoutModule), new InvalidOperationException("upload failed"));

        var restored = RoundTrip(original) as ModuleFailedException;

        await Assert.That(restored).IsNotNull();
        await Assert.That(restored!.ModuleType).IsEqualTo(typeof(TimeoutModule));
        await Assert.That(restored.InnerException!.Message).IsEqualTo("upload failed");
    }

    [Test]
    public async Task Types_Outside_The_System_Namespaces_Are_Not_Activated()
    {
        const string Json = """{"Type":"SystemX.Evil, SystemX","Message":"boom","StackTrace":null}""";

        var restored = JsonSerializer.Deserialize<Exception>(Json, Options);

        await Assert.That(restored).IsTypeOf<RemoteModuleException>();
        await Assert.That(((RemoteModuleException) restored!).OriginalExceptionType).IsEqualTo("SystemX.Evil, SystemX");
    }

    [Test]
    public async Task Timeout_For_An_Unknown_Module_Type_Falls_Back_To_Remote_Exception()
    {
        const string Json = """
            {"Type":"ModularPipelines.Exceptions.ModuleTimeoutException","Message":"timed out","StackTrace":null,
             "Properties":{"ModuleType":"Missing.Module, Missing","ConfiguredTimeout":"1","ElapsedTime":"1","WasCancellationTokenRespected":"true"}}
            """;

        var restored = JsonSerializer.Deserialize<Exception>(Json, Options);

        await Assert.That(restored).IsTypeOf<RemoteModuleException>();
    }

    private static Exception RoundTrip(Exception exception)
    {
        Exception thrown;
        try
        {
            throw exception;
        }
        catch (Exception caught)
        {
            thrown = caught;
        }

        var json = JsonSerializer.Serialize(thrown, Options);
        return JsonSerializer.Deserialize<Exception>(json, Options)!;
    }

    private sealed class CustomWorkerException(string message, Exception inner) : Exception(message, inner);

    private sealed class TimeoutModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(0);
    }
}
