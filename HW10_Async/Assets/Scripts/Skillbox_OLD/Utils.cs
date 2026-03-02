using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using System.Linq;

namespace Assets.Scripts.InputSystem.ECS_NEW
{
	public static class Utils
	{
		public static List<Collider> GetAllColliders(this GameObject gameObject)
		{
			return gameObject == null ? null : gameObject.GetComponents<Collider>().ToList<Collider>();
		}

		public static void ToWorldSpaceBox(this BoxCollider box, out float3 center, out float3 halfExtents, out quaternion orientation)
		{
			Transform transform = box.transform; //w_space
			orientation = transform.rotation; //w_space
			center = (float3)transform.TransformPoint(box.center); //box.center: l_space --> w_sp
			var lossyScale = transform.lossyScale; //w_sp
			var scale = Abs(lossyScale); // absolute
			halfExtents = Vector3.Scale(scale, box.size) * 0.5f; //multiply scale * size * 0.5 for half
		}

		public static void ToWorldSpaceCapsule(this CapsuleCollider capsule, out float3 point0, out float3 point1, out float radius)
		{
			Transform transform = capsule.transform;
			var center = (float3)transform.TransformPoint(capsule.center);
			radius = 0f;
			float height = 0f;
			float3 lossyScale = Abs(transform.lossyScale);
			float3 dir = float3.zero;

			switch (capsule.direction)
			{
				case 0: //x
					radius = Mathf.Max(lossyScale.y, lossyScale.z) * capsule.radius; //real radius scale
					height = lossyScale.x * capsule.height;
					dir = capsule.transform.TransformDirection(Vector3.right);
					break;
				case 1: //y
					radius = Mathf.Max(lossyScale.x, lossyScale.z) * capsule.radius;
					height = lossyScale.y * capsule.height;
					dir = capsule.transform.TransformDirection(Vector3.up);
					break;
				case 2://z
					radius = Mathf.Max(lossyScale.x, lossyScale.y) *capsule.radius;
					height = lossyScale.z * capsule.height;
					dir = capsule.transform.TransformDirection(Vector3.forward);
					break;
			}
			if (height < radius * 2f)
				dir = float3.zero;

			point0 = center + dir * (height * 0.5f - radius); //height is the extreme point --> -radius to get the center of the upper radius
			point1 = center - dir * (height * 0.5f - radius);
		}


		public static void ToWorldSpaceSphere(this SphereCollider sphere, out float3 center, out float radius)
		{
			Transform transform  = sphere.transform;
			center = transform.TransformPoint(sphere.center);
			radius = sphere.radius * Max(Abs(transform.lossyScale));
		}

		public static float3 Abs(float3 v)
		{
			return new float3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}

		public static float Max(float3 v)
		{
			return Mathf.Max(v.x, Mathf.Max(v.y, v.z));
		}
	}
}