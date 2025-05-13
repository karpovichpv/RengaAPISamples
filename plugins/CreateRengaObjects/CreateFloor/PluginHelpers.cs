using Renga;

internal static class PluginHelpers
{
	public static IModelObject CreateFloor()
	{
		Application application = new();
		IModel model = application.Project.Model;

		INewEntityArgs args = model.CreateNewEntityArgs();
		args.TypeId = ObjectTypes.Floor;
		Placement3D placement3D = new()
		{
			Origin = new() { X = 0, Y = 0, Z = 0 },
			xAxis = new() { X = 1, Y = 0, Z = 0 },
			zAxis = new() { X = 0, Y = 0, Z = 1 }
		};

		args.Placement3D = placement3D;

		var operation = application.Project.CreateOperation();
		operation.Start();

		IModelObject newObject = model.CreateObject(args);
		operation.Apply();

		return newObject;
	}
}