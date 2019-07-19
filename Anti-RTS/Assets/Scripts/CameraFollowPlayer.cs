using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private Vector3 offset;

	private void Update()
	{
		if(FindObjectOfType<Planner>().IsPaused() || target == null) {
			return;
		}
		this.transform.position = this.target.position + this.offset;

	}
}
