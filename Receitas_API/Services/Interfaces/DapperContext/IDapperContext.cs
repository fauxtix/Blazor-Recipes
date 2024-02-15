using System;
using System.Data;

namespace Receitas_API.Services.Interfaces.DapperContext
{
	public interface IDapperContext
	{
		public IDbConnection CreateConnection();
		public void Execute(Action<IDbConnection> @event);

	}
}
