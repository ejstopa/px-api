using Application.Features.Items;
using Application.Features.Items.Commands.CreateItem;
using Application.Features.Items.Commands.CreateItemReservation;
using Application.Features.Items.Commands.DeleteItemReservation;
using Application.Features.Items.GetItemByIdQuery.Queries;
using Application.Features.Items.Queries.GetItemsByDynamicParams;
using Application.Features.Items.Queries.GetItemsByFamily;
using Application.Features.Items.Queries.GetItemsByUserWorkspace;
using Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints
{
    public static class ItemEndpoints
    {
        public static void AddItemEndpoints(this WebApplication app)
        {
            app.MapGet("items/{id}", async (ISender sender, int id) =>
            {
                ItemResponseDto? item = await sender.Send(new GetItemByIdQuery { Id = id });

                if (item is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(item);
            }).WithName("GetItemById");

            app.MapGet("items", async (ISender sender, [FromQuery] string family) =>
            {
                List<ItemResponseDto> items = await sender.Send(new GetItemsByFamilyCommand { Family = family });

                return Results.Ok(items);
            });

            app.MapGet("users/{userId}/workspace/items", async (ISender sender, int userId) =>
            {
                Result<List<ItemResponseDto>> itemsResult = await sender.Send(new GetItemsByUserWorkspaceCommand { UserId = userId });

                if (itemsResult.Error != null)
                {
                    return Results.Problem(itemsResult.Error.Message, null, itemsResult.Error.StatusCode);
                }

                return Results.Ok(itemsResult.Value);

            }).WithName("GetItemsByUserWorkspace");

            app.MapPost("items", async (ISender sender, CreateItemCommand createItemCommand) =>
             {
                 Result<ItemResponseDto?> result = await sender.Send(createItemCommand);

                 if (result.Error != null)
                 {
                     return Results.Problem(result.Error.Message, null, result.Error.StatusCode);
                 }

                 return Results.Ok(result.Value);
             }).WithName("CreateItem");

            app.MapPost("items/reservations", async (ISender sender, CreateItemReservationCommand createItemReservationCommand) =>
            {
                var result = await sender.Send(createItemReservationCommand);

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result.Error.StatusCode);
                }

                return Results.Ok(result.Value!);
            }).WithName("CreateItemReservation");

            app.MapDelete("items/reservations", async (ISender sender, [FromQuery] int itemId, [FromQuery] int userId) =>
            {
                var result = await sender.Send(new DeleteItemReservationCommand { ItemId = itemId, UserId = userId });

                if (!result.IsSuccess)
                {
                    return Results.Problem(result.Error!.Message, null, result.Error.StatusCode);
                }

                return Results.Ok(result.Value!);
            }).WithName("DeleteItemReservation"); 

            app.MapPost("items/query", async (ISender sender, GetItemsByDynamicParamsQuery getItemsByDynamicParamsQuery) =>
            {
                List<ItemResponseDto> items = await sender.Send(getItemsByDynamicParamsQuery);

                return Results.Ok(items);
            }).WithName("GetItemsByDynamicParams");

            
        }

        
    }
}