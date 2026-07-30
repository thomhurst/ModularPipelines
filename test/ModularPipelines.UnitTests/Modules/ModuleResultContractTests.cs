using System.Text.Json;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Modules;

public class ModuleResultContractTests
{
    [Test]
    public async Task DistributedValueTypeResult_AppliesToModule()
    {
        var module = new IntModule();
        var distributedResult = CreateSuccess(42);

        var applied = ModuleCompletionSourceApplicator.TryApply(module, distributedResult);
        var result = await module;

        using (Assert.Multiple())
        {
            await Assert.That(applied).IsTrue();
            await Assert.That(result).IsSameReferenceAs(distributedResult);
            await Assert.That(result.TryGetValue(out var value)).IsTrue();
            await Assert.That(value).IsEqualTo(42);
            await Assert.That(((ModuleResult<int>.Success) result).Value).IsEqualTo(42);
        }
    }

    [Test]
    public async Task Failure_TryGetValue_ReturnsFalse()
    {
        ModuleResult<int> result = CreateFailure();

        var hasValue = result.TryGetValue(out var value);

        using (Assert.Multiple())
        {
            await Assert.That(hasValue).IsFalse();
            await Assert.That(value).IsEqualTo(default);
        }
    }

    [Test]
    public async Task Generic_Failure_Can_Be_Pattern_Matched()
    {
        ModuleResult<int> result = CreateFailure();

        var message = result switch
        {
            ModuleResult<int>.Failure { Exception: var exception } => exception.Message,
            _ => null,
        };

        using (Assert.Multiple())
        {
            await Assert.That(result).IsTypeOf<ModuleResult<int>.Failure>();
            await Assert.That(message).IsEqualTo("Failed");
            await Assert.That(result.ExceptionOrDefault?.Message).IsEqualTo("Failed");
            await Assert.That(result.ModuleTypeName).IsEqualTo(typeof(IntModule).FullName);
        }
    }

    [Test]
    public async Task Success_Value_ReturnsValue()
    {
        var success = CreateSuccess(42);
        ModuleResult<int> result = success;
        success.Deconstruct(out var deconstructedValue);

        using (Assert.Multiple())
        {
            await Assert.That(result.Value).IsEqualTo(42);
            await Assert.That(success.Value).IsEqualTo(42);
            await Assert.That(deconstructedValue).IsEqualTo(42);
        }
    }

    [Test]
    public async Task Generic_Skipped_Can_Be_Pattern_Matched()
    {
        var decision = SkipDecision.Skip("Not needed");
        ModuleResult<int> result = new ModuleResult.Skipped(decision)
        {
            ModuleName = nameof(IntModule),
            ModuleTypeName = typeof(IntModule).FullName,
            ModuleDuration = TimeSpan.Zero,
            ModuleStart = DateTimeOffset.UtcNow,
            ModuleEnd = DateTimeOffset.UtcNow,
            ModuleStatus = Status.Skipped,
        };

        var reason = result switch
        {
            ModuleResult<int>.Skipped { Decision: var skip } => skip.Reason,
            _ => null,
        };

        using (Assert.Multiple())
        {
            await Assert.That(result).IsTypeOf<ModuleResult<int>.Skipped>();
            await Assert.That(reason).IsEqualTo("Not needed");
            await Assert.That(result.SkipDecisionOrDefault).IsEqualTo(decision);
            await Assert.That(result.ModuleTypeName).IsEqualTo(typeof(IntModule).FullName);
        }
    }

    [Test]
    public async Task Generic_Failure_RoundTrips_Through_Json()
    {
        ModuleResult<int> result = CreateFailure();

        var json = JsonSerializer.Serialize(result);
        var deserialized = JsonSerializer.Deserialize<ModuleResult<int>>(json);

        using (Assert.Multiple())
        {
            await Assert.That(deserialized).IsTypeOf<ModuleResult<int>.Failure>();
            await Assert.That(deserialized!.ExceptionOrDefault?.Message).IsEqualTo("Failed");
            await Assert.That(deserialized.ModuleName).IsEqualTo(nameof(IntModule));
            await Assert.That(deserialized.ModuleTypeName).IsEqualTo(typeof(IntModule).FullName);
        }
    }

    [Test]
    public async Task Generic_Skipped_RoundTrips_Through_Json()
    {
        ModuleResult<int> result = new ModuleResult<int>.Skipped(SkipDecision.Skip("Not needed"))
        {
            ModuleName = nameof(IntModule),
            ModuleTypeName = typeof(IntModule).FullName,
            ModuleDuration = TimeSpan.Zero,
            ModuleStart = DateTimeOffset.UtcNow,
            ModuleEnd = DateTimeOffset.UtcNow,
            ModuleStatus = Status.Skipped,
        };

        var json = JsonSerializer.Serialize(result);
        var deserialized = JsonSerializer.Deserialize<ModuleResult<int>>(json);

        using (Assert.Multiple())
        {
            await Assert.That(deserialized).IsTypeOf<ModuleResult<int>.Skipped>();
            await Assert.That(deserialized!.SkipDecisionOrDefault?.Reason).IsEqualTo("Not needed");
            await Assert.That(deserialized.ModuleName).IsEqualTo(nameof(IntModule));
            await Assert.That(deserialized.ModuleTypeName).IsEqualTo(typeof(IntModule).FullName);
        }
    }

