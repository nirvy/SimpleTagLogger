using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Sample
{
	public class LoggerSample : MonoBehaviour
	{
		public Button reload;

		public Button log1,log2;
		public Button enable1,enable2;
		public Button enableAll;

		public Text logText;

		private TagLogger tagLogger1;
		private MyLogger tagLogger2;

		private void Start ()
		{
			tagLogger1 = new TagLogger ("log1");
			tagLogger2 = new MyLogger ();

			tagLogger1.onLog += (message) => { logText.text += string.Format ("\n{0:G}", message.ToString ()); };
			tagLogger2.onLog += (message) => { logText.text += string.Format ("\n{0:G}", message.ToString ()); };
			reload.onClick.AddListener ( () => { UnityEngine.SceneManagement.SceneManager.LoadScene ("Sample_SimpleTagLogger"); } );

			log1.onClick.AddListener (OutputLog1);
			log2.onClick.AddListener (OutputLog2);

			enable1.onClick.AddListener (EnableLog1);
			enable2.onClick.AddListener (EnableLog2);

			enableAll.onClick.AddListener (EnableLogAll);

			if (SimpleTagLoggerManager.instance.IsRegistered (tagLogger1.tag)) {
				bool isEnable1 = SimpleTagLoggerManager.instance.IsTagEnabled (tagLogger1.tag);
				SetEnableLog (tagLogger1.tag, enable1, isEnable1);
			}

			if (SimpleTagLoggerManager.instance.IsRegistered (tagLogger2.tag)) {
				bool isEnable2 = SimpleTagLoggerManager.instance.IsTagEnabled (tagLogger2.tag);
				SetEnableLog (tagLogger2.tag, enable2, isEnable2);
			}

			bool isEnableAll = SimpleTagLoggerManager.instance.IsEnabled ();
			SimpleTagLoggerManager.instance.SetEnable (isEnableAll);
			if (isEnableAll) {
				enableAll.image.color = Color.white;
			} else {
				enableAll.image.color = Color.gray;
			}
		}

		private void OutputLog1 ()
		{
			string log = System.DateTime.Now.ToString ();
			tagLogger1.TLog (log);
		}

		private void OutputLog2 ()
		{
			string log = System.DateTime.Now.ToString ();
			tagLogger2.TLog (log);
		}

		private void EnableLog1 ()
		{
			if (SimpleTagLoggerManager.instance.IsEnabled () && SimpleTagLoggerManager.instance.IsRegistered (tagLogger1.tag)) {
				bool isEnable = !SimpleTagLoggerManager.instance.IsTagEnabled (tagLogger1.tag);
				SetEnableLog (tagLogger1.tag, enable1, isEnable);
			}
		}

		private void EnableLog2 ()
		{
			if (SimpleTagLoggerManager.instance.IsEnabled () && SimpleTagLoggerManager.instance.IsRegistered (tagLogger2.tag)) {
				bool isEnable = !SimpleTagLoggerManager.instance.IsTagEnabled (tagLogger2.tag);
				SetEnableLog (tagLogger2.tag, enable2, isEnable);
			}
		}

		private void EnableLogAll ()
		{
			bool isEnableAll = !SimpleTagLoggerManager.instance.IsEnabled ();
			SimpleTagLoggerManager.instance.SetEnable (isEnableAll);
			if (isEnableAll) {
				enableAll.image.color = Color.white;
			} else {
				enableAll.image.color = Color.gray;
			}
		}

		private void SetEnableLog (string tag, Button button, bool value)
		{
			SimpleTagLoggerManager.instance.SetTagEnable (tag, value);
			if (value) {
				button.image.color = Color.white;
			} else {
				button.image.color = Color.gray;
			}
		}
	}
}
