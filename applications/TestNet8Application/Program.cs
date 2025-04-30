
using Renga;

IApplication app = new Application();
if (app is not null)
{
	Console.WriteLine($"app.ProductName: {app.ProductName}");
}
else
{
	Console.WriteLine($"connection failed");
}