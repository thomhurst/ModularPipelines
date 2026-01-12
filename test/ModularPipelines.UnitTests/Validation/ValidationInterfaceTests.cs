using ModularPipelines.Context;
using ModularPipelines.Validation;
using System.Reflection;

namespace ModularPipelines.UnitTests.Validation;

/// <summary>
/// Tests for validation interface structure.
/// </summary>
public class ValidationInterfaceTests
{
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
    public async Task IPipelineValidator_ShouldHaveOrderAndValidateMembers()
    {
        var validatorType = typeof(IPipelineValidator);

        var orderProperty = validatorType.GetProperty("Order");
        await Assert.That(orderProperty).IsNotNull()
            .Because("IPipelineValidator should have Order property");

        var validateMethod = validatorType.GetMethod("Validate");
        await Assert.That(validateMethod).IsNotNull()
            .Because("IPipelineValidator should have Validate method");
    }
}
