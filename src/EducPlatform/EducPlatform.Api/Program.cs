using EducPlatform.Api.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithJwt();

builder.Services.AddControllers();
builder.Services.AddCors();

// Registra o serviço que escuta a fila RabbitMQ
builder.Services.AddRabbitMQInjection(builder.Configuration);

// Registra MediatR
builder.Services.AddAlunoContext(builder.Configuration);
builder.Services.AddCursoContext(builder.Configuration);
builder.Services.AddPagamentoContext(builder.Configuration);

//Jeito alternativo de registrar os handlers
  //builder.Services.AddMediatR(cfg =>
  //  cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.InitializeDatabaseAsync();

await app.RunAsync();
