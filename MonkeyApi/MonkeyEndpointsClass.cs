using Microsoft.EntityFrameworkCore;
using MonkeyApi.Data;
using MonkeyApi.Model;
namespace MonkeyApi;

public static class MonkeyEndpointsClass
{
    public static void MapMonkeyEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Monkey", async (MonkeyApiContext db) =>
        {
            return await db.Monkey.ToListAsync();
        })
        .WithName("GetAllMonkeys")
        .Produces<List<Monkey>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Monkey/{id}", async (int Id, MonkeyApiContext db) =>
        {
            return await db.Monkey.FindAsync(Id)
                is Monkey model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetMonkeyById")
        .Produces<Monkey>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Monkey/{id}", async (int Id, Monkey monkey, MonkeyApiContext db) =>
        {
            var foundModel = await db.Monkey.FindAsync(Id);

            if (foundModel is null)
            {
                return Results.NotFound();
            }
            
            db.Update(monkey);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateMonkey")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Monkey/", async (Monkey monkey, MonkeyApiContext db) =>
        {
            db.Monkey.Add(monkey);
            await db.SaveChangesAsync();
            return Results.Created($"/Monkeys/{monkey.Id}", monkey);
        })
        .WithName("CreateMonkey")
        .Produces<Monkey>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Monkey/{id}", async (int Id, MonkeyApiContext db) =>
        {
            if (await db.Monkey.FindAsync(Id) is Monkey monkey)
            {
                db.Monkey.Remove(monkey);
                await db.SaveChangesAsync();
                return Results.Ok(monkey);
            }

            return Results.NotFound();
        })
        .WithName("DeleteMonkey")
        .Produces<Monkey>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
