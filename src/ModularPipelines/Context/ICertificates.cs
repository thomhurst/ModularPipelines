using System.Security.Cryptography.X509Certificates;

namespace ModularPipelines.Context;

public interface ICertificates
{
    public X509Certificate2? GetCertificateBySubject(StoreLocation storeLocation, string subject);

    public X509Certificate2? GetCertificateByThumbprint(StoreLocation storeLocation, string thumbprint);

    public X509Certificate2? GetCertificateBySerialNumber(StoreLocation storeLocation, string serialNumber);

    X509Certificate2? GetCertificateBy(StoreLocation storeLocation, X509FindType findType, string findValue);
}