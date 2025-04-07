using Core.MediatR;
using Microsoft.AspNetCore.Mvc;
using Topic.CommandService.Api.Comments.Commands.CreateComment;
using Topic.CommandService.Api.Endpoints;
using Topic.CommandService.Api.ResponceDtos;

namespace Topic.CommandService.Api.Comments.Commands.Endpoints;

public class CreateCommentEndpoint : BaseEndpoint<CreateCommentCommand>
{
    protected override string GetSuccessMessage => "Комментарий успешно добавлен";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/topics/{topicId:guid}/comments", async (
            Guid topicId,
            [FromBody] CreateCommentCommand command,
            ICommandDispatcher commandDispatcher,
            ILogger<CreateCommentEndpoint> logger) =>
        {
            command.MessageId = topicId;
            return await ExecuteCommandAsync(command,
                cmd => commandDispatcher.SendCommandAsync(cmd), logger);
        })
        .Produces<ResponseDto>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}