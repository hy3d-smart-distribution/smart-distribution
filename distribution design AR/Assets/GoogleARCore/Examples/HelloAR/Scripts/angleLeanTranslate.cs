using UnityEngine;

namespace Lean.Touch
{
	// This script allows you to translate the current GameObject relative to the camera
	public class angleLeanTranslate : MonoBehaviour
	{
		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreStartedOverGui = true;

		[Tooltip("Ignore fingers with IsOverGui?")]
		public bool IgnoreIsOverGui;

		[Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
		public int RequiredFingerCount;

		[Tooltip("Does translation require an object to be selected?")]
		public LeanSelectable RequiredSelectable;

		[Tooltip("The camera the translation will be calculated using (None = MainCamera)")]
		public Camera Camera;

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Start();
		}
#endif

		protected virtual void Start()
		{
			if (RequiredSelectable == null)
			{
				RequiredSelectable = GetComponent<LeanSelectable>();
			}
		}

		protected virtual void Update()
		{
			// Get the fingers we want to use
			var fingers = LeanSelectable.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount, RequiredSelectable);

			// Calculate the screenDelta value based on these fingers
			var screenDelta = LeanGesture.GetScreenDelta(fingers);

			// Perform the translation
			if (transform is RectTransform)
			{
				TranslateUI(screenDelta);
			}
			else
			{
				Translate(screenDelta);
			}
		}

		protected virtual void TranslateUI(Vector2 screenDelta)
		{
			// Screen position of the transform
			var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera, transform.position);

			// Add the deltaPosition
			screenPoint += screenDelta;

			// Convert back to world space
			var worldPoint = default(Vector3);

			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent as RectTransform, screenPoint, Camera, out worldPoint) == true)
			{
				transform.position = worldPoint;
			}
		}

		protected virtual void Translate(Vector2 screenDelta)
		{
			// Make sure the camera exists
			var camera = LeanTouch.GetCamera(Camera, gameObject);
            float[] angle = { 0f, 0f };
			if (camera != null)
			{
				// Screen position of the transform
				var screenPoint = camera.WorldToScreenPoint(transform.position);

				// Add the deltaPosition
				screenPoint += (Vector3)screenDelta;
                Vector3 originalPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                // Convert back to world space
                //transform.position = camera.ScreenToWorldPoint(screenPoint);
                Vector3 parentPos = GameObject.Find("pole").transform.position;
                Vector3 movePos = camera.ScreenToWorldPoint(screenPoint);
                Vector3 hidePos = GameObject.FindGameObjectsWithTag("defaultDirection")[1].transform.position;
                Quaternion actionAngle = Quaternion.identity;
                movePos = new Vector3(movePos.x, movePos.y, originalPos.z);


                //angle[0] = Vector3.Angle(hidePos - parentPos, originalPos - parentPos);
                //angle[1] = Vector3.Angle(hidePos - parentPos, movePos - parentPos);
                //float delta = angle[1] - angle[0];
                //var axis = transform.InverseTransformDirection(transform.forward*(delta/2));
                //transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * (Mathf.Abs(delta) / 5), axis);
                
                

                //transform.position = new Vector3(transform.position.x, originalPos.y, transform.position.z);
                //transform.rotation = Quaternion.Slerp(transform.rotation, actionAngle, Time.deltaTime * (delta / 10));
                //transform.Rotate(delta, 0f, 0f, Space.World);
            }
		}
	}
}