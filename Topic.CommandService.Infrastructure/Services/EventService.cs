﻿using Core.Events;
using Core.Events.Dao;
using Core.Exceptions;
using Core.KafkaProducer;
using Core.Service;
using Microsoft.Extensions.Configuration;
using Topic.CommandService.Domain.Aggregates;

namespace Topic.CommandService.Infrastructure.Services;

public class EventService(IEventStorage eventStorage, IEventKafkaProducer eventProducer,IConfiguration configuration)
                : IEventService
{
    public async Task<IEnumerable<BaseEvent>> GetEventsAsync(Guid aggregateId, CancellationToken ct)
    {
        var events = await eventStorage.FindByAggregateId(aggregateId, ct);

        if (events is null || !events.Any())
            throw new AggregateNotFoundException();

        return events.OrderBy(e => e.Version).Select(e => e.EventData).ToList();

    }

    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion, CancellationToken ct)
    {
        var eventStream = await eventStorage.FindByAggregateId(aggregateId, ct);

        if (expectedVersion != 0 && eventStream.Last().Version != expectedVersion)
            throw new VersionConflictException();

        var version = expectedVersion;

        foreach (var item in events)
        {
            version++;
            item.Version = version;
            var eventType = item.GetType().Name;

            var eventModel = new EventModel(
                Id: Guid.NewGuid(),  // В БД сменить типа поля на uuid
                CreatedAt: DateTime.Now,
                AggregateId: aggregateId,
                AggregateType: nameof(ContentAggregate),
                Version: version,
                EventType: eventType,
                EventData: item
            );

            await eventStorage.SaveAsync(eventModel, ct);
            var topic = configuration["Kafka:Topic"];
            await eventProducer.PublishEventAsync(topic, item);
        }
    }
}
