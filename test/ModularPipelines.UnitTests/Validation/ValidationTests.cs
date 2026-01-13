using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Host;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Validation;


namespace ModularPipelines.UnitTests.Validation;

public class ValidationTests
{
    // Test modules for various scenarios
    private class SimpleModule : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("success");
    }

    private class AnotherModule : Module<int>
    {
        public override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(42);
    }

    // Module that depends on itself (invalid)
    [ModularPipelines.Attributes.DependsOn<SelfReferencingModule>]
    private class SelfReferencingModule : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("success");
    }

    // Modules for circular dependency test
    [ModularPipelines.Attributes.DependsOn<ModuleB>]
    private class ModuleA : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("A");
    }

    [ModularPipelines.Attributes.DependsOn<ModuleC>]
    private class ModuleB : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("B");
    }

    [ModularPipelines.Attributes.DependsOn<ModuleA>]
    private class ModuleC : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("C");
    }

    // Module with missing dependency (not registered)
    [ModularPipelines.Attributes.DependsOn<MissingModule>]
    private class ModuleWithMissingDep : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("success");
    }

    private class MissingModule : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("missing");
    }

    // Module with optional dependency (missing but ignored)
    [ModularPipelines.Attributes.DependsOn<MissingModule>(IgnoreIfNotRegistered = true)]
    private class ModuleWithOptionalDep : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("success");
    }

    [Test]
    public async Task ValidateAsync_WithValidConfiguration_ReturnsNoErrors()
    {
        // Arrange
        var builder = PipelineHostBuilder.Create()
            .AddModule<SimpleModule>()
            .AddModule<AnotherModule>();

        // Act
        var result = await builder.ValidateAsync();

        // Assert
        await Assert.That(result.IsValid).IsTrue();
        await Assert.That(result.HasErrors).IsFalse();
        await Assert.That(result.Errors.Count).IsEqualTo(0);
    }

    [Test]
    public async Task ValidateAsync_WithNoModules_ReturnsError()
    {
        // Arrange
        var builder = PipelineHostBuilder.Create();

        // Act
        var result = await builder.ValidateAsync();

        // Assert
        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.Errors.Any(e => e.Category == ValidationErrorCategory.ModuleConfiguration)).IsTrue();
    }

    [Test]
    public async Task BuildAsync_WithValidConfiguration_ReturnsHost()
    {
        // Arrange
        var builder = PipelineHostBuilder.Create()
            .AddModule<SimpleModule>();

        // Act
        var host = await builder.BuildAsync();

        // Assert
        await Assert.That(host).IsNotNull();
        await Assert.That(host.Services).IsNotNull();

        // Cleanup
        await host.DisposeAsync();
    }

    [Test]
    public async Task BuildAsync_WithNoModules_ThrowsValidationException()
    {
        // Arrange
        var builder = PipelineHostBuilder.Create();

        // Act & Assert
        await Assert.ThrowsAsync<PipelineValidationException>(async () =>
        {
            await builder.BuildAsync();
        });
    }

    [Test]
    public async Task ValidateAsync_WithMissingDependency_ReturnsError()
    {
        // Arrange
        var builder = PipelineHostBuilder.Create()
            .AddModule<ModuleWithMissingDep>(); // MissingModule is not registered

        // Act
        var result = await builder.ValidateAsync();

        // Assert
        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.Errors.Any(e => e.Category == ValidationErrorCategory.Dependency)).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithOptionalMissingDependency_ReturnsNoError()
    {
        // Arrange
        var builder = PipelineHostBuilder.Create()
            .AddModule<ModuleWithOptionalDep>(); // MissingModule is not registered but marked as optional

        // Act
        var result = await builder.ValidateAsync();

        // Assert - Should only have the "no modules" error or be valid if optional deps are counted
        // The optional dependency should not cause a dependency error
        var hasDependencyError = result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Dependency &&
            e.Message.Contains("MissingModule"));
        await Assert.That(hasDependencyError).IsFalse();
    }

    [Test]
    public async Task RunAsync_AfterBuildAsync_ExecutesPipeline()
    {
        // Arrange
        var builder = PipelineHostBuilder.Create()
            .AddModule<SimpleModule>();

        // Act
        var host = await builder.BuildAsync();
        var summary = await host.RunAsync();

        // Assert
        await Assert.That(summary).IsNotNull();
        await Assert.That(summary.Modules).IsNotNull();

        // Cleanup
        await host.DisposeAsync();
    }

    [Test]
    public async Task ValidationResult_WithError_HasErrorsIsTrue()
    {
        // Arrange
        var error = new ValidationError(ValidationErrorCategory.Options, "Test error");
        var result = ValidationResult.WithError(error);

        // Act & Assert
        await Assert.That(result.HasErrors).IsEqualTo(true);
        await Assert.That(result.IsValid).IsEqualTo(false);
        await Assert.That(result.Errors.Count).IsEqualTo(1);
    }

    [Test]
    public async Task ValidationResult_Success_IsValid()
    {
        // Arrange
        var result = ValidationResult.Success();

        // Act & Assert
        await Assert.That(result.HasErrors).IsEqualTo(false);
        await Assert.That(result.IsValid).IsEqualTo(true);
    }

    [Test]
    public async Task ValidationResult_Merge_CombinesErrors()
    {
        // Arrange
        var error1 = new ValidationError(ValidationErrorCategory.Options, "Error 1");
        var error2 = new ValidationError(ValidationErrorCategory.Dependency, "Error 2");
        var result1 = ValidationResult.WithError(error1);
        var result2 = ValidationResult.WithError(error2);

        // Act
        result1.Merge(result2);

        // Assert
        await Assert.That(result1.Errors.Count).IsEqualTo(2);
    }

    [Test]
    public async Task ValidationError_ToString_IncludesCategory()
    {
        // Arrange
        var error = new ValidationError(ValidationErrorCategory.Dependency, "Test message");

        // Act
        var str = error.ToString();

        // Assert
        await Assert.That(str.Contains("Dependency")).IsEqualTo(true);
        await Assert.That(str.Contains("Test message")).IsEqualTo(true);
    }

    [Test]
    public async Task ValidationError_ToString_WithSourceType_IncludesTypeName()
    {
        // Arrange
        var error = new ValidationError(
            ValidationErrorCategory.ModuleConfiguration,
            "Test message",
            typeof(SimpleModule));

        // Act
        var str = error.ToString();

        // Assert
        await Assert.That(str.Contains("SimpleModule")).IsEqualTo(true);
    }

    [Test]
    public async Task ValidateAsync_WithNegativeRetryCount_ReturnsError()
    {
        // Arrange
        var builder = PipelineHostBuilder.Create()
            .AddModule<SimpleModule>()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.DefaultRetryCount = -1;
            });

        // Act
        var result = await builder.ValidateAsync();

        // Assert
        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Options &&
            e.Message.Contains("DefaultRetryCount"))).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithConflictingCategories_ReturnsError()
    {
        // Arrange
        var builder = PipelineHostBuilder.Create()
            .AddModule<SimpleModule>()
            .ConfigurePipelineOptions((_, options) =>
            {
                options.RunOnlyCategories = ["Category1"];
                options.IgnoreCategories = ["Category1"];
            });

        // Act
        var result = await builder.ValidateAsync();

        // Assert
        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Options &&
            e.Message.Contains("Category1"))).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithSelfReferencingModule_ReturnsError()
    {
        // Arrange
        var builder = PipelineHostBuilder.Create()
            .AddModule<SelfReferencingModule>();

        // Act
        var result = await builder.ValidateAsync();

        // Assert
        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Dependency &&
            e.Message.Contains("SelfReferencingModule") &&
            e.Message.Contains("cannot reference itself"))).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithCircularDependency_ReturnsError()
    {
        // Arrange - ModuleA -> ModuleB -> ModuleC -> ModuleA (circular)
        var builder = PipelineHostBuilder.Create()
            .AddModule<ModuleA>()
            .AddModule<ModuleB>()
            .AddModule<ModuleC>();

        // Act
        var result = await builder.ValidateAsync();

        // Assert
        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Dependency &&
            (e.Message.Contains("Circular dependency") || e.Message.Contains("Dependency collision")))).IsTrue();
    }

    [Test]
    public async Task BuildAsync_WithSelfReferencingModule_ThrowsValidationException()
    {
        // Arrange
        var builder = PipelineHostBuilder.Create()
            .AddModule<SelfReferencingModule>();

        // Act & Assert
        await Assert.ThrowsAsync<PipelineValidationException>(async () =>
        {
            await builder.BuildAsync();
        });
    }

    [Test]
    public async Task BuildAsync_WithCircularDependency_ThrowsValidationException()
    {
        // Arrange - ModuleA -> ModuleB -> ModuleC -> ModuleA (circular)
        var builder = PipelineHostBuilder.Create()
            .AddModule<ModuleA>()
            .AddModule<ModuleB>()
            .AddModule<ModuleC>();

        // Act & Assert
        await Assert.ThrowsAsync<PipelineValidationException>(async () =>
        {
            await builder.BuildAsync();
        });
    }
}
