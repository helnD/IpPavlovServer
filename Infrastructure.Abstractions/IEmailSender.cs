using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Abstractions;

/// <summary>
/// Interface that provides functionality for email sending.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Send question.
    /// </summary>
    /// <param name="name">Name of user who send question.</param>
    /// <param name="email">Email of sender.</param>
    /// <param name="question">Question text.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task SendQuestionAsync(string name, string email, string question, CancellationToken cancellationToken);

    Task SendCooperationRequest(string name, string company, string email, CancellationToken cancellationToken);
}