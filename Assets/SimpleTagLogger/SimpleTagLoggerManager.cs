using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SimpleTagLoggerManager : ScriptableObject
{
	private const string RESOURCE_PATH = "SimpleTagLogger/SimpleTagLoggerManager";

	static private SimpleTagLoggerManager thisInstance;
	static public SimpleTagLoggerManager instance {
		get {
			return GetInstance ();
		}
	}

	static private SimpleTagLoggerManager GetInstance ()
	{
		if (thisInstance == null && (thisInstance = Resources.Load<SimpleTagLoggerManager> (RESOURCE_PATH)) == null) {
			thisInstance = ScriptableObject.CreateInstance<SimpleTagLoggerManager> ();
			#if UNITY_EDITOR
			if (!System.IO.Directory.Exists ("Assets/Resources/SimpleTagLogger")) {
				System.IO.Directory.CreateDirectory ("Assets/Resources/SimpleTagLogger");
			}
			AssetDatabase.CreateAsset (thisInstance, "Assets/Resources/"+RESOURCE_PATH+".asset");
			#endif
		}
		return thisInstance;
	}


	#if UNITY_EDITOR
	[MenuItem ("Custom/SimpleTagLoggerManager")]
	static public void Select ()
	{
		Selection.activeObject = instance;
	}
	#endif


	public SimpleTagLoggerManager ()
	{
		enabled = true;
		enabledPrev = true;
		tags = new List<ManagedTag> ();
	}

	public bool enabled;
	public List<ManagedTag> tags;

	private bool enabledPrev;

	private bool IsChangedEnable ()
	{
		if (enabled != enabledPrev) {
			enabledPrev = enabled;
			return true;
		}
		return false;
	}

	public bool IsRegistered (string tag)
	{
		foreach (ManagedTag managedTag in tags) {
			if (managedTag.tag == tag) {
				return true;
			}
		}
		return false;
	}

	public bool IsEnabled ()
	{
		return enabled;
	}

	public void SetEnable (bool value)
	{
		enabled = value;
		UpdateEnables ();
	}

	public bool IsTagEnabled (string tag)
	{
		foreach (ManagedTag managedTag in tags) {
			if (managedTag.tag == tag) {
				return (enabled && managedTag.enabled);
			}
		}
		Debug.LogWarning ("\""+tag+"\" is not registered.");
		return false;
	}

	public void SetTagEnable (string tag, bool value)
	{
		foreach (ManagedTag managedTag in tags) {
			if (managedTag.tag == tag) {
				managedTag.SetEnable (value);
				managedTag.SetHandlersEnable (enabled && value);
				return;
			}
		}
		Debug.LogWarning ("\""+tag+"\" is not registered.");
	}

	public bool Register (string tag, TagLogger logHandler)
	{
		ManagedTag managedTag;
		bool isNewRegister = false;
		if (!TryGetManagedTag (tag, out managedTag)) {
			managedTag = new ManagedTag (tag);
			tags.Add (managedTag);
			isNewRegister = true;
		}
		managedTag.AddHandler (logHandler);
		return isNewRegister;
	}

	private bool TryGetManagedTag (string tag, out ManagedTag outManagedTag)
	{
		foreach (ManagedTag managedTag in tags) {
			if (managedTag.tag == tag) {
				outManagedTag = managedTag;
				return true;
			}
		}
		outManagedTag = null;
		return false;
	}

	public void UpdateEnables ()
	{
		bool isChanged = IsChangedEnable ();
		foreach (ManagedTag managedTag in tags) {
			if (isChanged || managedTag.IsChangedEnable ()) {
				managedTag.SetHandlersEnable (enabled && managedTag.enabled);
			}
		}
	}

	[System.Serializable]
	public class ManagedTag
	{
		public string tag;
		public bool enabled;

		private bool enabledPrev;
		private List<TagLogger> logHandlers;

		public ManagedTag (string tag)
		{
			this.tag = tag;
			this.enabled = true;
			this.enabledPrev = true;
		}

		public bool IsChangedEnable ()
		{
			if (enabled != enabledPrev) {
				enabledPrev = enabled;
				return true;
			}
			return false;
		}

		public void SetEnable (bool value)
		{
			enabled = value;
			enabledPrev = value;
		}

		public void SetHandlersEnable (bool value)
		{
			if (logHandlers != null) {
				foreach (TagLogger logHandler in logHandlers) {
					if (logHandler != null) {
						logHandler.enabled = value;
					}
				}
			}
		}

		public void AddHandler (TagLogger logHandler)
		{
			if (logHandlers == null) {
				logHandlers = new List<TagLogger> ();
			}

			if (!logHandlers.Contains (logHandler)) {
				logHandlers.Add (logHandler);
			}
		}
	}
}
