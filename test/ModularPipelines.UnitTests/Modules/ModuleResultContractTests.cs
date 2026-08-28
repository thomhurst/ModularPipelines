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
    public async Task MetadataSurface_UsesConciseNames()
    {
        var propertyNames = typeof(IModuleResult)
            .GetProperties()
            .Select(static property => property.Name)
            .ToArray();
        var constructorParameter = typeof(ModuleResult<int>.Success)
            .GetConstructors()
            .Single()
            .GetParameters()
            .Single();
        var deconstructParameter = typeof(ModuleResult<int>.Success)
            .GetMethod(nameof(ModuleResult<int>.Success.Deconstruct))!
            .GetParameters()
            .Single();

        using (Assert.Multiple())
        {
            await Assert.That(propertyNames).IsEquivalentTo(
            [
                "Name",
                "TypeName",
                "Duration",
                "StartTime",
                "EndTime",
                "Status",
                "ValueOrDefault",
                "ExceptionOrDefault",
                "SkipDecisionOrDefault",
            ]);
            await Assert.That(constructorParameter.Name).IsEqualTo("value");
            await Assert.That(deconstructParameter.Name).IsEqualTo("value");
        }
    }

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
        var result = CreateFailure();

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
        var result = CreateFailure();

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
            await Assert.That(result.TypeName).IsEqualTo(typeof(IntModule).FullName);
        }
    }

    [Test]
    public async Task Success_Value_ReturnsValue()
    {
        var success = CreateSuccess(42);
        ModuleResult<int> result = success;
        success.Deconstruct(value: out var deconstructedValue);

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
            Name = nameof(IntModule),
            TypeName = typeof(IntModule).FullName,
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Skipped,
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
            await Assert.That(result.TypeName).IsEqualTo(typeof(IntModule).FullName);
        }
    }

    [Test]
    public async Task Generic_Failure_RoundTrips_Through_Json()
    {
        var result = CreateFailure();

        var json = JsonSerializer.Serialize(result);
        var deserialized = JsonSerializer.Deserialize<ModuleResult<int>>(json);

        using (Assert.Multiple())
        {
            await Assert.That(deserialized).IsTypeOf<ModuleResult<int>.Failure>();
            await Assert.That(deserialized!.ExceptionOrDefault?.Message).IsEqualTo("Failed");
            await Assert.That(deserialized.Name).IsEqualTo(nameof(IntModule));
            await Assert.That(deserialized.TypeName).IsEqualTo(typeof(IntModule).FullName);
        }
    }

    [Test]
    public async Task Generic_Skipped_RoundTrips_Through_Json()
    {
        ModuleResult<int> result = new ModuleResult<int>.Skipped(SkipDecision.Skip("Not needed"))
        {
            Name = nameof(IntModule),
            TypeName = typeof(IntModule).FullName,
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Skipped,
        };

        var json = JsonSerializer.Serialize(result);
        var deserialized = JsonSerializer.Deserialize<ModuleResult<int>>(json);

        using (Assert.Multiple())
        {
            await Assert.That(deserialized).IsTypeOf<ModuleResult<int>.Skipped>();
            await Assert.That(deserialized!.SkipDecisionOrDefault?.Reason).IsEqualTo("Not needed");
            await Assert.That(deserialized.Name).IsEqualTo(nameof(IntModule));
            await Assert.That(deserialized.TypeName).IsEqualTo(typeof(IntModule).FullName);
        }
    }

    [Test]
    public async Task Generic_Result_Reads_Legacy_Metadata_Keys()
    {
        var startTime = new DateTimeOffset(2025, 1, 2, 3, 4, 5, TimeSpan.Zero);
        ModuleResult<int> result = new ModuleResult<int>.Success(42)
        {
            Name = "LegacyModule",
            TypeName = "Legacy.Module",
            Duration = TimeSpan.FromSeconds(6),
            StartTime = startTime,
            EndTime = startTime.AddSeconds(6),
            Status = ModuleStatus.Succeeded,
        };
        var legacyJson = RenameMetadataProperties(JsonSerializer.Serialize(result));

        var deserialized = JsonSerializer.Deserialize<ModuleResult<int>>(legacyJson);

        using (Assert.Multiple())
        {
            await Assert.That(deserialized!.Name).IsEqualTo(result.Name);
            await Assert.That(deserialized.TypeName).IsEqualTo(result.TypeName);
            await Assert.That(deserialized.Duration).IsEqualTo(result.Duration);
            await Assert.That(deserialized.StartTime).IsEqualTo(result.StartTime);
            await Assert.That(deserialized.EndTime).IsEqualTo(result.EndTime);
            await Assert.That(deserialized.Status).IsEqualTo(result.Status);
            await Assert.That(deserialized.Value).IsEqualTo(42);
        }
    }

    [Test]
    public async Task NonGeneric_Result_Reads_Legacy_Metadata_Keys()
    {
        var startTime = new DateTimeOffset(2025, 1, 2, 3, 4, 5, TimeSpan.Zero);
        ModuleResult result = new ModuleResult.Failure(new InvalidOperationException("Failed"))
        {
            Name = "LegacyModule",
            TypeName = "Legacy.Module",
            Duration = TimeSpan.FromSeconds(6),
            StartTime = startTime,
            EndTime = startTime.AddSeconds(6),
            Status = ModuleStatus.Failed,
        };
        var legacyJson = RenameMetadataProperties(JsonSerializer.Serialize(result));

        var deserialized = JsonSerializer.Deserialize<ModuleResult>(legacyJson);

        using (Assert.Multiple())
        {
            await Assert.That(deserialized!.Name).IsEqualTo(result.Name);
            await Assert.That(deserialized.TypeName).IsEqualTo(result.TypeName);
            await Assert.That(deserialized.Duration).IsEqualTo(result.Duration);
            await Assert.That(deserialized.StartTime).IsEqualTo(result.StartTime);
            await Assert.That(deserialized.EndTime).IsEqualTo(result.EndTime);
            await Assert.That(deserialized.Status).IsEqualTo(result.Status);
            await Assert.That(deserialized.ExceptionOrDefault?.Message).IsEqualTo("Failed");
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
            Name = nameof(IntModule),
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Skipped,
        };

        var json = JsonSerializer.Serialize(result);
        var deserialized = JsonSerializer.Deserialize<ModuleResult<int>>(json);

        await Assert.That(deserialized).IsTypeOf<ModuleResult<int>.Skipped>();
        await Assert.That(deserialized!.SkipDecisionOrDefault?.Reason).IsEqualTo("Not needed");
    }

    [Test]
    public async Task Success_Constructor_UsesLowercaseValueNamedArgument()
    {
        var success = new ModuleResult<int>.Success(value: 42)
        {
            Name = nameof(IntModule),
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        };

        await Assert.That(success.Value).IsEqualTo(42);
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
    public async Task Success_Without_Value_Is_Rejected()
    {
        const string json = """
                            {
                              "$type": "Success",
                              "Name": "MissingValueModule"
                            }
                            """;

        var exception = await Assert.That(() => JsonSerializer.Deserialize<ModuleResult<string>>(json))
            .Throws<JsonException>();

        await Assert.That(exception!.Message).Contains("requires a Value property");
    }

    [Test]
    public async Task Success_RuntimeValueContract_SurvivesJsonRoundTrip()
    {
        ModuleResult<IRuntimeValue> result = new ModuleResult<IRuntimeValue>.Success(
            new RuntimeValue("common", "derived"))
        {
            Name = nameof(RuntimeValue),
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        };

        var json = JsonSerializer.Serialize(result);
        var deserialized = JsonSerializer.Deserialize<ModuleResult<IRuntimeValue>>(json);

        var value = deserialized!.Value;
        await Assert.That(value).IsTypeOf<RuntimeValue>();
        await Assert.That(((RuntimeValue) value).Derived).IsEqualTo("derived");
    }

    [Test]
    public async Task Success_NullableNull_SurvivesJsonRoundTrip()
    {
        ModuleResult<int?> result = new ModuleResult<int?>.Success(null)
        {
            Name = nameof(IntModule),
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        };

        var json = JsonSerializer.Serialize(result);
        var deserialized = JsonSerializer.Deserialize<ModuleResult<int?>>(json);

        await Assert.That(deserialized!.TryGetValue(out var value)).IsTrue();
        await Assert.That(value).IsNull();
    }

    [Test]
    public async Task Failure_Value_ThrowsWithModuleContext()
    {
        var failure = new InvalidOperationException("Compilation failed");
        var result = CreateFailure(failure);

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
        var result = CreateSkipped("No source changes");

        var exception = await Assert.That(() => result.Value)
            .Throws<InvalidOperationException>();

        await Assert.That(exception!.Message).IsEqualTo("IntModule was skipped: No source changes");
    }

    [Test]
    public async Task ExplicitlyNullableSuccess_Value_ReturnsNull()
    {
        ModuleResult<string?> result = new ModuleResult<string?>.Success(null)
        {
            Name = "NullableModule",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        };

        await Assert.That(result.Value).IsNull();
    }

    [Test]
    public async Task Failure_ToString_DoesNotEvaluateRequiredValue()
    {
        var result = CreateFailure(new InvalidOperationException("Compilation failed"));

        var formatted = result.ToString();

        await Assert.That(formatted).Contains("Compilation failed");
    }

    [Test]
    public async Task Skipped_ToString_DoesNotEvaluateRequiredValue()
    {
        var result = CreateSkipped("No source changes");

        var formatted = result.ToString();

        await Assert.That(formatted).Contains("No source changes");
    }

    [Test]
    public async Task NullSuccess_ToString_DoesNotEvaluateRequiredValue()
    {
        ModuleResult<string?> result = new ModuleResult<string?>.Success(null)
        {
            Name = "NullableModule",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        };

        var formatted = result.ToString();

        await Assert.That(formatted).Contains(nameof(ModuleResult<string?>.Success));
    }

    [Test]
    public async Task Success_ToString_PrintsValueOnce()
    {
        ModuleResult<int> result = CreateSuccess(42);

        var formatted = result.ToString();
        var valueOccurrences = formatted
            .Split("Value = 42", StringSplitOptions.None)
            .Length - 1;

        await Assert.That(valueOccurrences).IsEqualTo(1);
    }

    [Test]
    public async Task NullSuccess_TryGetValue_ReturnsTrue()
    {
        ModuleResult<string?> result = new ModuleResult<string?>.Success(null)
        {
            Name = "NullableModule",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        };

        var hasValue = result.TryGetValue(out var value);

        using (Assert.Multiple())
        {
            await Assert.That(hasValue).IsTrue();
            await Assert.That(value).IsNull();
            await Assert.That(((ModuleResult<string?>.Success) result).Value).IsNull();
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
            Name = nameof(IntModule),
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        };
    }

    private static string RenameMetadataProperties(string json)
    {
        return json
            .Replace("\"Name\":", "\"ModuleName\":", StringComparison.Ordinal)
            .Replace("\"TypeName\":", "\"ModuleTypeName\":", StringComparison.Ordinal)
            .Replace("\"Duration\":", "\"ModuleDuration\":", StringComparison.Ordinal)
            .Replace("\"StartTime\":", "\"ModuleStart\":", StringComparison.Ordinal)
            .Replace("\"EndTime\":", "\"ModuleEnd\":", StringComparison.Ordinal)
            .Replace("\"Status\":", "\"ModuleStatus\":", StringComparison.Ordinal);
    }

    private static ModuleResult<int> CreateFailure(Exception? exception = null)
    {
        return new ModuleResult.Failure(exception ?? new InvalidOperationException("Failed"))
        {
            Name = nameof(IntModule),
            TypeName = typeof(IntModule).FullName,
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Failed,
        };
    }

    private static ModuleResult<int> CreateSkipped(string reason)
    {
        return new ModuleResult.Skipped(SkipDecision.Skip(reason))
        {
            Name = nameof(IntModule),
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Skipped,
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

    private interface IRuntimeValue
    {
        string Common { get; }
    }

    private sealed record RuntimeValue(string Common, string Derived) : IRuntimeValue;
}
