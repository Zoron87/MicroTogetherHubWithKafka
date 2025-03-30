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
            EventId = AggregateId,
            AuthorName = authorName,
            MessageText = messageText,
            CreateDate = DateTime.UtcNow
        });
    }

    public void Apply(CreateTopicEvent createTopicEvent)
    {
        Active = true;
        AggregateId = createTopicEvent.EventId;
        author = createTopicEvent.AuthorName;
    }

    public void UpdateTopic(string messageText)
    {
        EnsureTopicIsActive();
        EnsureMessageIsValid(messageText);

        RegisterEvent(new UpdateTopicEvent
        {
            EventId = AggregateId,
            MessageText = messageText
        });
    }

    public void Apply(UpdateTopicEvent updateTopicEvent)
    {
        AggregateId = updateTopicEvent.EventId;
    }

    public void RemoveTopic(string username)
    {
        EnsureTopicIsActive();
        EnsureUserIsAuthor(username);
        RegisterEvent(new RemoveTopicEvent { EventId = AggregateId });
    }

    public void Apply(RemoveTopicEvent removeTopicEvent)
    {
        AggregateId = removeTopicEvent.EventId;
        Active = false;
    }

    public void LikeTopic()
    {
        EnsureTopicIsActive();
        RegisterEvent(new LikeTopicEvent { EventId = AggregateId });
    }

    public void Apply(LikeTopicEvent likeTopicEvent)
    {
        AggregateId = likeTopicEvent.EventId;
        Active = false;
    }
}
