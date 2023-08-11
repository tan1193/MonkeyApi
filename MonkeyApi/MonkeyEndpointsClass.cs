using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonkeyApi.Data;
using MonkeyApi.Model;
namespace MonkeyApi;

public static class MonkeyEndpointsClass
{
    public static void MapMonkeyEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Monkey");

        group.MapGet("", async (MonkeyApiContext db) =>
        {
            return await db.Monkey.ToListAsync();
        })
        .WithName("GetAllMonkeys")
        .Produces<List<Monkey>>(StatusCodes.Status200OK);

        group.MapGet("{id}", async ([FromQuery] int Id, SqlConnectionFactory db) =>
        {
            using var connection = db.CreateConnection();

            const string sql = "SELECT * FROM Monkey WHERE Id = @Id";

            var monkey = await connection.QuerySingleOrDefaultAsync<Monkey>(sql, new { Id });

            return monkey
                is Monkey model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetMonkeyById")
        .Produces<Monkey>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("{id}", async (int Id, Monkey monkey, MonkeyApiContext db) =>
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

        group.MapPost("", async (Monkey monkey, MonkeyApiContext db) =>
        {
            db.Monkey.Add(monkey);
            await db.SaveChangesAsync();
            return Results.Created($"/Monkeys/{monkey.Id}", monkey);
        })
        .WithName("CreateMonkey")
        .Produces<Monkey>(StatusCodes.Status201Created);

        group.MapDelete("{id}", async (int Id, MonkeyApiContext db) =>
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
