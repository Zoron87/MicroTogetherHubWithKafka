﻿using Core.MediatR;
using Microsoft.AspNetCore.Mvc;
using Topic.CommandService.Api.Endpoints;
using Topic.CommandService.Api.ResponseDtos;
using Topic.CommandService.Api.Topics.Commands.CreateTopic;

namespace Topic.CommandService.Api.Topics.Endpoints;

public class CreateTopicEndpoint : BaseEndpoint<CreateTopicCommand>
{
    protected override string GetSuccessMessage => "Топик успешно создан";

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/topics", async ([FromBody] CreateTopicCommand command, ILogger<CreateTopicEndpoint> logger,
           ICommandDispatcher commandDispatcher) =>
        {
            command.Id = Guid.NewGuid();
            var test = await ExecuteCommandAsync(command,
                cmd => commandDispatcher.SendCommandAsync(cmd), logger);
            return test;
        })
       .Produces<ResponseDto>(StatusCodes.Status201Created)
       .ProducesProblem(StatusCodes.Status400BadRequest)
       .ProducesProblem(StatusCodes.Status404NotFound)
       .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
