using Renga;

namespace OpenClose
{
	public class Program
	{
		public static void Main(string[] args)
		{
			ArgumentNullException.ThrowIfNull(args);

			IApplication application = new Application();
			if (application is not null)
			{
				_ = application.OpenProject("c:\\other\\project2.rnp");
				IProject project = application.Project;
				IModel model = project.Model;

				CreateColumn(application, model);
				_ = project.Save();
				_ = application.CloseProject(false);
				application.Quit();
			}
		}

		private static void CreateColumn(IApplication application, IModel model)
		{
			INewEntityArgs newObjectArgs = model.CreateNewEntityArgs();
			newObjectArgs.TypeId = ObjectTypes.Column;
			Placement3D placement3D = new()
			{
				Origin = new() { X = 500, Y = 500, Z = 1000 },
				xAxis = new() { X = 1, Y = 1, Z = 0 },
				zAxis = new() { X = 0, Y = 0, Z = 1 }
			};

			newObjectArgs.Placement3D = placement3D;

			IOperation operation = application.Project.CreateOperation();
			operation.Start();
			_ = model.CreateObject(newObjectArgs);

			operation.Apply();
		}
	}
}