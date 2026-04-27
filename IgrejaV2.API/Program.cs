using System.Text;
using IgrejaV2.Aplicacao.Servico;
using IgrejaV2.API.Servicos;
using IgrejaV2.Infraestrutura;
using IgrejaV2.Infraestrutura.DatabaseConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1. Configura Serviços da API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. Swagger com comentários XML e autenticação JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "IgrejaV2 API",
        Version = "v1",
        Description = "API REST para gestão de igrejas: usuários, eventos, presenças, patrimônio e mais.",
        Contact = new Microsoft.OpenApi.OpenApiContact
        {
            Name = "Equipe IgrejaV2",
            Email = "suporte@igrejav2.com"
        }
    });

    // Lê os comentários XML gerados pelo compilador
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);

    // Configuração do botão "Authorize" para JWT Bearer
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Description = "Cole aqui o token JWT retornado pelo endpoint **POST /api/auth/login**.\n\nFormato: `Bearer {seu_token}`"
    });

    c.AddSecurityRequirement(doc => new Microsoft.OpenApi.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", doc),
            new List<string>()
        }
    });
});

// 3. Autenticação JWT
var jwtChave = builder.Configuration["Jwt:Chave"]
    ?? throw new InvalidOperationException("Configuração 'Jwt:Chave' não encontrada.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Emissor"],
            ValidAudience = builder.Configuration["Jwt:Audiencia"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtChave)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// 4. Registra toda a camada de Infraestrutura (EF, Dapper, Repositórios)
builder.Services.AddInfraestrutura(builder.Configuration);

// 5. Registra serviços da camada de Aplicação
builder.Services.AddScoped<UsuarioServico>();
builder.Services.AddScoped<AuthServico>();
builder.Services.AddScoped<PessoaServico>();
builder.Services.AddScoped<FamiliaServico>();
builder.Services.AddScoped<EventoServico>();
builder.Services.AddScoped<TipoEventoServico>();
builder.Services.AddScoped<PresencaServico>();

// 6. Registra serviços da API
builder.Services.AddScoped<TokenServico>();

var app = builder.Build();

// 7. Inicialização Automática do Banco de Dados
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

// 8. Configura o Pipeline de Requisição (Middleware)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IgrejaV2 API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
