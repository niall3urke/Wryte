using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TG.Blazor.IndexedDB;
using Wryte.Services;
using Wryte;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ModalService>();

ConfigureIndexedDB(builder.Services);

await builder.Build().RunAsync();



static void ConfigureIndexedDB(IServiceCollection services)
{
    // Define the tables

    var novels = new StoreSchema
    {
        Name = DatabaseService.Novels,
        PrimaryKey = new IndexSpec
        {
            Name = "id",
            KeyPath = "id",
            Unique = true
        }
    };

    var categories = new StoreSchema()
    {
        Name = DatabaseService.Categories,
        PrimaryKey = new IndexSpec
        {
            Name = "id",
            KeyPath = "id",
            Unique = true,
        }
    };

    var groups = new StoreSchema
    {
        Name = DatabaseService.Groups,
        PrimaryKey = new IndexSpec
        {
            Name = "id",
            KeyPath = "id",
            Unique = true
        }
    };

    var items = new StoreSchema
    {
        Name = DatabaseService.Items,
        PrimaryKey = new IndexSpec
        {
            Name = "id",
            KeyPath = "id",
            Unique = true
        }
    };



    var settings = new StoreSchema
    {
        Name = DatabaseService.Settings,
        PrimaryKey = new IndexSpec
        {
            Name = "id",
            KeyPath = "id",
            Unique = true
        }
    };

    var events = new StoreSchema
    {
        Name = DatabaseService.Events,
        PrimaryKey = new IndexSpec
        {
            Name = "id",
            KeyPath = "id",
            Unique = true
        }
    };

    var tags = new StoreSchema()
    {
        Name = DatabaseService.Tags,
        PrimaryKey = new IndexSpec
        {
            Name = "id",
            KeyPath = "id",
            Unique = true
        }
    };

    var novelTags = new StoreSchema()
    {
        Name = DatabaseService.ItemTags,
        PrimaryKey = new IndexSpec
        {
            Name = "id",
            KeyPath = "id",
            Unique = true,
        }
    };

    var sessions = new StoreSchema()
    {
        Name = "Sessions",
        PrimaryKey = new IndexSpec
        {
            Name = "id",
            KeyPath = "id",
            Unique = true
        }
    };

    services.AddIndexedDB(db =>
    {
        db.DbName = "WryteDB2";
        db.Version = 2;
        
        db.Stores.Add(settings);
        db.Stores.Add(sessions);

        db.Stores.Add(novelTags);
        db.Stores.Add(tags);

        db.Stores.Add(categories);
        db.Stores.Add(events);
        db.Stores.Add(novels);
        db.Stores.Add(groups);
        db.Stores.Add(items);

    });

    services.AddScoped<DatabaseService>();
}