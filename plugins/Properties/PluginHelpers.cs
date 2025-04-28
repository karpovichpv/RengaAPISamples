using Renga;
using System.Text;

internal static class PluginHelpers
{
	public static string AddPropertyToAllFloors(IApplication app)
	{
		IProject? project = null;
		if (app is not null)
			project = app.Project;

		if (project is not null)
		{
			if (project.HasActiveOperation())
				return "It's impossible to modify the project. Another operation in progress";

			IOperation operation = project.CreateOperation();
			operation.Start();

			IPropertyManager propertyManager = project.PropertyManager;
			PropertyDescription propertyDescription = new()
			{
				Name = "New property for floor",
				Type = PropertyType.PropertyType_String
			};

			Guid attributeGuid = Guid.NewGuid();
			propertyManager.RegisterProperty(attributeGuid, propertyDescription);
			propertyManager.AssignPropertyToType(attributeGuid, ObjectTypes.Floor);

			IModel model = project.Model;
			IModelObjectCollection objects = model.GetObjects();

			for (int i = 0; i < objects.Count; i++)
			{
				IModelObject obj = objects.GetByIndex(i);
				if (obj.ObjectType == ObjectTypes.Floor)
				{
					var property = obj.GetProperties().Get(attributeGuid);
					property.SetStringValue("Test floor parameter");
				}
			}

			operation.Apply();
		}

		return "Operation was successfully applied";
	}

	public static object? GetPropertyValue(IProperty property)
	{
		switch (property.Type)
		{
			case PropertyType.PropertyType_Angle: return property.GetAngleValue(Renga.AngleUnit.AngleUnit_Degrees);
			case PropertyType.PropertyType_Double: return property.GetDoubleValue();
			case PropertyType.PropertyType_String: return property.GetStringValue();
			case PropertyType.PropertyType_Area: return property.GetAreaValue(Renga.AreaUnit.AreaUnit_Meters2);
			case PropertyType.PropertyType_Boolean: return property.GetBooleanValue();
			case PropertyType.PropertyType_Enumeration: return property.GetEnumerationValue();
			case PropertyType.PropertyType_Integer: return property.GetIntegerValue();
			case PropertyType.PropertyType_Length: return property.GetLengthValue(Renga.LengthUnit.LengthUnit_Meters);
			case PropertyType.PropertyType_Logical: return property.GetLogicalValue();
			case PropertyType.PropertyType_Mass: return property.GetMassValue(Renga.MassUnit.MassUnit_Kilograms);
			case PropertyType.PropertyType_Volume: return property.GetVolumeValue(Renga.VolumeUnit.VolumeUnit_Meters3);
			case PropertyType.PropertyType_Undefined:
				break;
			default:
				break;
		}
		return null;
	}

	public static string PrintPropertyParameters(IApplication app)
	{

		IProject? project = null;
		if (app is not null)
			project = app.Project;

		if (project is not null)
		{
			IModel model = project.Model;
			IModelObjectCollection objects = model.GetObjects();

			for (int i = 0; i < objects.Count; i++)
			{
				IModelObject obj = objects.GetByIndex(i);
				if (obj.ObjectType == ObjectTypes.Floor)
				{
					IPropertyContainer container = obj.GetProperties();
					IGuidCollection guids = container.GetIds();

					StringBuilder builder = new("Floor properties");
					for (int j = 0; j < guids.Count; j++)
					{
						Guid current = guids.Get(j);
						IProperty property = container.Get(current);
						builder.AppendLine("");
						builder.Append($"Name: {property.Name}");
						builder.Append($"Type: {property.Type}");
						builder.Append($"Value: {GetPropertyValue(property)}");
					}
					return builder.ToString();
				}
			}
		}

		return "Operation was successfully applied";

	}
}