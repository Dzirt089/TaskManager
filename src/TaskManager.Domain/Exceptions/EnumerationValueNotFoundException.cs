namespace TaskManager.Domain.Exceptions
{
	[Serializable]
	internal class EnumerationValueNotFoundException : Exception
	{
		private string? name;
		private string v;

		public EnumerationValueNotFoundException()
		{
		}

		public EnumerationValueNotFoundException(string? message) : base(message)
		{
		}

		public EnumerationValueNotFoundException(string? name, string v)
		{
			this.name = name;
			this.v = v;
		}

		public EnumerationValueNotFoundException(string? message, Exception? innerException) : base(message, innerException)
		{
		}
	}
}