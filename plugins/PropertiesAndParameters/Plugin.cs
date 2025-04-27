using Renga;
using System.Text;

namespace PropertiesAndParameters
{
	public class Plugin : IPlugin
	{
		private readonly List<ActionEventSource> _eventSources = [];
		private Application? _app;

		public bool Initialize(string pluginFolder)
		{
			_app = new Application();

			IUI ui = _app.UI;
			IUIPanelExtension panelExtension = ui.CreateUIPanelExtension();

			Action<string, string, Func<string>> createMessageBox =
				(tooltip, title, callBack) =>
				{
					var action = ui.CreateAction();
					action.ToolTip = tooltip;
					panelExtension.AddToolButton(action);

					ActionEventSource events = new(action);
					events.Triggered += (sender, arguments) =>
					{
						ui.ShowMessageBox(
							MessageIcon.MessageIcon_Info,
							title,
							callBack?.Invoke());
					};
				};


			createMessageBox(
				"Add properties to all floors in the model",
				"Adding properties to all floors",
				AddPropertyToAllFloors
				);

			createMessageBox(
				"Printing the first floor properties values",
				"Print properties",
				PrintPropertyParameters
				);

			ui.AddExtensionToPrimaryPanel(panelExtension);

			return true;
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}

		private string AddPropertyToAllFloors()
		{
			IProject? project = null;
			if (_app is not null)
				project = _app.Project;

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

		private string PrintPropertyParameters()
		{

			IProject? project = null;
			if (_app is not null)
				project = _app.Project;

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

		private static object? GetPropertyValue(IProperty property)
		{
			switch (property.Type)
			{
				case Renga.PropertyType.PropertyType_Angle: return property.GetAngleValue(Renga.AngleUnit.AngleUnit_Degrees);
				case Renga.PropertyType.PropertyType_Double: return property.GetDoubleValue();
				case Renga.PropertyType.PropertyType_String: return property.GetStringValue();
				case Renga.PropertyType.PropertyType_Area: return property.GetAreaValue(Renga.AreaUnit.AreaUnit_Meters2);
				case Renga.PropertyType.PropertyType_Boolean: return property.GetBooleanValue();
				case Renga.PropertyType.PropertyType_Enumeration: return property.GetEnumerationValue();
				case Renga.PropertyType.PropertyType_Integer: return property.GetIntegerValue();
				case Renga.PropertyType.PropertyType_Length: return property.GetLengthValue(Renga.LengthUnit.LengthUnit_Meters);
				case Renga.PropertyType.PropertyType_Logical: return property.GetLogicalValue();
				case Renga.PropertyType.PropertyType_Mass: return property.GetMassValue(Renga.MassUnit.MassUnit_Kilograms);
				case Renga.PropertyType.PropertyType_Volume: return property.GetVolumeValue(Renga.VolumeUnit.VolumeUnit_Meters3);
				case PropertyType.PropertyType_Undefined:
					break;
				default:
					break;
			}
			return null;
		}
	}
}
