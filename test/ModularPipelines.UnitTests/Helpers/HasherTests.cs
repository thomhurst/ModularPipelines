using ModularPipelines.Context;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

/// <summary>
/// Parameterized tests for all hash algorithms using TUnit's data-driven testing.
/// Uses [Arguments] attribute to test Md5, Sha1, Sha256, Sha384, and Sha512 with a single test method.
/// This consolidates what would otherwise be 5 separate test classes with identical structure.
/// </summary>
public class HasherTests : TestBase
{
    private const string TestInput = TestConstants.TestString;

    [Test]
    [DisplayName("Hash algorithm '$algorithm' produces correct output")]
    [Arguments("Md5", "b9c291e3274aa5c8010a7c5c22a4e6dd")]
    [Arguments("Sha1", "cc3626c5ad2e3aff0779dc63e80555c463fd99dc")]
    [Arguments("Sha256", "d80c14a132a9ae008c78db4ee4cbc46b015b5e0f018f6b0a3e4ea5041176b852")]
    [Arguments("Sha384", "bb338a277da65d5663467d5fd98aa67349506150cd1287597b0eaa0f0988d2b22c33504fd85dd0b8c99ce8cc50666f88")]
    [Arguments("Sha512", "e399b0584705f5f229a4398baa31c4b7cc820ac208327d26e66f0668288536981c3460a7ea92ef6be488ce30ff5b6a991babfe24833094eba3226cea5c14162c")]
    public async Task Hash_Algorithm_Produces_Expected_Output(string algorithm, string expected)
    {
        var hasher = await GetService<IHasher>();

        var result = algorithm switch
        {
            "Md5" => hasher.Md5(TestInput),
            "Sha1" => hasher.Sha1(TestInput),
            "Sha256" => hasher.Sha256(TestInput),
            "Sha384" => hasher.Sha384(TestInput),
            "Sha512" => hasher.Sha512(TestInput),
            _ => throw new ArgumentException($"Unknown algorithm: {algorithm}", nameof(algorithm))
        };

        await Assert.That(result).IsEqualTo(expected);
    }
}
