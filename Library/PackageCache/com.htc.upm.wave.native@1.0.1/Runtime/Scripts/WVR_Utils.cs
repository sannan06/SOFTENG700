// "Wave SDK 
// © 2020 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the Wave SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using UnityEngine;

namespace Wave.Native
{
	public static class EnumExtensions
	{
		public static string Name(this WVR_DeviceType e)
		{
			switch (e)
			{
				case WVR_DeviceType.WVR_DeviceType_Controller_Left: return "controller left";
				case WVR_DeviceType.WVR_DeviceType_Controller_Right: return "controller right";
				case WVR_DeviceType.WVR_DeviceType_HMD: return "HMD";
				default: return "Invalidate";
			}
		}

		public static string Name(this WVR_InputId e)
		{
			switch (e)
			{
				case WVR_InputId.WVR_InputId_Alias1_System: return "Syste";
				case WVR_InputId.WVR_InputId_Alias1_Menu: return "Menu";
				case WVR_InputId.WVR_InputId_Alias1_Grip: return "Grip";
				case WVR_InputId.WVR_InputId_Alias1_DPad_Left: return "DPad_Left";
				case WVR_InputId.WVR_InputId_Alias1_DPad_Up: return "DPad_Up";
				case WVR_InputId.WVR_InputId_Alias1_DPad_Right: return "DPad_Right";
				case WVR_InputId.WVR_InputId_Alias1_DPad_Down: return "DPad_Down";
				case WVR_InputId.WVR_InputId_Alias1_Volume_Up: return "Volume_Up";
				case WVR_InputId.WVR_InputId_Alias1_Volume_Down: return "Volume_Down";
				case WVR_InputId.WVR_InputId_Alias1_Bumper: return "Digital_Trigger";
				case WVR_InputId.WVR_InputId_Alias1_Back: return "Back";
				case WVR_InputId.WVR_InputId_Alias1_Enter: return "Enter";
				case WVR_InputId.WVR_InputId_Alias1_Touchpad: return "Touchpad";
				case WVR_InputId.WVR_InputId_Alias1_Trigger: return "Trigger";
				case WVR_InputId.WVR_InputId_Alias1_Thumbstick: return "Thumbstick";
				default: return e.ToString();
			}
		}
	} // class EnumExtensions


	public static class Coordinate
	{
		public static Vector3 GetVectorFromGL(this Matrix4x4 matrix)
		{
			var x = matrix.m03;
			var y = matrix.m13;
			var z = matrix.m23;

			return new Vector3(x, y, z);
		}

		public static Vector3 GetVectorFromGL(WVR_Vector3f_t glVector)
		{
			return new Vector3(glVector.v0, glVector.v1, -glVector.v2);
		}

		public static void GetVectorFromGL(WVR_Vector3f_t gl_vec, out Vector3 unity_vec)
		{
			unity_vec.x = gl_vec.v0;
			unity_vec.y = gl_vec.v1;
			unity_vec.z = -gl_vec.v2;
		}

		public static Quaternion GetQuaternionFromGL(Matrix4x4 matrix)
		{
			float tr = matrix.m00 + matrix.m11 + matrix.m22;
			float qw, qx, qy, qz;
			if (tr > 0)
			{
				float S = Mathf.Sqrt(tr + 1.0f) * 2; // S=4*qw
				qw = 0.25f * S;
				qx = (matrix.m21 - matrix.m12) / S;
				qy = (matrix.m02 - matrix.m20) / S;
				qz = (matrix.m10 - matrix.m01) / S;
			}
			else if ((matrix.m00 > matrix.m11) & (matrix.m00 > matrix.m22))
			{
				float S = Mathf.Sqrt(1.0f + matrix.m00 - matrix.m11 - matrix.m22) * 2; // S=4*qx
				qw = (matrix.m21 - matrix.m12) / S;
				qx = 0.25f * S;
				qy = (matrix.m01 + matrix.m10) / S;
				qz = (matrix.m02 + matrix.m20) / S;
			}
			else if (matrix.m11 > matrix.m22)
			{
				float S = Mathf.Sqrt(1.0f + matrix.m11 - matrix.m00 - matrix.m22) * 2; // S=4*qy
				qw = (matrix.m02 - matrix.m20) / S;
				qx = (matrix.m01 + matrix.m10) / S;
				qy = 0.25f * S;
				qz = (matrix.m12 + matrix.m21) / S;
			}
			else
			{
				float S = Mathf.Sqrt(1.0f + matrix.m22 - matrix.m00 - matrix.m11) * 2; // S=4*qz
				qw = (matrix.m10 - matrix.m01) / S;
				qx = (matrix.m02 + matrix.m20) / S;
				qy = (matrix.m12 + matrix.m21) / S;
				qz = 0.25f * S;
			}
#if UNITY_2018_1_OR_NEWER
			return new Quaternion(qx, qy, qz, qw).normalized;
#else
		Vector4 un = new Vector4(qx, qy, qz, qw);
		un.Normalize();
		return new Quaternion(un.x, un.y, un.z, un.w);
#endif
		}

		public static Quaternion GetQuaternionFromGL(WVR_Quatf_t glQuat)
		{
			return new Quaternion(glQuat.x, glQuat.y, -glQuat.z, -glQuat.w);
		}

		public static void GetQuaternionFromGL(WVR_Quatf_t glQuat, out Quaternion unity_quat)
		{
			unity_quat.x = glQuat.x;
			unity_quat.y = glQuat.y;
			unity_quat.z = -glQuat.z;
			unity_quat.w = -glQuat.w;
		}

