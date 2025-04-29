using Renga;

namespace ViewSwitching
{
	public class Plugin : IPlugin
	{
		private readonly List<ActionEventSource> _eventSources = [];

		private IApplication? _app;

		public bool Initialize(string pluginFolder)
		{
			IApplication app = new Application();
			_app = app;
			IUI ui = app.UI;

			IUIPanelExtension extension = ui.CreateUIPanelExtension();
			extension.AddToolButton(CreateMainViewAction(ui));
			ui.AddExtensionToPrimaryPanel(extension);

			return true;
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}

		private IAction CreateMainViewAction(IUI ui)
		{
			IAction action = ui.CreateAction();
			action.DisplayName = "MainActionView";

			var events = new ActionEventSource(action);
			events.Triggered += (sender, arguments) =>
			{
				if (_app is not null)
				{
					int buildingId = _app.Project.BuildingInfo.Id;
					_app.OpenViewByEntity(buildingId);
				}
			};

			_eventSources.Add(events);

			return action;
		}
	}
}