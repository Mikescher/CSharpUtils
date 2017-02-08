using System;

namespace MSHC.Exceptions
{
	public class XMLStructureException : Exception
	{
		public XMLStructureException() { }
		public XMLStructureException(string message) : base(message) { }
		public XMLStructureException(string message, Exception innerException) : base(message, innerException) { }
	}
}
