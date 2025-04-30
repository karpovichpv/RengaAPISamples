using Renga;

namespace VisualStylesAndVisibility
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
			extension.AddToolButton(CreateChangeViewStyleAction(ui));
			ui.AddExtensionToPrimaryPanel(extension);

			return true;
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}

		private IAction CreateChangeViewStyleAction(IUI ui)
		{
			IAction action = ui.CreateAction();
			action.ToolTip = "Change to the current view style to color";
			ActionEventSource source = new(action);
			source.Triggered += (sender, arguments) =>
			{
				if (_app is not null)
				{
					IView? view = _app.ActiveView;
					if (view is IModelView modelView)
					{
						VisualStyle style = modelView.VisualStyle;
						modelView.VisualStyle = VisualStyle.VisualStyle_Color;
					}
				}
			};

			return action;
		}

		private void Source_Triggered(object? sender, EventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}