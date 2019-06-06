namespace AndroidPerm
{

using UnityEngine;

using System;
using System.Collections;



public class AndroidPermission {
	#if !UNITY_EDITOR && UNITY_ANDROID
	private AndroidJavaObject permissionCheck;
	private AndroidJavaObject currentActivity;
	#endif

	private IPermissionCheckListener checkListener;
	private IPermissionResultListener resultListener;

	public Action<CheckEventArgs> OnCheckExplainAction, OnCheckNonExplainAction, OnCheckAlreadyAction;
	public Action<ErrorEventArgs> OnCheckFailedAction;
	public Action<ResultEventArgs> OnResultAction;

	public AndroidPermission () {
		#if !UNITY_EDITOR && UNITY_ANDROID
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		permissionCheck = new AndroidJavaObject("com.aoneg.unity.permissioncheck.PermissionCheckUnity");
		#endif
	}

	public void Init () {
		#if !UNITY_EDITOR && UNITY_ANDROID
		if(permissionCheck != null) {
			checkListener = new PermissionCheckListener();
			resultListener = new PermissionResultListener();

			checkListener.OnExplain += OnCheckExplain;
			checkListener.OnNonExplain += OnCheckNonExplain;
			checkListener.OnAlready += OnCheckAlready;
			checkListener.OnFailed += OnCheckFailed;

			resultListener.OnResult += OnRequestResult;

			permissionCheck.Call("Init", currentActivity, checkListener, resultListener);
		} else {
			Debug.Log("Permission Check Init Failed.");
		}
		#endif
	}

	public void CheckPermission (string permission, int requestCode) {
		#if !UNITY_EDITOR && UNITY_ANDROID
		if(permissionCheck != null) {
			permissionCheck.Call("CheckPermission", permission, requestCode);
		}
		#endif
	}

	public string[] DeninedPermissions (string[] permissions) {
		#if !UNITY_EDITOR && UNITY_ANDROID
		if(permissionCheck != null) {
			return permissionCheck.Call<string[]>("DeninedPermissions", new object[]{permissions});
		}
		#endif

		return null;
	}

	public void RequestPermission (string permission, int requestCode) {
		#if !UNITY_EDITOR && UNITY_ANDROID
		if(permissionCheck != null) {
			permissionCheck.Call("RequestPermission", permission, requestCode);
		}
		#endif
	}

	public void RequestPermissions (string[] permissions, int requestCode) {
		#if !UNITY_EDITOR && UNITY_ANDROID
		if(permissionCheck != null) {
			permissionCheck.Call("RequestPermissions", new object[]{permissions, requestCode});
		}
		#endif
	}

	public void ShowDialog (string permission, int requestCode, string title, string message) {
		#if !UNITY_EDITOR && UNITY_ANDROID
		if(permissionCheck != null) {
			permissionCheck.Call("ShowDialog", requestCode, permission, title, message);
		} else {
			Debug.Log("Permission Check Init Failed.");
		}
		#endif
	}

	// Override Method

	public void OnCheckExplain (object sender, CheckEventArgs args) {
		Debug.Log("Need Permission Expain (" + args.permission + ")");

		if(OnCheckExplainAction != null) {
			OnCheckExplainAction(args);
		}
	}

	public void OnCheckNonExplain (object sender, CheckEventArgs args) {
		Debug.Log("Not need Permission Expain (" + args.permission + ")");

		if(OnCheckNonExplainAction != null) {
			OnCheckNonExplainAction(args);
		}
	}

	public void OnCheckAlready (object sender, CheckEventArgs args) {
		Debug.Log("Permission already granted (" + args.permission + ")");

		if(OnCheckAlreadyAction != null) {
			OnCheckAlreadyAction(args);
		}
	}

	public void OnCheckFailed (object sender, ErrorEventArgs errArgs) {
		Debug.Log("Check Permission Failed (" + errArgs.permission + ") : " + errArgs.message);

		if(OnCheckFailedAction != null) {
			OnCheckFailedAction(errArgs);
		}
	}

	public void OnRequestResult (object sender, ResultEventArgs args) {
		if(OnResultAction != null) {
			OnResultAction(args);
		}
	}
}

//#if !UNITY_EDITOR && UNITY_ANDROID
internal class PermissionCheckListener : AndroidJavaProxy, IPermissionCheckListener {
	public event EventHandler<CheckEventArgs> OnExplain;
	public event EventHandler<CheckEventArgs> OnNonExplain;
	public event EventHandler<CheckEventArgs> OnAlready;
	public event EventHandler<ErrorEventArgs> OnFailed;

	public PermissionCheckListener () : base("com.aoneg.unity.permissioncheck.PermissionCheckListener") {

	}

	void onExplain (int requestCode, string permission) {
		CheckEventArgs data = new CheckEventArgs();
		data.requestCode = requestCode;
		data.permission = permission;

		OnExplain(this, data);
	}

	void onNonExplain (int requestCode, string permission) {
		CheckEventArgs data = new CheckEventArgs();
		data.requestCode = requestCode;
		data.permission = permission;

		OnNonExplain(this, data);
	}

	void onAlready (int requestCode, string permission) {
		CheckEventArgs data = new CheckEventArgs();
		data.requestCode = requestCode;
		data.permission = permission;

		OnAlready(this, data);
	}

	void onFailed (int requestCode, string permission, string msg) {
		ErrorEventArgs data = new ErrorEventArgs();
		data.requestCode = requestCode;
		data.permission = permission;
		data.message = msg;

		OnFailed(this, data);
	}
}

internal class PermissionResultListener : AndroidJavaProxy, IPermissionResultListener {
	public event EventHandler<ResultEventArgs> OnResult;

	public PermissionResultListener () : base("com.aoneg.unity.permissioncheck.PermissionResultListener") {

	}

	void onResult (int requestCode, string[] denined, string[] granted) {
		ResultEventArgs data = new ResultEventArgs();
		data.requestCode = requestCode;
		data.denined = denined;
		data.granted = granted;

		OnResult(this, data);
	}

	void onResult (AndroidJavaObject dataObject) {
		ResultEventArgs data = new ResultEventArgs();

		data.requestCode = dataObject.Get<int>("requestCode");

		AndroidJavaObject deninedObject = dataObject.Get<AndroidJavaObject>("denined");
		AndroidJavaObject grantedObject = dataObject.Get<AndroidJavaObject>("granted");

		data.denined = AndroidJNIHelper.ConvertFromJNIArray<string[]>(deninedObject.GetRawObject());
		data.granted = AndroidJNIHelper.ConvertFromJNIArray<string[]>(grantedObject.GetRawObject());

		OnResult(this, data);
	}
}
/*#else
internal class PermissionCheckListener : IPermissionCheckListener {
	public event EventHandler<CheckEventArgs> OnExplain;
	public event EventHandler<CheckEventArgs> OnNonExplain;
	public event EventHandler<CheckEventArgs> OnAlready;
	public event EventHandler<ErrorEventArgs> OnFailed;

	void onExplain (int requestCode, string permission) {
		CheckEventArgs data = new CheckEventArgs();
		data.requestCode = requestCode;
		data.permission = permission;

		OnExplain(this, data);
	}

	void onNonExplain (int requestCode, string permission) {
		CheckEventArgs data = new CheckEventArgs();
		data.requestCode = requestCode;
		data.permission = permission;

		OnNonExplain(this, data);
	}

	void onAlready (int requestCode, string permission) {
		CheckEventArgs data = new CheckEventArgs();
		data.requestCode = requestCode;
		data.permission = permission;

		OnAlready(this, data);
	}

	void onFailed (int requestCode, string permission, string msg) {
		ErrorEventArgs data = new ErrorEventArgs();
		data.requestCode = requestCode;
		data.permission = permission;
		data.message = msg;

		OnFailed(this, data);
	}
}

internal class PermissionResultListener : IPermissionResultListener {
	public event EventHandler<ResultEventArgs> OnResult;

	void onResult (int requestCode, string[] denined, string[] granted) {
		ResultEventArgs data = new ResultEventArgs();
		data.requestCode = requestCode;
		data.denined = denined;
		data.granted = granted;

		OnResult(this, data);
	}
}
#endif */

internal interface IPermissionCheckListener {
	event EventHandler<CheckEventArgs> OnExplain;
	event EventHandler<CheckEventArgs> OnNonExplain;
	event EventHandler<CheckEventArgs> OnAlready;
	event EventHandler<ErrorEventArgs> OnFailed;
}

internal interface IPermissionResultListener {
	event EventHandler<ResultEventArgs> OnResult;
}

public class ErrorEventArgs : CheckEventArgs {
	public string message { get; set; }
}

public class CheckEventArgs : EventArgs {
	public int requestCode { get; set; }
	public string permission { get; set; }
}

public class ResultEventArgs : EventArgs {
	public int requestCode { get; set; }
	public string[] denined { get; set; }
	public string[] granted { get; set; }
}

}