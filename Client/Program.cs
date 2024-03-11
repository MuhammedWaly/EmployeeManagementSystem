using BaseLibrary.Entities;
using Blazored.LocalStorage;
using Client;
using Client.ApplicationStates;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using ClientLibrary.Services.Implementions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddHttpClient("SystemApiClient", Client =>
{
    Client.BaseAddress = new Uri("https://localhost:7031");
}).AddHttpMessageHandler<CustomHttpHandler>();
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7031") });
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<GetHttpClient>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider,CustomAuthenticationSatateProviders>();
builder.Services.AddScoped<IUserAccountService,UserAccountService>();
builder.Services.AddScoped<IGenericReposaitory<GeneralDepartment>,GenericReposaitory<GeneralDepartment>>();
builder.Services.AddScoped<IGenericReposaitory<Department>, GenericReposaitory<Department>>();
builder.Services.AddScoped<IGenericReposaitory<Branch>,GenericReposaitory<Branch>>();

builder.Services.AddScoped<IGenericReposaitory<Country>,GenericReposaitory<Country>>();
builder.Services.AddScoped<IGenericReposaitory<City>,GenericReposaitory<City>>();
builder.Services.AddScoped<IGenericReposaitory<Town>,GenericReposaitory<Town>>();

builder.Services.AddScoped<IGenericReposaitory<Employee>,GenericReposaitory<Employee>>();
builder.Services.AddScoped<IGenericReposaitory<Vacation>,GenericReposaitory<Vacation>>();
builder.Services.AddScoped<IGenericReposaitory<VacationType>,GenericReposaitory<VacationType>>();
builder.Services.AddScoped<IGenericReposaitory<Doctor>,GenericReposaitory<Doctor>>();
builder.Services.AddScoped<IGenericReposaitory<Sanction>,GenericReposaitory<Sanction>>();
builder.Services.AddScoped<IGenericReposaitory<SanctionType>,GenericReposaitory<SanctionType>>();
builder.Services.AddScoped<IGenericReposaitory<Overtime>,GenericReposaitory<Overtime>>();
builder.Services.AddScoped<IGenericReposaitory<OvertimeType>,GenericReposaitory<OvertimeType>>();


builder.Services.AddScoped<AllStates>();

builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<SfDialogService>();

await builder.Build().RunAsync();
