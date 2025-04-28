using Renga;
using System.Runtime.InteropServices;

namespace SystemEvents
{
	public class Plugin : IPlugin
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		static extern int MessageBox(nint hWnd, string text, string caption, uint type);

		private const uint MB_ICONQUESTION = 0x00000020;
		private const uint MB_ICONINFORMATION = 0x00000040;
		private const uint MB_OK = 0x00000000;
		private const uint MB_YESNO = 0x00000004;
		private const uint MB_DEFBUTTON1 = 0x00000000;
		private const int IDNO = 7;
		private const string MessageHeader = "Example of Renga event processing";

		private ApplicationEventSource? _appEvents;

		public bool Initialize(string pluginFolder)
		{
			Application _app = new Application();
			_appEvents = new ApplicationEventSource(_app);
			_appEvents.BeforeApplicationClose += OnBeforeApplicationClose;
			_appEvents.BeforeProjectClose += OnBeforeProjectClosed;
			_appEvents.ProjectClosed += OnProjectClosed;
			_appEvents.ProjectCreated += OnProjectCreate;
			_appEvents.ProjectOpened += OnProjectOpened;
			_appEvents.ProjectSaved += OnProjectSaved;


			return true;
		}

		private void OnProjectSaved(string obj)
		{
			_ = MessageBox(IntPtr.Zero,
						  "Project saved",
						  MessageHeader,
						  MB_ICONQUESTION | MB_YESNO | MB_DEFBUTTON1);
		}

		private void OnProjectOpened(string obj)
		{
			_ = MessageBox(IntPtr.Zero,
						  "Project opened",
						  MessageHeader,
						  MB_ICONQUESTION | MB_YESNO | MB_DEFBUTTON1);
		}

		private void OnProjectCreate()
		{
			_ = MessageBox(IntPtr.Zero,
						  "Project create",
						  MessageHeader,
						  MB_ICONQUESTION | MB_YESNO | MB_DEFBUTTON1);
		}

		private void OnProjectClosed()
		{
			_ = MessageBox(IntPtr.Zero,
						  "On project close",
						  MessageHeader,
						  MB_ICONQUESTION | MB_YESNO | MB_DEFBUTTON1);
		}

		private void OnBeforeProjectClosed(ProjectCloseEventArgs args)
		{
			int msgboxID = MessageBox(IntPtr.Zero,
			  "Before closing project",
			  MessageHeader,
			  MB_ICONQUESTION | MB_YESNO | MB_DEFBUTTON1);

			if (msgboxID == IDNO)
				args.Prevent();
		}

		private void OnBeforeApplicationClose(ApplicationCloseEventArgs args)
		{
			int msgboxID = MessageBox(IntPtr.Zero,
						  "Before application close",
						  MessageHeader,
						  MB_ICONQUESTION | MB_YESNO | MB_DEFBUTTON1);

			if (msgboxID == IDNO)
				args.Prevent();
		}

		public void Stop()
		{
			_appEvents?.Dispose();
		}
	}
}