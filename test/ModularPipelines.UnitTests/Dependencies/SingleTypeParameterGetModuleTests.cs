using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Dependencies;

/// <summary>
/// Tests for the simplified single-type-parameter GetModule API.
/// </summary>
public class SingleTypeParameterGetModuleTests : TestBase
{
    /// <summary>
    /// A simple module that returns a string value.
    /// </summary>
    private class StringModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "Hello from StringModule";
        }
    }

    /// <summary>
    /// A module that returns a complex type.
    /// </summary>
    private class ComplexResultModule : Module<ComplexResult>
    {
        protected internal override async Task<ComplexResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return new ComplexResult { Id = 42, Name = "Test" };
        }
    }

    /// <summary>
    /// A complex result type for testing type inference.
    /// </summary>
    private record ComplexResult
    {
        public int Id { get; init; }
        public string? Name { get; init; }
    }

    /// <summary>
    /// A module that uses the new single-type-parameter GetModule API.
    /// </summary>
    [ModularPipelines.Attributes.DependsOn<StringModule>]
    private class ConsumerModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Use the new single-type-parameter API
            var result = await context.GetModule<StringModule>();

            // Verify the result is properly typed (ModuleResult<string?>)
            if (result.IsSuccess)
            {
                return result.ValueOrDefault;
            }

            return "failed";
        }
    }

    /// <summary>
    /// A module that uses GetModule to access a complex result type.
    /// </summary>
    [ModularPipelines.Attributes.DependsOn<ComplexResultModule>]
    private class ComplexConsumerModule : Module<int>
    {
        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Type inference should work: result is ModuleResult<ComplexResult?>
            var result = await context.GetModule<ComplexResultModule>();

            if (result.IsSuccess && result.ValueOrDefault is not null)
            {
                return result.ValueOrDefault.Id;
            }

            return -1;
        }
    }

    /// <summary>
    /// A module that uses GetModuleIfRegistered with single type parameter.
    /// </summary>
    [ModularPipelines.Attributes.DependsOn<StringModule>(Optional = true)]
    private class OptionalConsumerModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Use the new single-type-parameter GetModuleIfRegistered API
            var module = context.GetModuleIfRegistered<StringModule>();

            if (module is not null)
            {
                var result = await module;
                return result.ValueOrDefault ?? "default";
            }

            return "not registered";
        }
    }

    /// <summary>
    /// A module that tries to get itself (should throw).
    /// </summary>
    private class SelfReferencingModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // This should throw ModuleReferencingSelfException
            _ = context.GetModule<SelfReferencingModule>();
            await Task.Yield();
            return true;
        }
    }

    /// <summary>
    /// A module that tries to get an unregistered module (should throw).
    /// </summary>
    private class UnregisteredConsumerModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // StringModule is not registered, should throw
            _ = context.GetModule<StringModule>();
            await Task.Yield();
            return true;
        }
    }

    [Test]
    public async Task GetModule_SingleTypeParameter_ReturnsCorrectlyTypedResult()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<StringModule>()
            .AddModule<ConsumerModule>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task GetModule_SingleTypeParameter_WithComplexType_InfersTypeCorrectly()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<ComplexResultModule>()
            .AddModule<ComplexConsumerModule>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task GetModuleIfRegistered_SingleTypeParameter_ReturnsModule_WhenRegistered()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<StringModule>()
            .AddModule<OptionalConsumerModule>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task GetModuleIfRegistered_SingleTypeParameter_ReturnsNull_WhenNotRegistered()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<OptionalConsumerModule>()
            .ExecutePipelineAsync();

        await Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task GetModule_SingleTypeParameter_ThrowsModuleReferencingSelfException()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(
            async () => await TestPipelineHostBuilder.Create()
                .AddModule<SelfReferencingModule>()
                .ExecutePipelineAsync());

        await Assert.That(exception.InnerException).IsTypeOf<ModuleReferencingSelfException>();
    }

    [Test]
    public async Task GetModule_SingleTypeParameter_ThrowsModuleNotRegisteredException()
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(
            async () => await TestPipelineHostBuilder.Create()
                .AddModule<UnregisteredConsumerModule>()
                .ExecutePipelineAsync());

        await Assert.That(exception.InnerException).IsTypeOf<ModuleNotRegisteredException>();
    }
}
