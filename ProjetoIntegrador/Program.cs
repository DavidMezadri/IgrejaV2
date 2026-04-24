using IgrejaV2.Infraestrutura;
using IgrejaV2.Infraestrutura.DatabaseConfig;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura Serviços da API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Registra toda a camada de Infraestrutura (EF, Dapper, Repositórios)
builder.Services.AddInfraestrutura(builder.Configuration);

var app = builder.Build();

// 3. Inicialização Automática do Banco de Dados
// Isso executa o script 01_TabelasIniciais.sql no startup
using (var scope = app.Services.CreateScope())
{
    var inicializador = scope.ServiceProvider.GetRequiredService<IInicializadorBanco>();
    try 
    {
        await inicializador.InicializarAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao inicializar o banco de dados.");
    }
}

// 4. Configura o Pipeline de Requisição (Middleware)
if (app.Environment.IsDevelopment())
{
    // Habilita Swagger no modo Desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IgrejaV2 API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
