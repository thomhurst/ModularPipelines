using System.Text.Json;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class ModuleResultSerializerTests
{
    private sealed class WorkerException(string message) : Exception(message);

    private class SimpleResult
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    private class SimpleModule : ModularPipelines.Module<SimpleResult>
    {
        protected internal override Task<SimpleResult> ExecuteAsync(
            ModularPipelines.IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<SimpleResult>(new SimpleResult());
        }
    }

    [Test]
    public async Task Serialize_And_Deserialize_Success_Result()
    {
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(SimpleModule));
        var commandExecutionCounter = new CommandExecutionCounter();
        commandExecutionCounter.Add(typeof(SimpleModule), 3);
        var serializer = new ModuleResultSerializer(registry, commandExecutionCounter);

        var result = new ModuleResult<SimpleResult>.Success(new SimpleResult { Name = "test", Count = 42 })
        {
            Name = "SimpleModule",
            TypeName = typeof(SimpleModule).FullName,
            Duration = TimeSpan.FromSeconds(1),
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow.AddSeconds(1),
            Status = ModuleStatus.Succeeded
        };

        var serialized = serializer.Serialize(result, typeof(SimpleModule).FullName!, typeof(SimpleResult).FullName!, 1);

        await Assert.That(serialized.ModuleTypeName).IsEqualTo(typeof(SimpleModule).FullName);
        await Assert.That(serialized.WorkerIndex).IsEqualTo(1);
        await Assert.That(serialized.CommandCount).IsEqualTo(3);
        await Assert.That(serialized.SerializedJson).IsNotNull();

        var deserialized = serializer.Deserialize(serialized);
        await Assert.That(deserialized).IsNotNull();
        await Assert.That(deserialized!.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(deserialized.TypeName).IsEqualTo(ModuleTypeIdentifier.Get(typeof(SimpleModule)));
    }

    [Test]
    public async Task Deserialize_Unknown_Module_Throws()
    {
        var registry = new ModuleTypeRegistry();
        var serializer = new ModuleResultSerializer(registry);

        var serialized = new SerializedModuleResult(
            ModuleTypeName: "Unknown.Module",
            ResultTypeName: "Unknown.Result",
            WorkerIndex: 1,
            SerializedJson: "{}",
            CompletedAt: DateTimeOffset.UtcNow);

        Assert.Throws<InvalidOperationException>(() => serializer.Deserialize(serialized));
    }

    [Test]
    public async Task Deserialize_Custom_Exception_Preserves_Remote_Diagnostics()
    {
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(SimpleModule));
        var serializer = new ModuleResultSerializer(registry);
        var originalException = CaptureWorkerException();
        ModuleResult<SimpleResult> result = new ModuleResult<SimpleResult>.Failure(originalException)
        {
            Name = nameof(SimpleModule),
            TypeName = typeof(SimpleModule).FullName,
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Failed,
        };

        var serialized = serializer.Serialize(
            result,
            typeof(SimpleModule).FullName!,
            typeof(SimpleResult).FullName!,
            workerIndex: 7);

        var deserialized = serializer.Deserialize(serialized);
        var remoteException = deserialized!.ExceptionOrDefault as RemoteModuleException;

        using (Assert.Multiple())
        {
            await Assert.That(remoteException).IsNotNull();
            await Assert.That(remoteException!.OriginalExceptionType)
                .IsEqualTo(typeof(WorkerException).FullName);
            await Assert.That(remoteException.OriginalMessage).IsEqualTo("worker failed");
            await Assert.That(remoteException.WorkerIndex).IsEqualTo(7);
            await Assert.That(((ModuleResult) deserialized).WorkerIndex).IsEqualTo(7);
            await Assert.That(remoteException.RemoteStackTrace)
                .Contains(nameof(CaptureWorkerException));
            await Assert.That(remoteException.StackTrace)
                .Contains(remoteException.RemoteStackTrace!);
            await Assert.That(remoteException.Message)
                .IsEqualTo($"Remote worker 7 threw {typeof(WorkerException).FullName}: worker failed");
        }
    }

    [Test]
    public async Task Deserialize_System_Exception_Preserves_Type_And_Remote_Stack_Trace()
    {
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(SimpleModule));
        var serializer = new ModuleResultSerializer(registry);
        ModuleResult<SimpleResult> result = new ModuleResult<SimpleResult>.Failure(
            CaptureSystemException())
        {
            Name = nameof(SimpleModule),
            TypeName = typeof(SimpleModule).FullName,
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Failed,
        };

        var serialized = serializer.Serialize(
            result,
            typeof(SimpleModule).FullName!,
            typeof(SimpleResult).FullName!,
            workerIndex: 7);

        var deserialized = serializer.Deserialize(serialized);

        await Assert.That(deserialized!.ExceptionOrDefault)
            .IsTypeOf<InvalidOperationException>();
        await Assert.That(deserialized.ExceptionOrDefault!.Message)
            .IsEqualTo("worker failed");
        await Assert.That(deserialized.ExceptionOrDefault.StackTrace)
            .Contains(nameof(CaptureSystemException));
        await Assert.That(((ModuleResult) deserialized).WorkerIndex).IsEqualTo(7);
    }

    [Test]
    public async Task Serialized_Result_Preserves_Six_Parameter_Constructor()
    {
        var constructor = typeof(SerializedModuleResult).GetConstructor(
        [
            typeof(string),
            typeof(string),
            typeof(int),
            typeof(string),
            typeof(DateTimeOffset),
            typeof(IReadOnlyList<ArtifactReference>),
        ]);

        await Assert.That(constructor).IsNotNull();
    }

    private static WorkerException CaptureWorkerException()
    {
        try
        {
            throw new WorkerException("worker failed");
        }
        catch (WorkerException exception)
        {
            return exception;
        }
    }

    private static InvalidOperationException CaptureSystemException()
    {
        try
        {
            throw new InvalidOperationException("worker failed");
        }
        catch (InvalidOperationException exception)
        {
            return exception;
        }
    }
}
