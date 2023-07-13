using System.Data;

namespace NotificationSystem.DataAccessLayer.Implementation
{
    public class RepositoryBase
    {
        protected IDbTransaction Transaction;
        protected IDbConnection Connection;
        public RepositoryBase(IDbTransaction transaction, IDbConnection connection)
        {
            if(Transaction!= null)
            {
                Connection = transaction.Connection;
            }
            else
            {
                Connection = connection;
            }
            Transaction = transaction;
        }
    }
}
