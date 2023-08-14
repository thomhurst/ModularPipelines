using System.Security.Cryptography.X509Certificates;

namespace ModularPipelines.Context;

internal class Certificates : ICertificates
{
    public X509Certificate2? GetCertificateBySubject(StoreLocation storeLocation, string subject)
    {
        return GetCertificateBy(storeLocation, X509FindType.FindBySubjectName, subject);
    }

    public X509Certificate2? GetCertificateByThumbprint(StoreLocation storeLocation, string thumbprint)
    {
        return GetCertificateBy(storeLocation, X509FindType.FindByThumbprint, thumbprint);
    }

    public X509Certificate2? GetCertificateBySerialNumber(StoreLocation storeLocation, string serialNumber)
    {
        return GetCertificateBy(storeLocation, X509FindType.FindBySerialNumber, serialNumber);
    }

    public X509Certificate2? GetCertificateBy(StoreLocation storeLocation, X509FindType findType, string findValue)
    {
        using var store = new X509Store(storeLocation);
        store.Open(OpenFlags.ReadOnly);

        var certificate2Collection = store.Certificates.Find(findType, findValue, false);

        return certificate2Collection.FirstOrDefault();
    }
}
