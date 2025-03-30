namespace Core.Events;

public record EventModel(
    Guid Id, 
    DateTime CreatedAt, 
    Guid AggregateId, 
    string AggregateType, 
    int Version,
    string EventType,
    BaseEvent EventData
    );