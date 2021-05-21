namespace Infrastructure.Settings
{
    /// <summary>
    /// Class provide settings of database initialization.
    /// </summary>
    public class DatabaseInitialization
    {
        /// <summary>
        /// Path to file with categories description.
        /// </summary>
        public string CategoriesFile { get; init; }

        /// <summary>
        /// Path to file with partners description.
        /// </summary>
        public string PartnersFile { get; init; }

        /// <summary>
        /// Path to file with certificates description.
        /// </summary>
        public string CertificatesFile { get; init; }
    }
}