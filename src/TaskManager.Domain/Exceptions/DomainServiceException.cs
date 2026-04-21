namespace TaskManager.Domain.Exceptions
{
	/// <summary>
	/// Общее исключение доменной сервисной модели
	/// TODO: лучше использовать определенные исключения для конкретных случаев
	/// </summary>
	[Serializable]
	public class DomainServiceException : Exception
	{
		public DomainServiceException()
		{
		}
		public DomainServiceException(string message)
			: base(message)
		{
		}
		public DomainServiceException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
