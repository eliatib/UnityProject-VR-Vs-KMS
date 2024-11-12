using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( CapsuleCollider ) )]
	public class BodyCollider : MonoBehaviour
	{
		public Transform head;
        public float offset;
		private CapsuleCollider capsuleCollider;

		//-------------------------------------------------
		void Awake()
		{
			capsuleCollider = GetComponent<CapsuleCollider>();
		}


		//-------------------------------------------------
		void FixedUpdate()
		{
			float distanceFromFloor = Vector3.Dot(head.position, Vector3.up );
            capsuleCollider.height = Mathf.Max( capsuleCollider.radius, distanceFromFloor );
			transform.position = head.position - offset * distanceFromFloor * Vector3.up;
		}

       
    }
}
