using Core.Events.Topics.CreateTopic;
using Core.Events.Topics.LikeTopic;
using Core.Events.Topics.RemoveTopic;
using Core.Events.Topics.UpdateTopic;

namespace Topic.CommandService.Domain.Aggregates;

public partial class ContentAggregate
{
    private string author = default!;
    private bool Active { get; set; } = default!;
    public ContentAggregate(Guid id, string authorName, string messageText)
    {
        RegisterEvent(new CreateTopicEvent
        {
            MessageId = id,
            AuthorName = authorName,
            MessageText = messageText,
            CreateDate = DateTime.UtcNow
        });
    }

    public void Apply(CreateTopicEvent createTopicEvent)
    {
        Active = true;
        AggregateId = createTopicEvent.MessageId;
        author = createTopicEvent.AuthorName;
    }

    public void UpdateTopic(string messageText)
    {
        EnsureTopicIsActive();
        EnsureMessageIsValid(messageText);

        RegisterEvent(new UpdateTopicEvent
        {
            MessageId = AggregateId,
            MessageText = messageText
        });
    }

    public void Apply(UpdateTopicEvent updateTopicEvent)
    {
        AggregateId = updateTopicEvent.MessageId;
    }

    public void RemoveTopic(string username)
    {
        EnsureTopicIsActive();
        EnsureUserIsAuthor(username);
        RegisterEvent(new RemoveTopicEvent { MessageId = AggregateId });
    }

    public void Apply(RemoveTopicEvent removeTopicEvent)
    {
        AggregateId = removeTopicEvent.MessageId;
        Active = false;
    }

    public void LikeTopic()
    {
        EnsureTopicIsActive();
        RegisterEvent(new LikeTopicEvent { MessageId = AggregateId });
    }

    public void Apply(LikeTopicEvent likeTopicEvent)
    {
        AggregateId = likeTopicEvent.MessageId;
        Active = false;
    }
}
