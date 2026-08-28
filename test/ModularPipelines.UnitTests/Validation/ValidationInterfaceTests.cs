using ModularPipelines.Context;
using ModularPipelines.Validation;
using System.Reflection;

namespace ModularPipelines.UnitTests.Validation;

/// <summary>
/// Tests for validation interface structure.
/// </summary>
public class ValidationInterfaceTests
{
    public sealed class TestValidator : IPipelineValidator
    {
        public int Order => 0;

        public Task<ValidationResult> ValidateAsync(IServiceProvider services) =>
            Task.FromResult(ValidationResult.Success());
    }

    [Test]
    public async Task IPipelineValidationService_ShouldBeInternal()
    {
        var assembly = typeof(IModuleContext).Assembly;
        var iface = assembly.GetType("ModularPipelines.Validation.IPipelineValidationService");

        await Assert.That(iface).IsNotNull()
            .Because("IPipelineValidationService should exist");
        await Assert.That(iface!.IsPublic).IsFalse()
            .Because("IPipelineValidationService should be internal (implementation detail)");
    }

    [Test]
    public async Task IPipelineValidator_ShouldRemainPublic()
    {
        // IPipelineValidator is a user extension point - custom validators can implement it
        var validatorType = typeof(IPipelineValidator);

        await Assert.That(validatorType.IsPublic).IsTrue()
            .Because("IPipelineValidator should be public for custom implementations");
    }

    [Test]
    public async Task IPipelineValidator_ShouldHaveOrderAndValidateAsyncMembers()
    {
        var validatorType = typeof(IPipelineValidator);

        var orderProperty = validatorType.GetProperty("Order");
        await Assert.That(orderProperty).IsNotNull()
            .Because("IPipelineValidator should have Order property");

        var validateAsyncMethod = validatorType.GetMethod(nameof(IPipelineValidator.ValidateAsync));
        await Assert.That(validateAsyncMethod).IsNotNull()
            .Because("IPipelineValidator should have ValidateAsync method");
    }

    [Test]
    public async Task SpecializedValidatorInterfacesShouldBeInternal()
    {
        var assembly = typeof(IPipelineValidator).Assembly;
        var typeNames = new[]
        {
            "ModularPipelines.Validation.IDependencyValidator",
            "ModularPipelines.Validation.IModuleConfigurationValidator",
            "ModularPipelines.Validation.IOptionsValidator",
        };

        foreach (var typeName in typeNames)
        {
            var type = assembly.GetType(typeName);
            await Assert.That(type).IsNotNull();
            await Assert.That(type!.IsNotPublic).IsTrue();
        }
    }

    [Test]
    public async Task AddValidatorRegistersPublicValidatorExtensionPoint()
    {
        var builder = Pipeline.CreateBuilder();

        var result = builder.AddValidator<TestValidator>();
        var descriptor = builder.Services.Single(service => service.ServiceType == typeof(IPipelineValidator));

        await Assert.That(result).IsSameReferenceAs(builder);
        await Assert.That(descriptor.ImplementationType).IsEqualTo(typeof(TestValidator));
    }
}
