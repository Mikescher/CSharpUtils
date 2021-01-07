using System.Collections.Generic;

namespace MSHC.Lang.Collections
{
	public interface IListWithMove<T> : IList<T>
	{
		void Move(int oldIndex, int newIndex);
	}
}
