using Renga;

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
	}
}
