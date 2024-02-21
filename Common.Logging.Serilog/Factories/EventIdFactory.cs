using Common.Logging.Serilog.Factories.Abstractions;
using Microsoft.Extensions.Logging;

namespace Common.Logging.Serilog.Factories;

public class EventIdFactory : IEventIdFactory
{
    public EventId Create<TEnum>(TEnum eventType) where TEnum : Enum
    {
        return new EventId(Convert.ToInt32(eventType), eventType.ToString());
    }
}