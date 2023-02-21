using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(conf => conf.Filters.Add<ValidationFilter>()).ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
builder.Services.AddFluentValidationAutoValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage<AzureStorage>();
builder.Services.AddCors(opt => opt.AddDefaultPolicy( policy => policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
