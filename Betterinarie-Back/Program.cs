using Betterinarie_Back.Application.Interfaces.Implementation;
using Betterinarie_Back.Application.Interfaces.Security;
using Betterinarie_Back.Application.Mappings;
using Betterinarie_Back.Application.Services.Implementation;
using Betterinarie_Back.Application.Services.Security;
using Betterinarie_Back.Core.Entities.Implementation;
using Betterinarie_Back.Core.Interfaces.Implementation;
using Betterinarie_Back.Infrastructure.Data;
using Betterinarie_Back.Infrastructure.Repositories.Implementation;
using Betterinarie_Back.Infrastructure.Repositories.RootBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDataServices(connectionString);

builder.Services.AddIdentity<Usuario, Rol>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<BetterinarieContext>()
.AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var key = Convert.FromBase64String(builder.Configuration["Jwt:Key"] ?? Convert.ToBase64String(Encoding.ASCII.GetBytes("tu_clave_secreta_aqui_muy_segura")));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IMascotaService, MascotaService>();
builder.Services.AddScoped<IMascotaRepository, MascotaRepository>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IErrorLogService, ErrorLogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ErrorLoggingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BetterinarieContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Rol>>();

    await SeedData.Seed(context, userManager, roleManager);
}

app.MapControllers();

app.Run();
