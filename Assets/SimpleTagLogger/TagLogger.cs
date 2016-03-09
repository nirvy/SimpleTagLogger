using UnityEngine;

public class TagLogger : ILogHandler
{
	public TagLogger (string tag)
	{
		this.tag = tag;
		SimpleTagLoggerManager.instance.Register (tag, this);
		SetActive (SimpleTagLoggerManager.instance.IsTagEnabled (tag));
	}

	public string tag {
		get;
		private set;
	}

	public bool enabled {
		get;
		set;
	}

	public delegate void OnLog (object message);
	public event OnLog onLog;

	public void SetActive (bool value)
	{
		enabled = value;
	}

	public void Log (object message)
	{
		if (enabled) {
			Debug.Log (message);
			if (onLog != null) {
				onLog.Invoke (message);
			}
		}
	}

	public void Log (object message, Object context)
	{
		if (enabled) {
			Debug.Log (message, context);
			if (onLog != null) {
				onLog.Invoke (message);
			}
		}
	}

	public void TLog (object message)
	{
		Log (PutTag (message));
	}

	public void TLog (object message, Object context)
	{
		Log (PutTag (message), context);
	}

	private object PutTag (object message)
	{
		return ((object)"["+tag+"] "+message);
	}

	public void LogException (System.Exception exception, Object context)
	{
		Debug.logger.LogException (exception, context);
	}

	public void LogFormat (LogType logType, Object context, string format, params object[] args)
	{
		Debug.logger.LogFormat (logType, context, format, args);
	}
}
