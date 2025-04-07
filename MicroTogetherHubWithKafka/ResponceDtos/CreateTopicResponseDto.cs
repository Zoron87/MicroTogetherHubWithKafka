using Core.ResponseDtos;

namespace Topic.CommandService.Api.ResponceDtos;

public class CreateTopicResponseDto : BaseResponse
{
    public Guid Id { get; set; }
}