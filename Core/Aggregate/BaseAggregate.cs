using Core.Events;

namespace Core.Aggregate;

public abstract class BaseAggregate
{
    public Guid AggregateId { get; protected set; }
    public int Version { get; set; } = 0;

    private readonly List<BaseEvent> events = new();

    public IEnumerable<BaseEvent> GetPendingEvents => events.AsReadOnly();

    public void ClearPendingEvents() => events.Clear();
    private void HandleEvent(BaseEvent baseEvent, bool isNew)
    {
        var applyMethod = this.GetType().GetMethod("Apply", [baseEvent.GetType()])
            ?? throw new InvalidOperationException($"Метод apply для типа {baseEvent.GetType().Name} не найден");

        applyMethod.Invoke(this, [baseEvent]);

        if (isNew)
            events.Add(baseEvent);
    }

    protected void RegisterEvent(BaseEvent baseEvent) => 
        HandleEvent(baseEvent, true);

    public void RebuildState(IEnumerable<BaseEvent> events)
    {
        foreach (var item in events)
            HandleEvent(item, false);
    }
}
