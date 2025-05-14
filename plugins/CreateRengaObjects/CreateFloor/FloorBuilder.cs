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
		//IModelObject opening = CreateDefaultObject(ObjectTypes.Opening, floor.Id);
		//IModelObject modifiedFloor = ChangeOpening(opening);
		ChangeFloorContour(floor);

		return floor;
	}

	private void ChangeFloorContour(IModelObject floor)
	{
		var operation = _application.Project.CreateOperationWithUndo(_model.Id);

		IBaseline2DObject baseLineObject = floor as IBaseline2DObject;
		ICurve2D currentBaseLine = baseLineObject.GetBaseline();
		Point2D p1 = new() { X = -2000.0, Y = -1000.0 };
		Point2D p2 = new() { X = 2000.0, Y = 1000.0 };
		Point2D p3 = new() { X = 2000.0, Y = 2000.0 };
		ICurve2D curve1 = _application.Math.CreateLineSegment2D(p1, p2);
		ICurve2D curve2 = _application.Math.CreateLineSegment2D(p2, p3);
		ICurve2D curve3 = _application.Math.CreateLineSegment2D(p3, p1);
		ICurve2D[] curves = [curve1, curve2, curve3,];
		ICurve2D compositeCurve = _application.Math.CreateCompositeCurve2D(curves);

		operation.Start();
		baseLineObject.SetBaseline(compositeCurve);
		operation.Apply();
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

	private IModelObject ChangeOpening(IModelObject floor)
	{
		var operation = _application.Project.CreateOperationWithUndo(_model.Id);

		IBaseline2DObject baseLineObject = floor as IBaseline2DObject;
		ICurve2D currentBaseLine = baseLineObject.GetBaseline();
		Point2D p1 = new() { X = 100.0, Y = 100.0 };
		Point2D p2 = new() { X = 200.0, Y = 100.0 };
		Point2D p3 = new() { X = 200.0, Y = 200.0 };
		ICurve2D curve1 = _application.Math.CreateLineSegment2D(p1, p2);
		ICurve2D curve2 = _application.Math.CreateLineSegment2D(p2, p3);
		ICurve2D curve3 = _application.Math.CreateLineSegment2D(p3, p1);
		ICurve2D[] curves = [curve1, curve2, curve3,];
		ICurve2D compositeCurve = _application.Math.CreateCompositeCurve2D(curves);

		operation.Start();
		baseLineObject.SetBaseline(compositeCurve);
		operation.Apply();

		return null;
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