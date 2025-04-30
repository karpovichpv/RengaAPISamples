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
			extension.AddToolButton(CreateHideAllWallsAction(ui));
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
			_eventSources.Add(source);
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

		private IAction CreateHideAllWallsAction(IUI ui)
		{
			IAction action = ui.CreateAction();
			action.ToolTip = "Hide walls";
			ActionEventSource sourceEvent = new(action);
			_eventSources.Add(sourceEvent);
			sourceEvent.Triggered += (sender, arguments) =>
			{
				if (_app is not null)
				{
					IProject project = _app.Project;
					IModel model = project.Model;

					IModelObjectCollection modelObjects = model.GetObjects();
					List<IModelObject> walls = [];
					for (int i = 0; i < modelObjects.Count; i++)
					{
						IModelObject current = modelObjects.GetByIndex(i);
						if (current.ObjectType == ObjectTypes.Wall)
							walls.Add(current);
					}

					IView view = _app.ActiveView;
					if (view is IModelView modelView)
					{
						modelView.SetObjectsVisibility(walls.Select(w => w.Id).ToArray(), false);
					}
				}
			};

			return action;
		}
	}
}