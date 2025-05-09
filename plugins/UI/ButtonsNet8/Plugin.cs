using Renga;

namespace ButtonsNet8
{
	public class Plugin : IPlugin
	{
		private readonly List<ActionEventSource> _eventSources = [];
		private IImage? _icon;

		public bool Initialize(string pluginFolder)
		{
			Application app = new();
			IUI ui = app.UI;

			string icoPath = Path.GetFullPath(Path.Combine(pluginFolder, "ico.png"));
			_icon = ui.CreateImage();
			_icon.LoadFromFile(icoPath);

			IUIPanelExtension panelExtension = ui.CreateUIPanelExtension();
			panelExtension.AddToolButton(CreateAction(ui, "OrdynaryButton"));
			AddDropDownButton(ui, panelExtension);
			AddSplitButton(ui, panelExtension);

			ui.AddExtensionToPrimaryPanel(panelExtension);
			ui.AddToolButtonToActionsPanel(CreateAction(ui, "ToolButton for the action Panel"), ViewType.ViewType_View3D);
			ui.AddExtensionToActionsPanel(panelExtension, ViewType.ViewType_View3D);
			ui.AddExtensionToActionsPanel(panelExtension, ViewType.ViewType_Sheet);

			return true;
		}

		private void AddSplitButton(IUI ui, IUIPanelExtension panelExtension)
		{
			ISplitButton splitButton = ui.CreateSplitButton(
			  CreateAction(ui, "SplitButton"));

			splitButton.AddAction(
			  CreateAction(ui, "SplitButton 2"));

			splitButton.AddAction(
			  CreateAction(ui, "SplitButton 3"));

			panelExtension.AddSplitButton(splitButton);
		}

		private void AddDropDownButton(IUI ui, IUIPanelExtension panelExtension)
		{
			IDropDownButton dropDownButton = ui.CreateDropDownButton();
			dropDownButton.ToolTip = "DropDownButton";
			dropDownButton.Icon = _icon;

			dropDownButton.AddAction(
			  CreateAction(ui, "DropDownButton 1"));

			dropDownButton.AddAction(
			  CreateAction(ui, "DropDownButton 2"));
			dropDownButton.AddSeparator();

			dropDownButton.AddAction(
			  CreateAction(ui, "DropDownButton 3"));

			panelExtension.AddDropDownButton(dropDownButton);
		}

		private void AddOrdynaryToolButton(IUI ui, IUIPanelExtension panelExtension)
		{
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}

		private IAction CreateAction(IUI ui, string displayName)
		{
			IAction action = ui.CreateAction();
			action.DisplayName = displayName;
			action.Icon = _icon;

			ActionEventSource events = new(action);
			events.Triggered += (s, e) =>
			{
				ui.ShowMessageBox(MessageIcon.MessageIcon_Info, "Plugin message", displayName + " Handler");
			};

			_eventSources.Add(events);

			return action;
		}
	}
}
