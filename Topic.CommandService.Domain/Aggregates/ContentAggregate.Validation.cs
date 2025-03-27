using System.Runtime.CompilerServices;

namespace Topic.CommandService.Domain.Aggregates;

internal partial class ContentAggregate
{
    private void EnsureMessageIsValid(string message)
    {
        if (String.IsNullOrWhiteSpace(,message))
        {
            throw new InvalidOperationException($@"Значение {nameof(message)} не может быть пустым.
                                                   Пожалуйста, укажите действительный {nameof(message)}!");
        }
    }

    private void EnsureCommentTextIsValid(string commentText)
    {
        if (String.IsNullOrWhiteSpace(commentText))
        {
            throw new InvalidOperationException($@"Значение {nameof(commentText)} не может быть пустым.
                                                   Пожалуйста, укажите действительный {nameof(commentText)}");
        }
    }
}
