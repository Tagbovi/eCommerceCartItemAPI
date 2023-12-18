using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoppingCart.Controllers;
using ShoppingCart.DBContext;
using ShoppingCart.LoggingError;
using ShoppingCart.Services;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateActor = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };
    }
    );
builder.Services.AddLogging();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartUserService,CartUserService>();
builder.Services.AddScoped<ILoggerWrapper,LoggerWrapper>();
builder.Services.AddScoped<ILoggerWrapperLogin,LoggerWrapperLogin>();
builder.Services.AddSingleton(builder.Configuration);
//builder.Services.AddScoped<ITokenGeneratorService,TokenGeneratorService>();
//builder.Services.AddMvc().AddControllersAsServices();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<CartDBContext>(options=> options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options=>
    {
      var xmlFile=$"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath=Path.Combine(AppContext.BaseDirectory, xmlFile);

        options.IncludeXmlComments(xmlPath);
    }
    
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
