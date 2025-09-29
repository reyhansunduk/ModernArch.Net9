using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Basit global hata yakalama
app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (Exception ex)
    {
        ctx.Response.StatusCode = 500;
        await ctx.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});

// Migration otomatik çalýþsýn
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

// Minimal API endpoint group
var customers = app.MapGroup("/api/customers");

// CREATE
customers.MapPost("", async (AppDbContext db, CustomerCreateDto dto) =>
{
    var c = new Domain.Entities.Customer(dto.FirstName, dto.LastName, dto.Email);
    db.Customers.Add(c);
    await db.SaveChangesAsync();
    return Results.Created($"/api/customers/{c.Id}", new { c.Id });
});

// LIST
customers.MapGet("", async (AppDbContext db) =>
{
    var list = await db.Customers
        .OrderByDescending(x => x.Id)
        .Select(x => new CustomerDto(x.Id, x.FirstName, x.LastName, x.Email, x.IsActive))
        .ToListAsync();
    return Results.Ok(list);
});

// GET by ID
customers.MapGet("{id:long}", async (long id, AppDbContext db) =>
{
    var c = await db.Customers.FindAsync(id);
    return c is null
        ? Results.NotFound()
        : Results.Ok(new CustomerDto(c.Id, c.FirstName, c.LastName, c.Email, c.IsActive));
});

// UPDATE
customers.MapPut("{id:long}", async (long id, AppDbContext db, CustomerUpdateDto dto) =>
{
    var c = await db.Customers.FindAsync(id);
    if (c is null) return Results.NotFound();
    c.Update(dto.FirstName, dto.LastName, dto.Email);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// DEACTIVATE
customers.MapPost("{id:long}/deactivate", async (long id, AppDbContext db) =>
{
    var c = await db.Customers.FindAsync(id);
    if (c is null) return Results.NotFound();
    c.Deactivate();
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();