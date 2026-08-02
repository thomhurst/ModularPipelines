using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Validation;


namespace ModularPipelines.UnitTests.Validation;

public class ValidationTests
{
    private sealed class LegacyOptionsValidator : IOptionsValidator
    {
        public int Order => 0;

        public ValidationResult Validate(IServiceProvider services) => ValidationResult.Success();

        public ValidationResult ValidateOptions(PipelineOptions options) =>
            ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.Options,
                "legacy validator called"));
    }

    // Test modules for various scenarios
    private class SimpleModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("success");
    }

    private class AnotherModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(42);
    }

    [ModuleCategory("Build")]
    private class CategorizedModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("categorized");
    }

    // Module that depends on itself (invalid)
    [ModularPipelines.Attributes.DependsOn<SelfReferencingModule>]
    private class SelfReferencingModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("success");
    }

    // Modules for circular dependency test
    [ModularPipelines.Attributes.DependsOn<ModuleB>]
    private class ModuleA : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("A");
    }

    [ModularPipelines.Attributes.DependsOn<ModuleC>]
    private class ModuleB : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("B");
    }

    [ModularPipelines.Attributes.DependsOn<ModuleA>]
    private class ModuleC : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("C");
    }

    // Module with missing required dependency (not registered)
    [ModularPipelines.Attributes.DependsOn<MissingModule>(Optional = false)]
    private class ModuleWithMissingDep : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("success");
    }

    private class MissingModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("missing");
    }

    // Module with optional dependency (missing but ignored)
    [ModularPipelines.Attributes.DependsOn<MissingModule>(Optional = true)]
    private class ModuleWithOptionalDep : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("success");
    }

    private class FluentMissingDependencyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("missing");
    }

    private class ModuleWithFluentMissingDependency : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<FluentMissingDependencyModule>()
            .Build();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("success");
    }

    private class NeverRun : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(false);
    }

    [RunIfAll<NeverRun>]
    private class ExecutionSkippedModuleWithFluentMissingDependency : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<FluentMissingDependencyModule>()
            .Build();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("success");
    }

    private class FluentSelfReferencingModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<FluentSelfReferencingModule>()
            .Build();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("self");
    }

    private class FluentCycleModuleA : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<FluentCycleModuleB>()
            .Build();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("a");
    }

    private class FluentCycleModuleB : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<FluentCycleModuleA>()
            .Build();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("b");
    }

    [ModuleTag("selector-cycle-a")]
    [DependsOnModulesWithTag("selector-cycle-b")]
    private class SelectorCycleModuleA : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("a");
    }

    [ModuleTag("selector-cycle-b")]
    [DependsOnModulesWithTag("selector-cycle-a")]
    private class SelectorCycleModuleB : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("b");
    }

    private sealed class ThrowingValidator : IPipelineValidator, IAsyncDisposable
    {
        public int Order => int.MaxValue;

        public bool IsDisposed { get; private set; }

        public ValidationResult Validate(IServiceProvider services)
        {
            throw new InvalidOperationException("Custom validation failed.");
        }

        public ValueTask DisposeAsync()
        {
            IsDisposed = true;
            return ValueTask.CompletedTask;
        }
    }

    [Test]
    public async Task ValidateAsync_WithValidConfiguration_ReturnsNoErrors()
    {
        // Arrange
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SimpleModule>().AddModule<AnotherModule>();

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
        var builder = Pipeline.CreateBuilder();

        // Act
        var result = await builder.ValidateAsync();

        // Assert
        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.Errors.Any(e => e.Category == ValidationErrorCategory.ModuleConfiguration)).IsTrue();
    }

    [Test]
    public async Task BuildAsync_WithValidConfiguration_ReturnsPipeline()
    {
        // Arrange
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SimpleModule>();

        // Act
        var pipeline = await builder.BuildAsync();

        // Assert
        await Assert.That(pipeline).IsNotNull();
        await Assert.That(pipeline.Services).IsNotNull();

        // Cleanup
        await pipeline.DisposeAsync();
    }

    [Test]
    public async Task BuildAsync_WithNoModules_ThrowsValidationException()
    {
        // Arrange
        var builder = Pipeline.CreateBuilder();

        // Act & Assert
        await Assert.ThrowsAsync<PipelineValidationException>(async () =>
        {
            await builder.BuildAsync();
        });
    }

    [Test]
    public async Task BuildAsync_WhenCustomValidatorThrows_DisposesPipeline()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SimpleModule>();
        ThrowingValidator? validator = null;
        builder.Services.AddSingleton<IPipelineValidator>(_ => validator = new ThrowingValidator());

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => builder.BuildAsync());

        await Assert.That(exception!.Message).IsEqualTo("Custom validation failed.");
        await Assert.That(validator).IsNotNull();
        await Assert.That(validator!.IsDisposed).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithMissingRequiredDependency_ReturnsNoError_BecauseAutoRegistered()
    {
        // Arrange
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<ModuleWithMissingDep>(); // MissingModule is not registered but will be auto-registered

        // Act
        var result = await builder.ValidateAsync();

        // Assert - Required dependencies are auto-registered, so no dependency error
        var hasDependencyError = result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Dependency &&
            e.Message.Contains("MissingModule"));
        await Assert.That(hasDependencyError).IsFalse();
    }

    [Test]
    public async Task ValidateAsync_WithOptionalMissingDependency_ReturnsNoError()
    {
        // Arrange
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<ModuleWithOptionalDep>(); // MissingModule is not registered but marked as optional

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
    public async Task BuildAsync_WithFluentMissingDependency_ThrowsValidationException()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<ModuleWithFluentMissingDependency>();

        await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());
    }

    [Test]
    public async Task BuildAsync_Rejects_Fluent_Missing_Dependency_For_Execution_Skipped_Module()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<ExecutionSkippedModuleWithFluentMissingDependency>();

        await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());
    }

    [Test]
    public async Task ValidateAsync_Rejects_Fluent_Missing_Dependency_For_Execution_Skipped_Module()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<ExecutionSkippedModuleWithFluentMissingDependency>();

        await AssertDependencyValidationError(builder);
    }

    [Test]
    public async Task ValidateAsync_Rejects_Fluent_Missing_Dependency()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<ModuleWithFluentMissingDependency>();

        await AssertDependencyValidationError(builder);
    }

    [Test]
    public async Task ValidateAsync_Rejects_Fluent_Self_Dependency()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<FluentSelfReferencingModule>();

        await AssertDependencyValidationError(builder);
    }

    [Test]
    public async Task ValidateAsync_Rejects_Fluent_Cycle()
    {
        var builder = Pipeline.CreateBuilder();
        builder
            .AddModule<FluentCycleModuleA>()
            .AddModule<FluentCycleModuleB>();

        await AssertDependencyValidationError(builder);
    }

    [Test]
    public async Task BuildAsync_Rejects_Selector_Cycle()
    {
        var builder = Pipeline.CreateBuilder();
        builder
            .AddModule<SelectorCycleModuleA>()
            .AddModule<SelectorCycleModuleB>();

        await Assert.ThrowsAsync<PipelineValidationException>(() => builder.BuildAsync());
    }

    [Test]
    public async Task ValidateAsync_Rejects_Selector_Cycle()
    {
        var builder = Pipeline.CreateBuilder();
        builder
            .AddModule<SelectorCycleModuleA>()
            .AddModule<SelectorCycleModuleB>();

        await AssertDependencyValidationError(builder);
    }

    private static async Task AssertDependencyValidationError(PipelineBuilder builder)
    {
        var result = await builder.ValidateAsync();

        await Assert.That(result.Errors).Contains(error => error.Category == ValidationErrorCategory.Dependency);
    }

    [Test]
    public async Task RunAsync_AfterBuildAsync_ExecutesPipeline()
    {
        // Arrange
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SimpleModule>();

        // Act
        var pipeline = await builder.BuildAsync();
        var summary = await pipeline.RunAsync();

        // Assert
        await Assert.That(summary).IsNotNull();
        await Assert.That(summary.Modules).IsNotNull();

        // Cleanup
        await pipeline.DisposeAsync();
    }

    [Test]
    public async Task ValidationResult_WithError_HasErrorsIsTrue()
    {
        // Arrange
        var error = new ValidationError(ValidationErrorCategory.Options, "Test error");
        var result = ValidationResult.WithError(error);

        // Act & Assert
        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors.Count).IsEqualTo(1);
    }

    [Test]
    public async Task ValidationResult_Success_IsValid()
    {
        // Arrange
        var result = ValidationResult.Success();

        // Act & Assert
        await Assert.That(result.HasErrors).IsFalse();
        await Assert.That(result.IsValid).IsTrue();
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
        await Assert.That(str.Contains("Dependency")).IsTrue();
        await Assert.That(str.Contains("Test message")).IsTrue();
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
        await Assert.That(str.Contains("SimpleModule")).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithNegativeRetryCount_ReturnsError()
    {
        // Arrange
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SimpleModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            DefaultRetryCount = -1,
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
    public async Task ValidateAsync_WithNegativeDefaultModuleTimeout_ReturnsError()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SimpleModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            DefaultModuleTimeout = TimeSpan.FromSeconds(-1),
        });

        var result = await builder.ValidateAsync();

        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Options &&
            e.Message.Contains("DefaultModuleTimeout"))).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithNegativeModuleOutputFlushInterval_ReturnsError()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SimpleModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            ModuleOutputFlushInterval = TimeSpan.FromSeconds(-1),
        });

        var result = await builder.ValidateAsync();

        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Options &&
            e.Message.Contains("ModuleOutputFlushInterval"))).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithTooLargeModuleOutputFlushInterval_ReturnsError()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SimpleModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            ModuleOutputFlushInterval = TimeSpan.FromMilliseconds(uint.MaxValue),
        });

        var result = await builder.ValidateAsync();

        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Options &&
            e.Message.Contains("ModuleOutputFlushInterval"))).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithNegativeModuleOutputFlushThreshold_ReturnsError()
    {
        var builder = Pipeline.CreateBuilder();
        builder.Services.AddModule<SimpleModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            ModuleOutputFlushThreshold = -1,
        });

        var result = await builder.ValidateAsync();

        await Assert.That(result.HasErrors).IsTrue();
        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Options &&
            e.Message.Contains("ModuleOutputFlushThreshold"))).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithConflictingCategories_ReturnsError()
    {
        // Arrange
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SimpleModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            RunOnlyCategories = ["Category1"],
            IgnoreCategories = ["Category1"],
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
    public async Task ValidateAsync_WithCaseInsensitiveCategoryConflict_ReturnsError()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<CategorizedModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            RunOnlyCategories = ["Build"],
            IgnoreCategories = ["build"],
        });

        var result = await builder.ValidateAsync();

        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Options &&
            e.Message.Contains("Build", StringComparison.OrdinalIgnoreCase))).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithUnknownRunOnlyCategory_ReturnsError()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<CategorizedModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            RunOnlyCategories = ["Typo"],
        });

        var result = await builder.ValidateAsync();

        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Options &&
            e.Message.Contains("RunOnlyCategories") &&
            e.Message.Contains("zero registered modules") &&
            e.Message.Contains("Typo"))).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithPartlyUnknownRunOnlyCategories_ReturnsError()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<CategorizedModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            RunOnlyCategories = ["build", "Typo"],
        });

        var result = await builder.ValidateAsync();

        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Options &&
            e.Message.Contains("RunOnlyCategories") &&
            e.Message.Contains("Typo"))).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithUnknownIgnoreCategory_ReturnsError()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<CategorizedModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            IgnoreCategories = ["Typo"],
        });

        var result = await builder.ValidateAsync();

        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Options &&
            e.Message.Contains("IgnoreCategories") &&
            e.Message.Contains("Typo"))).IsTrue();
    }

    [Test]
    public async Task ValidateAsync_WithCaseInsensitiveCategoryMatch_ReturnsNoCategoryErrors()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<CategorizedModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            RunOnlyCategories = ["build"],
        });

        var result = await builder.ValidateAsync();

        await Assert.That(result.Errors.Any(e =>
            e.Category == ValidationErrorCategory.Options &&
            e.Message.Contains("categor", StringComparison.OrdinalIgnoreCase))).IsFalse();
    }

    [Test]
    public async Task ValidateOptions_WithRegisteredCategories_RejectsUnknownCategory()
    {
        IOptionsValidator validator = new OptionsValidator();
        var options = new PipelineOptions
        {
            RunOnlyCategories = ["Typo"],
        };

        var result = validator.ValidateOptions(
            options,
            new HashSet<string>(["Build"], StringComparer.OrdinalIgnoreCase));

        await Assert.That(result.Errors).Contains(error =>
            error.Category == ValidationErrorCategory.Options &&
            error.Message.Contains("Typo"));
    }

    [Test]
    public async Task ValidateOptions_WithCaseSensitiveCategorySet_MatchesIgnoringCase()
    {
        IOptionsValidator validator = new OptionsValidator();
        var options = new PipelineOptions
        {
            RunOnlyCategories = ["build"],
            IgnoreCategories = ["test"],
        };

        var result = validator.ValidateOptions(
            options,
            new HashSet<string>(["Build", "Test"]));

        await Assert.That(result.Errors.Any(error =>
            error.Category == ValidationErrorCategory.Options &&
            error.Message.Contains("categor", StringComparison.OrdinalIgnoreCase))).IsFalse();
    }

    [Test]
    public async Task CategoryAwareOverloadUsesLegacyValidatorImplementationByDefault()
    {
        IOptionsValidator validator = new LegacyOptionsValidator();

        var result = validator.ValidateOptions(
            new PipelineOptions(),
            new HashSet<string>(StringComparer.OrdinalIgnoreCase));

        await Assert.That(result.Errors).Contains(error =>
            error.Message == "legacy validator called");
    }

    [Test]
    public async Task ValidateAsync_WithSelfReferencingModule_ReturnsError()
    {
        // Arrange
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SelfReferencingModule>();

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
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<ModuleA>().AddModule<ModuleB>().AddModule<ModuleC>();

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
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SelfReferencingModule>();

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
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<ModuleA>().AddModule<ModuleB>().AddModule<ModuleC>();

        // Act & Assert
        await Assert.ThrowsAsync<PipelineValidationException>(async () =>
        {
            await builder.BuildAsync();
        });
    }
}
