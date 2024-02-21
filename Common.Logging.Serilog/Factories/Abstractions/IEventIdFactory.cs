using Microsoft.Extensions.Logging;

namespace Common.Logging.Serilog.Factories.Abstractions;

public interface IEventIdFactory
{
    EventId Create<TEnum>(TEnum eventType) where TEnum : Enum;
}