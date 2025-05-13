using Renga;

internal class FloorBuilder
{
	private readonly Application _application;
	private readonly IModel _model;

	public FloorBuilder()
	{
		Application application = new();
		_application = application;
		_model = application.Project.Model;
	}

	public IModelObject Build()
	{
		IModelObject floor = CreateDefaultObject(ObjectTypes.Floor);
		IModelObject opening = CreateDefaultObject(ObjectTypes.Opening, floor.Id);

		return floor;
	}

	private IModelObject CreateDefaultObject(Guid objectGuid, int? hostObjectId = null)
	{
		INewEntityArgs args = _model.CreateNewEntityArgs();
		args.TypeId = objectGuid;

		if (hostObjectId is not null)
			args.HostObjectId = (int)hostObjectId;

		Placement3D placement3D = GetDefaultPlacement();

		args.Placement3D = placement3D;

		var operation = _application.Project.CreateOperation();
		operation.Start();

		IModelObject newObject = _model.CreateObject(args);
		operation.Apply();

		return newObject;
	}

	private static Placement3D GetDefaultPlacement()
	{
		return new()
		{
			Origin = new() { X = 0, Y = 0, Z = 0 },
			xAxis = new() { X = 1, Y = 0, Z = 0 },
			zAxis = new() { X = 0, Y = 0, Z = 1 }
		};
	}
}