		public static Vector3 GetScale(this Matrix4x4 matrix)
		{
			Vector3 scale;
			scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
			scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
			scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
			return scale;
		}

		public static Vector4 MatrixMulVector(Matrix4x4 m, Vector4 v)
		{
			Vector4 row0 = m.GetRow(0);
			Vector4 row1 = m.GetRow(1);
			Vector4 row2 = m.GetRow(2);
			Vector4 row3 = m.GetRow(3);

			float v0 = row0.x * v.x + row0.y * v.y + row0.z * v.z + row0.w * v.w;
			float v1 = row1.x * v.x + row1.y * v.y + row1.z * v.z + row1.w * v.w;
			float v2 = row2.x * v.x + row2.y * v.y + row2.z * v.z + row2.w * v.w;
			float v3 = row3.x * v.x + row3.y * v.y + row3.z * v.z + row3.w * v.w;

			return new Vector4(v0, v1, v2, v3);
		}
	} // class Coordinate

	// get new position and rotation from new pose
	[System.Serializable]
    public struct RigidTransform
    {
        public Vector3 pos;
        public Quaternion rot;

        public static RigidTransform identity
        {
            get { return new RigidTransform(Vector3.zero, Quaternion.identity); }
        }

        public RigidTransform(Vector3 pos, Quaternion rot)
        {
            this.pos = pos;
            this.rot = rot;
        }

        public RigidTransform(Transform t)
        {
            this.pos = t.position;
            this.rot = t.rotation;
        }

        public RigidTransform(WVR_Matrix4f_t pose)
        {
            var m = toMatrix44(pose);
            this.pos = Coordinate.GetVectorFromGL(m);
            this.rot = Coordinate.GetQuaternionFromGL(m);
        }

        public static Matrix4x4 toMatrix44(WVR_Matrix4f_t pose, bool glToUnity = true)
        {
            var m = Matrix4x4.identity;
            int sign = glToUnity ? -1 : 1;

            m[0, 0] = pose.m0;
            m[0, 1] = pose.m1;
            m[0, 2] = pose.m2 * sign;
            m[0, 3] = pose.m3;

            m[1, 0] = pose.m4;
            m[1, 1] = pose.m5;
            m[1, 2] = pose.m6 * sign;
            m[1, 3] = pose.m7;

            m[2, 0] = pose.m8 * sign;
            m[2, 1] = pose.m9 * sign;
            m[2, 2] = pose.m10;
            m[2, 3] = pose.m11 * sign;

            m[3, 0] = pose.m12;
            m[3, 1] = pose.m13;
            m[3, 2] = pose.m14;
            m[3, 3] = pose.m15;

            return m;
        }

        public static WVR_Matrix4f_t ToWVRMatrix(Matrix4x4 m, bool unityToGL = true)
        {
            WVR_Matrix4f_t pose;
            int sign = unityToGL ? -1 : 1;

            pose.m0 = m[0, 0];
            pose.m1 = m[0, 1];
            pose.m2 = m[0, 2] * sign;
            pose.m3 = m[0, 3];

            pose.m4 = m[1, 0];
            pose.m5 = m[1, 1];
            pose.m6 = m[1, 2] * sign;
            pose.m7 = m[1, 3];

            pose.m8 = m[2, 0] * sign;
            pose.m9 = m[2, 1] * sign;
            pose.m10 = m[2, 2];
            pose.m11 = m[2, 3] * sign;

            pose.m12 = m[3, 0];
            pose.m13 = m[3, 1];
            pose.m14 = m[3, 2];
            pose.m15 = m[3, 3];

            return pose;
        }

        public static Vector3 ToUnityPos(Vector3 glPos)
        {
            glPos.z *= -1;
            return glPos;
        }

        public void update(WVR_Matrix4f_t pose)
        {
            var m = toMatrix44(pose);
            this.pos = Coordinate.GetVectorFromGL(m);
            this.rot = Coordinate.GetQuaternionFromGL(m);
        }

        public void update(Vector3 position, Quaternion orientation)
        {
            this.pos = position;
            this.rot = orientation;
        }

        public override bool Equals(object o)
        {
            if (o is RigidTransform)
            {
                RigidTransform t = (RigidTransform)o;
                return pos == t.pos && rot == t.rot;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return pos.GetHashCode() ^ rot.GetHashCode();
        }

        public static bool operator ==(RigidTransform a, RigidTransform b)
        {
            return a.pos == b.pos && a.rot == b.rot;
        }

        public static bool operator !=(RigidTransform a, RigidTransform b)
        {
            return a.pos != b.pos || a.rot != b.rot;
        }

        public static RigidTransform operator *(RigidTransform a, RigidTransform b)
        {
            return new RigidTransform
            {
                rot = a.rot * b.rot,
                pos = a.pos + a.rot * b.pos
            };
        }

        public void Inverse()
        {
            rot = Quaternion.Inverse(rot);
            pos = -(rot * pos);
        }

        public RigidTransform GetInverse()
        {
            var t = new RigidTransform(pos, rot);
            t.Inverse();
            return t;
        }

        public Vector3 TransformPoint(Vector3 point)
        {
            return pos + (rot * point);
        }

        public static Vector3 operator *(RigidTransform t, Vector3 v)
        {
            return t.TransformPoint(v);
        }

	} // struct RigidTransform
}
