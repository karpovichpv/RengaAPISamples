using Renga;

namespace SelectObjects
{
	public class Plugin : IPlugin
	{
		private ISelection? _selection;
		private SelectionEventSource? _selectionEventSource;

		public bool Initialize(string pluginFolder)
		{
			IApplication app = new Application();

			_selection = app.Selection;

			_selectionEventSource = new SelectionEventSource(app.Selection);
			_selectionEventSource.ModelSelectionChanged += OnModelSelectionChanged;

			return true;
		}

		public void Stop()
		{
			_selectionEventSource?.Dispose();
		}

		private void OnModelSelectionChanged(object? sender, EventArgs args)
		{
			IApplication application = new Application();

			IProject project = application.Project;

			if (project == null)
				return;

			IModel model = project.Model;
			IModelObjectCollection modelObjectCollection = model.GetObjects();
			if (_selection is not null)
			{
				int[] selectedObjects = (int[])_selection.GetSelectedObjects();
				if (selectedObjects.Length != 0)
				{
					IModelObject modelObject = modelObjectCollection.GetById(selectedObjects.First());

				}
			}
		}
	}
}
