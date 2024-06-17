using Dental_clinic_WebApi.Data;
using Dental_clinic_WebApi.Module;

public class Program 
{
    public static void Main(string[] args) 
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ConsumptionDb>();
        builder.Services.AddDbContext<DrugsDb>();
        builder.Services.AddDbContext<DepartmentsDb>();
        builder.Services.AddDbContext<InventoryDb>();
        builder.Services.AddDbContext<SuppliersDb>();
        builder.Services.AddDbContext<SupplyDb>();
        builder.Services.AddDbContext<UsersDb>();
        /*(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("Dental_clinic"));
        });*/

        //builder.Services.AddScoped<IPatientRepository, PatientRepository>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            /*using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PatientsDb>();
            db.Database.EnsureCreated();*/
        }

        app.MapEntityEndpoints<Consumption, ConsumptionDb>("/consumption"); 
        app.MapEntityEndpoints<Departments, DepartmentsDb>("/departments");
        app.MapEntityEndpoints<Drugs, DrugsDb>("/drugs");
        app.MapEntityEndpoints<Inventory, InventoryDb>("/inventory");
        app.MapEntityEndpoints<Suppliers, SuppliersDb>("/suppliers");
        app.MapEntityEndpoints<Supply, SupplyDb>("/supply");
        app.MapEntityEndpoints<Users, UsersDb>("/users");
        app.Run();
    }
}

public static class EntityEndpoints 
{
    public static void MapEntityEndpoints<TEntity, TDbContext>(this WebApplication app, string routePrefix)
        where TEntity : class, IEntity
        where TDbContext : DbContext
    {
        app.MapGet(routePrefix, async (TDbContext dbContext) =>
        {
            var entities = await dbContext.Set<TEntity>().ToListAsync();
            return entities;
        });

        app.MapGet($"{routePrefix}/{{id}}", async (int id, TDbContext dbContext) =>
            {
                var entity = await dbContext.Set<TEntity>().FindAsync(id);
                if(entity == null) return Results.NotFound();
                return Results.Ok(entity);
            });

        app.MapPost(routePrefix, async (TDbContext dbContext, TEntity entity) =>
        {
            dbContext.Set<TEntity>().Add(entity);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/{routePrefix}/{entity.Id}", entity);
        });

        app.MapPut($"{routePrefix}/{{id}}", async (int id, TDbContext dbContext, TEntity entit) =>
        {
            var existingEntity = await dbContext.Set<TEntity>().FindAsync(id);
            if (existingEntity == null) return Results.NotFound();
            dbContext.Entry(existingEntity).CurrentValues.SetValues(entit);
            await dbContext.SaveChangesAsync();
            return Results.Ok(entit);
        });

        app.MapDelete($"{routePrefix}/{{id}}", async (int id, TDbContext dbContext) =>
        {
            if (await dbContext.Set<TEntity>().FindAsync(id) is TEntity entity) 
            {
                dbContext.Set<TEntity>().Remove(entity);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.NotFound();
        });

        app.UseHttpsRedirection();

    }
}

