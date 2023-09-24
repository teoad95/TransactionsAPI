using Microsoft.EntityFrameworkCore;
using TransactionsAPI;
using TransactionsAPI.Data;
using TransactionsAPI.Models;
using TransactionsAPI.Models.Validators;
using TransactionsAPI.Models.Validators.Interfaces;
using TransactionsAPI.Repository;
using TransactionsAPI.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});
builder.Services.AddScoped<ITransactionRepository, TransactionsRepository>();
builder.Services.AddScoped<ITransactionValidator, TransactionDataValidator>();
builder.Services.AddScoped<ITransactionBuilder, TransactionBuilder>();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
