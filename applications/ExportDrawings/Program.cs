
using Renga;
using System.Reflection;

public class Program
{
	public static void Main(string[] args)
	{
		IApplication application = new Application();
		if (application is not null)
		{
			int result = application.OpenProject("c:\\other\\project.rnp");
			IProject project = application.Project;
			if (project is not null)
			{
				IDrawingCollection drawings = project.Drawings;
				DirectoryInfo? directoryInfo = Directory.GetParent(Assembly.GetExecutingAssembly().Location);
				string? path = null;
				if (directoryInfo is not null)
				{
					path = directoryInfo.FullName;
				}
				if (path is null)
				{
					Console.WriteLine($"Cannot defined the program path");
					return;
				}

				for (int i = 0; i < drawings.Count; i++)
				{
					IDrawing drawing = drawings.Get(i);
					drawing.ExportToPdf(Path.Combine(path, drawing.Name), true);
					Console.WriteLine($"Drawing {drawing.Name} was exported");
				}
			}
		}
	}
}