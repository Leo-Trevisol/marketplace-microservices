var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // descobre os endpoints
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";

    options.IncludeXmlComments(
        Path.Combine(AppContext.BaseDirectory, xmlFilename)
    );
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // serve JSON da spec em /swagger/v1/swagger.json
    app.UseSwaggerUI(); // serve a interface visual em /swagger
}
app.UseAuthorization();
app.MapControllers();
app.Run();