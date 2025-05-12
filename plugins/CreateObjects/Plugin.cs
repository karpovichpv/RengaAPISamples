using Renga;

namespace CreateObjects
{
	public class Plugin : IPlugin
	{
		private readonly List<ActionEventSource> _eventSources = [];
		private IImage? _icon;

		public bool Initialize(string pluginFolder)
		{
			Application app = new();
			IUI ui = app.UI;

			string icoPath = pluginFolder + @"\ico.png";
			_icon = ui.CreateImage();
			_icon.LoadFromFile(icoPath);

			IUIPanelExtension panelExtension = ui.CreateUIPanelExtension();

			// DropDownButton:
			IDropDownButton dropDownButton = ui.CreateDropDownButton();
			dropDownButton.ToolTip = "Net8 DropDownButton";

			dropDownButton.AddAction(CreateObject(ui, "AssemblyInstance", ObjectTypes.AssemblyInstance)); //+
			dropDownButton.AddAction(CreateObject(ui, "Column", ObjectTypes.Column)); //+
			dropDownButton.AddAction(CreateObject(ui, "Door", ObjectTypes.Door));
			dropDownButton.AddAction(CreateObject(ui, "Element", ObjectTypes.Element)); //+
			dropDownButton.AddAction(CreateObject(ui, "IsolatedFoundation", ObjectTypes.IsolatedFoundation)); //+
			dropDownButton.AddAction(CreateObject(ui, "Level", ObjectTypes.Level)); //+
			dropDownButton.AddAction(CreateObject(ui, "Plate", ObjectTypes.Plate)); //+
			dropDownButton.AddAction(CreateObject(ui, "TextObject", ObjectTypes.TextObject)); //+
			dropDownButton.AddAction(CreateObject(ui, "Window", ObjectTypes.Window));
			dropDownButton.AddAction(CreateObject(ui, "Floor", ObjectTypes.Floor));

			panelExtension.AddDropDownButton(dropDownButton);

			ui.AddExtensionToPrimaryPanel(panelExtension);
			ui.AddExtensionToActionsPanel(panelExtension, ViewType.ViewType_View3D);

			return true;
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}

		private IAction CreateObject(IUI ui, string displayName, Guid objGuid)
		{
			IAction action = ui.CreateAction();
			action.DisplayName = displayName;
			action.Icon = _icon;

			ActionEventSource events = new(action);
			events.Triggered += (s, e) =>
			{
				// With Building or Assembly model
				Application application = new();
				IModel model = application.Project.Model;

				INewEntityArgs args = model.CreateNewEntityArgs();
				args.TypeId = objGuid;
				Placement3D placement3D = new()
				{
					Origin = new() { X = 500, Y = 500, Z = 1000 },
					xAxis = new() { X = 1, Y = 1, Z = 0 },
					zAxis = new() { X = 0, Y = 0, Z = 1 }
				};

				args.Placement3D = placement3D;
				//args.HostObjectId = AnyLevelId;

				var operation = application.Project.CreateOperation();
				operation.Start();

				IModelObject newObject = model.CreateObject(args);
				operation.Apply();
			};

			_eventSources.Add(events);

			return action;
		}
	}
}