    [Test]
    public async Task Concrete_Generic_Failure_Serializes_Through_Json()
    {
        var result = (ModuleResult<int>.Failure) CreateFailure();

        var json = JsonSerializer.Serialize(result);
        var deserialized = JsonSerializer.Deserialize<ModuleResult<int>>(json);

        await Assert.That(deserialized).IsTypeOf<ModuleResult<int>.Failure>();
        await Assert.That(deserialized!.ExceptionOrDefault?.Message).IsEqualTo("Failed");
    }

    [Test]
    public async Task Concrete_Generic_Skipped_Serializes_Through_Json()
    {
        var result = new ModuleResult<int>.Skipped(SkipDecision.Skip("Not needed"))
        {
            ModuleName = nameof(IntModule),
            ModuleDuration = TimeSpan.Zero,
            ModuleStart = DateTimeOffset.UtcNow,
            ModuleEnd = DateTimeOffset.UtcNow,
            ModuleStatus = Status.Skipped,
        };

        var json = JsonSerializer.Serialize(result);
        var deserialized = JsonSerializer.Deserialize<ModuleResult<int>>(json);

        await Assert.That(deserialized).IsTypeOf<ModuleResult<int>.Skipped>();
        await Assert.That(deserialized!.SkipDecisionOrDefault?.Reason).IsEqualTo("Not needed");
    }

    [Test]
    public async Task Success_Value_SurvivesJsonRoundTrip()
    {
        ModuleResult<int> result = CreateSuccess(42);

        var json = JsonSerializer.Serialize(result);
        var deserialized = JsonSerializer.Deserialize<ModuleResult<int>>(json);

        await Assert.That(deserialized!.Value).IsEqualTo(42);
    }

    [Test]
    public async Task Failure_Value_ThrowsWithModuleContext()
    {
        var failure = new InvalidOperationException("Compilation failed");
        ModuleResult<int> result = CreateFailure(failure);

        var exception = await Assert.That(() => result.Value)
            .Throws<InvalidOperationException>();

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Message).IsEqualTo("IntModule failed: Compilation failed");
            await Assert.That(exception.InnerException).IsSameReferenceAs(failure);
        }
    }

    [Test]
    public async Task Skipped_Value_ThrowsWithModuleContext()
    {
        ModuleResult<int> result = CreateSkipped("No source changes");

        var exception = await Assert.That(() => result.Value)
            .Throws<InvalidOperationException>();

        await Assert.That(exception!.Message).IsEqualTo("IntModule was skipped: No source changes");
    }

    [Test]
    public async Task NullSuccess_Value_ThrowsWithModuleContext()
    {
        ModuleResult<string> result = new ModuleResult<string>.Success(null)
        {
            ModuleName = "NullableModule",
            ModuleDuration = TimeSpan.Zero,
            ModuleStart = DateTimeOffset.UtcNow,
            ModuleEnd = DateTimeOffset.UtcNow,
            ModuleStatus = Status.Successful,
        };

        var exception = await Assert.That(() => result.Value)
            .Throws<InvalidOperationException>();

        await Assert.That(exception!.Message).IsEqualTo("NullableModule succeeded but returned null");
    }

    [Test]
    public async Task NullSuccess_TryGetValue_ReturnsTrue()
    {
        ModuleResult<string> result = new ModuleResult<string>.Success(null)
        {
            ModuleName = "NullableModule",
            ModuleDuration = TimeSpan.Zero,
            ModuleStart = DateTimeOffset.UtcNow,
            ModuleEnd = DateTimeOffset.UtcNow,
            ModuleStatus = Status.Successful,
        };

        var hasValue = result.TryGetValue(out var value);

        using (Assert.Multiple())
        {
            await Assert.That(hasValue).IsTrue();
            await Assert.That(value).IsNull();
            await Assert.That(((ModuleResult<string>.Success) result).Value).IsNull();
            await Assert.That(result.Match(
                onSuccess: success => success,
                onFailure: _ => "failure",
                onSkipped: _ => "skipped")).IsNull();
        }
    }

    private static ModuleResult<int>.Success CreateSuccess(int value)
    {
        return new ModuleResult<int>.Success(value)
        {
            ModuleName = nameof(IntModule),
            ModuleDuration = TimeSpan.Zero,
            ModuleStart = DateTimeOffset.UtcNow,
            ModuleEnd = DateTimeOffset.UtcNow,
            ModuleStatus = Status.Successful,
        };
    }

    private static ModuleResult<int> CreateFailure(Exception? exception = null)
    {
        return new ModuleResult.Failure(exception ?? new InvalidOperationException("Failed"))
        {
            ModuleName = nameof(IntModule),
            ModuleTypeName = typeof(IntModule).FullName,
            ModuleDuration = TimeSpan.Zero,
            ModuleStart = DateTimeOffset.UtcNow,
            ModuleEnd = DateTimeOffset.UtcNow,
            ModuleStatus = Status.Failed,
        };
    }

    private static ModuleResult<int> CreateSkipped(string reason)
    {
        return new ModuleResult.Skipped(SkipDecision.Skip(reason))
        {
            ModuleName = nameof(IntModule),
            ModuleDuration = TimeSpan.Zero,
            ModuleStart = DateTimeOffset.UtcNow,
            ModuleEnd = DateTimeOffset.UtcNow,
            ModuleStatus = Status.Skipped,
        };
    }

    private sealed class IntModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(42);
        }
    }
}
