using Renga;

namespace Parameters
{
	public class Plugin : IPlugin
	{
		private readonly List<ActionEventSource> _eventSources = [];
		private Application? _app;
		private IUI? _ui;
		private IUIPanelExtension? _panelExtension;

		public bool Initialize(string pluginFolder)
		{
			_app = new Application();

			_ui = _app.UI;
			_panelExtension = _ui.CreateUIPanelExtension();


			CreateMessageBox(
				"Plate styles parameters",
				"Plate styles parameters",
				PluginHelpers.ShowPlateStylesParameters);

			_ui.AddExtensionToPrimaryPanel(_panelExtension);

			return true;
		}

		private void CreateMessageBox(
			string tooltip,
			string title,
			Func<IApplication, string> callBack)
		{
			if (_ui is not null && _app is not null)
			{
				var action = _ui.CreateAction();
				action.ToolTip = tooltip;
				_panelExtension?.AddToolButton(action);

				ActionEventSource events = new(action);
				events.Triggered += (sender, arguments) =>
				{
					_ui.ShowMessageBox(
						MessageIcon.MessageIcon_Info,
						title,
						callBack?.Invoke(_app));
				};
			}
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}
	}
}