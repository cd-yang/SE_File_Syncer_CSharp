using SE_File_Syncer;
using System.Management;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(new AppHelper(configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


//Create a WMI query to monitor USB drive changes
string query = "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2";

//Create a ManagementEventWatcher object and subscribe to the EventArrived event
var watcher = new ManagementEventWatcher(query);
if (watcher != null)
{
    watcher.EventArrived += new EventArrivedEventHandler((object sender, EventArrivedEventArgs e) =>
    {
        Console.WriteLine("USB drive change detected.");
        FileSyncer.AutoCopyFiles(e);
    });

    //Start the watcher
    watcher.Start();
}

//Stop the watcher when no longer needed
//watcher.Stop();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
