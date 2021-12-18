using UnhollowerRuntimeLib.XrefScans;
using System.Reflection;
using VRC.Animation;
using UnityEngine;
using System.Linq;

namespace KabulClient
{
    public static class Reflections
    {
        public delegate void ResetLastPositionAction(InputStateController @this);
        public delegate void ResetAction(VRCMotionState @this);

        private static ResetLastPositionAction ourResetLastPositionAction;
        private static ResetAction ourResetAction;

		public static ResetLastPositionAction ResetLastPositionAct
		{
			get
			{
				if (ourResetLastPositionAction != null)
				{
					return ourResetLastPositionAction;
				}

				MethodInfo method = typeof(InputStateController).GetMethods(BindingFlags.DeclaredOnly | 
					BindingFlags.Instance | BindingFlags.Public).Single((MethodInfo it) =>
						XrefScanner.XrefScan(it).Any((XrefInstance jt) => 
							jt.Type == XrefType.Method && jt.TryResolve() != null && 
							jt.TryResolve().Name == "get_transform"));

				ourResetLastPositionAction = (ResetLastPositionAction)System.Delegate.CreateDelegate(
					typeof(ResetLastPositionAction), method);
				
				return ourResetLastPositionAction;
			}
		}

		public static void ResetLastPosition(this InputStateController instance)
		{
			ResetLastPositionAct(instance);
		}

		public static ResetAction ResetAct
		{
			get
			{
				if (ourResetAction != null)
				{
					return ourResetAction;
				}
				
				MethodInfo method = typeof(VRCMotionState).GetMethods(BindingFlags.DeclaredOnly | 
					BindingFlags.Instance | BindingFlags.Public).Single((MethodInfo it) => 
						XrefScanner.XrefScan(it).Count((XrefInstance jt) => 
							jt.Type == XrefType.Method && jt.TryResolve() != null && 
							jt.TryResolve().ReflectedType == typeof(Vector3)) == 4);

				ourResetAction = (ResetAction)System.Delegate.CreateDelegate(typeof(ResetAction), method);
				return ourResetAction;
			}
		}

		public static void Reset(this VRCMotionState instance, bool something = false)
		{
			ResetAct(instance);
		}

		public static float GetFramerate(this PlayerNet net)
		{
			// byte value = net.field_Private_Byte_0;

			if (net.field_Private_Byte_0 == 0f)
			{
				return -1f;
			}
			
			return Mathf.Floor(1000f / net.field_Private_Byte_0);
		}
	}
}
