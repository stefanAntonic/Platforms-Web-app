using PlatformService.Dto;

namespace PlatformService.AsyncDataServices;

public interface IMessageBusClient
{
    void PublishNewPlatform( PlatformPublishDto platformPublishDto);
}