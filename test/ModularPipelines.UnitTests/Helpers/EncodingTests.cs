using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.UnitTests.Helpers;

/// <summary>
/// Tests for bidirectional encoding operations (Base64 and Hex).
/// Hash algorithm tests are in HasherTests.cs using parameterized tests.
/// </summary>
public class EncodingTests : TestBase
{
    private const string TestInput = TestConstants.TestString;

    #region Module Definitions

    // Concrete module classes for each encoding operation
    // These are required because RunModule<T> needs concrete types

    private class ToBase64Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Data.Base64.ToBase64String(TestInput);
        }
    }

    private class FromBase64Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Data.Base64.FromBase64String("Rm9vIGJhciE=");
        }
    }

    private class ToHexModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Data.Hex.ToHex(TestInput);
        }
    }

    private class FromHexModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Data.Hex.FromHex("466f6f2062617221");
        }
    }

    #endregion

    #region Bidirectional Encoding Tests (Base64, Hex)

    [Test]
    [DisplayName("Base64: ToBase64String does not error and produces correct output")]
    public async Task To_Base64_Works_Correctly()
    {
        var moduleResult = await await RunModule<ToBase64Module>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.ValueOrDefault).IsEqualTo("Rm9vIGJhciE=");
    }

    [Test]
    [DisplayName("Base64: FromBase64String does not error and produces correct output")]
    public async Task From_Base64_Works_Correctly()
    {
        var moduleResult = await await RunModule<FromBase64Module>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.ValueOrDefault).IsEqualTo(TestInput);
    }

    [Test]
    [DisplayName("Hex: ToHex does not error and produces correct output")]
    public async Task To_Hex_Works_Correctly()
    {
        var moduleResult = await await RunModule<ToHexModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.ValueOrDefault).IsEqualTo("466f6f2062617221");
    }

    [Test]
    [DisplayName("Hex: FromHex does not error and produces correct output")]
    public async Task From_Hex_Works_Correctly()
    {
        var moduleResult = await await RunModule<FromHexModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.ValueOrDefault).IsEqualTo(TestInput);
    }

    #endregion
}
