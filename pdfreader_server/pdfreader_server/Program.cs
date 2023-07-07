// Add services to the container.




using Microsoft.AspNetCore.Hosting;
using pdfreader_server;



var app = Startup.InitializeApp(args);



// Configure the HTTP request pipeline.




app.Run();