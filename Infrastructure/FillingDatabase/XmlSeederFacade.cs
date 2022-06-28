using Domain;

namespace Infrastructure.FillingDatabase;

/// <summary>
/// Facade for seeding data based on XML.
/// </summary>
public class XmlSeederFacade
{
    public XmlSeederFacade(XmlSeeder<Category> categoriesSeeder, XmlSeeder<Certificate> certificatesSeeder,
        XmlSeeder<Partner> partnersSeeder, XmlSeeder<SalesRepresentative> salesRepresentativesSeeder)
    {
        CategoriesSeeder = categoriesSeeder;
        CertificatesSeeder = certificatesSeeder;
        PartnersSeeder = partnersSeeder;
        SalesRepresentativesSeeder = salesRepresentativesSeeder;
    }

    /// <summary>
    /// Categories seeder.
    /// </summary>
    public XmlSeeder<Category> CategoriesSeeder { get; }

    /// <summary>
    /// Certificates seeder.
    /// </summary>
    public XmlSeeder<Certificate> CertificatesSeeder { get; }

    /// <summary>
    /// Partners seeder.
    /// </summary>
    public XmlSeeder<Partner> PartnersSeeder { get; }

    /// <summary>
    /// Representatives seeder.
    /// </summary>
    public XmlSeeder<SalesRepresentative> SalesRepresentativesSeeder { get; }
}