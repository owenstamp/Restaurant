using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Data.Repositories;
using Restaurant.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages (or add controllers if you plan to use APIs later)
builder.Services.AddRazorPages();
builder.Services.AddSession();


// Register the EF Core DbContext using a connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register all repository implementations with their interfaces
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStaffScheduleRepository, StaffScheduleRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPOSLogRepository, POSLogRepository>();
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();;
builder.Services.AddScoped<ITimeOffRequestRepository, TimeOffRequestRepository>();
builder.Services.AddScoped<IShiftSwapRequestRepository, ShiftSwapRequestRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Map Razor Pages endpoints (for example; if you add controllers later you can call app.MapControllers())
app.MapRazorPages();

app.Run();