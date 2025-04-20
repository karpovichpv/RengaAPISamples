using Renga;

namespace RengaAppInfo
{
	public class Plugin : IPlugin
	{
		private readonly List<ActionEventSource> _eventSources = [];

		public bool Initialize(string pluginFolder)
		{
			IApplication app = new Application();
			IUI ui = app.UI;
			IUIPanelExtension panelExtension = ui.CreateUIPanelExtension();

			panelExtension.AddToolButton(CreateAction(ui, app));

			ui.AddExtensionToPrimaryPanel(panelExtension);
			ui.AddExtensionToActionsPanel(panelExtension, ViewType.ViewType_View3D);

			return true;
		}

		private IAction CreateAction(IUI ui, IApplication app)
		{
			IAction action = ui.CreateAction();
			ProductVersion version = app.Version;

			ActionEventSource events = new(action);
			events.Triggered += (obj, args) =>
			{
				string message = $"Version of Renga.\n" +
									$"Major:{version.Major};\n" +
									$"Minor:{version.Minor};\n" +
									$"Patch:{version.Patch};\n" +
									$"BuildNumber:{version.BuildNumber}";
				ui.ShowMessageBox(
					MessageIcon.MessageIcon_Info,
					"Version of the Renga",
					message
					);
			};

			_eventSources.Add(events);
			return action;
		}

		public void Stop()
		{
			foreach (var @event in _eventSources)
			{
				@event.Dispose();
			}

			_eventSources.Clear();
		}
	}
}
