using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.UnitTests.Helpers;

/// <summary>
/// Consolidated tests for encoding and hashing operations using TUnit's data-driven testing.
/// This replaces the individual Base64Tests, HexTests, Md5Tests, Sha1Tests, Sha256Tests,
/// Sha384Tests, and Sha512Tests classes to eliminate DRY violations.
/// </summary>
public class EncodingTests : TestBase
{
    private const string TestInput = TestConstants.TestString;

    #region Module Definitions

    // Concrete module classes for each encoding/hash operation
    // These are required because RunModule<T> needs concrete types

    private class ToBase64Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Base64.ToBase64String(TestInput);
        }
    }

    private class FromBase64Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Base64.FromBase64String("Rm9vIGJhciE=");
        }
    }

    private class ToHexModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hex.ToHex(TestInput);
        }
    }

    private class FromHexModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hex.FromHex("466f6f2062617221");
        }
    }

    private class Md5Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Md5(TestInput);
        }
    }

    private class Sha1Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha1(TestInput);
        }
    }

    private class Sha256Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha256(TestInput);
        }
    }

    private class Sha384Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha384(TestInput);
        }
    }

    private class Sha512Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha512(TestInput);
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
        await Assert.That(moduleResult.Value).IsEqualTo("Rm9vIGJhciE=");
    }

    [Test]
    [DisplayName("Base64: FromBase64String does not error and produces correct output")]
    public async Task From_Base64_Works_Correctly()
    {
        var moduleResult = await await RunModule<FromBase64Module>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.Value).IsEqualTo(TestInput);
    }

    [Test]
    [DisplayName("Hex: ToHex does not error and produces correct output")]
    public async Task To_Hex_Works_Correctly()
    {
        var moduleResult = await await RunModule<ToHexModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.Value).IsEqualTo("466f6f2062617221");
    }

    [Test]
    [DisplayName("Hex: FromHex does not error and produces correct output")]
    public async Task From_Hex_Works_Correctly()
    {
        var moduleResult = await await RunModule<FromHexModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.Value).IsEqualTo(TestInput);
    }

    #endregion

    #region Hash Tests

    [Test]
    [DisplayName("Md5: Hash does not error and produces correct output")]
    public async Task Md5_Works_Correctly()
    {
        var moduleResult = await await RunModule<Md5Module>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.Value).IsEqualTo("b9c291e3274aa5c8010a7c5c22a4e6dd");
    }

    [Test]
    [DisplayName("Sha1: Hash does not error and produces correct output")]
    public async Task Sha1_Works_Correctly()
    {
        var moduleResult = await await RunModule<Sha1Module>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.Value).IsEqualTo("cc3626c5ad2e3aff0779dc63e80555c463fd99dc");
    }

    [Test]
    [DisplayName("Sha256: Hash does not error and produces correct output")]
    public async Task Sha256_Works_Correctly()
    {
        var moduleResult = await await RunModule<Sha256Module>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.Value).IsEqualTo("d80c14a132a9ae008c78db4ee4cbc46b015b5e0f018f6b0a3e4ea5041176b852");
    }

    [Test]
    [DisplayName("Sha384: Hash does not error and produces correct output")]
    public async Task Sha384_Works_Correctly()
    {
        var moduleResult = await await RunModule<Sha384Module>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.Value).IsEqualTo("bb338a277da65d5663467d5fd98aa67349506150cd1287597b0eaa0f0988d2b22c33504fd85dd0b8c99ce8cc50666f88");
    }

    [Test]
    [DisplayName("Sha512: Hash does not error and produces correct output")]
    public async Task Sha512_Works_Correctly()
    {
        var moduleResult = await await RunModule<Sha512Module>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
        await Assert.That(moduleResult.Value).IsEqualTo("e399b0584705f5f229a4398baa31c4b7cc820ac208327d26e66f0668288536981c3460a7ea92ef6be488ce30ff5b6a991babfe24833094eba3226cea5c14162c");
    }

    #endregion
}
