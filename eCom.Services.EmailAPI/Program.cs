
using eCom.Services.EmailAPI.Data;
using eCom.Services.EmailAPI.Extension;
using eCom.Services.EmailAPI.Messaging;
using eCom.Services.EmailAPI.Service;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
{

});

var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));


builder.Services.AddSingleton(new EmailService(optionBuilder.Options));
builder.Services.AddSingleton<EmailSender>(); // Register EmailSender without interface
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();

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
ApplyMigration();
app.UseAzureServiceBusConsumer();
app.Run();

void ApplyMigration()
{
	using (var scope = app.Services.CreateScope())
	{
		var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		if (_db.Database.GetPendingMigrations().Count() > 0)
		{
			_db.Database.Migrate();
		}
	}
}