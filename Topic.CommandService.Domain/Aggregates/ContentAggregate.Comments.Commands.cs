using Core.Events.Comments.CreateComment;
using Core.Events.Comments.RemoveComment;
using Core.Events.Comments.UpdateComment;

namespace Topic.CommandService.Domain.Aggregates;

public partial class ContentAggregate
{
    private readonly Dictionary<Guid, (string text, string authorName)> comments = new();
    public void CreateComment(string commentText, string authorName)
    {
        EnsureTopicIsActive();
        EnsureCommentTextIsValid(commentText);

        RegisterEvent(new CreateCommentEvent
        {
            MessageId = AggregateId,
            CommentId = Guid.NewGuid(),
            Text = commentText,
            AuthorName = authorName,
            CreateDate = DateTime.UtcNow,
        });
    }

    public void Apply(CreateCommentEvent createCommentEvent)
    {
        AggregateId = createCommentEvent.MessageId;
        comments.Add(createCommentEvent.CommentId, (createCommentEvent.Text, createCommentEvent.AuthorName));
    }

    public void UpdateComment(Guid commentId, string commentText, string authorName)
    {
        EnsureTopicIsActive();
        EnsureCommentTextIsValid(commentText);
        EnsureCommentBelongsToUser(commentId, authorName);

        RegisterEvent(new UpdateCommentEvent
        {
            MessageId = AggregateId,
            CommentId = commentId,
            Text = commentText,
            AuthorName = authorName,
            UpdateDate = DateTime.UtcNow
        });
    }

    public void Apply(UpdateCommentEvent updateCommentEvent)
    {
        AggregateId = updateCommentEvent.MessageId;

        comments[updateCommentEvent.CommentId] = (updateCommentEvent.Text, comments[updateCommentEvent.CommentId].authorName);
    }

    public void RemoveComment(Guid commentId, string username)
    {
        EnsureTopicIsActive();
        EnsureCommentBelongsToUser(commentId, username);

        RegisterEvent(new RemoveCommentEvent 
        {
            MessageId = AggregateId,
            CommentId = commentId
        });
    }

    public void Apply(RemoveCommentEvent removeCommentEvent)
    {
        AggregateId = removeCommentEvent.MessageId;
        Active = false;
    }
}
