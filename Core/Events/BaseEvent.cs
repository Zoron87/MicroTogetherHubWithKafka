﻿using Core.Messages;

namespace Core.Events;

public abstract class BaseEvent : BaseMessage
{
    public string EventType { get; set; }
    public int Version { get; set; }

    protected BaseEvent(string eventType)
    {
        EventType = eventType;
    }
}
