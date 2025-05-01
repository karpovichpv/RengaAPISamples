using Renga;
using System.Reflection;

namespace ExportDrawings
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IApplication application = new Application();
			if (application is not null)
			{
				_ = application.OpenProject("c:\\other\\project1.rnp");
				IProject project = application.Project;

				DirectoryInfo? directoryInfo = Directory.GetParent(Assembly.GetExecutingAssembly().Location);
				string? path = null;
				if (directoryInfo is not null)
				{
					path = directoryInfo.FullName;
				}

				if (project is not null)
				{
					ExportToIFC(project, path, application);
					ExportDrawing(project, path);
				}
				application.CloseProject(true);
				application.Quit();
			}
		}

		private static void ExportToIFC(IProject project, string? path, IApplication app)
		{
			if (path is not null)
			{
				IIfcExportSettings settings = app.CreateIfcExportSettings();
				settings.Version = IfcVersion.IfcVersion_4;
				string filePath = Path.GetFullPath(Path.Combine(path, "export.ifc"));
				int result = project.ExportToIfc2(filePath, true, settings);
			}
		}

		private static void ExportDrawing(IProject project, string? path)
		{
			IDrawingCollection drawings = project.Drawings;
			if (path is null)
			{
				Console.WriteLine($"Cannot defined the program path");
				return;
			}

			for (int i = 0; i < drawings.Count; i++)
			{
				IDrawing drawing = drawings.Get(i);
				string filePath = Path.Combine(path, drawing.Name);
				drawing.ExportToPdf(filePath, true);
				drawing.ExportToDwg(filePath, AutocadVersion.AutocadVersion_v2000, true);
				drawing.ExportToDxf(filePath, AutocadVersion.AutocadVersion_v2000, true);
				drawing.ExportToOpenXps(filePath, true);
				Console.WriteLine($"Drawing {drawing.Name} was exported");
			}
		}
	}
}