using System;

namespace MSHC.Lang.Weak
{
	[Serializable]
	public class TypedWeakReference<T> : WeakReference
	{
		public TypedWeakReference(T target)
			: base(target)
		{
		}

		public TypedWeakReference(T target, bool trackResurrection) : base(target, trackResurrection)
		{
		}
		
		public new T Target => (T)base.Target;
	}
}
