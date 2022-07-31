namespace Infrastructure.Settings;

/// <summary>
/// This class describes SMTP configuration.
/// </summary>
public class SmtpConfiguration
{
    /// <summary>
    /// Path to folder with templates.
    /// </summary>
    public string PathToTemplates { get; init; }

    /// <summary>
    /// Email server domain.
    /// </summary>
    public string Domain { get; init; }

    /// <summary>
    /// Email server port.
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// Login.
    /// </summary>
    public string Login { get; init; }

    /// <summary>
    /// Password.
    /// </summary>
    public string Password { get; init; }
}