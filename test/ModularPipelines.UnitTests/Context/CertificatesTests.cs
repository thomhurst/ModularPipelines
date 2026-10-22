using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using ModularPipelines.Context;

namespace ModularPipelines.UnitTests.Context;

public class CertificatesTests
{
    [Test]
    public async Task Lookup_Disposes_All_Input_Certificates_And_Returns_An_Independent_Certificate()
    {
        using var first = CreateCertificate("CN=contoso.com");
        using var second = CreateCertificate("CN=contoso.com");
        using var unrelated = CreateCertificate("CN=notcontoso.com");
        var thumbprint = first.Thumbprint;

        using var result = Certificates.FindCertificate(new X509Certificate2Collection { first, second, unrelated },
            X509FindType.FindBySubjectDistinguishedName, first.Subject, validOnly: false);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Thumbprint).IsEqualTo(thumbprint);
        await Assert.That(first.Handle).IsEqualTo(IntPtr.Zero);
        await Assert.That(second.Handle).IsEqualTo(IntPtr.Zero);
        await Assert.That(unrelated.Handle).IsEqualTo(IntPtr.Zero);
        using var key = result.GetRSAPrivateKey();
        var data = new byte[] { 1, 2, 3 };
        var signature = key!.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        await Assert.That(key.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)).IsTrue();
    }

    [Test]
    public async Task Exact_Subject_Lookup_Does_Not_Match_A_Substring()
    {
        using var unrelated = CreateCertificate("CN=notcontoso.com");
        using var result = Certificates.FindCertificate(new X509Certificate2Collection(unrelated),
            X509FindType.FindBySubjectDistinguishedName, "CN=contoso.com", validOnly: false);

        await Assert.That(result).IsNull();
        await Assert.That(unrelated.Handle).IsEqualTo(IntPtr.Zero);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Lookup_Respects_ValidOnly(bool validOnly)
    {
        using var expired = CreateCertificate("CN=expired.example", expired: true);
        var subject = expired.Subject;
        using var result = Certificates.FindCertificate(new X509Certificate2Collection(expired),
            X509FindType.FindBySubjectDistinguishedName, subject, validOnly);

        await Assert.That(result is null).IsEqualTo(validOnly);
        await Assert.That(expired.Handle).IsEqualTo(IntPtr.Zero);
    }

    [Test]
    public async Task Lookup_Disposes_Input_Certificates_When_Find_Throws()
    {
        using var certificate = CreateCertificate("CN=contoso.com");

        await Assert.That(() => Certificates.FindCertificate(new X509Certificate2Collection(certificate),
            (X509FindType) int.MaxValue, "invalid", validOnly: false)).Throws<CryptographicException>();

        await Assert.That(certificate.Handle).IsEqualTo(IntPtr.Zero);
    }

    [Test]
    public async Task Empty_Collection_Returns_Null()
    {
        using var result = Certificates.FindCertificate(new X509Certificate2Collection(),
            X509FindType.FindBySubjectDistinguishedName, "CN=missing.example", validOnly: false);

        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task Store_Lookup_Uses_The_Selected_Store_And_Exact_Subject_By_Default()
    {
        await WithCertificateInPersonalStore(async (context, certificate) =>
        {
            var simpleName = certificate.GetNameInfo(X509NameType.SimpleName, forIssuer: false);
            using var exact = context.GetCertificateBySubject(StoreName.My, StoreLocation.CurrentUser, certificate.Subject, validOnly: false);
            using var partial = context.GetCertificateBySubject(StoreName.My, StoreLocation.CurrentUser, simpleName, validOnly: false);
            using var substring = context.GetCertificateBySubject(StoreName.My, StoreLocation.CurrentUser, simpleName, validOnly: false, exactMatch: false);
            using var wrongStore = context.GetCertificateByThumbprint(StoreName.Root, StoreLocation.CurrentUser, certificate.Thumbprint, validOnly: false);

            await Assert.That(exact?.Thumbprint).IsEqualTo(certificate.Thumbprint);
            await Assert.That(partial).IsNull();
            await Assert.That(substring?.Thumbprint).IsEqualTo(certificate.Thumbprint);
            await Assert.That(wrongStore).IsNull();
        });
    }

    [Test]
    public async Task Store_Overloads_Check_Validity_By_Default_And_Can_Include_Invalid_Certificates()
    {
        await WithCertificateInPersonalStore(async (context, certificate) =>
        {
            using var subject = context.GetCertificateBySubject(StoreName.My, StoreLocation.CurrentUser, certificate.Subject);
            using var thumbprint = context.GetCertificateByThumbprint(StoreName.My, StoreLocation.CurrentUser, certificate.Thumbprint);
            using var serial = context.GetCertificateBySerialNumber(StoreName.My, StoreLocation.CurrentUser, certificate.SerialNumber);
            using var custom = context.GetCertificateBy(StoreName.My, StoreLocation.CurrentUser, X509FindType.FindByThumbprint, certificate.Thumbprint);
            using var invalidSubject = context.GetCertificateBySubject(StoreName.My, StoreLocation.CurrentUser, certificate.Subject, validOnly: false);
            using var invalidThumbprint = context.GetCertificateByThumbprint(StoreName.My, StoreLocation.CurrentUser, certificate.Thumbprint, validOnly: false);
            using var invalidSerial = context.GetCertificateBySerialNumber(StoreName.My, StoreLocation.CurrentUser, certificate.SerialNumber, validOnly: false);
            using var invalidCustom = context.GetCertificateBy(StoreName.My, StoreLocation.CurrentUser, X509FindType.FindByThumbprint, certificate.Thumbprint, validOnly: false);

            await Assert.That(subject).IsNull();
            await Assert.That(thumbprint).IsNull();
            await Assert.That(serial).IsNull();
            await Assert.That(custom).IsNull();
            await Assert.That(invalidSubject?.Thumbprint).IsEqualTo(certificate.Thumbprint);
            await Assert.That(invalidThumbprint?.Thumbprint).IsEqualTo(certificate.Thumbprint);
            await Assert.That(invalidSerial?.Thumbprint).IsEqualTo(certificate.Thumbprint);
            await Assert.That(invalidCustom?.Thumbprint).IsEqualTo(certificate.Thumbprint);
        });
    }

    [Test]
    public async Task Legacy_Overloads_Preserve_Personal_Store_And_Invalid_Substring_Matches()
    {
        await WithCertificateInPersonalStore(async (context, certificate) =>
        {
            var simpleName = certificate.GetNameInfo(X509NameType.SimpleName, forIssuer: false);
            using var subject = context.GetCertificateBySubject(StoreLocation.CurrentUser, simpleName);
            using var thumbprint = context.GetCertificateByThumbprint(StoreLocation.CurrentUser, certificate.Thumbprint);
            using var serial = context.GetCertificateBySerialNumber(StoreLocation.CurrentUser, certificate.SerialNumber);
            using var custom = context.GetCertificateBy(StoreLocation.CurrentUser, X509FindType.FindByThumbprint, certificate.Thumbprint);

            await Assert.That(subject?.Thumbprint).IsEqualTo(certificate.Thumbprint);
            await Assert.That(thumbprint?.Thumbprint).IsEqualTo(certificate.Thumbprint);
            await Assert.That(serial?.Thumbprint).IsEqualTo(certificate.Thumbprint);
            await Assert.That(custom?.Thumbprint).IsEqualTo(certificate.Thumbprint);
        });
    }

    private static async Task WithCertificateInPersonalStore(Func<ICertificatesContext, X509Certificate2, Task> action)
    {
        using var source = CreateCertificate($"CN=certificate-test-{Guid.NewGuid():N}", expired: true);
        using var certificate = X509CertificateLoader.LoadCertificate(source.RawData);
        using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadWrite);
        try
        {
            store.Add(certificate);
            await action(new Certificates(), certificate);
        }
        finally
        {
            store.Remove(certificate);
        }
    }

    private static X509Certificate2 CreateCertificate(string subject, bool expired = false)
    {
        using var key = RSA.Create(2048);
        var request = new CertificateRequest(subject, key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var now = DateTimeOffset.UtcNow;
        return request.CreateSelfSigned(now.AddDays(-2), expired ? now.AddDays(-1) : now.AddDays(1));
    }
}
